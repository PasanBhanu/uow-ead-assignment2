﻿using Finance_App.Models;

namespace Finance_App.REST
{
    internal class CategoriesResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Category[] Data { get; set; }
    }
}
