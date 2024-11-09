import os
from PIL import Image

def convert_jpg_to_png(directory):
    # Ensure the provided directory exists
    if not os.path.isdir(directory):
        print(f"The directory {directory} does not exist.")
        return

    # Iterate over each file in the directory
    for filename in os.listdir(directory):
        # Check if the file is a JPG image
        if filename.lower().endswith('.jpg'):
            # Define the full path for the JPG image
            jpg_path = os.path.join(directory, filename)
            
            # Open the JPG image
            with Image.open(jpg_path) as img:
                # Define the new PNG filename
                png_filename = filename.rsplit('.', 1)[0] + '.png'
                png_path = os.path.join(directory, png_filename)
                
                # Save the image as PNG
                img.save(png_path, 'PNG')
                print(f"Converted {filename} to {png_filename}")

            # Remove the original JPG image
            os.remove(jpg_path)
            print(f"Removed the original file {filename}")

# Example usage
directory_path = r'C:\Users\Mariam\Desktop\chestxray\Chest-X-Ray-Report-Generator\dataset\images'
convert_jpg_to_png(directory_path)
