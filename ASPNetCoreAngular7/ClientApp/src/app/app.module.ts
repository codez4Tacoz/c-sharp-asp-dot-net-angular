import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { ListNewsComponent } from './components/list-news/list-news.component';
import { NewsServiceService } from './services/news-service.service';
import { AssumptionsComponent } from './components/assumptions/assumptions.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ListNewsComponent,
    AssumptionsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule.forRoot(),
    RouterModule.forRoot([
        { path: '', component: HomeComponent, pathMatch: 'full' },
        { path: 'list-news', component: ListNewsComponent },
        { path: 'assumptions', component: AssumptionsComponent }
    ])
    ],
    providers: [NewsServiceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
