using Finance_App.Api;
using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class ManageCategoryForm : Form
    {
        Category category = new Category();

        public ManageCategoryForm(int id)
        {
            InitializeComponent();

            // Load category object
            CategoriesApiClient client = new CategoriesApiClient();
            category = client.GetCategory(id);

            txtCategoryName.Text = category.Title;
            cmbCategoryType.Text = category.Type.ToString();
        }

        private void UpdateCategory(object sender, EventArgs e)
        {
            // Validations
            if (txtCategoryName.Text == "" || cmbCategoryType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all the data fields!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Update category object
            category.Title = txtCategoryName.Text;
            category.Type = cmbCategoryType.SelectedItem.ToString();

            // Api call
            CategoriesApiClient client = new CategoriesApiClient();
            BaseResponse response = client.UpdateCategory(category);
            if (response == null)
            {
                MessageBox.Show("Category update failed!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
