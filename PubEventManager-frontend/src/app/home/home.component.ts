import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  counter = 1; // Brojač za slajdove

  ngOnInit(): void {
    this.autoSlide(); // Pozovi funkciju za automatsku promenu slajdova
  }

  autoSlide(): void {
    setInterval(() => {
      const radioBtn = document.getElementById('radio' + this.counter) as HTMLInputElement;
      if (radioBtn) {
        radioBtn.checked = true; // Selektuj sledeći slajd
      }
      this.counter++;
      if (this.counter > 4) {
        this.counter = 1; // Vrati se na prvi slajd nakon poslednjeg
      }
    }, 5000); // Menjanje slajda svakih 5 sekundi
  }
}
