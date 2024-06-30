using Microsoft.AspNetCore.Mvc;
using University_of_free_state_booking_Facilities.Models.ViewModels;

using Stripe;
using Microsoft.Extensions.Options;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Controllers
{


    public class PaymentController : Controller
    {
        private readonly IOptions<StripeSettings> _stripeSettings;

        public PaymentController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings;
            StripeConfiguration.ApiKey = _stripeSettings.Value.SecretKey;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessPayment(CardViewModel model)
        {
          
            StripeConfiguration.ApiKey = "sk_test_51NmOUNIBk1oH8xoDI1tOrp418VP9xk1NmqZhh1IbFBRKe4jrrrvzJc6tqsf5NngcLfxsWsnpoMnPhqMphXij8G2f00Z4VtuOHx";

            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(model.Amount * 100), 
                    Currency = "zar", 
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = "Payment for your order",
                    CaptureMethod = "automatic",
                };

              

                return View("PaymentSuccess");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("PaymentError");
            }
        }
    }

}
