using Finance_App.REST;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class ReportsApiClient
    {
        public ReportResponse GetReport()
        {
            ReportResponse response = new ReportResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                    var responseTask = client.GetAsync("reports");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ReportResponse>();
                        readTask.Wait();

                        response = readTask.Result;
                    }
                }

                return response;
            } 
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
