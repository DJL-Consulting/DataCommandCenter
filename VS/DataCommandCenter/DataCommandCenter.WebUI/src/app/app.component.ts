import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'Data Command Center';

  ngOnInit() {
  }

  isUserLoggedIn(): boolean {

    let storeData = localStorage.getItem("isUserLoggedIn");
    if (storeData != null && storeData == "true")
      return true;

    return false;
  }

}
