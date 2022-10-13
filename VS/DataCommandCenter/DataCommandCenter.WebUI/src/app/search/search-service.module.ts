import { query } from "@angular/animations";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, tap, throwError, map } from "rxjs";
import { LineageDTO } from "../models/LineageDTO";
import { ServerDTO, ObjectSearch, SearchObjectTypes, MetadataDTO } from "../models/MetadataDTOs";

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  private serverUrl = '/api/metadata/GetServers';  //https://localhost:7115
  private searchUrl = '/api/metadata/SearchObjects'; 
  private metadataUrl = '/api/metadata/GetMetadataForObject';  
  private lineageUrl = '/api/metadata/GetLineageForObject';

  jwt!: string;
  //jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZ2l2ZW5fbmFtZSI6IkRlbW8iLCJmYW1pbHlfbmFtZSI6IlVzZXIiLCJlbWFpbCI6ImRlbW9AdXNlci5vcmciLCJ1c2VyX3R5cGUiOiJBZG1pbiIsIm5iZiI6MTY2NTU5ODE5MCwiZXhwIjoxNjY2MjAyOTkwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTE1IiwiYXVkIjoiZGF0YWNvbW1hbmRjZW50ZXJhcGkifQ.xE3za9mtaUq5hOX6_IwS4f_vfcqIudEIY8rywY42qmk";

  

  constructor(private http: HttpClient) { }

  getToken(): string {
    var token = localStorage.getItem('loginToken');
    return (token == null ? "" : token);
  }

  getHeaders() {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.getToken()}`
    })
  }

  getServers(): Observable<ServerDTO[]> {


    return this.http.get<ServerDTO[]>(this.serverUrl, { headers: this.getHeaders() })
      .pipe(
        //tap(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getSearchResults(queryString: string, options: SearchObjectTypes): Observable<ObjectSearch[]> {
    options.QueryString = queryString;

    return this.http.post<ObjectSearch[]>(this.searchUrl, options, { headers: this.getHeaders() })
      .pipe(
        //tap(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getMetadataForObject(selectedItem: ObjectSearch): Observable<MetadataDTO> {
    return this.http.post<MetadataDTO>(this.metadataUrl, selectedItem, { headers: this.getHeaders() })
      .pipe(
        //(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getLineageForObject(selectedItem: ObjectSearch): Observable<LineageDTO> {
    return this.http.post<LineageDTO>(this.lineageUrl, selectedItem, { headers: this.getHeaders() })
      .pipe(
        //(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }


}
