using Microsoft.AspNetCore.Mvc;
using ReferenciaActualizada;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("auth")]
    public class LoginContoller : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] user request)
        {
            if (request == null)
            {
                BadRequest();
            }
            await using (var client = new ServiceClient())
            {
                try
                {
                    loginResponse response = await client.loginAsync(request);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    string errorMessage = response.@return.details;
                    if (errorMessage.Contains("Usuario no registrado"))
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new { Msg = errorMessage });
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
