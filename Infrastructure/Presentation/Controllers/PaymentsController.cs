using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager serviceManager) : ApiControllerBase
    {
        [HttpPost("{basketId}")] // POST: BaseUrl/Payments/basket01
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePayment(string basketId)
            => Ok(await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
    }
}