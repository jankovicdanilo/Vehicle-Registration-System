import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { UserService } from './../services/user.service';

export const IsLoggedGuard = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot,
  userService = inject(UserService), 
  router = inject(Router)
): boolean | UrlTree => {
  if (userService.isSignedIn()) {
    return true;
  } else {
    router.navigate(['/auth']);
    localStorage.clear();
    return false;
  }
}