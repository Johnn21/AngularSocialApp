import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private toastr: ToastrService) { 
    if (this.router.getCurrentNavigation().extras.state && this.router.getCurrentNavigation().extras.state.post) {
      this.toastr.success("Post added successfully");
    }
  }

  ngOnInit(): void {
  }

}
