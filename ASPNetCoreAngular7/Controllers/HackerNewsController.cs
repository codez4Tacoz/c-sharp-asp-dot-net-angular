using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASPNetCoreAngular7.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ASPNetCoreAngular7
{

    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private INewsService _service;

        public HackerNewsController(INewsService service)
        {
            _service = service;
        }

        /**
         * This method will return a PagingResultObject for the requested pageNumber at the given page size.
         * Page numbers start at 1.
         * Page size minimum is 1.
         * If an error occurs retrieving one of the page items, an error item will be returned whose title 
         * attribute will be "<Error, Item not found>".
         * 
         * The resultant PagingResultObject will contain 
         * the actual page number (in some cases it will be changed for the caller),
         * the new list if Ids (likely different than the last time the method was called),
         * the new totalNumber of pages & items (could have changed),
         * and the page items for the pageNumber returned.
         * 
         * If a page number is requested that is larger than the last page, then the last page of results is returned.
         * If a page number is requested that is less than 1, the first page will be returned.
         * 
         */
        [HttpGet("[action]")]
        public async Task<PagingResultObject> GetLatestNews(int pageNum, int pageSize)
        {
            try
            {
                if (pageNum < 1)
                {
                    pageNum = 0;
                }

                //this is a purposefully inconsisten handling of validation.  see assumptions.component.html for details
                if (pageSize < 1 || pageSize > 100)
                {
                    //Ideally this would be returning a proper HTTP response code...such as 422
                    throw new ValidationException("Page size must be greater than or equal to 1 and cannot exceed 100");
                }

                //if an error occurs, we can log it and return an empty array.
                //or, if hackerNews is down, perhaps we should return an error response 500 or something
                int[] latestNewsIdsArray = await _service.GetIdsAsync();

                PagingResultObject pageResult;
                if (latestNewsIdsArray == null || latestNewsIdsArray.Length == 0)
                {
                    //presumably this is a valid scenario and so we're not going to return an error response.  Just make sure the caller handles this.
                    pageResult = new PagingResultObject
                    {
                        PageSize = pageSize,
                        PageNum = 0,
                        NumItemsTotal = 0,
                        NumPagesTotal = 0
                    };

                }
                else
                {
                    int numPages = this.CalculateNumPages(latestNewsIdsArray, pageSize);
                    int actualPageNum = this.CalculateRealPageNumber(latestNewsIdsArray, pageNum, pageSize);
                    int pageStartIndex = this.CalculatePageStartIndex(latestNewsIdsArray, actualPageNum, pageSize);
                    int pageEndIndex = this.CalculatePageEndIndex(latestNewsIdsArray, actualPageNum, pageSize);

                    pageResult = new PagingResultObject
                    {
                        PageSize = pageSize,
                        PageNum = actualPageNum,
                        NumItemsTotal = latestNewsIdsArray.Length,
                        NumPagesTotal = numPages,
                        PageStartItemNum = pageStartIndex + 1,
                        PageEndItemNum = pageEndIndex + 1,
                        PageIds = latestNewsIdsArray.Skip(pageStartIndex).Take(pageEndIndex - pageStartIndex).ToArray()
                    };

                    //some sort of c# map => Promise => promise.all would be better here 
                    List<NewsArticle> pageArticles = new List<NewsArticle>();
                    for (int i = 0; i < pageResult.PageIds.Length; i++)
                    {
                        int articleId = pageResult.PageIds[i];
                        NewsArticle article = await _service.GetItemByIdAsync(articleId);  

                        //I have seen some Ids come back with null responses.  I have double-checked by hitting their api directly.
                        //seems like overkill to prevent the page from loading because one of the items has an issue,
                        //so we'll just create a dummy entry and make sure the caller handles it..
                        if (article == null)
                        {
                            article = new NewsArticle
                            {
                                Id = articleId,
                                Title = "<Error, Item not found>",
                                By = "?",
                                Url = null
                            };
                        }
                        pageArticles.Add(article);
                    }
                    pageResult.PageItems = pageArticles.ToArray();
                }

                return pageResult;
            }
            catch(Exception ex)
            {
                throw;
            }
        }



        public int CalculateNumPages(int[] ids, int pageSize)
        {
            if(pageSize == 0)
            {
                throw new Exception("Page size cannot be 0");
            }
            int numItems = ids.Length;
            int numPages = numItems / pageSize;
            if (numItems % pageSize > 0)
            {
                numPages++;
            }
            return numPages;
        }

        /**
         * There is a possibility, however rare, that the number of pages has been reduced and now 
         * the requested page number no longer exists.  This method will adjust the requested pageNumber
         * and, in that case, return the last page number.
         */
        public int CalculateRealPageNumber(int[] ids, int pageNum, int pageSize)
        {
            int currentNumPages = this.CalculateNumPages(ids, pageSize);
            int realPageNum = Math.Min(pageNum, currentNumPages);

            return realPageNum;
        }

        /**
         * Calculate what the ids array index is for the first element on the desired page, given the ids list, pageSize, and desired pageNum.
         */
        public int CalculatePageStartIndex(int[] ids, int realPageNum, int pageSize)
        {
            if (ids.Length == 0)
            {
                throw new InvalidOperationException("There are 0 elements, no pages, " +
                    "and thus no page start indices.");
            }

            //because page items are 0-indexed, so pg1: 0-24, pg2: 25-49, etc.
            int startIndex = (realPageNum - 1) * pageSize;
            return startIndex;
        }

        /**
         * Calculate what the ids array index is for the last element on the desired page, given the ids list, pageSize, and desired pageNum.
         */
        public int CalculatePageEndIndex(int[] ids, int realPageNum, int pageSize)
        {
            int numItems = ids.Length;
            if (numItems == 0)
            {
                throw new InvalidOperationException("There are 0 elements, no pages, " +
                    "and thus no page end indices.");
            }

            int endIndex = (realPageNum * pageSize) - 1;
            //if we're on the last page and the total num is not a multiple of page size...just return the last item index
            endIndex = Math.Min(endIndex, (numItems - 1));
            return endIndex;
        }
    }

}
