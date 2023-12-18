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

    [Route("api/Status")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StatusController : ControllerBase
    {
        private readonly IStatusServices I_StatusServices;

        public StatusController(IStatusServices _I_StatusServices)
        {
            this.I_StatusServices = _I_StatusServices;
        }

        [HttpGet("StatusList")]
        public async Task<ActionResult<ResultModel<Status[]>>> StatusList()
        {
            var Result = await I_StatusServices.StatusList();
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("StatusAdd")]
        public async Task<ActionResult<ResultModel<string>>> RolAdd([FromBody] Status StatusModel)
        {
            var Result = await I_StatusServices.StatusAdd(StatusModel);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPost("GetStatusByStatusId")]
        public async Task<ActionResult<ResultModel<Status>>> GetStatusByStatusId([FromBody] int StatusId)
        {
            var Result = await I_StatusServices.GetStatusByStatusId(StatusId);
            if (Result.HasError)
            {
                return BadRequest(Result);
            }
            else
            {
                return Ok(Result);
            }
        }

        [HttpPut("StatusUpdt")]
        public async Task<ActionResult<ResultModel<string>>> StatusUpdt([FromBody] Status StatusModel)
        {
            var Result = await I_StatusServices.StatusUpdate(StatusModel);
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
