using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BraintreeController : ControllerBase
    {
        BraintreeGateway gateway;

        public BraintreeController()
        {
            gateway = new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "2p74nh8g8w24swnd",
                PublicKey = "d4hbp8tbx8yryk3j",
                PrivateKey = "fecf8ebde9e0da0f62d6fa1f3e2a9f99"
            };
        }

        // GET: api/<BraintreeController>
        [HttpGet]
        public string Get()
        {
            // Return client token
            var clientToken = gateway.ClientToken.Generate();
            return clientToken;
        }

        // POST api/<BraintreeController>
        [HttpPost]
        public IActionResult Post([FromBody] BraintreeViewModel value)
        {
            var nonce = value.Nonce;
            var amount = value.Amount;
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(amount),
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var result = gateway.Transaction.Sale(request);
            if (result.Target.ProcessorResponseText.Equals("Approved"))
                return Ok(result);
            else
                return NotFound();
        }
    }
}
