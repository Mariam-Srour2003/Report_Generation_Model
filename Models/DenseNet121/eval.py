import config  # Importing configuration settings, such as device (CPU/GPU) and other parameters
import utils  # Utility functions for loading datasets, models, and checkpoints
import numpy as np  # NumPy for numerical operations
from tqdm import tqdm  # Tqdm for displaying progress bars during loops
from nltk.translate.bleu_score import sentence_bleu  # BLEU score from NLTK for evaluating generated text
from nltk.translate.meteor_score import meteor_score  # METEOR score from NLTK for evaluating generated text
from rouge import Rouge  # type: ignore # Import the ROUGE score module for evaluating generated text
from text_utils import normalize_text  # Text normalization function (ensures consistent text preprocessing)

def check_accuracy(dataset, model):
    """
    Evaluate the model's performance on the given dataset using BLEU, ROUGE, and METEOR metrics.
    
    Args:
        dataset (Dataset): The dataset on which to evaluate the model.
        model (nn.Module): The trained model to be evaluated.
    """
    print('=> Testing')

    model.eval()  # Set the model to evaluation mode (disables dropout, batch norm, etc.)

    # Lists to store individual scores for each metric
    bleu1_score = []
    bleu2_score = []
    bleu3_score = []
    bleu4_score = []
    rouge_scores = []
    meteor_scores = []

    rouge = Rouge()  # Initialize the ROUGE scoring object

    # Iterate over each image-caption pair in the dataset with a progress bar
    for image, caption in tqdm(dataset):
        image = image.to(config.DEVICE)  # Move the image tensor to the specified device (CPU/GPU)

        # Generate a caption for the image using the model, with a maximum length equal to the reference caption length
        generated = model.generate_caption(image.unsqueeze(0), max_length=len(caption.split(' ')))

        # Convert the generated and reference text to tokenized lists of words
        generated_text = generated  # Generated caption as a list of words
        reference_text = caption.split()  # Reference (true) caption split into words

        # Calculate BLEU scores (1-gram, 2-gram, 3-gram, and 4-gram precision)
        bleu1_score.append(
            sentence_bleu([reference_text], generated_text, weights=(1, 0, 0, 0))  # BLEU-1 (unigram precision)
        )

        bleu2_score.append(
            sentence_bleu([reference_text], generated_text, weights=(0.5, 0.5, 0, 0))  # BLEU-2 (bigram precision)
        )

        bleu3_score.append(
            sentence_bleu([reference_text], generated_text, weights=(0.33, 0.33, 0.33, 0))  # BLEU-3 (trigram precision)
        )

        bleu4_score.append(
            sentence_bleu([reference_text], generated_text, weights=(0.25, 0.25, 0.25, 0.25))  # BLEU-4 (4-gram precision)
        )

        # Calculate ROUGE scores (overlap measures) between the generated and reference captions
        rouge_score = rouge.get_scores(' '.join(generated_text), ' '.join(reference_text), avg=True)
        rouge_scores.append(rouge_score)  # Append the ROUGE score for this pair to the list

        # Calculate METEOR score (precision, recall, and synonymy) between the generated and reference captions
        meteor_scores.append(meteor_score([reference_text], generated_text))  # Append METEOR score to the list

    # Calculate and print the average BLEU scores for the entire dataset
    print(f'=> BLEU 1: {np.mean(bleu1_score)}')
    print(f'=> BLEU 2: {np.mean(bleu2_score)}')
    print(f'=> BLEU 3: {np.mean(bleu3_score)}')
    print(f'=> BLEU 4: {np.mean(bleu4_score)}')

    # Calculate and print the average ROUGE scores for the entire dataset
    avg_rouge = {
        'rouge-1': np.mean([score['rouge-1']['f'] for score in rouge_scores]),  # ROUGE-1 score (f-measure)
        'rouge-2': np.mean([score['rouge-2']['f'] for score in rouge_scores]),  # ROUGE-2 score (f-measure)
        'rouge-l': np.mean([score['rouge-l']['f'] for score in rouge_scores]),  # ROUGE-L score (f-measure)
    }
    print(f'=> ROUGE 1: {avg_rouge["rouge-1"]}')
    print(f'=> ROUGE 2: {avg_rouge["rouge-2"]}')
    print(f'=> ROUGE L: {avg_rouge["rouge-l"]}')

    # Calculate and print the average METEOR score for the entire dataset
    print(f'=> METEOR: {np.mean(meteor_scores)}')


def main():
    """
    Main function to load the dataset and model, and evaluate the model on the test set.
    """
    all_dataset = utils.load_dataset(raw_caption=True)  # Load the entire dataset with raw captions

    model = utils.get_model_instance(all_dataset.vocab)  # Get the model instance with the vocabulary from the dataset

    utils.load_checkpoint(model)  # Load the model weights from a saved checkpoint

    _, test_dataset = utils.train_test_split(dataset=all_dataset)  # Split the dataset into training and test sets

    # Evaluate the model's performance on the test dataset
    check_accuracy(
        test_dataset,
        model
    )


if __name__ == '__main__':
    main()  # Run the main function if this script is executed
