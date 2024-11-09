import config  # Importing configuration settings (e.g., DEVICE, BATCH_SIZE, LEARNING_RATE)
import utils  # Importing utility functions for dataset loading, model creation, checkpointing, etc.
import torch.nn as nn  # Importing PyTorch's neural network module for loss functions and layers
import torch.optim as optim  # Importing PyTorch's optimization algorithms
import numpy as np  # Importing NumPy for numerical operations
from tqdm import tqdm  # Importing tqdm for displaying progress bars during training
from torch.utils.data import DataLoader  # Importing DataLoader for batching and loading data
from dataset import CollateDataset  # Importing custom collate function to handle padding in batches
import os  # Importing os for file and directory management

# Create the 'checkpoints' directory if it doesn't exist, to store model checkpoints during training
if not os.path.exists('./checkpoints'):
    os.makedirs('./checkpoints')

def train_epoch(loader, model, optimizer, loss_fn, epoch):
    """
    Train the model for one epoch.
    
    Args:
        loader (DataLoader): The data loader for the training dataset.
        model (nn.Module): The model to be trained.
        optimizer (torch.optim.Optimizer): The optimizer used for updating the model's weights.
        loss_fn (nn.Module): The loss function to compute the training loss.
        epoch (int): The current epoch number.
    """
    model.train()  # Set the model to training mode (enables dropout, batch norm, etc.)
    losses = []  # List to keep track of losses for each batch
    loader = tqdm(loader)  # Wrap the data loader with tqdm to show a progress bar

    for img, captions in loader:
        # Move images and captions to the specified device (CPU/GPU)
        img = img.to(config.DEVICE)
        captions = captions.to(config.DEVICE)

        # Forward pass: Compute model output for the images and captions
        output = model(img, captions)
        
        # Compute the loss between the predicted outputs and the target captions
        loss = loss_fn(
            output.reshape(-1, output.shape[2]),  # Reshape output to (batch_size * seq_len, vocab_size)
            captions[:, 1:].reshape(-1)  # Shift captions by one token (ignoring <SOS>) and flatten
        )

        optimizer.zero_grad()  # Zero the gradients from the previous step
        loss.backward()  # Backpropagation to compute gradients
        optimizer.step()  # Update model parameters based on gradients

        # Update the progress bar with the current loss
        loader.set_postfix(loss=loss.item())
        losses.append(loss.item())  # Append the loss for this batch to the list

    # Save a checkpoint after each epoch if the SAVE_MODEL flag is set
    if config.SAVE_MODEL:
        utils.save_checkpoint({
            'state_dict': model.state_dict(),  # Save the model's state dictionary
            'optimizer': optimizer.state_dict(),  # Save the optimizer's state dictionary
            'epoch': epoch,  # Save the current epoch
            'loss': np.mean(losses)  # Save the average loss for this epoch
        })

    # Print the average loss for this epoch
    print(f'Epoch[{epoch}]: Loss {np.mean(losses)}')


def main():
    """
    Main function to handle the training process.
    """
    all_dataset = utils.load_dataset()  # Load the entire dataset

    # Check if the dataset is empty and raise an error if it is
    if len(all_dataset) == 0:
        raise ValueError("The dataset is empty. Please check your dataset loading function.")

    # Split the dataset into training and testing sets (only training set is used here)
    train_dataset, _ = utils.train_test_split(dataset=all_dataset)

    # Create a DataLoader for the training dataset
    train_loader = DataLoader(
        dataset=train_dataset,
        batch_size=config.BATCH_SIZE,  # Set batch size from the configuration
        pin_memory=config.PIN_MEMORY,  # Pin memory to speed up data transfer to GPU
        drop_last=False,  # Do not drop the last incomplete batch
        shuffle=True,  # Shuffle the data at the beginning of each epoch
        collate_fn=CollateDataset(pad_idx=all_dataset.vocab.stoi['<PAD>']),  # Custom collate function for padding
    )

    # Get the model instance, initialized with the dataset's vocabulary
    model = utils.get_model_instance(all_dataset.vocab)
    # Set up the optimizer (Adam) with the model parameters and learning rate from the configuration
    optimizer = optim.Adam(model.parameters(), lr=config.LEARNING_RATE)
    # Set up the loss function (CrossEntropyLoss), ignoring the padding index
    loss_fn = nn.CrossEntropyLoss(ignore_index=all_dataset.vocab.stoi['<PAD>'])

    starting_epoch = 1  # Initialize the starting epoch

    # Check if a checkpoint exists and load it to resume training from the last saved state
    if utils.can_load_checkpoint():
        starting_epoch = utils.load_checkpoint(model, optimizer)

    # Loop over the range of epochs and train the model for each epoch
    for epoch in range(starting_epoch, config.EPOCHS):
        train_epoch(
            train_loader,
            model,
            optimizer,
            loss_fn,
            epoch
        )


if __name__ == '__main__':
    main()  # Run the main function if this script is executed directly
