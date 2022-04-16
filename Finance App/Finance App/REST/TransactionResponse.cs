﻿using Finance_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_App.REST
{
    internal class TransactionResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Transaction Data { get; set; }
    }
}
