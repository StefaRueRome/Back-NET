using Microsoft.AspNetCore.Mvc;
using ReferenciaActualizada;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("auth")]
    public class RegistroController : ControllerBase
    {
        [HttpPost("register")]
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
                    registerResponse response = await client.registerAsync(request);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    if (response.@return.statusCode != 201)
                    {
                        string errorMessage = response.@return.details;
                        if (errorMessage.Contains("Usuario no registrado"))
                        {
                            return StatusCode(StatusCodes.Status401Unauthorized, new { Msg = errorMessage });
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
