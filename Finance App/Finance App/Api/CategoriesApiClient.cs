using Finance_App.Models;
using Finance_App.REST;
using Finance_App.Xml;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class CategoriesApiClient
    {
        CategoryStore store = new CategoryStore();
        StoreUtil storeUtil = new StoreUtil();

        public Category[] GetCategories()
        {
            Category[] categories = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.GetCategories(categories);
                        }
                    }
                }
                catch (Exception ex)
                {
                    categories = store.GetCategories(null);
                }
            } else
            {
                categories = store.GetCategories(null);
            }

            return categories;
        }

        public Category GetCategory(int id)
        {
            Category category = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            Category data = response.Data;
                            category = data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    category = store.GetCategory(id);
                }
            }
            else
            {
                category = store.GetCategory(id);
            }

            return category;
        }

        public BaseResponse CreateCategory(Category category)
        {
            BaseResponse response = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.CreateCategory(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    storeUtil.StorePendingSyncFlag();
                    category.Id = Variables.GetCategoryId();
                    response = store.CreateCategory(category);
                }
            }
            else
            {
                storeUtil.StorePendingSyncFlag();
                category.Id = Variables.GetCategoryId();
                response = store.CreateCategory(category);
            } 

            return response;
        }

        public BaseResponse UpdateCategory(Category category)
        {
            BaseResponse response = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.UpdateCategory(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    storeUtil.StorePendingSyncFlag();
                    response = store.UpdateCategory(category);
                }
            }
            else
            {
                storeUtil.StorePendingSyncFlag();
                response = store.UpdateCategory(category);
            }

            return response;
        }

        public BaseResponse DeleteCategory(int id)
        {
            BaseResponse response = null;
            try
            {
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
                    }
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse();
                response.Status = "error";
                response.Message = "Unable to reach server. Please check your internet connection and retry.";
            }

            return response;
        }
    }
}
