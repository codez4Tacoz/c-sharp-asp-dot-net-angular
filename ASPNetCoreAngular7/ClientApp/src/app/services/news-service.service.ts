import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PageResult } from '../models/page-result';
import { Article } from '../models/article';
import { Observable } from 'rxjs';
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class NewsServiceService {

    apiBaseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

        this.apiBaseUrl = baseUrl;
    }

    getPageData(pageSize: number, pageNumber: number): Observable<PageResult> {

        return this.http.get(this.apiBaseUrl + 'api/HackerNews/GetLatestNews?pageNum=' + pageNumber + '&pageSize=' + pageSize)
            .pipe(map((result :any) => {

                    var newsResponse = new PageResult();

                    newsResponse.numItemsTotal = result.numItemsTotal;
                    newsResponse.numPagesTotal = result.numPagesTotal;
                    newsResponse.pageSize = result.pageSize;

                    if (result.numItemsTotal != 0) {

                        newsResponse.pageItems = (result.pageItems).map(
                            (jsonObject: any) => new Article(jsonObject.id, jsonObject.title, jsonObject.by, jsonObject.url)
                        );

                        newsResponse.pageNum = result.pageNum;
                        newsResponse.pageIds = result.pageIds;
                        newsResponse.pageStartItemNum = result.pageStartItemNum;
                        newsResponse.pageEndItemNum = result.pageEndItemNum;
                    }

                    return newsResponse;
                } 
            ));
    }
}
