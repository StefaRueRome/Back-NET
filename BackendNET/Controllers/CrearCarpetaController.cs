using Microsoft.AspNetCore.Mvc;
using ReferenciaActualizada;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("folder")]
    public class CrearCarpetaController : ControllerBase
    {
        [HttpPost("createFolder")]
        public async Task<IActionResult> Post([FromBody] folder request)
        {
            if (request == null)
            {
                BadRequest();
            }
            await using (var client = new ServiceClient())
            {
                try
                {
                    createFolderResponse response = await client.createFolderAsync(request);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    if (response.@return.statusCode != 200)
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
