using Finance_App.Models;
using Finance_App.REST;
using Finance_App.Xml;
using System;
using System.Net.Http;

namespace Finance_App.Api
{
    internal class TransactionsApiClient
    {
        TransactionStore store = new TransactionStore();
        StoreUtil storeUtil = new StoreUtil();
        public Transaction[] GetTransactions()
        {
            Transaction[] transactions = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.GetTransactions(transactions);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transactions = store.GetTransactions(null);
                }
            }
            else
            {
                transactions = store.GetTransactions(null);
            }

            return transactions;
        }

        public Transaction GetTransaction(int id)
        {
            Transaction transaction = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                }
                catch (Exception ex)
                {
                    transaction = store.GetTransaction(id);
                }
            } else
            {
                transaction = store.GetTransaction(id);
            }

            return transaction;
        }

        public BaseResponse CreateTransaction(Transaction transaction)
        {
            BaseResponse response = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.CreateTransaction(transaction);
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    storeUtil.StorePendingSyncFlag();
                    transaction.Id = Variables.GetTransactionId();
                    response = store.CreateTransaction(transaction);
                }
            }
            else
            {
                storeUtil.StorePendingSyncFlag();
                transaction.Id = Variables.GetTransactionId();
                response = store.CreateTransaction(transaction);
            }

            return response;
        }

        public BaseResponse UpdateTransaction(Transaction transaction)
        {
            BaseResponse response = null;

            if (Variables.IsAppOnline())
            {
                try
                {
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
                            store.UpdateTransaction(transaction);
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    storeUtil.StorePendingSyncFlag();
                    response = store.UpdateTransaction(transaction);
                }
            }else
            {
                storeUtil.StorePendingSyncFlag();
                response = store.UpdateTransaction(transaction);
            }

            return response;
        }

        public BaseResponse DeleteTransaction(int id)
        {
            BaseResponse response = null;

            if (Variables.IsAppOnline())
            {

                try
                {
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
                            store.DeleteTransaction(id);
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    storeUtil.StorePendingSyncFlag();
                    response = store.DeleteTransaction(id);
                }
            }
            else
            {
                storeUtil.StorePendingSyncFlag();
                response = store.DeleteTransaction(id);
            }

            return response;
        }
    }
}
