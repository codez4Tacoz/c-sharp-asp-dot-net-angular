using ASPNetCoreAngular7.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace ASPNetCoreAngular7
{
    public class NewsService : INewsService
    {
        private static HttpClient client = new HttpClient();

        public async Task<int[]> GetIdsAsync()
        {
            string path = "https://hacker-news.firebaseio.com/v0/newstories.json";
            int[] ids = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                ids = await response.Content.ReadAsAsync<int[]>();
            }
            else
            {
                throw new RemoteApiException("An error occurred attempting to retrieve the latest news story ids");
            }
            return ids;
        }

        public async Task<NewsArticle> GetItemByIdAsync(int id)
        {
            string pathTemplate = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
            string path = String.Format(pathTemplate, id);

            NewsArticle article = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                article = await response.Content.ReadAsAsync<NewsArticle>();
            }
            else
            {
                throw new RemoteApiException("An error occurred attempting to retrieve item " + id);
            }
            return article;
        }

    }
}