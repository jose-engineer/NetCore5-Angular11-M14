import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "./store.service";

@Injectable()
export class AuthActivator implements CanActivate {

  constructor(private store: Store, private router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

    if (this.store.loginRequired) {  //validate loginRequired property from Store service/class
      this.router.navigate(["login"]) //this "login" needs to be in the routes
      return false;
    } else {
      return true;
    }
  }

}