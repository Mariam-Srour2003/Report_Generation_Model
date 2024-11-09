import albumentations as A
import torch
from albumentations.pytorch import ToTensorV2

# Path to the model checkpoint file used for saving and loading model weights
CHECKPOINT_FILE = './checkpoints/x_ray_model.pth.tar'

# Path to the dataset directory containing images and reports    /reports for  reports
DATASET_PATH = './dataset'
IMAGES_DATASET = './dataset/images'  # Directory where the images are stored


# THIS  WAS ADDED JUST  IN CASE TRIED TO RUN CODE  ON THE COMPUTER WITH GPU
# Set the device to use for computation, either GPU (CUDA) if available, or CPU
DEVICE = 'cuda' if torch.cuda.is_available() else 'cpu'


# Number of samples per batch during training
BATCH_SIZE = 16

#THE FOLLOWING WORKD ONLY ON CUDA (GPU) SO SETTING IT TRUE OR FALSE IS SAME FOR CPU
# Whether to use pinned memory for faster data transfer to GPU
PIN_MEMORY = False

# Threshold for the minimum frequency of words in the vocabulary 
# ONLY WORDS THAT APPEARS AT LEAST TWICE IN THE DATA ARE BEING CONSIDERED 
# ---> Reduce Vocabulary Size: Reduce noise or complexity
# ---> Improve Efficiency: Allows for faster training model doesn't have to process as many unique words.
VOCAB_THRESHOLD = 2

# IT IS BASED ON THE MODEL USED 
# Dimensionality of the feature vector extracted from images
# model extract features from the X-ray images outputs a 1024-dimensional vector for each image
FEATURES_SIZE = 1024

# Dimensionality of the word embedding vectors 300 is a common choice
EMBED_SIZE = 300

# Dimensionality of the hidden state in the model's recurrent layer (LSTM/GRU)
# THIS IS TOO MUCH ON A CPU WHERE IT SHOULD BE 128 OR 64 BUT STILL IT LEARNED ON 256
HIDDEN_SIZE = 256

# Learning rate for the optimizer, controls the step size during gradient descent
LEARNING_RATE = 4e-5

# Number of times to iterate over the entire dataset during training
# 50 epochs were chosen to allow the model to continue improving as long as the loss decreases steadily.
# If the loss continues to decrease steadily, continuing to 50 epochs could help the model converge more effectively.
EPOCHS = 50

# Flags to control whether the model should load a pre-trained checkpoint or save its state after training
LOAD_MODEL = True
SAVE_MODEL = True

# Define a set of basic image transformations to apply to the dataset
basic_transforms = A.Compose([
    A.Resize(
        height=256,  # Resize the image to 256 pixels in height
        width=256    # Resize the image to 256 pixels in width
    ),
    A.Normalize(
        mean=(0.485, 0.456, 0.406),  # Normalize the image with the mean values for each channel (RGB)
        std=(0.229, 0.224, 0.225),   # Normalize the image with the standard deviation values for each channel (RGB)
    ),
    ToTensorV2()  # Convert the image from a NumPy array to a PyTorch tensor
])


# Note: The hyperparameters here including BATCH_SIZE, VOCAB_THRESHOLD, FEATURES_SIZE, 
# EMBED_SIZE, HIDDEN_SIZE, LEARNING_RATE, and EPOCHS, are crucial for model performance. It's 
# advisable to experiment with different values for these parameters to optimize results, 
# although they have not been adjusted in this instance due to time and resources limitations.