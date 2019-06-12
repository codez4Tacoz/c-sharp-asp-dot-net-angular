import { Component, OnInit } from '@angular/core';

import { Article } from '../../models/article';
import { NewsServiceService } from '../../services/news-service.service';

@Component({
  selector: 'app-list-news',
  templateUrl: './list-news.component.html',
  styleUrls: ['./list-news.component.css']
})
export class ListNewsComponent implements OnInit {

    emptyItemsMessage: string;

    numItemsTotal: number;
    numPagesTotal: number;
    pageNum: number = 1;
    pageSize: number = 15;
    pageIds: number[];
    pageItems: Article[];
    pageStartItemNum: number;
    pageEndItemNum: number;

    constructor(private newsService: NewsServiceService) {
    }

    ngOnInit() {
        this.loadPage();
    }

    loadPage() {
        //re-set this value for each new request
        this.emptyItemsMessage = null;

        this.newsService.getPageData(this.pageSize, this.pageNum).subscribe(
            pageData => {

                if (this.numItemsTotal == 0) {
                    this.emptyItemsMessage = "There are no news items!  Try refreshing the page if you want to check again.";
                }
                else {
                    this.numItemsTotal = pageData.numItemsTotal;
                    this.numPagesTotal = pageData.numPagesTotal;
                    this.pageNum = pageData.pageNum;
                    this.pageSize = pageData.pageSize;
                    this.pageIds = pageData.pageIds;
                    this.pageItems = pageData.pageItems;

                    this.pageStartItemNum = pageData.pageStartItemNum;
                    this.pageEndItemNum = pageData.pageEndItemNum;
                }
            },
            error => {
                this.emptyItemsMessage = "An error occurred retrieving the data. Try refreshing the page if you want to check again.";
            }
       );
        
    }
}
