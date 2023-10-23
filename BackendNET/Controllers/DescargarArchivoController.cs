using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.Controllers
{
    [ApiController]
    [Route("file")]
    public class DescargarArchivoController : ControllerBase
    {
        [HttpPost("downloadFile")]
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
                    downloadFileResponse response = await client.downloadFileAsync(request);
                    if (response == null || response.@return == null || response?.@return == null)
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
                    byte[] fileContent = response.@return.fileData;
                    string fileBase64 = Convert.ToBase64String(fileContent);
                    return Ok(new { FileBase64String = fileBase64 });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
        }
    }
}
