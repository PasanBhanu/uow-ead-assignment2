using Finance_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class AddTransactionForm : Form
    {
        DataStore DataStore = Variables.dataStore;

        public AddTransactionForm()
        {
            InitializeComponent();

            // Load categories from DB
            DataTable tblCategories = DataStore.Tables["Categories"];
            foreach (DataRow row in tblCategories.Rows)
            {
                cmbCategory.Items.Add(row["Title"].ToString());
            }
        }

        private void AddTransaction(object sender, EventArgs e)
        {
            // Validations
            if (txtDescription.Text == "" || txtAmount.Text == "" || cmbCategory.SelectedIndex == -1 || cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all the data fields!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Create new transaction object
            Transaction transaction = new Transaction();
            transaction.Description = txtDescription.Text;
            transaction.Amount = double.Parse(txtAmount.Text);
            transaction.Date = DateTime.Parse(dtpDate.Text);
            transaction.IsReccuring = chkRecurring.Checked;
            transaction.Type = cmbTransactionType.SelectedItem.ToString();
            // Get category id by title
            DataTable tblCategories = DataStore.Tables["Categories"];
            DataRow[] results = tblCategories.Select("Title = '" + cmbCategory.Text +"'");
            foreach (DataRow row in results)
            {
                transaction.CategoryId = (int)row["Id"];
            }
            //transaction.CreateDatasetRow(DataStore);

            MessageBox.Show("Transaction added successfully!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void AmountInlineValidation(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
