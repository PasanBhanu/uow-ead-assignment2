using Finance_App_Service.Data;
using Finance_App_Service.REST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Finance_App_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ApplicationDbContext _applicationDbCotext;

        public CategoriesController(ILogger<CategoriesController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbCotext = applicationDbContext;
        }

        // GET: categories
        [HttpGet]
        public BaseResponse Index()
        {
            IEnumerable<Category> categories = _applicationDbCotext.Categories.ToList();
            BaseResponse response = new BaseResponse
            {
                Status = "success",
                Message = "Categories retrieved successfully",
                Data = categories
            };
            return response;
        }

        // GET: categories/details?id=1
        [HttpGet("details")]
        public BaseResponse Details(int id)
        {
            Category category = _applicationDbCotext.Categories.FirstOrDefault(s => s.Id == id);
            BaseResponse response = new BaseResponse
            {
                Status = "success",
                Message = "Category retrieved successfully",
                Data = category
            };
            return response;
        }

        // POST: categories/create
        [HttpPost("create")]
        public BaseResponse Create([FromBody] Category formBody)
        {
            BaseResponse response = new BaseResponse();
            int existingCategoryCount = _applicationDbCotext.Categories.Where(c => c.Title == formBody.Title).Count();
            if (existingCategoryCount > 0)
            {
                response.Status = "error";
                response.Message = "Category with the same name already exists. Please use a different name!";
            }
            else
            {
                _applicationDbCotext.Categories.Add(formBody);
                _applicationDbCotext.SaveChanges();

                response.Status = "success";
                response.Message = "Category added successfully";
            }
            return response;
        }

        // POST: categories/edit?id=1
        [HttpPost("edit")]
        public BaseResponse Edit(int id, [FromBody] Category formBody)
        {
            var category = _applicationDbCotext.Categories.FirstOrDefault(s => s.Id == id);
            BaseResponse response = new BaseResponse();
            if (category != null)
            {
                int existingCategoryCount = _applicationDbCotext.Categories.Where(c => c.Title == formBody.Title).Where(c => c.Id != id).Count();
                if (existingCategoryCount > 0)
                {
                    response.Status = "error";
                    response.Message = "Category with the same name already exists. Please use a different name!";
                }
                else
                {
                    formBody.Id = id;
                    _applicationDbCotext.Entry(category).CurrentValues.SetValues(formBody);
                    _applicationDbCotext.SaveChanges();

                    response.Status = "success";
                    response.Message = "Category updated successfully";
                }
            }
            else
            {
                response.Status = "error";
                response.Message = "Category not found";
            }
            return response;
        }

        // POST: categories/delete?id=1
        [HttpPost("delete")]
        public BaseResponse Delete(int id)
        {
            var category = _applicationDbCotext.Categories.FirstOrDefault(s => s.Id == id);
            BaseResponse response = new BaseResponse();
            if (category != null)
            {
                int existingCategoryCount = _applicationDbCotext.Transactions.Where(c => c.Category.Id == id).Count();
                if (existingCategoryCount > 0)
                {
                    response.Status = "error";
                    response.Message = "This category has transactions. Delete not permitted!";
                }
                else
                {
                    _applicationDbCotext.Remove(category);
                    _applicationDbCotext.SaveChanges();

                    response.Status = "success";
                    response.Message = "Category deleted successfully";
                }
            }
            else
            {
                response.Status = "error";
                response.Message = "Category not found";
            }
            return response;
        }
    }
}
