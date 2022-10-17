import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SearchGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree {
    let url: string = state.url;

    var can = this.checkLogin(url, state);

    return true;
    //return this.checkLogin(url);
  }

  checkLogin(url: string, state: RouterStateSnapshot): boolean  {  //true | UrlTree
    var val = localStorage.getItem('isUserLoggedIn') == null ? "false" : localStorage.getItem('isUserLoggedIn');

    if (val != null && val == "true") {
      if (url == "/login")
        this.router.parseUrl('/search/metadata');
      else
        return true;
    }
    this.router.navigate(["/login"], { queryParams: { returnUrl: state.url } })  //.parseUrl('/login');
    return false;
  }
}
