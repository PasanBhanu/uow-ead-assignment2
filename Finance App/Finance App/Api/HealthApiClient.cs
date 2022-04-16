using Finance_App.REST;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class HealthApiClient
    {
        public bool GetHealth()
        {
            bool health = false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                    var responseTask = client.GetAsync("health");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<BaseResponse>();
                        readTask.Wait();

                        var response = readTask.Result;
                        if (response.Status == "success")
                        {
                            health = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            

            return health;
        }
    }
}
