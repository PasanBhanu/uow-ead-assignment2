using Finance_App.Api;
using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class CategoriesForm : Form
    {
        public CategoriesForm()
        {
            InitializeComponent();
        }

        private void AddCategory(object sender, EventArgs e)
        {
            AddCategoryForm addCategoryForm = new AddCategoryForm();
            addCategoryForm.ShowDialog();
            addCategoryForm.Dispose();
            LoadCategories();
        }

        private void EditCategory(object sender, EventArgs e)
        {
            int id = int.Parse(listCategories.SelectedItems[0].SubItems[0].Text);
            ManageCategoryForm manageCategoryForm = new ManageCategoryForm(id);
            manageCategoryForm.ShowDialog();
            manageCategoryForm.Dispose();
            LoadCategories();
        }

        private void DeleteCategory(object sender, EventArgs e)
        {
            int id = int.Parse(listCategories.SelectedItems[0].SubItems[0].Text);

            // Api call
            CategoriesApiClient client = new CategoriesApiClient();
            BaseResponse response = client.DeleteCategory(id);
            if (response == null)
            {
                MessageBox.Show("Category delete failed!", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            LoadCategories();
        }

        private void LoadFormData(object sender, EventArgs e)
        {
            LoadCategories();
        }

        public void LoadCategories()
        {
            CategoriesApiClient client = new CategoriesApiClient();

            listCategories.Items.Clear();
            foreach (Category category in client.GetCategories())
            {
                string[] listItem = new string[3];
                listItem[0] = category.Id.ToString();
                listItem[1] = category.Title.ToString();
                listItem[2] = category.Type.ToString();

                ListViewItem item = new ListViewItem(listItem);
                listCategories.Items.Add(item);
            }

            btnEditCategory.Enabled = false;
            btnDeleteCategory.Enabled = false;
        }

        private void DisplayCategoryOptions(object sender, EventArgs e)
        {
            if (listCategories.SelectedItems.Count == 1)
            {
                btnEditCategory.Enabled = true;
                btnDeleteCategory.Enabled = true;
            } else
            {
                btnEditCategory.Enabled = false;
                btnDeleteCategory.Enabled = false;
            }
        }
    }
}
