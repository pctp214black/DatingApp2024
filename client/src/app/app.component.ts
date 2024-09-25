import { CommonModule, NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor,NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Cualquier cosa';
  http=inject(HttpClient);
  users:any
  
  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/v1/users").subscribe({
      next:(response)=>{this.users=response},
      error:(error)=>{console.error(error)},
      complete:()=>{console.info("Request Completed")}
    }
      
    );
  }
}
