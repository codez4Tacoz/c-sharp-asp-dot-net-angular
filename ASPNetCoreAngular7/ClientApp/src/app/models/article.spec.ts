import { Article } from './article';

describe('Article', () => {
    it('should create an instance', () => {

        let article = new Article(1111, "Sample Article", "tacoCoder", "http://www.google.com");

        expect(article).toBeTruthy();
        expect(article.id).toEqual(1111);
        expect(article.title).toEqual("Sample Article");
        expect(article.author).toEqual("tacoCoder");
        expect(article.url).toEqual("http://www.google.com");
  });
});
