using ShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWebApp.Models.ViewModels
{
    public class LoginVM
    {
        public Login Credential { get; set; }

        public string ReturnUrl { get; set; }
    }
}
