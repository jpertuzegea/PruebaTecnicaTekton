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
    [Route("api/Users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserServices I_UserServices;

        public UserController(IUserServices _I_UserServices)
        {
            this.I_UserServices = _I_UserServices;
        }

        [HttpPost("UserAdd")]
        public async Task<ActionResult<ResultModel<string>>> UserAdd([FromBody] Users UserModel)
        {
            var Result = await I_UserServices.UserAdd(UserModel);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpGet("UserList")]
        public async Task<ActionResult<ResultModel<Users[]>>> UserList()
        {
            var Result = await I_UserServices.UserList();
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("GetUserByUserId")]
        public async Task<ActionResult<ResultModel<Users>>> GetUserByUserId([FromBody] int UserId)
        {
            var Result = await I_UserServices.GetUserByUserId(UserId);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPut("UserUpdate")]
        public async Task<ActionResult<ResultModel<string>>> UserUpdate([FromBody] Users UserModel)
        {
            var Result = await I_UserServices.UserUpdate(UserModel);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }


        [HttpGet("DeleteCacheSecurity")]
        public async Task<ActionResult<ResultModel<string>>> DeleteCacheSecurity()
        {
            var Result = await I_UserServices.DeleteCacheSecurity();
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel<LoginModel>>> LogIn([FromBody] LoginModel LoginModel)
        {
            var Result = await I_UserServices.LogIn(LoginModel);
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