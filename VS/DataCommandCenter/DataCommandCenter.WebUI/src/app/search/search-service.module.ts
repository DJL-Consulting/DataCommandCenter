import { query } from "@angular/animations";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, tap, throwError, map } from "rxjs";
import { ServerDTO, ObjectSearch, SearchObjectTypes, MetadataDTO } from "../models/MetadataDTOs";

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  private serverUrl = '/api/metadata/GetServers';  //https://localhost:7115
  private searchUrl = '/api/metadata/SearchObjects'; //'/weatherforecast'; 
  private metadataUrl = '/api/metadata/GetMetadataForObject'; //'/weatherforecast'; 
  

  constructor(private http: HttpClient) { }

  getServers(): Observable<ServerDTO[]> {
    return this.http.get<ServerDTO[]>(this.serverUrl)
      .pipe(
        //tap(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getSearchResults(queryString: string, options: SearchObjectTypes): Observable<ObjectSearch[]> {
    options.QueryString = queryString;

    return this.http.post<ObjectSearch[]>(this.searchUrl, options)
      .pipe(
        //tap(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getMetadataForObject(selectedItem: ObjectSearch): Observable<MetadataDTO> {
    return this.http.post<MetadataDTO>(this.metadataUrl, selectedItem)
      .pipe(
        //(data => console.log('All: ', JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getLineageForObject(selectedItem: ObjectSearch): Observable<MetadataDTO> {
    return this.http.post<MetadataDTO>(this.metadataUrl, selectedItem)
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
