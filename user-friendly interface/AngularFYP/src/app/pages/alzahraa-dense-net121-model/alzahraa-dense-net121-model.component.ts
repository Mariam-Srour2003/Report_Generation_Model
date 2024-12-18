import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-alzahraa-dense-net121-model',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './alzahraa-dense-net121-model.component.html',
  styleUrl: './alzahraa-dense-net121-model.component.scss'
})
export class AlzahraaDenseNet121ModelComponent {

  selectedFile: File | null = null;
  generatedReport: string | null = null;
  errorMessage: string | null = null;

  constructor(private http: HttpClient) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] ?? null;
  }

  onSubmit(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      this.http.post<{ report: string }>('http://localhost:8080/report/generate', formData)
        .subscribe({
          next: (response) => {
            this.generatedReport = response.report;
            this.errorMessage = null;
          },
          error: (error: HttpErrorResponse) => {
            this.errorMessage = error.message;
            this.generatedReport = null;
          }
        });
    } else {
      this.errorMessage = 'Please select a file first!';
    }
  }

  exportAsPDF(): void {
    const dataElement = document.querySelector('.generated-report') as HTMLElement;

    if (dataElement) {
      html2canvas(dataElement).then((canvas) => {
        const imgData = canvas.toDataURL('image/png');
        const doc = new jsPDF('p', 'mm', 'a4');
        const imgWidth = 210;
        const pageHeight = 295;
        const imgHeight = canvas.height * imgWidth / canvas.width;
        let heightLeft = imgHeight;
        let position = 0;

        doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;

        while (heightLeft >= 0) {
          position = heightLeft - imgHeight;
          doc.addPage();
          doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
          heightLeft -= pageHeight;
        }

        doc.save('AlzahraaDENSENET121ModelReport.pdf');
      });
    }
  }
}
