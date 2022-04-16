using Finance_App.Models;
using Finance_App.REST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Finance_App.Xml
{
    internal class CategoryStore
    {
        public Category[] GetCategories(Category[] categories)
        {
            if (categories != null)
            {
                var xmlDoc = XElement.Load("Store.xml");
                var categoriesXml = categories.ToList()
                                    .Select(m => new XElement("Category", new XAttribute("Id", m.Id),
                                        new XAttribute("Title", m.Title),
                                        new XAttribute("Type", m.Type))
                                    ).ToList();
                xmlDoc.Descendants("Categories").Remove();
                xmlDoc.Add(new XElement("Categories", categoriesXml));
                xmlDoc.Save("Store.xml");
            } else
            {
                var xmlDoc = XElement.Load("Store.xml");
                categories = xmlDoc.Descendants("Category")
                    .Select(m => new Category
                    {
                        Id = int.Parse(m.Attribute("Id").Value.ToString()),
                        Title = m.Attribute("Title").Value,
                        Type = m.Attribute("Type").Value
                    }).ToArray();
            }

            return categories;
        }

        public Category GetCategory(int id)
        {
            Category category = null;

            var xmlDoc = XElement.Load("Store.xml");
            category = xmlDoc.Descendants("Category")
                .Where(m => int.Parse(m.Attribute("Id").Value) == id)
                .Select(m => new Category
                {
                    Id = int.Parse(m.Attribute("Id").Value.ToString()),
                    Title = m.Attribute("Title").Value,
                    Type = m.Attribute("Type").Value
                })
                .FirstOrDefault();

            return category;
        }

        public BaseResponse CreateCategory(Category category)
        {
            BaseResponse response = new BaseResponse();

            var xmlDoc = XElement.Load("Store.xml");
            int existingCategoryCount = xmlDoc.Descendants("Category")
                .Where(m => m.Attribute("Title").Value == category.Title)
                .Count();

            if (existingCategoryCount > 0)
            {
                response.Status = "error";
                response.Message = "Category with the same name already exists. Please use a different name!";
            }
            else
            {
                xmlDoc.Add(new XElement("Categories" , new XElement("Category", new XAttribute("Id", category.Id),
                                        new XAttribute("Title", category.Title),
                                        new XAttribute("Type", category.Type))));
                xmlDoc.Save("Store.xml");
                response.Status = "success";
                response.Message = "Category added successfully";
            }

            return response;
        }

        public BaseResponse UpdateCategory(Category category)
        {
            BaseResponse response = new BaseResponse();

            var xmlDoc = XElement.Load("Store.xml");
            int existingCategoryCount = xmlDoc.Descendants("Category")
                .Where(m => m.Attribute("Title").Value == category.Title)
                .Where(m => int.Parse(m.Attribute("Id").Value) == category.Id)
                .Count();

            if (existingCategoryCount > 0)
            {
                response.Status = "error";
                response.Message = "Category with the same name already exists. Please use a different name!";
            }
            else
            {
                var doc = xmlDoc.Descendants("Category")
                    .Where(m => int.Parse(m.Attribute("Id").Value) == category.Id)
                    .First();

                doc.Attribute("Title").Value = category.Title;
                doc.Attribute("Type").Value = category.Type;
                xmlDoc.Save("Store.xml");

                response.Status = "success";
                response.Message = "Category updated successfully";
            }

            return response;
        }
    }
}
