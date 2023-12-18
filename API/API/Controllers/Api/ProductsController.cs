//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Infraestructure.Entitys;
using Infraestructure.Models;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Api
{

    [Route("api/Products")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsServices I_ProductServices;

        public ProductsController(IProductsServices _I_ProductServices)
        {
            this.I_ProductServices = _I_ProductServices;
        }

        [HttpGet("ProductList")]
        public async Task<ActionResult<ResultModel<Products[]>>> ProductList()
        {
            var Result = await I_ProductServices.ProductsList();

            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("ProductAdd")]
        public async Task<ActionResult<ResultModel<string>>> RolAdd([FromBody] Products ProductModel)
        {
            var Result = await I_ProductServices.ProductsAdd(ProductModel);

            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("GetProductByProductId")]
        public async Task<ActionResult<ResultModel<Products>>> GetProductByProductId([FromBody] int ProductId)
        {
            var Result = await I_ProductServices.GetProductsByProductsId(ProductId);

            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPut("ProductUpdt")]
        public async Task<ActionResult<ResultModel<string>>> ProductUpdt([FromBody] Products ProductModel)
        {
            var Result = await I_ProductServices.ProductsUpdate(ProductModel);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

    }
}
