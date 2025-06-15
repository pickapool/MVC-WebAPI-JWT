﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
