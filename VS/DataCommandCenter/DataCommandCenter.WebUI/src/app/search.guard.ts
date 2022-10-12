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

    return this.checkLogin(url);
  }

  checkLogin(url: string): true | UrlTree {
    var val = localStorage.getItem('isUserLoggedIn') == null ? "false" : localStorage.getItem('isUserLoggedIn');

    if (val != null && val == "true") {
      if (url == "/login")
        this.router.parseUrl('/search');
      else
        return true;
    }
      return this.router.parseUrl('/login');
  }
}
