using Finance_App.Api;
using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class AddCategoryForm : Form
    {
        public AddCategoryForm()
        {
            InitializeComponent();
        }

        private void AddCategory(object sender, EventArgs e)
        {
            // Validations
            if (txtCategoryName.Text == "" || cmbCategoryType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all the data fields!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Create new category object
            Category category = new Category();
            category.Title = txtCategoryName.Text;
            category.Type = cmbCategoryType.SelectedItem.ToString();

            // Api call
            CategoriesApiClient client = new CategoriesApiClient();
            BaseResponse response = client.CreateCategory(category);
            if (response == null)
            {
                MessageBox.Show("Category add failed!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
