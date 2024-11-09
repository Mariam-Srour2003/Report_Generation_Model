import re
import torch  # Importing PyTorch for building and training neural networks
import config  # Importing configuration settings, such as device (CPU/GPU) and other parameters
import torch.nn as nn  # Importing PyTorch's neural network module
import torch.nn.functional as F  # Importing functional tools like activations and loss functions
import torchvision.models as models  # Importing pretrained models from torchvision

from collections import OrderedDict  # OrderedDict to maintain the order of keys in a dictionary

class DenseNet121(nn.Module):
    def __init__(self, out_size=14, checkpoint=None):
        """
        Initialize the DenseNet121 model for feature extraction and classification.
        
        Args:
            out_size (int): Number of output classes (default is 14 for multi-label classification).
            checkpoint (str): Path to a model checkpoint to load pre-trained weights.
        """
        super(DenseNet121, self).__init__()

        # Load the pretrained DenseNet121 model from torchvision with default weights
        self.densenet121 = models.densenet121(weights='DEFAULT')

        # Get the number of input features for the final classifier layer
        num_classes = self.densenet121.classifier.in_features

        # Replace the original classifier with a custom classifier (Linear layer followed by Sigmoid)
        self.densenet121.classifier = nn.Sequential(
            nn.Linear(num_classes, out_size),
            nn.Sigmoid()  # Sigmoid for multi-label classification (output range [0,1])
        )

        # If a checkpoint is provided, load the state dictionary from the checkpoint
        if checkpoint is not None:
            checkpoint = torch.load(checkpoint, map_location=torch.device('cpu'))  # Load the checkpoint onto CPU
            state_dict = checkpoint['state_dict']

            # Create a new state dictionary, removing any 'module.' prefix (from DataParallel models)
            new_state_dict = OrderedDict()
            for k, v in state_dict.items():
                if k.startswith('module.'):
                    k = k[7:]  # Remove 'module.' prefix
                new_state_dict[k] = v

            # Load the new state dictionary into the DenseNet model (strict=False allows for missing keys)
            self.densenet121.load_state_dict(new_state_dict, strict=False)

    def forward(self, x):
        """
        Forward pass through the DenseNet121 model.
        
        Args:
            x (Tensor): Input tensor (images).
        
        Returns:
            Tensor: Output tensor (predictions).
        """
        return self.densenet121(x)

class EncoderCNN(nn.Module):
    def __init__(self, checkpoint=None):
        """
        Initialize the EncoderCNN class using DenseNet121 as the backbone.
        
        Args:
            checkpoint (str): Path to a model checkpoint to load pre-trained weights.
        """
        super(EncoderCNN, self).__init__()

        # Initialize the DenseNet121 model with optional checkpoint loading
        self.model = DenseNet121(
            checkpoint=checkpoint
        )

        # Freeze all layers in the DenseNet121 model to prevent them from being updated during training
        for param in self.model.densenet121.parameters():
            param.requires_grad_(False)

    def forward(self, images):
        """
        Forward pass through the EncoderCNN to extract image features.
        
        Args:
            images (Tensor): Input tensor (images).
        
        Returns:
            Tensor: Extracted features tensor.
        """
        # Pass images through the DenseNet121 feature extractor
        features = self.model.densenet121.features(images)

        # Get the size of the features tensor
        batch, maps, size_1, size_2 = features.size()

        # Permute the tensor to move the channels to the last dimension and flatten the spatial dimensions
        features = features.permute(0, 2, 3, 1)
        features = features.view(batch, size_1 * size_2, maps)

        return features

class Attention(nn.Module):
    def __init__(self, features_size, hidden_size, output_size=1):
        """
        Initialize the Attention mechanism to focus on relevant parts of the image features.
        
        Args:
            features_size (int): Size of the input features from the encoder.
            hidden_size (int): Size of the hidden state from the decoder.
            output_size (int): Size of the attention output (default is 1).
        """
        super(Attention, self).__init__()

        # Linear transformation to project the features into the hidden size
        self.W = nn.Linear(features_size, hidden_size)
        # Linear transformation to project the decoder output into the hidden size
        self.U = nn.Linear(hidden_size, hidden_size)
        # Linear transformation to project the combined features to the output size (usually 1)
        self.v = nn.Linear(hidden_size, output_size)

    def forward(self, features, decoder_output):
        """
        Forward pass through the Attention mechanism.
        
        Args:
            features (Tensor): Extracted image features from the encoder.
            decoder_output (Tensor): Hidden state output from the decoder.
        
        Returns:
            context (Tensor): Context vector resulting from the weighted sum of features.
            weights (Tensor): Attention weights over the features.
        """
        # Unsqueeze the decoder output to match the dimensions of features
        decoder_output = decoder_output.unsqueeze(1)

        # Apply linear transformations to both features and decoder output
        w = self.W(features)
        u = self.U(decoder_output)

        # Compute attention scores using a tanh activation followed by a linear transformation
        scores = self.v(torch.tanh(w + u))
        # Apply softmax to compute attention weights
        weights = F.softmax(scores, dim=1)
        # Compute the context vector as the weighted sum of features
        context = torch.sum(weights * features, dim=1)

        # Remove the last dimension from the weights tensor
        weights = weights.squeeze(2)

        return context, weights

class DecoderRNN(nn.Module):
    def __init__(self, features_size, embed_size, hidden_size, vocab_size):
        """
        Initialize the DecoderRNN class to generate captions from image features.
        
        Args:
            features_size (int): Size of the input features from the encoder.
            embed_size (int): Size of the word embeddings.
            hidden_size (int): Size of the hidden state in the LSTM.
            vocab_size (int): Size of the vocabulary.
        """
        super(DecoderRNN, self).__init__()

        self.vocab_size = vocab_size  # Vocabulary size

        # Embedding layer to convert word indices to dense vectors
        self.embedding = nn.Embedding(vocab_size, embed_size)
        # LSTM cell that takes in embeddings and features to generate hidden states
        self.lstm = nn.LSTMCell(embed_size + features_size, hidden_size)

        # Fully connected layer to map hidden states to vocabulary size (for word prediction)
        self.fc = nn.Linear(hidden_size, vocab_size)

        # Attention mechanism to focus on relevant image features
        self.attention = Attention(features_size, hidden_size)

        # Linear layers to initialize the hidden and cell states of the LSTM
        self.init_h = nn.Linear(features_size, hidden_size)
        self.init_c = nn.Linear(features_size, hidden_size)

    def forward(self, features, captions):
        """
        Forward pass through the DecoderRNN to generate captions.
        
        Args:
            features (Tensor): Extracted image features from the encoder.
            captions (Tensor): Ground truth captions as input for the decoder.
        
        Returns:
            outputs (Tensor): Predicted word scores for each time step.
            atten_weights (Tensor): Attention weights over the features for each time step.
        """
        # Convert captions to embeddings
        embeddings = self.embedding(captions)

        # Initialize the hidden and cell states of the LSTM using the image features
        h, c = self.init_hidden(features)

        # Get the sequence length (excluding the <EOS> token) and batch size
        seq_len = len(captions[0]) - 1
        features_size = features.size(1)
        batch_size = captions.size(0)

        # Initialize tensors to store the outputs and attention weights
        outputs = torch.zeros(batch_size, seq_len, self.vocab_size).to(config.DEVICE)
        atten_weights = torch.zeros(batch_size, seq_len, features_size).to(config.DEVICE)

        # Iterate through the sequence to generate each word in the caption
        for i in range(seq_len):
            # Compute the context vector and attention weights
            context, attention = self.attention(features, h)

            # Concatenate the embedding of the current word with the context vector
            inputs = torch.cat((embeddings[:, i, :], context), dim=1)

            # Pass the inputs through the LSTM to update the hidden and cell states
            h, c = self.lstm(inputs, (h, c))
            # Apply dropout to the hidden state for regularization
            h = F.dropout(h, p=0.5)

            # Generate the output (predicted word scores) from the hidden state
            output = self.fc(h)

            # Store the output and attention weights
            outputs[:, i, :] = output
            atten_weights[:, i, :] = attention

        return outputs, atten_weights

    def init_hidden(self, features):
        """
        Initialize the hidden and cell states of the LSTM using the mean of the image features.
        
        Args:
            features (Tensor): Extracted image features from the encoder.
        
        Returns:
            h (Tensor): Initialized hidden state.
            c (Tensor): Initialized cell state.
        """
        # Compute the mean of the features across the spatial dimensions
        features = torch.mean(features, dim=1)

        # Initialize the hidden and cell states using linear layers
        h = self.init_h(features)
        c = self.init_c(features)

        return h, c

class EncoderDecoderNet(nn.Module):
    def __init__(self, features_size, embed_size, hidden_size, vocabulary, encoder_checkpoint=None):
        """
        Initialize the EncoderDecoderNet class, combining the EncoderCNN and DecoderRNN.
        
        Args:
            features_size (int): Size of the input features from the encoder.
            embed_size (int): Size of the word embeddings.
            hidden_size (int): Size of the hidden state in the LSTM.
            vocabulary (Vocabulary): Vocabulary object containing the word-to-index mapping.
            encoder_checkpoint (str): Path to a model checkpoint to load pre-trained encoder weights.
        """
        super(EncoderDecoderNet, self).__init__()

        self.vocabulary = vocabulary  # Store the vocabulary for later use in caption generation

        # Initialize the encoder (CNN) with an optional checkpoint for pre-trained weights
        self.encoder = EncoderCNN(
            checkpoint=encoder_checkpoint
        )
        # Initialize the decoder (RNN) with the specified parameters
        self.decoder = DecoderRNN(
            features_size=features_size,
            embed_size=embed_size,
            hidden_size=hidden_size,
            vocab_size=len(self.vocabulary)  # Vocabulary size for the final classification layer
        )

    def forward(self, images, captions):
        """
        Forward pass through the EncoderDecoderNet to generate captions.
        
        Args:
            images (Tensor): Input tensor (images).
            captions (Tensor): Ground truth captions as input for the decoder.
        
        Returns:
            outputs (Tensor): Predicted word scores for each time step.
        """
        # Pass the images through the encoder to extract features
        features = self.encoder(images)
        # Pass the features and captions through the decoder to generate word predictions
        outputs, _ = self.decoder(features, captions)

        return outputs

    def generate_caption(self, image, max_length=25):
        """
        Generate a caption for a given image using the trained model.
        
        Args:
            image (Tensor): Input tensor (image).
            max_length (int): Maximum length of the generated caption.
        
        Returns:
            List[str]: The generated caption as a list of words.
        """
        caption = []  # List to store the generated words

        # Disable gradient calculation for inference
        with torch.no_grad():
            # Pass the image through the encoder to extract features
            features = self.encoder(image)
            # Initialize the hidden and cell states of the LSTM
            h, c = self.decoder.init_hidden(features)

            # Start the caption with the <SOS> (start of sequence) token
            word = torch.tensor(self.vocabulary.stoi['<SOS>']).view(1, -1).to(config.DEVICE)
            embeddings = self.decoder.embedding(word).squeeze(0)

            # Generate words until the maximum length is reached or <EOS> (end of sequence) is generated
            for _ in range(max_length):
                # Compute the context vector and attention weights
                context, _ = self.decoder.attention(features, h)

                # Concatenate the embedding of the current word with the context vector
                inputs = torch.cat((embeddings, context), dim=1)

                # Pass the inputs through the LSTM to update the hidden and cell states
                h, c  = self.decoder.lstm(inputs, (h, c))

                # Generate the output (predicted word scores) from the hidden state
                output = self.decoder.fc(F.dropout(h, p=0.5))
                output = output.view(1, -1)

                # Get the index of the word with the highest score
                predicted = output.argmax(1)

                # Break the loop if the <EOS> token is generated
                if self.vocabulary.itos[predicted.item()] == '<EOS>':
                    break

                # Append the generated word to the caption list
                caption.append(predicted.item())

                # Get the embedding of the predicted word for the next iteration
                embeddings = self.decoder.embedding(predicted)

        # Convert the list of indices to the corresponding words and return the caption
        return [self.vocabulary.itos[idx] for idx in caption]
