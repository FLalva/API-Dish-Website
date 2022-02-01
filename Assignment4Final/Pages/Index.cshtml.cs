using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Assignment4Final.Pages
{
    public class IndexModel : PageModel
    {
        public string youtubeUrl;
        public string mealUrl;
        public string strMeal;
        public string strCategory;

        public string mealName;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await FindMealInfo();
            await FindYouTube();
            return Page();
        }

        private async Task FindMealInfo()
        {
            string url = "https://www.themealdb.com/api/json/v1/1/random.php";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var randomMeal = JsonConvert.DeserializeObject<MealModel>(json);

                var uriSource = new Uri(randomMeal.meals[0].strMealThumb, UriKind.Absolute);
                mealUrl = uriSource.ToString();

                mealName = randomMeal.meals[0].strMeal; 
                strMeal = mealName;
                strCategory = randomMeal.meals[0].strCategory;
            }
        }

        private async Task FindYouTube()
        {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyB0REgP42AiFQoAhfs-DdeeTG3X-5XHuAo",
                    ApplicationName = GetType().ToString()
                });

                List<SearchResult> result = new List<SearchResult>();
                var request = youtubeService.Search.List("snippet");
                request.Q = "how to make " + mealName;
                request.MaxResults = 1;

                var response = await request.ExecuteAsync();

                foreach (var item in response.Items)
                {
                    switch (item.Id.Kind)
                    {
                        case "youtube#video":
                            result.Add(item);
                            break;
                    }
                }

                youtubeUrl = "https://www.youtube.com/embed/" + result[0].Id.VideoId;
        }
    }
}
