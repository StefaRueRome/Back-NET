using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.Controllers
{
    public class FileRequest
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
    public class SubirArchivoController : ControllerBase
    {
        [HttpPost("uploadFile")]
        public async Task<IActionResult> Post([FromBody] FileRequest request)
        {
            if (request == null)
            {
                BadRequest();
            }

            List<byte> totalfileContent = new List<byte>();

            try
            {
                foreach (var base64 in request.fileData)
                {
                    var fileContent = Convert.FromBase64String(base64);
                    totalfileContent.AddRange(fileContent);
                }
            }
            catch (FormatException)
            {
                return BadRequest(new { Msg = "a" });
            }

            var requestFile = new file
            {
                id = request.id,
                name = request.name,
                path = request.path,
                size = request.size,
                userId = request.userId,
                folderId = request.folderId,
                nodeId = request.nodeId,
                fileData = totalfileContent.ToArray(),
            };

            await using (var client = new ServiceClient())
            {
                try
                {
                    uploadFileResponse response = await client.uploadFileAsync(requestFile);
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
