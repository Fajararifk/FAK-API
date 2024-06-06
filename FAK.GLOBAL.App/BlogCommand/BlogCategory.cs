﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.BlogCommand
{
    public class BlogCategory
    {
        [Key]
        public string Id { get; set; }
        public string CategoryName { get; set; }
    }
}
