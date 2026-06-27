using Mansari.Store.Gateway.Services.Aggregation;
using Microsoft.AspNetCore.Mvc;

namespace Mansari.Store.Gateway.Controllers
{
    public class BasketController : Controller
    {
        private BasketAggregationService _basketAggregationService;

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetBasket(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _basketAggregationService.GetBasketAsync(userId, cancellationToken);

            return result.ToActionResult();
        }
    }
}
