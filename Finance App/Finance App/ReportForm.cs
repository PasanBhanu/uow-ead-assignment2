using Finance_App.Api;
using Finance_App.REST;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();

            ReportsApiClient client = new ReportsApiClient();
            ReportResponse response = client.GetReport();

            foreach (ReportResponse.DailyRecord report in response.DailyRecords)
            {
                string[] listItem = new string[3];
                listItem[0] = report.Date;
                listItem[1] = report.Income;
                listItem[2] = report.Expense;

                ListViewItem item = new ListViewItem(listItem);
                listReport.Items.Add(item);
            }

            lblExpense.Text = response.NextWeekExpense;
            lblIncome.Text = response.NextWeekIncome;
        }
    }
}
