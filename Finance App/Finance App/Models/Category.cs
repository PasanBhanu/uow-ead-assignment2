using Finance_App.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Finance_App.Models
{
    internal class Category
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
