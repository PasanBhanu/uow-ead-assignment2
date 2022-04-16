using Finance_App_Service.Data;
using Finance_App_Service.REST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance_App_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly ApplicationDbContext _applicationDbCotext;

        public ReportsController(ILogger<ReportsController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbCotext = applicationDbContext;
        }

        // GET: reports
        [HttpGet]
        public ReportResponse Index()
        {
            ReportResponse response = new ReportResponse();
            
            List<Transaction> transactions;

            response.DailyRecords = new List<ReportResponse.DailyRecord>();

            // Get daily transaction total for last 7 days
            for (int i = 0; i < 7; i++)
            {
                double totalDayIncome = 0;
                double totalDayExpense = 0;
                transactions = _applicationDbCotext.Transactions.Where(a => a.Date == DateTime.Today.AddDays(-1 * i)).ToList();

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Type == "Income")
                    {
                        totalDayIncome += transaction.Amount;
                    }
                    else
                    {
                        totalDayExpense += transaction.Amount;
                    }
                }

                response.DailyRecords.Add(new ReportResponse.DailyRecord
                {
                    Date = DateTime.Today.AddDays(-1 * i).ToString(),
                    Expense = totalDayExpense.ToString(),
                    Income = totalDayIncome.ToString()
                });
            }

            double nextWeekIncome = 0;
            double nextWeekExpense = 0;
            // Get total recurring
            transactions = _applicationDbCotext.Transactions.Where(a => a.IsReccuring == true).ToList();
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Type == "Income")
                {
                    nextWeekIncome += transaction.Amount;
                }
                else
                {
                    nextWeekExpense += transaction.Amount;
                }
            }

            // Get total for last week
            double lastWeekNonRecurringIncome = 0;
            double lastWeekNonRecurringExpense = 0;
            DateTime today = DateTime.Today;
            DateTime lastWeekStart = DateTime.Today.AddDays(-(int)today.DayOfWeek - 7);
            DateTime lastWeekEnd = DateTime.Today.AddDays(-(int)today.DayOfWeek);
            transactions = _applicationDbCotext.Transactions
                .Where(a => a.IsReccuring == false)
                .Where(a => a.Date >= lastWeekStart)
                .Where(a => a.Date < lastWeekEnd)
                .ToList();
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Type == "Income")
                {
                    lastWeekNonRecurringIncome += transaction.Amount;
                }
                else
                {
                    lastWeekNonRecurringExpense += transaction.Amount;
                }
            }

            // Assume 80% for next week
            nextWeekExpense += lastWeekNonRecurringExpense * 0.8;
            nextWeekIncome += lastWeekNonRecurringIncome * 0.8;

            response.NextWeekExpense = nextWeekExpense.ToString();
            response.NextWeekIncome = nextWeekIncome.ToString();

            response.Status = "success";
            response.Message = "Report generated successfully";
            return response;
        }
    }
}
