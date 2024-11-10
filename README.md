

Final year project by Mariam Srour, Aya Haydous, and Hiba Rezek: an X-Ray report generator using machine learning models. We built a user-friendly interface with .NET and Angular, and collected data from Al-Zahraa and Bahman hospitals. The project involved data preprocessing, translation, and report generation.

name: Nested_Folder_Image_Copier_and_Renamer
This script traverses through a nested folder structure in the specified input_dir and copies JPEG images (.jpeg or .jpg) from the deepest subfolders to an output_dir. It renames each copied image based on the name of its main folder (first level of subfolders), overwriting any existing image with the same name. The function ensures the output directory exists before copying files.

name: Bahman_Hospital_Image_Converter_with_Unique_Renaming
This script processes image data from Bahman Hospital, where multiple .jpg images with the same name exist for the same report. It navigates through a nested folder structure in the specified input_dir, converts .jpg images to .png format, and saves them in an output_dir with unique names. Each image is renamed using its main folder name and an alphabetical suffix (A, B, C, etc.) to handle name duplicates. If there are more than 26 images, the suffix wraps around. It ensures the output directory exists before saving the files.

name: JPG_to_PNG_Converter_with_Original_File_Removal
This script processes all .jpg images in a specified directory, converts each image to .png format, and saves it with the same name but with a .png extension. After converting each image, the original .jpg file is deleted to avoid duplicates. It ensures the directory exists before performing these operations.

name: Excel_to_Text_File_Converter_for_X-Ray_Reports
This script reads an Excel file containing X-ray reports, extracts the accession numbers and corresponding English reports, and saves each report as a separate text file in a newly created directory called English_Reports_USAL. If the report is missing (NaN), it saves an empty text file. The filename for each report is based on the accession number. Finally, it prints a confirmation message once all reports are saved successfully.

name: Duplicated_Code_Finder
The find_replicated_codes function takes a multi-line string of codes as input, counts the occurrences of each code, and identifies those that appear more than once. It stores the counts in a dictionary and then prints each duplicated code along with the number of times it occurs. If no duplicates are found, the function outputs a message indicating that there are no replicated codes.

name: Folder_Name_Printer
The print_folder_names function takes a specified directory path as input and lists all the subdirectories within that path. It uses the os module to retrieve the items in the directory and checks each item to determine if it is a directory. If an item is a directory, its name is printed to the console. The function also includes error handling to catch and report any issues that arise during the process, such as an invalid path or permission errors.

name: Folder_Management_Tool
The manage_folders function manages folders in a specified directory by comparing existing folders to a user-defined list of folder names. It first identifies and deletes any folders in the directory that are not present in the provided list. Afterward, it checks for any folder names from the list that do not exist in the directory, printing those missing names. The get_folder_names function prompts the user to input folder names one at a time until they type 'done', returning the complete list for management. Overall, this tool helps maintain a directory by ensuring it only contains specified folders while reporting any discrepancies.
