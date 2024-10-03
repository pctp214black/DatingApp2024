import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  http = inject(HttpClient);
  registerMode=false;
  users: any;

  ngOnInit(): void {
      this.getUsers();
  }

  registerToggle(){
    this.registerMode=!this.registerMode;
  }

  cancelRegister(event: boolean):void{
    this.registerMode=event;
  }

  getUsers() {
    this.http.get<Object>("https://localhost:5001/api/v1/users").subscribe({
      next: (response) => { this.users = response; },
      error: (error) => { console.log(error); },
      complete: () => { console.log("Request completed!"); }
    });
  }
}
