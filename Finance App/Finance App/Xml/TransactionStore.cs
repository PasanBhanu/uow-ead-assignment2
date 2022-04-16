using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Finance_App.Xml
{
    internal class TransactionStore
    {
        public Transaction[] GetTransactions(Transaction[] transactions)
        {
            if (transactions != null)
            {
                var xmlDoc = XElement.Load("Store.xml");
                var transactionsXml = transactions.ToList()
                                    .Select(m => new XElement("Transaction", new XAttribute("Id", m.Id),
                                        new XAttribute("Description", m.Description),
                                        new XAttribute("Type", m.Type),
                                        new XAttribute("Amount", m.Amount),
                                        new XAttribute("IsReccuring", m.IsReccuring),
                                        new XAttribute("Date", m.Date),
                                        new XAttribute("CategoryId", m.CategoryId),
                                        new XElement("TransactionCategory", new XAttribute("Id", m.Category.Id),
                                        new XAttribute("Title", m.Category.Title),
                                        new XAttribute("Type", m.Category.Type)))
                                    ).ToList();
                xmlDoc.Descendants("Transactions").Remove();
                xmlDoc.Add(new XElement("Transactions", transactionsXml));
                xmlDoc.Save("Store.xml");
            }
            else
            {
                var xmlDoc = XElement.Load("Store.xml");
                transactions = xmlDoc.Descendants("Transaction")
                    .Select(m => new Transaction
                    {
                        Id = int.Parse(m.Attribute("Id").Value.ToString()),
                        Description = m.Attribute("Description").Value,
                        Type = m.Attribute("Type").Value,
                        Amount = double.Parse(m.Attribute("Amount").Value),
                        IsReccuring = bool.Parse(m.Attribute("IsReccuring").Value),
                        Date = DateTime.Parse(m.Attribute("Date").Value),
                        CategoryId = int.Parse(m.Attribute("CategoryId").Value.ToString()),
                        Category = new Category
                        {
                            Id = int.Parse(m.Element("TransactionCategory").Attribute("Id").Value.ToString()),
                            Title = m.Element("TransactionCategory").Attribute("Title").Value,
                            Type = m.Element("TransactionCategory").Attribute("Type").Value,
                        }
                    }).ToArray();
            }

            return transactions;
        }
        
        public Transaction GetTransaction(int id)
        {
            Transaction transaction = null;

            var xmlDoc = XElement.Load("Store.xml");
            transaction = xmlDoc.Descendants("Transaction")
                .Where(m => int.Parse(m.Attribute("Id").Value) == id)
                .Select(m => new Transaction
                {
                    Id = int.Parse(m.Attribute("Id").Value.ToString()),
                    Description = m.Attribute("Description").Value,
                    Type = m.Attribute("Type").Value,
                    Amount = double.Parse(m.Attribute("Amount").Value),
                    IsReccuring = bool.Parse(m.Attribute("IsReccuring").Value),
                    Date = DateTime.Parse(m.Attribute("Date").Value),
                    CategoryId = int.Parse(m.Attribute("CategoryId").Value.ToString()),
                    Category = new Category
                    {
                        Id = int.Parse(m.Element("TransactionCategory").Attribute("Id").Value.ToString()),
                        Title = m.Element("TransactionCategory").Attribute("Title").Value,
                        Type = m.Element("TransactionCategory").Attribute("Type").Value,
                    }
                })
                .FirstOrDefault();

            return transaction;
        }

        public BaseResponse CreateTransaction(Transaction transaction)
        {
            BaseResponse response = new BaseResponse();
            var xmlDoc = XElement.Load("Store.xml");
            Category category = xmlDoc.Descendants("Category")
                .Where(m => int.Parse(m.Attribute("Id").Value) == transaction.CategoryId)
                .Select(m => new Category
                {
                    Id = int.Parse(m.Attribute("Id").Value.ToString()),
                    Title = m.Attribute("Title").Value,
                    Type = m.Attribute("Type").Value
                })
                .FirstOrDefault();

            xmlDoc.Add(new XElement("Transactions", new XElement("Transaction", new XAttribute("Id", transaction.Id),
                                        new XAttribute("Description", transaction.Description),
                                        new XAttribute("Type", transaction.Type),
                                        new XAttribute("Amount", transaction.Amount),
                                        new XAttribute("IsReccuring", transaction.IsReccuring),
                                        new XAttribute("Date", transaction.Date),
                                        new XAttribute("CategoryId", transaction.CategoryId),
                                        new XElement("TransactionCategory", new XAttribute("Id", category.Id),
                                        new XAttribute("Title", category.Title),
                                        new XAttribute("Type", category.Type)))));
            xmlDoc.Save("Store.xml");
            response.Status = "success";
            response.Message = "Transaction added successfully";

            return response;
        }

        public BaseResponse UpdateTransaction(Transaction transaction)
        {
            BaseResponse response = new BaseResponse();
            var xmlDoc = XElement.Load("Store.xml");
            var doc = xmlDoc.Descendants("Transaction")
                    .Where(m => int.Parse(m.Attribute("Id").Value) == transaction.Id)
                    .First();

            doc.Attribute("Description").Value = transaction.Description;
            doc.Attribute("Type").Value = transaction.Type;
            doc.Attribute("Amount").Value = transaction.Amount.ToString();
            doc.Attribute("IsReccuring").Value = transaction.IsReccuring.ToString();
            doc.Attribute("Date").Value = transaction.Date.ToString();
            doc.Attribute("CategoryId").Value = transaction.CategoryId.ToString();
            doc.Descendants("TransactionCategory").Remove();
            doc.Add(new XElement("TransactionCategory", new XAttribute("Id", transaction.Category.Id),
                                        new XAttribute("Title", transaction.Category.Title),
                                        new XAttribute("Type", transaction.Category.Type)));
            xmlDoc.Save("Store.xml");

            response.Status = "success";
            response.Message = "Transaction updated successfully";

            return response;
        }

        public BaseResponse DeleteTransaction(int id)
        {
            BaseResponse response = new BaseResponse();
            var xmlDoc = XElement.Load("Store.xml");
            xmlDoc.Descendants("Transaction")
                    .Where(m => int.Parse(m.Attribute("Id").Value) == id)
                    .Remove();
            response.Status = "success";
            response.Message = "Transaction deleted successfully";
            return response;
        }
    }
}
