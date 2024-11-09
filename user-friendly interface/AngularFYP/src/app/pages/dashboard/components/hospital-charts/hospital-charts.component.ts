// src/app/components/hospital-charts/hospital-charts.component.ts
import { Component, Input, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { ExcelDataService } from '../../../../services/excel-data.service';

Chart.register(...registerables);

@Component({
  selector: 'app-hospital-charts',
  standalone: true,
  imports: [
    
  ],
  templateUrl: './hospital-charts.component.html',
  styleUrl: './hospital-charts.component.scss'
})
export class HospitalChartsComponent implements OnInit {

  @Input() hospital!: string;
  @Input() filePath!: string;
  @ViewChild('chartCanvas', { static: true }) chartCanvas!: ElementRef<HTMLCanvasElement>;

  chart!: Chart;

  constructor(private excelService: ExcelDataService) { }

  ngOnInit(): void {
    this.excelService.readExcel(this.filePath).then(data => {
      const chartData = this.processData(data);
      this.createChart(chartData);
    });
  }

  private processData(data: any[]): { labels: string[], dataset: number[] } {
    const dateMap: Map<string, number> = new Map();

    if (this.hospital === 'Bahman') {
        data.forEach(row => {
            const serialDate = row[1]; // Assuming the "Date" is in the second column
            const date = this.convertSerialDateToJSDate(serialDate);

            if (!isNaN(date.getTime())) { // Check if the date is valid
                const dateString = date.toISOString().split('T')[0]; // Convert to YYYY-MM-DD format
                const currentCount = dateMap.get(dateString) ?? 0; // Use 0 if undefined
                dateMap.set(dateString, currentCount + 1);
            }
        });
    } else if (this.hospital === 'Al Zahraa') {
        data.forEach(row => {
            const serialStudyDate = row[2]; // Assuming the "Study Date" is in the third column
            const date = this.convertSerialDateToJSDate(serialStudyDate);

            if (!isNaN(date.getTime())) { // Check if the date is valid
                const dateString = date.toISOString().split('T')[0]; // Convert to YYYY-MM-DD format
                const currentCount = dateMap.get(dateString) ?? 0; // Use 0 if undefined
                dateMap.set(dateString, currentCount + 1);
            }
        });
    }

    // Convert the dateMap to sorted arrays
    const labels = Array.from(dateMap.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());
    const dataset = labels.map(label => dateMap.get(label) as number); // Ensure to get the values after sorting labels

    return { labels, dataset };
  }
  private convertSerialDateToJSDate(serial: number): Date {
      if (serial <= 0 || typeof serial !== 'number') {
          return new Date(NaN); // Return an invalid date if the serial is not valid
      }

      const excelEpoch = new Date(1899, 11, 30); // Excel uses 1900 epoch, with an offset of -1
      const date = new Date(excelEpoch.getTime() + serial * 86400000); // Multiply by the number of milliseconds in a day
      return date;
  }


  private createChart(chartData: { labels: string[], dataset: number[] }): void {
    this.chart = new Chart(this.chartCanvas.nativeElement, {
      type: 'line', // or 'line', 'pie', etc.
      data: {
        labels: chartData.labels,
        datasets: [{
          label: `${this.hospital} Study Count by Date`,
          data: chartData.dataset,
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Number of Studies'
            }
          },
          x: {
            title: {
              display: true,
              text: 'Date'
            }
          }
        },
        plugins: {
          title: {
            display: true,
            text: `${this.hospital} Hospital - Number of Studies by Date`
          }
        }
      }
    });
  }
}

