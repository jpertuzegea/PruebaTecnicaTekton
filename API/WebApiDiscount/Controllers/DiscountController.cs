using Microsoft.AspNetCore.Mvc;

namespace WebApiDiscount.Controllers
{
    [Route("api/Discount")]
    public class DiscountController : ControllerBase
    {
        [HttpGet("GetDiscountByProductId")]
        public async Task<ActionResult< int>> GetStatusByStatusId(int productId)
        {
            if (productId > 0)
            {
                var randomNumber = new Random().Next(0, 100);
                return Ok(randomNumber);
            }
            else {
                return BadRequest();
            }
           
        }


    }

}

