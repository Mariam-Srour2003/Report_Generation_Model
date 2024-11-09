import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-bahman-model-implementation',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './bahman-model-implementation.component.html',
  styleUrls: ['./bahman-model-implementation.component.scss']
})
export class BahmanModelImplementationComponent {

  selectedFile: File | null = null;
  generatedReport: string | null = null;
  errorMessage: string | null = null;
  imageSrc: string | null = null;

  constructor(private http: HttpClient) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] ?? null;

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e) => this.imageSrc = reader.result as string;
      reader.readAsDataURL(this.selectedFile);
    } else {
      this.imageSrc = null;
    }
  }

  onSubmit(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      this.http.post<{ report: string }>('http://localhost:5000/report/generate', formData)
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

        
          doc.save('BahmanModelReport.pdf');
        
      });
    }
  }
}
