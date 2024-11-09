import pandas as pd
import os

# Load the Excel file
file_path = r'C:\Users\Mariam\Desktop\USAL\XRay Reports USAL Final.xlsx'
df = pd.read_excel(file_path)

# Create a directory to save the text files if it doesn't exist
output_dir = 'English_Reports_USAL'
if not os.path.exists(output_dir):
    os.makedirs(output_dir)

# Iterate over each row and save the English report to a text file
for index, row in df.iterrows():
    accession_number = row['CODE']
    english_report = row['English Reports']
    
    # Convert the report to a string and handle NaN values
    if pd.isna(english_report):
        english_report = ""
    else:
        english_report = str(english_report)
    
    # Define the output file path
    output_file_path = os.path.join(output_dir, f'{accession_number}.txt')
    
    # Save the report to a text file
    with open(output_file_path, 'w', encoding='utf-8') as file:
        file.write(english_report)

print("Reports have been successfully saved.")
