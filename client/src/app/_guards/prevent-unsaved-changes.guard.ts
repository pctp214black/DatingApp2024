import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn <MemberEditComponent>=(component)=>{
  if(component.editForm?.dirty){
    return confirm("If you dont save, the changes will lost, continue?")
  }
  return true;
}
