using Finance_App.Api;
using Finance_App.Models;
using Finance_App.REST;
using Finance_App.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ViewCategories(object sender, EventArgs e)
        {
            CategoriesForm categoriesForm = new CategoriesForm();
            categoriesForm.ShowDialog();
        }

        private void AddTransaction(object sender, EventArgs e)
        {
            AddTransactionForm addTransactionForm = new AddTransactionForm();
            addTransactionForm.ShowDialog();
            addTransactionForm.Dispose();
            LoadTransactions();
        }

        private void EditTransaction(object sender, EventArgs e)
        {
            int id = int.Parse(listTransactions.SelectedItems[0].SubItems[0].Text);
            ManageTransactionForm manageTransactionForm = new ManageTransactionForm(id);
            manageTransactionForm.ShowDialog();
            manageTransactionForm.Dispose();
            LoadTransactions();
        }

        private void DeleteTransaction(object sender, EventArgs e)
        {
            int id = int.Parse(listTransactions.SelectedItems[0].SubItems[0].Text);
            DialogResult dialogResult = MessageBox.Show("Are you sure do you want to delete this transaction?", "Simply Finance App", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Api call
                TransactionsApiClient client = new TransactionsApiClient();
                BaseResponse response = client.DeleteTransaction(id);
                if (response == null)
                {
                    MessageBox.Show("Transaction delete failed!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (response.Status == "success")
                    {
                        MessageBox.Show(response.Message, "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(response.Message, "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                LoadTransactions();
            }
        }

        private void LoadFormData(object sender, EventArgs e)
        {
            // Load xml
            StoreUtil storeUtl = new StoreUtil();
            storeUtl.CheckPendingSyncFlagEnabled();

            Console.WriteLine("Can Go Online : " + Variables.CanGoOnline().ToString());

            // Start worker
            backgroundWorker.RunWorkerAsync();

            // Load transactions
            LoadTransactions();

            // Show login form
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }
        public void LoadTransactions()
        {
            DateTime today = DateTime.Today;
            DateTime weekStart = DateTime.Today.AddDays(-(int)today.DayOfWeek);
            double totalDailyIncome = 0;
            double totalDailyExpense = 0;
            double totalWeeklyIncome = 0;
            double totalWeeklyExpense = 0;

            // Get all transactions
            TransactionsApiClient client = new TransactionsApiClient();

            listTransactions.Items.Clear();
            Transaction[] transactions = client.GetTransactions();
            if (transactions != null)
            {
                foreach (Transaction transaction in transactions)
                {
                    string[] listItem = new string[6];
                    listItem[0] = transaction.Id.ToString();
                    listItem[1] = transaction.Date.ToString("d");
                    listItem[2] = transaction.Description.ToString();
                    listItem[3] = transaction.Category.Title.ToString();
                    listItem[4] = transaction.Type.ToString();
                    listItem[5] = transaction.Amount.ToString();

                    ListViewItem item = new ListViewItem(listItem);
                    listTransactions.Items.Add(item);

                    // Calculate statistics
                    if (transaction.Date == today)
                    {
                        if (transaction.Type.ToString() == "Income")
                        {
                            totalDailyIncome += double.Parse(transaction.Amount.ToString());
                        }
                        else
                        {
                            totalDailyExpense += double.Parse(transaction.Amount.ToString());
                        }
                    }

                    if (transaction.Date >= weekStart)
                    {
                        if (transaction.Type.ToString() == "Expense")
                        {
                            totalWeeklyIncome += double.Parse(transaction.Amount.ToString());
                        }
                        else
                        {
                            totalWeeklyExpense += double.Parse(transaction.Amount.ToString());
                        }
                    }
                }
            }
            
            lblTotalDailyExpense.Text = totalDailyExpense.ToString();
            lblTotalDailyIncome.Text = totalDailyIncome.ToString();
            lblTotalWeeklyIncome.Text = totalWeeklyIncome.ToString();
            lblTotalWeeklyExpense.Text = totalWeeklyExpense.ToString();

            btnEditTransaction.Enabled = false;
            btnDeleteTransaction.Enabled = false;
        }

        private void ViewReport(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            if (!reportForm.IsDisposed)
            {
                reportForm.ShowDialog();
            }
        }

        private void DisplayTransactionOptions(object sender, EventArgs e)
        {
            if (listTransactions.SelectedItems.Count == 1)
            {
                btnEditTransaction.Enabled = true;
                btnDeleteTransaction.Enabled = true;
            }
            else
            {
                btnEditTransaction.Enabled = false;
                btnDeleteTransaction.Enabled = false;
            }
        }

        private void ShowAboutMe(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void ServerHealthCheck(object sender, DoWorkEventArgs e)
        {
            HealthApiClient healthApiClient = new HealthApiClient();
            while (true)
            {
                bool ServerStatus = healthApiClient.GetHealth();
                Variables.SetServerStatus(ServerStatus);
                Console.WriteLine("Server Health : " + ServerStatus.ToString());
                if (ServerStatus)
                {
                    // Server Online
                    if (!Variables.CanGoOnline())
                    {
                        // Start Sync
                        stlServer.Text = "Server : Connected - Pending Syncing";
                        Console.WriteLine("Syncing Started");
                        stlEvent.Text = "Syncting Starting";

                        // Remove pending flags
                        StoreUtil storeUtil = new StoreUtil();
                        storeUtil.RemovePendingSyncFlag();

                        // Sync Categories
                        CategoryStore categoryStore = new CategoryStore();
                        CategoriesApiClient categoriesApiClient = new CategoriesApiClient();

                        // Gat categories from xml
                        Category[] categories = categoryStore.GetCategories(null);

                        foreach (Category category in categories)
                        {
                            stlEvent.Text = "Syncting Category - " + category.Title;
                            if (category.Id > 0)
                            {
                                // Update
                                categoriesApiClient.UpdateCategory(category);
                            }
                            else
                            {
                                // Create
                                Category _category = new Category
                                {
                                    Title = category.Title,
                                    Type = category.Type,
                                };
                                categoriesApiClient.CreateCategory(_category);
                            }
                        }
                        
                        // Load latest category data
                        Dictionary<string, int> categoryMap = new Dictionary<string, int>();
                        Category[] categoriesLatest = categoriesApiClient.GetCategories();
                        foreach (Category category in categoriesLatest)
                        {
                            categoryMap.Add(category.Title, category.Id);
                        }

                        // Sync Transactions
                        TransactionStore transactionStore = new TransactionStore();
                        TransactionsApiClient transactionsApiClient = new TransactionsApiClient();

                        // Get transactions from xml
                        Transaction[] transactions = transactionStore.GetTransactions(null);

                        foreach (Transaction transaction in transactions)
                        {
                            stlEvent.Text = "Syncting Transaction - " + transaction.Description;
                            if (transaction.Id > 0)
                            {
                                // Update transaction
                                Transaction _transaction = new Transaction
                                {
                                    Id = transaction.Id,
                                    Description = transaction.Description,
                                    Type = transaction.Type,
                                    Amount = transaction.Amount,
                                    IsReccuring = transaction.IsReccuring,
                                    Date = transaction.Date,
                                    CategoryId = categoryMap[transaction.Category.Title],
                                    Category = new Category
                                    {
                                        Id = categoryMap[transaction.Category.Title],
                                        Type = transaction.Category.Type,
                                        Title = transaction.Category.Title
                                    }
                                };
                                transactionsApiClient.UpdateTransaction(_transaction);
                            }
                            else
                            {
                                // Create transaction
                                Transaction _transaction = new Transaction
                                {
                                    Description = transaction.Description,
                                    Type = transaction.Type,
                                    Amount = transaction.Amount,
                                    IsReccuring = transaction.IsReccuring,
                                    Date = transaction.Date,
                                    CategoryId = categoryMap[transaction.Category.Title]
                                };
                                transactionsApiClient.CreateTransaction(_transaction);
                            }
                        }

                        // Find deleted transactions
                        int[] deletedTransactionIds = transactionStore.GetDeletedTransactions();
                        if (deletedTransactionIds != null)
                        {
                            foreach (int transactionId in deletedTransactionIds)
                            {
                                stlEvent.Text = "Deleting Transaction - " + transactionId;
                                transactionsApiClient.DeleteTransaction(transactionId);
                            }
                        }

                        stlEvent.Text = "";

                        // Load transactions
                        backgroundWorker.ReportProgress(1, null);
                    }
                    else
                    {
                        stlServer.Text = "Server : Connected";
                    }
                } else
                {
                    stlServer.Text = "Server : Not Connected";
                }

                Thread.Sleep(5000);
            }
        }

        private void UpdateUI(object sender, ProgressChangedEventArgs e)
        {
            LoadTransactions();
        }
    }
}
