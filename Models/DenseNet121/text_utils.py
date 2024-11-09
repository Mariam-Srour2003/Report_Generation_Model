import re  # Importing the regular expressions module for pattern matching and text substitution
import html  # Importing the html module to handle HTML escape characters
import string  # Importing the string module to access string constants like punctuation
import unicodedata  # Importing the unicodedata module for handling Unicode characters
from nltk.tokenize import word_tokenize  # Importing word_tokenize from NLTK for tokenizing text into words

def remove_special_chars(text):
    """
    Remove special characters and normalize text by replacing common HTML entities and other special characters.
    
    Args:
        text (str): The input text string to be cleaned.
        
    Returns:
        str: The cleaned text string with special characters replaced or removed.
    """
    re1 = re.compile(r'  +')  # Compile a regex pattern to match multiple spaces
    # Replace specific HTML entities and other patterns with their corresponding characters
    x1 = text.lower().replace('#39;', "'").replace('amp;', '&').replace('#146;', "'").replace(
        'nbsp;', ' ').replace('#36;', '$').replace('\\n', "\n").replace('quot;', "'").replace(
        '<br />', "\n").replace('\\"', '"').replace('<unk>', 'u_n').replace(' @.@ ', '.').replace(
        ' @-@ ', '-').replace('\\', ' \\ ')
    # Remove multiple spaces and return the cleaned text
    return re1.sub(' ', html.unescape(x1))

def remove_non_ascii(text):
    """
    Remove non-ASCII characters from the text by normalizing to ASCII equivalent.
    
    Args:
        text (str): The input text string containing potential non-ASCII characters.
        
    Returns:
        str: The text string with all non-ASCII characters removed.
    """
    # Normalize the text to ASCII using NFKD normalization and encode/decode to remove non-ASCII characters
    return unicodedata.normalize('NFKD', text).encode('ascii', 'ignore').decode('utf-8', 'ignore')

def to_lowercase(text):
    """
    Convert all characters in the text to lowercase.
    
    Args:
        text (str): The input text string to be converted.
        
    Returns:
        str: The text string in lowercase.
    """
    return text.lower()

def remove_punctuation(text):
    """
    Remove all punctuation characters from the text.
    
    Args:
        text (str): The input text string that may contain punctuation.
        
    Returns:
        str: The text string with all punctuation removed.
    """
    # Create a translation table that maps each punctuation character to None
    translator = str.maketrans('', '', string.punctuation)
    # Translate the text using the translation table to remove punctuation
    return text.translate(translator)

def replace_numbers(text):
    """
    Replace all numerical digits in the text with an empty string (i.e., remove them).
    
    Args:
        text (str): The input text string that may contain numbers.
        
    Returns:
        str: The text string with all numbers removed.
    """
    # Substitute all sequences of digits with an empty string
    return re.sub(r'\d+', '', text)

def text2words(text):
    """
    Tokenize the text into individual words using NLTK's word_tokenize function.
    
    Args:
        text (str): The input text string to be tokenized.
        
    Returns:
        list: A list of words (tokens) extracted from the text.
    """
    return word_tokenize(text)

def normalize_text(text):
    """
    Normalize the input text by applying a series of cleaning steps, including:
    - Removing special characters
    - Removing non-ASCII characters
    - Removing punctuation
    - Converting to lowercase
    - Removing numbers
    
    Args:
        text (str): The input text string to be normalized.
        
    Returns:
        str: The fully normalized text string.
    """
    text = remove_special_chars(text)  # Step 1: Clean special characters
    text = remove_non_ascii(text)  # Step 2: Remove non-ASCII characters
    text = remove_punctuation(text)  # Step 3: Remove punctuation
    text = to_lowercase(text)  # Step 4: Convert to lowercase
    text = replace_numbers(text)  # Step 5: Remove numbers
    return text

def normalize_corpus(corpus):
    """
    Normalize a list of text documents (corpus) by applying the normalize_text function to each document.
    
    Args:
        corpus (list of str): A list of text strings to be normalized.
        
    Returns:
        list of str: A list of normalized text strings.
    """
    return [normalize_text(t) for t in corpus]  # Apply normalize_text to each document in the corpus
