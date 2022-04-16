using System;
using System.Data;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();


            DataTable tblTransactions = DataStore.Tables["Transactions"];
            DataRow[] results;

            // Get daily transaction total for last 7 days
            for (int i = 0; i < 7; i++)
            {
                double totalDayIncome = 0;
                double totalDayExpense = 0;
                results = tblTransactions.Select("Date = '" + DateTime.Today.AddDays(-1 * i) + "'");
                foreach (DataRow row in results)
                {
                    if (row["Type"].ToString() == "Income")
                    {
                        totalDayIncome += double.Parse(row["Amount"].ToString());
                    }
                    else
                    {
                        totalDayExpense += double.Parse(row["Amount"].ToString());
                    }
                }

                string[] listItem = new string[3];
                listItem[0] = DateTime.Today.AddDays(-1 * i).ToString("d");
                listItem[1] = totalDayIncome.ToString();
                listItem[2] = totalDayExpense.ToString();

                ListViewItem item = new ListViewItem(listItem);
                listReport.Items.Add(item);
            }

            double nextWeekIncome = 0;
            double nextWeekExpense = 0;
            // Get total recurring
            results = tblTransactions.Select("IsRecurring = 1");
            foreach (DataRow row in results)
            {
                if (row["Type"].ToString() == "Income")
                {
                    nextWeekIncome += double.Parse(row["Amount"].ToString());
                }
                else
                {
                    nextWeekExpense += double.Parse(row["Amount"].ToString());
                }
            }

            // Get total for last week
            double lastWeekNonRecurringIncome = 0;
            double lastWeekNonRecurringExpense = 0;
            DateTime today = DateTime.Today;
            DateTime lastWeekStart = DateTime.Today.AddDays(-(int)today.DayOfWeek - 7);
            DateTime lastWeekEnd = DateTime.Today.AddDays(-(int)today.DayOfWeek);
            results = tblTransactions.Select("IsRecurring = 0 AND Date >= '" + lastWeekStart + "' AND Date < '" + lastWeekEnd + "'");
            foreach (DataRow row in results)
            {
                if (row["Type"].ToString() == "Income")
                {
                    lastWeekNonRecurringIncome += double.Parse(row["Amount"].ToString());
                }
                else
                {
                    lastWeekNonRecurringExpense += double.Parse(row["Amount"].ToString());
                }
            }

            // Assume 80% for next week
            nextWeekExpense += lastWeekNonRecurringExpense * 0.8;
            nextWeekIncome += lastWeekNonRecurringIncome * 0.8;

            lblExpense.Text = nextWeekExpense.ToString();
            lblIncome.Text = nextWeekIncome.ToString();
        }
    }
}
