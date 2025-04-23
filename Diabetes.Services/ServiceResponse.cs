﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Services
{
    public class ServiceResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }

            public ServiceResponse(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }  
}
