using System.Collections.Generic;

namespace Finance_App.REST
{
    internal class ReportResponse : BaseResponse
    {
        public class DailyRecord
        {
            public string Date { get; set; }
            public string Income { get; set; }
            public string Expense { get; set; }
        }

        public List<DailyRecord> DailyRecords { get; set; }
        public string NextWeekExpense { get; set; }
        public string NextWeekIncome { get; set; }
    }
}
