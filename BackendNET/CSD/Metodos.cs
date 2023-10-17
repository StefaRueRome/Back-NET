﻿using Microsoft.AspNetCore.Mvc;
using MiReferencia;

namespace BackendNET.CSD
{
    [ApiController]
    [Route("api/")]
    public class Metodos:ControllerBase
    {
        [HttpPost("registro")]
        public async Task<string> Registro(user User)
        {
            registerResponse usuarioRespuesta = new registerResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.registerAsync(User);

            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return ("Usuario registrado exitosamente");

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }

        [HttpPost("login")]
        public async Task<string> Login(user User)
        {

            loginResponse usuarioRespuesta = new loginResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.loginAsync(User);
            string token;
            token = usuarioRespuesta.@return.json;


            if (usuarioRespuesta.@return.statusCode == 201)
            {
                Console.WriteLine("Ingreso exitoso");
                return (token);

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }


        }

        [HttpPost("verificarSesion")]
        public async Task<string> verificarSesion(string token)
        {
            verifySessionResponse usuarioRespuesta = new verifySessionResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.verifySessionAsync(token);


            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return (usuarioRespuesta.@return.json);

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }

        [HttpPost("crearCarpeta")]
        public async Task<string> crearCarpeta(folder carpeta)
        {
            createFolderResponse usuarioRespuesta = new createFolderResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.createFolderAsync(carpeta);


            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return ("Carpeta creada");

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }

        [HttpPost("subirArchivo")]
        public async Task<string> subirArchivo(file archivo)
        {
            uploadFileResponse usuarioRespuesta = new uploadFileResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.uploadFileAsync(archivo);


            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return ("Archivo subido");

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }

        [HttpPut("moverArchivo")]
        public async Task<string> moverArchivo(string ruta, int idArchivo)
        {
            moveFileResponse usuarioRespuesta = new moveFileResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.moveFileAsync(ruta, idArchivo);

            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return ("El archivo se ha movido correctamente");

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }
        [HttpPut("eliminarArchivos")]
        public async Task<string> eliminarArchivo(file archivo)
        {
            deleteFileResponse usuarioRespuesta = new deleteFileResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.deleteFileAsync(archivo);

            if (usuarioRespuesta.@return.statusCode == 201)
            {
                return ("El archivo se ha eliminado correctamente");

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }

        [HttpGet("obtenerArchivos")]
        public async Task<string> obtenerArchivosUsuario(int idUser)
        {
            getUserFilesResponse usuarioRespuesta = new getUserFilesResponse();
            MiReferencia.ServiceClient oRegistro = new MiReferencia.ServiceClient();
            usuarioRespuesta = await oRegistro.getUserFilesAsync(idUser);

            if (usuarioRespuesta.@return.statusCode == 201)
            {
                Console.WriteLine("Operación exitosa");
                return (usuarioRespuesta.@return.json);

            }
            else if (usuarioRespuesta.@return.statusCode == 400)
            {
                return (usuarioRespuesta.@return.details);
            }
            else
            {
                return ("Respuesta inesperada");
            }



        }
    }
}
