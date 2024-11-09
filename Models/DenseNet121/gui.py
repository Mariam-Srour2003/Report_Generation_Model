# gui.py
import os
import re
import html
import string
import torch
import config
import unicodedata
from nltk.tokenize import word_tokenize
from PIL import Image, ImageTk
from tkinter import GROOVE, LEFT, TOP, filedialog, Toplevel, Frame, Label, Button, Tk, X, BOTH
import numpy as np

from dataset import XRayDataset, Vocabulary
from model import EncoderDecoderNet
from torch.utils.data import Subset
from sklearn.model_selection import train_test_split as sklearn_train_test_split

def load_dataset(raw_caption=False):
    return XRayDataset(
        root=config.DATASET_PATH,
        transform=config.basic_transforms,
        freq_threshold=config.VOCAB_THRESHOLD,
        raw_caption=raw_caption
    )

def get_model_instance(vocabulary):
    model = EncoderDecoderNet(
        features_size=config.FEATURES_SIZE,
        embed_size=config.EMBED_SIZE,
        hidden_size=config.HIDDEN_SIZE,
        vocabulary=vocabulary,
        encoder_checkpoint='./weights/chexnet.pth.tar'
    )
    model = model.to(config.DEVICE)
    return model

def train_test_split(dataset, test_size=0.25, random_state=44):
    train_idx, test_idx = sklearn_train_test_split(
        list(range(len(dataset))),
        test_size=test_size,
        random_state=random_state
    )
    return Subset(dataset, train_idx), Subset(dataset, test_idx)

def save_checkpoint(checkpoint):
    print('=> Saving checkpoint')
    torch.save(checkpoint, config.CHECKPOINT_FILE)

def load_checkpoint(model, optimizer=None):
    print('=> Loading checkpoint')
    checkpoint = torch.load(config.CHECKPOINT_FILE, map_location=torch.device('cpu'))
    model.load_state_dict(checkpoint['state_dict'])
    if optimizer is not None:
        optimizer.load_state_dict(checkpoint['optimizer'])
    return checkpoint['epoch']

def can_load_checkpoint():
    return os.path.exists(config.CHECKPOINT_FILE) and config.LOAD_MODEL

def remove_special_chars(text):
    re1 = re.compile(r'  +')
    x1 = text.lower().replace('#39;', "'").replace('amp;', '&').replace('#146;', "'").replace(
        'nbsp;', ' ').replace('#36;', '$').replace('\\n', "\n").replace('quot;', "'").replace(
        '<br />', "\n").replace('\\"', '"').replace('<unk>', 'u_n').replace(' @.@ ', '.').replace(
        ' @-@ ', '-').replace('\\', ' \\ ')
    return re1.sub(' ', html.unescape(x1))

def remove_non_ascii(text):
    return unicodedata.normalize('NFKD', text).encode('ascii', 'ignore').decode('utf-8', 'ignore')

def to_lowercase(text):
    return text.lower()

def remove_punctuation(text):
    translator = str.maketrans('', '', string.punctuation)
    return text.translate(translator)

def replace_numbers(text):
    return re.sub(r'\d+', '', text)

def text2words(text):
    return word_tokenize(text)

def normalize_text(text):
    text = remove_special_chars(text)
    text = remove_non_ascii(text)
    text = remove_punctuation(text)
    text = to_lowercase(text)
    text = replace_numbers(text)
    return text

def normalize_corpus(corpus):
    return [normalize_text(t) for t in corpus]

def choose_image():
    global label, image, photo
    path = filedialog.askopenfilename(initialdir='images', title='Select Photo')

    screen = Toplevel(root)
    screen.title('Report Generator')
    screen.geometry('800x600')

    ff1 = Frame(screen, bg='#2c3e50', borderwidth=6, relief=GROOVE)
    ff1.pack(side=TOP, fill=X)

    ff2 = Frame(screen, bg='#ecf0f1', borderwidth=6, relief=GROOVE)
    ff2.pack(side=TOP, fill=BOTH, expand=True, padx=10, pady=10)

    ff3 = Frame(screen, bg='#2c3e50', borderwidth=6, relief=GROOVE)
    ff3.pack(side=TOP, fill=X)

    original_img = Image.open(path).convert('L')

    image = np.array(original_img)
    image = np.expand_dims(image, axis=-1)
    image = image.repeat(3, axis=-1)

    image = config.basic_transforms(image=image)['image']

    photo = ImageTk.PhotoImage(original_img)

    image_label = Label(ff2, image=photo)
    image_label.pack(side=TOP, pady=10)

    label = Label(ff2, text='', fg='#2980b9', bg='#ecf0f1', font='Helvetica 16 bold', wraplength=700, justify=LEFT)
    label.pack(side=TOP, pady=10)

    Button(ff3, text='Quit', bg='#e74c3c', fg='white', command=quit_gui, height=2, width=20, font='Helvetica 16 bold').pack(pady=10)

    screen.bind('<Configure>', lambda event: label.configure(wraplength=label.winfo_width()))

    generate_report()
    screen.mainloop()

def generate_report():
    global label, image, model

    model.eval()

    image = image.to(config.DEVICE)

    report = model.generate_caption(image.unsqueeze(0), max_length=25)
    report_text = ' '.join(report)

    label.config(text=report_text, fg='#2980b9', bg='#ecf0f1', font='Helvetica 16 bold', width=40)
    label.update_idletasks()

    print("Generated Report: ", report_text)

def quit_gui():
    root.destroy()

root = Tk()
root.title('Chest X-Ray Report Generator')
root.geometry('400x300')

f1 = Frame(root, bg='#2c3e50', borderwidth=6, relief=GROOVE)
f1.pack(side=TOP, fill=X, padx=10, pady=10)

f2 = Frame(root, bg='#2c3e50', borderwidth=6, relief=GROOVE)
f2.pack(side=TOP, fill=X, padx=10, pady=10)

btn1 = Button(root, text='Choose Chest X-Ray', command=choose_image, height=2, width=20, bg='#3498db', fg='white', font="Helvetica 16 bold", pady=10)
btn1.pack(pady=20)

Button(root, text='Quit', command=quit_gui, height=2, width=20, bg='#e74c3c', fg='white', font='Helvetica 16 bold', pady=10).pack(pady=20)

if __name__ == '__main__':
    dataset = load_dataset()
    model = get_model_instance(dataset.vocab)
    
    load_checkpoint(model)

    root.mainloop()
