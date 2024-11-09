import os

def print_folder_names(path):
    try:
        # List all items in the given path
        items = os.listdir(path)
        
        # Iterate over the list of items
        for item in items:
            # Construct the full path of the item
            full_path = os.path.join(path, item)
            
            # Check if the item is a directory
            if os.path.isdir(full_path):
                print(item)
    except Exception as e:
        print(f"An error occurred: {e}")

# Specify the path here
path_to_check = r'C:\Users\Mariam\Desktop\hiba data\xray'

# Call the function with the specified path
print_folder_names(path_to_check)
