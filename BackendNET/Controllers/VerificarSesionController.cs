using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.Controllers
{
    public class VerifySession
    {
        public string token { get; set; } = null!;
    }

    [ApiController]
    [Route("auth")]
    public class VerificarSesionController : ControllerBase
    {
        [HttpPost("verifySession")]
        public async Task<IActionResult> Post([FromBody] VerifySession request)
        {
            if (request == null || string.IsNullOrEmpty(request.token))
            {
                BadRequest();
            }
            await using (var client = new ServiceClient())
            {
                try
                {
                    verifySessionResponse response = await client.verifySessionAsync(request?.token);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    if (response.@return.statusCode != 202)
                    {
                        return BadRequest();
                    }
                    return StatusCode(StatusCodes.Status202Accepted, new { Msg = response.@return, Code = response.@return.statusCode });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
        }
    }
}
