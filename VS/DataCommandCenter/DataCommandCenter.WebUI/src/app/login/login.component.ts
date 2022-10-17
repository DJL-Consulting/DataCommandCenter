import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AuthService } from '../auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SearchService } from "../search/search-service.module";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userName!: string;
  password!: string;
  formData!: FormGroup;
  sub!: Subscription;
  returnUrl!: string;
  
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute, private searchService: SearchService) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/search/metadata";
    this.formData = new FormGroup({
      userName: new FormControl("demo"),
      password: new FormControl("demo"),
    });
  }

  async onClickSubmit(data: any) {
    this.userName = data.userName;
    this.password = data.password;

    this.password = this.password; // base 64 encode?

    var isLoggedIn = await this.authService.login(this.userName, this.password);

    if (isLoggedIn)
      this.router.navigate([this.returnUrl]);  //(['/search/metadata']);
    else
      alert("Login failed!");
  }


}
