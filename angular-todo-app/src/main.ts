import { enableProdMode, provideZoneChangeDetection } from '@angular/core';
import { environment } from './environments/environment';
import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withXhr } from '@angular/common/http';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(
  AppComponent, {
    providers: [
      provideZoneChangeDetection(),
      provideHttpClient(withXhr()),
    ]
  }
).catch(error => console.error(error));
