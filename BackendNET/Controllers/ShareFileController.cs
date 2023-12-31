﻿using Microsoft.AspNetCore.Mvc;
using ReferenciaActualizada;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("share")]
    public class ShareFileController : Controller
    {

        [HttpPost("shareFile")]
        public async Task<IActionResult> Share([FromBody] string email, int fileId)
        {
            if (email == null || fileId == null)
            {
                return BadRequest();
            }

            await using (var client = new ServiceClient())
            {
                try
                {
                    shareFileResponse response = await client.shareFileAsync(email, fileId);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    if (response.@return.statusCode != 201)
                    {
                        string errorMessage = response.@return.details;
                        if (errorMessage.Contains("Error"))
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new { Msg = errorMessage });
                        }
                        return BadRequest();
                    }
                    return StatusCode(StatusCodes.Status200OK, new { Msg = response.@return, Code = response.@return.statusCode });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
        }
    }
}
