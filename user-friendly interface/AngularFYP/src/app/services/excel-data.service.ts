import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';

@Injectable({
  providedIn: 'root'
})
export class ExcelDataService {

  constructor() { }

  public readExcel(filePath: string): Promise<any[]> {
    return new Promise((resolve, reject) => {
      // Fetch the file as a blob
      fetch(filePath).then(response => {
        if (!response.ok) {
          throw new Error(`Failed to fetch file: ${response.statusText}`);
        }
        return response.blob();
      }).then(blob => {
        const reader = new FileReader();
        
        reader.onload = (e: any) => {
          try {
            const data = new Uint8Array(e.target.result);
            const workbook = XLSX.read(data, { type: 'array' });
            const firstSheetName = workbook.SheetNames[0];
            const worksheet = workbook.Sheets[firstSheetName];
            const jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
            resolve(jsonData);
          } catch (error) {
            reject(`Error reading Excel data: ${error}`);
          }
        };

        reader.onerror = () => {
          reject('Error reading the file.');
        };

        reader.readAsArrayBuffer(blob);
      }).catch(error => {
        reject(`Failed to load Excel file: ${error.message}`);
      });
    });
  }
}
