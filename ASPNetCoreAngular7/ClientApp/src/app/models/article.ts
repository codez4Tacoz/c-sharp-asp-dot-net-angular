export class Article {

    id: number;
    title: string;
    author: string;
    url: string;

    constructor(id: number, title: string, author: string, url: string) {
        this.title = title;
        this.id = id;
        this.author = author;
        this.url = url;
    }
}