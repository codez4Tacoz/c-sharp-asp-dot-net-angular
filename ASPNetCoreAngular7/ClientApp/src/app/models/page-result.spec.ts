import { PageResult } from './page-result';

describe('PageResult', () => {
  it('should create an instance', () => {
      

      var pageResult = new PageResult();
      pageResult = {
          
          numItemsTotal: 502,
          numPagesTotal: 51,
          pageNum:  1,
          pageSize: 11,
          pageIds: null,
          pageItems: null,
          pageStartItemNum: 3,
          pageEndItemNum: 10
      }

      expect(pageResult).toBeTruthy();
      expect(pageResult.numItemsTotal).toEqual(502);
      expect(pageResult.numPagesTotal).toEqual(51);
      expect(pageResult.pageNum).toEqual(1);
      expect(pageResult.pageSize).toEqual(11);
      expect(pageResult.pageIds).toEqual(null);
      expect(pageResult.pageItems).toEqual(null);
      expect(pageResult.pageStartItemNum).toEqual(3);
      expect(pageResult.pageEndItemNum).toEqual(10);
  });

});
