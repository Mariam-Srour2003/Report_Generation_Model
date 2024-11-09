import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-hospital-cards',
  templateUrl: './hospital-cards.component.html',
  styleUrls: ['./hospital-cards.component.scss'],
  imports: [
    CommonModule,
    MatIconModule
  ],
  standalone: true,
})
export class HospitalCardsComponent implements OnInit {

  bahmanData = {
    images: 3220,
    reports: 1001
  };

  alZahraaData = {
    images: 1218,
    reports: 1218
  };

  constructor() { }

  ngOnInit(): void {
  }

}
