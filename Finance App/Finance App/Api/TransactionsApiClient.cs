using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class TransactionsApiClient
    {
        public Transaction[] GetTransactions()
        {
            Transaction[] transactions = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var responseTask = client.GetAsync("transactions");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TransactionsResponse>();
                    readTask.Wait();

                    var response = readTask.Result;
                    transactions = response.Data;
                }
            }

            return transactions;
        }

        public Transaction GetTransaction(int id)
        {
            Transaction transaction = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var responseTask = client.GetAsync("transactions/details?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TransactionResponse>();
                    readTask.Wait();

                    var response = readTask.Result;
                    transaction = response.Data;
                }
            }

            return transaction;
        }

        public BaseResponse CreateTransaction(Transaction transaction)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<Transaction>("transactions/create", transaction);
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

        public BaseResponse UpdateTransaction(Transaction transaction)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<Transaction>("transactions/edit?id=" + transaction.Id, transaction);
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

        public BaseResponse DeleteTransaction(int id)
        {
            BaseResponse response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiUrl);
                var postTask = client.PostAsJsonAsync<Transaction>("transactions/delete?id=" + id, null);
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
