using Finance_App_Service.Data;
using Finance_App_Service.REST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Finance_App_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly ApplicationDbContext _applicationDbCotext;

        public TransactionsController(ILogger<TransactionsController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbCotext = applicationDbContext;
        }

        // GET: transactions
        [HttpGet]
        public BaseResponse Index()
        {
            IEnumerable<Transaction> transactions = _applicationDbCotext.Transactions.ToList();
            //IEnumerable<Category> categories = _applicationDbCotext.Categories.Select(x => new Category{Id=x.Id, Title=x.Title, Type=x.Type}).ToList();
            IEnumerable<Category> categories = _applicationDbCotext.Categories.ToList();
            foreach (Category category in categories)
            {
                category.Transactions = null;
            }
            BaseResponse response = new BaseResponse
            {
                Status = "success",
                Message = "Transactions retrieved successfully",
                Data = transactions
            };
            return response;
        }

        // GET: transactions/details?id=1
        [HttpGet("details")]
        public BaseResponse Details(int id)
        {
            Transaction transaction = _applicationDbCotext.Transactions.FirstOrDefault(s => s.Id == id);
            Category category = _applicationDbCotext.Categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
            // Set transactions to null to stop json cycle
            transaction.Category.Transactions = null;
            BaseResponse response = new BaseResponse();
            if (transaction != null)
            {
                response.Status = "success";
                response.Message = "Transaction retrieved successfully";
                response.Data = transaction;
            }
            
            return response;
        }

        // POST: transactions/create
        [HttpPost("create")]
        public BaseResponse Create([FromBody] Transaction formBody)
        {
            _applicationDbCotext.Transactions.Add(formBody);
            _applicationDbCotext.SaveChanges();

            BaseResponse response = new BaseResponse();
            response.Status = "success";
            response.Message = "Transaction added successfully";
            return response;
        }

        // POST: transactions/edit?id=1
        [HttpPost("edit")]
        public BaseResponse Edit(int id, [FromBody] Transaction formBody)
        {
            var transaction = _applicationDbCotext.Categories.FirstOrDefault(s => s.Id == id);
            BaseResponse response = new BaseResponse();
            if (transaction != null)
            {
                formBody.Id = id;
                _applicationDbCotext.Entry(transaction).CurrentValues.SetValues(formBody);
                _applicationDbCotext.SaveChanges();

                response.Status = "success";
                response.Message = "Transaction updated successfully";
            }
            else
            {
                response.Status = "error";
                response.Message = "Transaction not found";
            }
            return response;
        }

        // POST: transactions/delete?id=1
        [HttpPost("delete")]
        public BaseResponse Delete(int id)
        {
            var transaction = _applicationDbCotext.Transactions.FirstOrDefault(s => s.Id == id);
            BaseResponse response = new BaseResponse();
            if (transaction != null)
            {
                _applicationDbCotext.Remove(transaction);
                _applicationDbCotext.SaveChanges();

                response.Status = "success";
                response.Message = "Transaction deleted successfully";
            }
            else
            {
                response.Status = "error";
                response.Message = "Transaction not found";
            }
            return response;
        }
    }
}
