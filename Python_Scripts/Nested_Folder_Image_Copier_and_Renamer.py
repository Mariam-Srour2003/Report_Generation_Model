import os
import shutil

def rename_and_save_images(input_dir, output_dir):
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
    
    for main_folder in os.listdir(input_dir):
        main_folder_path = os.path.join(input_dir, main_folder)
        if os.path.isdir(main_folder_path):
            for sub_folder in os.listdir(main_folder_path):
                sub_folder_path = os.path.join(main_folder_path, sub_folder)
                if os.path.isdir(sub_folder_path):
                    for inner_folder in os.listdir(sub_folder_path):
                        inner_folder_path = os.path.join(sub_folder_path, inner_folder)
                        if os.path.isdir(inner_folder_path):
                            for file in os.listdir(inner_folder_path):
                                if file.endswith('.jpeg') or file.endswith('.jpg'):
                                    old_image_path = os.path.join(inner_folder_path, file)
                                    new_image_name = f"{main_folder}.jpg"
                                    new_image_path = os.path.join(output_dir, new_image_name)
                                    shutil.copy(old_image_path, new_image_path)
                                    print(f"Copied {old_image_path} to {new_image_path}")

# Example usage
input_directory = r'C:\Users\Mariam\Desktop\pic'
output_directory = r'C:\Users\Mariam\Desktop\p'
rename_and_save_images(input_directory, output_directory)
