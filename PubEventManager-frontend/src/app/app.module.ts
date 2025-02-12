import { NgModule, ɵsetInjectorProfilerContext } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'; 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './layout/nav-bar/nav-bar.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { EventsPageComponent } from './events-page/events-page.component';
import { EventDetailsComponent } from './event-details/event-details.component';
import { ManagerProfileComponent } from './manager-profile/manager-profile.component';
import { EmployeeProfileComponent } from './employee-profile/employee-profile.component';
import { FooterComponent } from './layout/footer/footer.component';  // Dodaj ovu liniju


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavBarComponent,
    LoginComponent,
    RegisterComponent,
    EventsPageComponent,
    EventDetailsComponent,
    ManagerProfileComponent,
    EmployeeProfileComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
