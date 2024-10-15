using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.OrderEntity;
using Store.Service.PaymentService;
using Store.Service.Services.basketService.CustomerBasketDto;
using Store.Web.Controllers;
using Stripe;


public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;
    const string endpointSecret = "whsec_bf63bbdb352228f880e3312850a785e7cf0a5969648063fdf6cdfb3ab6c93ba7";
    public PaymentController(IPaymentService paymentService ,ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(CustomerBasketDto input)
        => Ok(await _paymentService.CreateOrUpdatePaymentIntent(input));

    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {

            var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
            PaymentIntent paymentIntent;
            if (stripeEvent.Type == "Payment_intent.payment_failed")
            { paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                _logger.LogInformation("Payment Failed :", paymentIntent.Id);
               var order= await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                _logger.LogInformation("order update to payment failed :",order.Id);

            }
            else if (stripeEvent.Type == "Payment_intent.succeeded")
            {
                if (stripeEvent.Type == "Payment_intent.payment_succeeded")
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment succeeded :", paymentIntent.Id);
                    var order = await _paymentService.UpdateOrderPaymentSuccessed(paymentIntent.Id);
                    _logger.LogInformation("order update to payment succeeded :", order.Id);

                }
            }
            else if (stripeEvent.Type == "Payment_intent.created")
            {
                _logger.LogInformation("Payment created");

            }

            else
            {
                Console.WriteLine("Unhandled event type:{0}", stripeEvent.Type);

            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }

    }
}
