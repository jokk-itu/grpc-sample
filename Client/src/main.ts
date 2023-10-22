import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import * as dkfds from 'dkfds'; // fake error
import { AppModule } from './app/app.module';

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));

dkfds.init();