using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReferenciaActualizada;

namespace BackendNET.Controllers
{
    public class FileR
    {
        public int id { get; set; }

        public string name { get; set; } = null!;

        public string path { get; set; } = null!;

        public List<string> fileData { get; set; } = null!;

        public double size { get; set; }

        public int userId { get; set; }

        public int folderId { get; set; }

        public int nodeId { get; set; }
    }

    [ApiController]
    [Route("file")]
    public class ObtenerArchivosController : ControllerBase
    {
        [HttpGet("getFiles")]
        public async Task<IActionResult> Get([FromBody] int id)
        {

            if (id == null)
            {
                BadRequest();
            }

            await using (var client = new ServiceClient())
            {
                try
                {
                    getUserFilesResponse response = await client.getUserFilesAsync(id);

                    String archivos = response.@return.json;
                    Console.WriteLine(archivos);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { Msg = "La respuesta del SOAP es nula" });
                    }
                    string errorMessage = response.@return.details;
                    if (errorMessage.Contains("El usuario no tiene archivos"))
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new { Msg = errorMessage });
                    }
                    if (!string.IsNullOrEmpty(archivos) && archivos.Length > 2)
                    {

                        FileR resFile = JsonConvert.DeserializeObject<FileR>(archivos);

                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            statusCode = (int)response.@return.statusCode,
                            details = "Operacion exitosa",
                            files = resFile,

                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            statusCode = (int)response.@return.statusCode,
                            details = errorMessage
                        });
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }

            }
        }


    }

}
