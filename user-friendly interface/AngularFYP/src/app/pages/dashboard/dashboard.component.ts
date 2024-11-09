import { Component } from '@angular/core';
import { HospitalCardsComponent } from './components/hospital-cards/hospital-cards.component';
import { HospitalChartsComponent } from './components/hospital-charts/hospital-charts.component';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [HospitalCardsComponent, HospitalChartsComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
