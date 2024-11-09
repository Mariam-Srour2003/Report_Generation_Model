import os
from PIL import Image
import string

def convert_and_rename_images(input_dir, output_dir):
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    main_folders = [f for f in os.listdir(input_dir) if os.path.isdir(os.path.join(input_dir, f))]
    
    for main_folder in main_folders:
        main_folder_path = os.path.join(input_dir, main_folder)
        subfolders = [os.path.join(main_folder_path, sf) for sf in os.listdir(main_folder_path) if os.path.isdir(os.path.join(main_folder_path, sf))]
        
        suffix_index = 0
        
        for subfolder in subfolders:
            images = [img for img in os.listdir(subfolder) if img.lower().endswith('.jpg')]
            
            for img in images:
                img_path = os.path.join(subfolder, img)
                img_suffix = string.ascii_uppercase[suffix_index % 26]
                suffix_index += 1
                
                new_img_name = f"{main_folder}_{img_suffix}.png"
                new_img_path = os.path.join(output_dir, new_img_name)
                
                with Image.open(img_path) as image:
                    image.save(new_img_path, 'PNG')
                
                print(f"Converted and saved {img_path} as {new_img_path}")

input_dir = r'C:\Users\Mariam\Desktop\USAL - Copy'
output_dir = r'C:\Users\Mariam\Desktop\BahmanImagesPNG'
convert_and_rename_images(input_dir, output_dir)
