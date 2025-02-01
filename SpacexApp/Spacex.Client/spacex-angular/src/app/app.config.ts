import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { appRoutes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([JwtInterceptor])), 
    importProvidersFrom(
      BrowserAnimationsModule, 
      ToastrModule.forRoot({
        positionClass: 'toast-top-right', 
        timeOut: 4000, 
        closeButton: false,
        progressBar: true
      })
    )
  ]
};
