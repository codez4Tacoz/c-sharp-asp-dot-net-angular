import { Article } from "./article";

export class PageResult {

    numItemsTotal: number;
    numPagesTotal: number;
    pageNum: number = 1;
    pageSize: number = 10;
    pageIds: number[];
    pageItems: Article[];
    pageStartItemNum: number;
    pageEndItemNum: number;
}
