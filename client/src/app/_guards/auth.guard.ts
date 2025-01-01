import { CanActivateFn } from '@angular/router';
import { AccountsService } from '../_services/accounts.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountsService);
  
  if(accountService.currentUser()) {
    return true
  } else {
    console.log("cannot authenticate user - you shall not pass")
    return false
  }
};
