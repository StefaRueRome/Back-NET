using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.Controllers
{
    public class MoveFile
    {
        public string token { get; set; } = null!;
    }

    [ApiController]
    [Route("file")]
    public class MoverArchivoController : ControllerBase
    {
        [HttpPost("moveFile")]
        public async Task<IActionResult> Post([FromBody] file request)
        {
            if (request == null)
            {
                BadRequest();
            }
            await using (var client = new ServiceClient())
            {
                try
                {
                    moveFileResponse response = await client.moveFileAsync(request.id, request.path, request.path);
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
