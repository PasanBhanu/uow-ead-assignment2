using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class CategoriesApiClient
    {
        public Category[] GetCategories()
        {
            Category[] categories = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var responseTask = client.GetAsync("categories");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CategoriesResponse>();
                    readTask.Wait();

                    var response = readTask.Result;
                    categories = response.Data;
                }
            }

            return categories;
        }

        public Category GetCategory(int id)
        {
            Category category = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var responseTask = client.GetAsync("categories/details?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CategoryResponse>();
                    readTask.Wait();

                    var response = readTask.Result;
                    category = response.Data;
                }
            }

            return category;
        }

        public BaseResponse CreateCategory(Category category)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<Category>("categories/create", category);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BaseResponse>();
                    readTask.Wait();

                    response = readTask.Result;
                    return response;
                }
                else
                {
                    // Cache path
                    return null;
                }
            }
        }

        public BaseResponse UpdateCategory(Category category)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<Category>("categories/edit?id=" + category.Id, category);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BaseResponse>();
                    readTask.Wait();

                    response = readTask.Result;
                    return response;
                }
                else
                {
                    // Cache path
                    return null;
                }
            }
        }

        public BaseResponse DeleteCategory(int id)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<String>("categories/delete?id=" + id, null);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BaseResponse>();
                    readTask.Wait();

                    response = readTask.Result;
                    return response;
                }
                else
                {
                    // Cache path
                    return null;
                }
            }
        }
    }
}
