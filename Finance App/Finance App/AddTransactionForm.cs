﻿using Finance_App.Api;
using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class AddTransactionForm : Form
    {
        Dictionary<String, Category> categories = new Dictionary<String, Category>();

        public AddTransactionForm()
        {
            InitializeComponent();

            // Load categories from DB
            CategoriesApiClient client = new CategoriesApiClient();
            foreach (Category category in client.GetCategories())
            {
                cmbCategory.Items.Add(category.Title);
                categories.Add(category.Title, category);
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
            transaction.CategoryId = categories[cmbCategory.Text].Id;

            // Api call
            TransactionsApiClient client = new TransactionsApiClient();
            BaseResponse response = client.CreateTransaction(transaction);
            if (response == null)
            {
                MessageBox.Show("Transaction add failed!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                if (response.Status == "success")
                {
                    MessageBox.Show(response.Message, "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show(response.Message, "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                }
            }
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
