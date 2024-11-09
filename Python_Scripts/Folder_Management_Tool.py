import os
import shutil

def manage_folders(directory_path, folder_names):
    # Convert folder names to a set for faster lookup
    folder_names_set = set(folder_names)
    
    # List of folders to be deleted
    to_delete = []
    
    # List of folder names that do not exist in the directory
    not_found = []

    # Iterate through the folders in the directory
    for folder in os.listdir(directory_path):
        folder_path = os.path.join(directory_path, folder)
        if os.path.isdir(folder_path):
            if folder not in folder_names_set:
                to_delete.append(folder_path)
    
    # Delete the folders that are not in the list
    for folder_path in to_delete:
        shutil.rmtree(folder_path)
        print(f"Deleted folder: {folder_path}")
    
    # Check for folder names in the list that do not exist in the directory
    for folder_name in folder_names:
        folder_path = os.path.join(directory_path, folder_name)
        if not os.path.exists(folder_path):
            not_found.append(folder_name)
    
    # Print the folder names that do not exist in the directory
    if not_found:
        print("Folders not found in the directory:")
        for folder_name in not_found:
            print(folder_name)
    else:
        print("All folder names from the list exist in the directory.")

def get_folder_names():
    print("Enter folder names, one per line. Type 'done' when finished:")
    folder_names = []
    while True:
        folder_name = input()
        if folder_name.lower() == 'done':
            break
        folder_names.append(folder_name)
    return folder_names

# Example usage
directory_path = input("Enter the path to your directory: ")
folder_names = get_folder_names()

manage_folders(directory_path, folder_names)
