import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

//interface AuthenticationRequestBody {
//  userName: string | null;
//  password: string | null;
//};

export class AuthService {


  private loginUrl = "/api/Authentication/authenticate";

  token?: string = "";

  isUserLoggedIn: boolean = false;

  public isLoggedIn(): boolean {
    return (localStorage.getItem("isUserLoggedIn") == "true");
  }

  async login(userName: string, password: string): Promise<boolean> {
    const creds = { userName: userName, password: password }; 

    const response = await this.http.post(this.loginUrl, creds, { responseType: "text" }).pipe(catchError(this.handleError)).toPromise();

    this.token = response;
    this.isUserLoggedIn = (this.token != null);

    localStorage.setItem('isUserLoggedIn', this.isUserLoggedIn ? "true" : "false");
    localStorage.setItem('loginToken', this.token == null ? "" : this.token);

    return this.isUserLoggedIn;
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    alert("Login failure!");
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }

  logout(): void {
    this.isUserLoggedIn = false;
    localStorage.removeItem('isUserLoggedIn');
    localStorage.removeItem('loginToken');
  }

  constructor(private http: HttpClient) { }
}
