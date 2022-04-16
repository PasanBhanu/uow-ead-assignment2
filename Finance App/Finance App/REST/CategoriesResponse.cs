using Finance_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_App.REST
{
    internal class CategoriesResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Category[] Data { get; set; }
    }
}
