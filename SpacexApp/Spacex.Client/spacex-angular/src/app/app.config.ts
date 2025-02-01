import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { appRoutes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(appRoutes),
     provideClientHydration(withEventReplay()), 
     provideHttpClient(),
     importProvidersFrom(BrowserAnimationsModule, ToastrModule.forRoot({
      positionClass: 'toast-top-right', 
      timeOut: 5000, 
      closeButton: false,
      progressBar: true
    }))
    ]
};
