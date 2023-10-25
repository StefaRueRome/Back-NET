using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("file")]
    public class ObtenerArchivosController : ControllerBase
    {
        [HttpGet("getFiles")]
        public async Task<IActionResult> Get([FromBody] user request)
        {
            if (request == null)
            {
                BadRequest();
            }
            await using (var client = new ServiceClient())
            {
                try
                {
                    loginResponse response = await client.getUserFilesAsync(int id);
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
