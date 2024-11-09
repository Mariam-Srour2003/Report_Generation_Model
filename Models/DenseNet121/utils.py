import os  # Importing the os module for file and directory management
import torch  # Importing PyTorch for model saving/loading and tensor operations
import config  # Importing configuration settings like file paths, model parameters, etc.
from text_utils import normalize_text  # Importing text normalization functions from text_utils.py
from dataset import XRayDataset  # Importing the custom X-ray dataset class
from model import EncoderDecoderNet  # Importing the model class that combines the encoder and decoder
from torch.utils.data import Subset  # Importing Subset to create subsets of a dataset
from sklearn.model_selection import train_test_split as sklearn_train_test_split  # Importing train-test split function

def load_dataset(raw_caption=False):
    """
    Load the X-ray dataset, applying transformations and building a vocabulary.
    
    Args:
        raw_caption (bool): If True, returns raw captions. If False, returns numericalized captions.
    
    Returns:
        XRayDataset: An instance of the XRayDataset class loaded with the specified parameters.
    """
    return XRayDataset(
        root=config.DATASET_PATH,  # Root directory of the dataset (images and captions)
        transform=config.basic_transforms,  # Transformations to be applied to the images
        freq_threshold=config.VOCAB_THRESHOLD,  # Frequency threshold for building the vocabulary
        raw_caption=raw_caption  # Whether to return raw text captions or numericalized captions
    )

def get_model_instance(vocabulary):
    """
    Initialize and return an instance of the EncoderDecoderNet model.
    
    Args:
        vocabulary (Vocabulary): The vocabulary object containing word-to-index mappings.
    
    Returns:
        nn.Module: The initialized model instance moved to the configured device (CPU/GPU).
    """
    # Instantiate the EncoderDecoderNet model with the given parameters and load the encoder checkpoint
    model = EncoderDecoderNet(
        features_size=config.FEATURES_SIZE,  # Size of the feature vectors output by the encoder
        embed_size=config.EMBED_SIZE,  # Size of the word embeddings in the decoder
        hidden_size=config.HIDDEN_SIZE,  # Size of the hidden state in the LSTM of the decoder
        vocabulary=vocabulary,  # Vocabulary object used for caption generation
        encoder_checkpoint='./weights/chexnet.pth.tar'  # Path to the encoder's pre-trained weights
    )
    model = model.to(config.DEVICE)  # Move the model to the configured device (CPU or GPU)

    return model

def train_test_split(dataset, test_size=0.25, random_state=44):
    """
    Split the dataset into training and testing subsets.
    
    Args:
        dataset (Dataset): The full dataset to be split.
        test_size (float): The proportion of the dataset to include in the test split.
        random_state (int): Random seed for reproducibility.
    
    Returns:
        Subset, Subset: The training and testing subsets.
    """
    # Use sklearn's train_test_split to get indices for training and testing subsets
    train_idx, test_idx = sklearn_train_test_split(
        list(range(len(dataset))),  # List of indices representing the entire dataset
        test_size=test_size,  # Fraction of the dataset to allocate for testing
        random_state=random_state  # Seed for random number generator to ensure reproducibility
    )

    # Return the training and testing subsets based on the calculated indices
    return Subset(dataset, train_idx), Subset(dataset, test_idx)

def save_checkpoint(checkpoint):
    """
    Save the current state of the model and optimizer to a checkpoint file.
    
    Args:
        checkpoint (dict): A dictionary containing the model state, optimizer state, epoch number, etc.
    """
    print('=> Saving checkpoint')
    # Save the checkpoint dictionary to the specified file path
    torch.save(checkpoint, config.CHECKPOINT_FILE)

def load_checkpoint(model, optimizer=None):
    """
    Load a saved checkpoint to resume training or evaluation.
    
    Args:
        model (nn.Module): The model instance to load the checkpoint into.
        optimizer (torch.optim.Optimizer, optional): The optimizer to load the state from the checkpoint.
    
    Returns:
        int: The epoch number from which training should resume.
    """
    print('=> Loading checkpoint')
    
    # Load the checkpoint from the specified file path (mapped to CPU for safety)
    checkpoint = torch.load(config.CHECKPOINT_FILE, map_location=torch.device('cpu'))
    model.load_state_dict(checkpoint['state_dict'])  # Load the model's state dictionary
    
    if optimizer is not None:
        optimizer.load_state_dict(checkpoint['optimizer'])  # Load the optimizer's state dictionary if provided
    
    return checkpoint['epoch']  # Return the epoch number from which to resume training

def can_load_checkpoint():
    """
    Check if a checkpoint file exists and whether loading the model is enabled.
    
    Returns:
        bool: True if a checkpoint exists and loading is enabled, False otherwise.
    """
    return os.path.exists(config.CHECKPOINT_FILE) and config.LOAD_MODEL  # Check if checkpoint file exists and loading is enabled
