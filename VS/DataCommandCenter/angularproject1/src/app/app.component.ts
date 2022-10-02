import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public forecasts?: WeatherForecast[];

  constructor(http: HttpClient) {
    
    http.get<WeatherForecast[]>('/weatherforecast').subscribe(result => {
    //http.get<WeatherForecast[]>('/api/metadata/GetServers').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  title = 'angularproject1';
}

/*
interface WeatherForecast {
  Id: number;
  ServerName: string;
  ServerInstance: string;
  Version: string;
  PullMetadata: boolean;
  ServerType: string;
}
*/


interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

