%pip install pytesseract
import pydicom
from PIL import Image
import numpy as np
import cv2  # Add this import for OpenCV
import pytesseract

def dicom_to_bitmap(dicom_path):
    # Load DICOM image
    ds = pydicom.dcmread(dicom_path)
    
    # Convert DICOM to bitmap
    bitmap = ds.pixel_array
    
    # Convert to grayscale
    bitmap = np.uint8(bitmap)  # Convert to 8-bit unsigned integer (required by PIL)
    image = Image.fromarray(bitmap)
    gray = image.convert('L')  # Convert to grayscale
    
    # Thresholding
    _, thresh = cv2.threshold(np.array(gray), 150, 255, cv2.THRESH_BINARY_INV)
    
    # OCR to detect text regions
    text_boxes = pytesseract.image_to_boxes(thresh)
    
    # Replace text regions with black pixels
    for box in text_boxes.splitlines():
        box = box.split()
        x, y, w, h = map(int, box[1:5])
        cv2.rectangle(bitmap, (x, ds.Rows - y), (w, ds.Rows - h), (0, 0, 0), -1)
    
    return bitmap

def save_bitmap(bitmap, output_path):
    # Convert bitmap array to PIL Image
    image = Image.fromarray(bitmap)
    
    # Convert image mode to 'L' (grayscale)
    image = image.convert('L')
    
    # Save as bitmap
    image.save(output_path)


bitmap = dicom_to_bitmap(r'C:\Users\Mariam\Desktop\mmm.dcm')
save_bitmap(bitmap, r'C:\Users\Mariam\Desktop\converted_image.bmp')

