
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Parcial2.Clases;

namespace Parcial2.Controllers
{
    [RoutePrefix("api/Archivos")]
    public class UploadController : ApiController
    {

        [HttpPost]
        [Route("CargarArchivo")]
        public async Task<HttpResponseMessage> CargarArchivo(HttpRequestMessage request, string Datos, string Datos1)
        {
            clsUpload upload = new clsUpload();
            upload.Datos = Datos;
            upload.Datos1 = Datos1;
            upload.request = request;
            return await upload.GrabarArchivo(false);
        }

        [HttpPost]
        [Route("ActualizarArchivo")]
        public async Task<HttpResponseMessage> ActualizarArchivo(HttpRequestMessage request, string Datos)
        {
            clsUpload upload = new clsUpload();
            upload.Datos = Datos;
            upload.request = request;
            return await upload.GrabarArchivo(true);
        }
    }
}