import os
import spacy  # Importing the spaCy library for natural language processing
import torch  # PyTorch for building and training the model
import config  # Importing a custom configuration file (assumed to be in the same directory)
import numpy as np  # NumPy for numerical operations
from PIL import Image  # Python Imaging Library for image processing
from torch.nn.utils.rnn import pad_sequence  # Utility function to pad sequences to the same length
from torch.utils.data import Dataset, DataLoader  # PyTorch classes for managing datasets and loading data
from utils import normalize_text  # Utility function to normalize text (assumed to handle text preprocessing)
from text_utils import normalize_text  # Ensuring correct import of text normalization (might be redundant)

# Load the small English language model from spaCy, used for tokenization
spacy_eng = spacy.load('en_core_web_sm')

class Vocabulary:
    def __init__(self, freq_threshold):
        """
        Initialize the Vocabulary class.
        This class creates a mapping between words and indices and vice versa.
        It also includes special tokens for padding, start of sentence, end of sentence, and unknown words.
        
        Args:
            freq_threshold (int): The minimum frequency a word must have to be included in the vocabulary.
        """
        # Mapping from index to string (itos) and string to index (stoi)
        self.itos = { 
            0: '<PAD>',  # Padding token
            1: '<SOS>',  # Start of sentence token
            2: '<EOS>',  # End of sentence token
            3: '<UNK>',  # Unknown word token
        }
        self.stoi = { 
            '<PAD>': 0,
            '<SOS>': 1,
            '<EOS>': 2,
            '<UNK>': 3,
        }
        self.freq_threshold = freq_threshold  # Minimum frequency for a word to be added to the vocabulary

    @staticmethod
    def tokenizer(text):
        """
        Tokenize a given text using spaCy.
        This breaks down the text into individual words (tokens) and converts them to lowercase.
        
        Args:
            text (str): The input text to be tokenized.
        
        Returns:
            List[str]: A list of tokens (words) in the text.
        """
        return [tok.text.lower() for tok in spacy_eng.tokenizer(text)]

    def build_vocabulary(self, sentence_list):
        """
        Build a vocabulary from a list of sentences.
        Only words that appear more than 'freq_threshold' times are added to the vocabulary.
        
        Args:
            sentence_list (List[str]): A list of sentences from which to build the vocabulary.
        """
        frequencies = {}  # Dictionary to store word frequencies
        idx = 4  # Starting index for new words (since first 4 indices are reserved for special tokens)

        # Loop over all sentences in the list
        for sent in sentence_list:
            # Tokenize each sentence into words
            for word in self.tokenizer(sent):
                if word not in frequencies:
                    frequencies[word] = 1  # If word is new, add it with frequency 1
                else:
                    frequencies[word] += 1  # Otherwise, increment its frequency

                # If the word frequency reaches the threshold, add it to the vocabulary
                if frequencies[word] == self.freq_threshold:
                    self.stoi[word] = idx  # Add to string-to-index dictionary
                    self.itos[idx] = word  # Add to index-to-string dictionary
                    idx += 1  # Increment the index for the next word

    def numericalize(self, text):
        """
        Convert a text into a list of numerical indices based on the vocabulary.
        
        Args:
            text (str): The input text to be converted into numerical indices.
        
        Returns:
            List[int]: A list of indices corresponding to the words in the input text.
        """
        tokenized_text = self.tokenizer(text)  # Tokenize the input text

        # Convert each token to its corresponding index or to <UNK> if it's not in the vocabulary
        return [
            self.stoi[token] if token in self.stoi else self.stoi['<UNK>']
            for token in tokenized_text
        ]

    def __len__(self):
        """
        Return the number of words in the vocabulary, including special tokens.
        
        Returns:
            int: The size of the vocabulary.
        """
        return len(self.itos)  # The length of the itos dictionary gives the vocabulary size

class XRayDataset(Dataset):
    def __init__(self, root, transform=None, freq_threshold=3, raw_caption=False):
        """
        Initialize the dataset for X-ray images and their corresponding reports.
        This class is responsible for loading the images and reports, applying transformations, 
        and converting reports to numerical format using the Vocabulary class.
        
        Args:
            root (str): The root directory where images and reports are stored.
            transform (callable, optional): A function/transform to apply to the images.
            freq_threshold (int): The minimum frequency a word must have to be included in the vocabulary.
            raw_caption (bool): Whether to return the raw caption (text) or the numericalized version.
        """
        self.root = root  # Root directory containing images and reports
        self.transform = transform  # Optional transformation to be applied to images
        self.raw_caption = raw_caption  # Flag to return raw caption or numericalized caption

        self.vocab = Vocabulary(freq_threshold=freq_threshold)  # Initialize vocabulary with the given threshold

        self.captions = []  # List to store all captions (report findings)
        self.imgs = []  # List to store image file paths

        # Iterate over all report files in the 'reports' directory
        for file in os.listdir(os.path.join(self.root, 'reports')):
            if file.endswith('.txt'):  # Process only text files
                img_name = file.replace('.txt', '.png')  # Replace '.txt' with '.png' to find corresponding image
                img_path = os.path.join(self.root, 'images', img_name)
                
                if not os.path.exists(img_path):  # Skip if image doesn't exist
                    continue
                
                # Read the findings from the report file
                with open(os.path.join(self.root, 'reports', file), 'r') as f:
                    findings = f.read().strip()  # Remove any leading/trailing whitespace
                
                if not findings:  # Skip if report is empty
                    continue

                # Store the findings (caption) and corresponding image path
                self.captions.append(findings)  # Add findings to the list of captions
                self.imgs.append(img_path)  # Add image path to the list of images

        # Build the vocabulary from the collected captions
        self.vocab.build_vocabulary(self.captions)

    def __getitem__(self, item):
        """
        Get an image and its corresponding caption by index.
        
        Args:
            item (int): The index of the item to retrieve.
        
        Returns:
            Tuple: A tuple containing the image and its corresponding caption (either raw or numericalized).
        """
        img = self.imgs[item]  # Get the image path at the specified index
        caption = normalize_text(self.captions[item])  # Normalize the caption using the normalize_text function

        # Open the image, convert it to grayscale, and then convert it to RGB by repeating the single channel
        img = np.array(Image.open(img).convert('L'))  # Convert to grayscale
        img = np.expand_dims(img, axis=-1)  # Expand dimensions to add a channel axis
        img = img.repeat(3, axis=-1)  # Repeat the single channel to create a 3-channel image (like RGB)

        if self.transform is not None:
            img = self.transform(image=img)['image']  # Apply any provided transformations

        if self.raw_caption:
            return img, caption  # Return raw caption if specified
        
        # Convert the caption to numerical indices using the vocabulary
        numericalized_caption = [self.vocab.stoi['<SOS>']]  # Start with the <SOS> token
        numericalized_caption += self.vocab.numericalize(caption)  # Add the numericalized words
        numericalized_caption.append(self.vocab.stoi['<EOS>'])  # End with the <EOS> token

        # Return the image and the numericalized caption as a PyTorch tensor
        return img, torch.as_tensor(numericalized_caption, dtype=torch.long)

    def __len__(self):
        """
        Return the number of samples in the dataset.
        
        Returns:
            int: The total number of captions (and thus images) in the dataset.
        """
        return len(self.captions)  # The length of the captions list gives the number of samples

    def get_caption(self, item):
        """
        Get the caption as a list of words for a given index.
        
        Args:
            item (int): The index of the caption to retrieve.
        
        Returns:
            List[str]: The caption split into a list of words.
        """
        return self.captions[item].split(' ')  # Split the caption into words by spaces

class CollateDataset:
    def __init__(self, pad_idx):
        """
        Initialize the collate function with a padding index.
        This class is used to combine a batch of images and captions, padding the captions so they are the same length.
        
        Args:
            pad_idx (int): The index used for padding sequences.
        """
        self.pad_idx = pad_idx  # Padding index, used to pad sequences to the same length

    def __call__(self, batch):
        """
        Collate function to combine a batch of images and captions.
        It pads all captions to ensure they have the same length within a batch.
        
        Args:
            batch (List[Tuple]): A batch of data, where each item is a tuple (image, caption).
        
        Returns:
            Tuple[torch.Tensor, torch.Tensor]: A tuple containing a tensor of images and a tensor of padded captions.
        """
        images, captions = zip(*batch)  # Unzip images and captions from the batch

        images = torch.stack(images, 0)  # Stack images along a new dimension to create a batch tensor
        
        # Pad captions to ensure they all have the same length in the batch
        targets = [item for item in captions]  # Convert captions to a list of tensors
        targets = pad_sequence(targets, batch_first=True, padding_value=self.pad_idx)  # Pad the sequences

        return images, targets  # Return the batched images and padded captions
