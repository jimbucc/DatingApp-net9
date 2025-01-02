import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from "@angular/platform-browser/animations";

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './_interceptors/error.interceptor';
//import { provideToastr } from 'ngx-toaster'

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor])),
    provideAnimations(),
    // provideToastr({
    //   positionClass: 'toast-bottom-right'
    // })
  ]
};
