using System.Collections.Generic;

namespace Finance_App_Service.REST
{
    public class ReportResponse : BaseResponse
    {
        private ICollection<DailyRecord> dailyRecords;

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
