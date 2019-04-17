using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class entityModel
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public bool IsChecked { get; set; }
    }
}