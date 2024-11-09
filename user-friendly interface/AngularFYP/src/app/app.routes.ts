import { Routes } from '@angular/router';
import { LoginComponent } from './layouts/login/login.component';
import { MainComponent } from './layouts/main/main.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AllAppointmentsComponent } from './pages/all-appointments/all-appointments.component';
import { UsersComponent } from './pages/users/users.component';
import { BahmanModelImplementationComponent } from './pages/bahman-model-implementation/bahman-model-implementation.component';
import { AlzahraaClipModelComponent } from './pages/alzahraa-clip-model/alzahraa-clip-model.component';
import { AlzahraaDenseNet121ModelComponent } from './pages/alzahraa-dense-net121-model/alzahraa-dense-net121-model.component';
export const routes: Routes = [
    // { path: 'dashboard', title: 'Dashboard', loadChildren: () => import('./pages/dashboard/dashboard.module').then(m => m.DashboardModule) },
    {
        path: '',
         component: LoginComponent,
        children: []
    },
    {
        path: 'main',
         component: MainComponent,
        children: [
            { path: '', component: DashboardComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'allappointments', component: AllAppointmentsComponent },
            { path: 'users', component: UsersComponent },
            { path: 'bahmanDenseNet121Model', component: BahmanModelImplementationComponent },
            { path: 'alzahraaDenseNet121Model', component: AlzahraaDenseNet121ModelComponent },
            { path: 'alzahraaclipModel', component: AlzahraaClipModelComponent },
        ]
    }
];