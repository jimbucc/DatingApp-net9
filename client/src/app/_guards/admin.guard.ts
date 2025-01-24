import { CanActivateFn } from '@angular/router';
import { AccountsService } from '../_services/accounts.service';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountsService);
  
  if (accountService.roles().includes('Admin') || accountService.roles().includes('Moderator'))
  {
    return true;
  }
  console.error("You cannot enter this area");
  return false;
};
