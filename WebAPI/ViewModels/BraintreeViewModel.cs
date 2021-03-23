using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class BraintreeViewModel
    {
        public double Amount { get; set; }
        public string Nonce { get; set; }
    }
}
