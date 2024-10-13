import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { toArray } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService=inject(AccountService);
  const toastr=inject(ToastrService);
  if(accountService.currentUser()){
    return true;
  }else{
    toastr.error("You dont hace access right now");
    return false;
  }
  return true;
};
