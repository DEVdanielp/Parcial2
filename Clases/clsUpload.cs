using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Parcial2.Models;

namespace Parcial2.Clases
{
    public class clsUpload
    {
        public string Datos { get; set; }
        public string Datos1 { get; set; }
        public HttpRequestMessage request { get; set; }

        private List<string> Archivos;
        private DBExamenEntities bdExam = new DBExamenEntities();

        public async Task<HttpResponseMessage> GrabarArchivo(bool Actualizar)
        {
            try
            {
                if (!request.Content.IsMimeMultipartContent())
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se envió un archivo para procesar");
                }

                string root = HttpContext.Current.Server.MapPath("~/Archivos");
                var provider = new MultipartFormDataStreamProvider(root);


                bool Existe = false;
                //Lee el contenido de los archivos
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    Archivos = new List<string>();
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileName = file.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        if (File.Exists(Path.Combine(root, fileName)))
                        {
                            if (Actualizar)
                            {
                                File.Delete(Path.Combine(root, fileName));
                                //Actualizar el nombre del primer archivo
                                File.Move(file.LocalFileName, Path.Combine(root, fileName));
                                return request.CreateResponse(System.Net.HttpStatusCode.OK, "Se actualizó la imagen");
                            }
                            else
                            {
                                //Opciones si un archivo ya existe, no se va a cargar, se va a eliminar el temporal y se devolverá el error

                                File.Delete(Path.Combine(root, file.LocalFileName));
                                Existe = true;
                            }

                        }
                        else
                        {
                            if (!Actualizar)
                            {
                                Existe = false;
                                Archivos.Add(fileName); //Agrego en una lista el nombre de los archivos que se cargaron
                                File.Move(file.LocalFileName, Path.Combine(root, fileName)); // lo renombra
                            }
                            else
                            {
                                File.Delete(Path.Combine(root, file.LocalFileName));
                                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "El archivo no existe se debe agregar");
                            }
                        }
                    }
                    if (!Existe)
                    {
                        //Proceso de gestión en la base de datos
                        string RptaBD = ProcesarBD();
                        // Termina el ciclo y se muestra un mensaje de éxito
                        return request.CreateResponse(System.Net.HttpStatusCode.OK, "Se cargaron los archivos en el servidor" + RptaBD);
                    }
                    else
                    {
                        return request.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, "El archivo ya existe");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se envió un archivo para procesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        private string ProcesarBD()
        {
            clsPrenda prenda = new clsPrenda();
            return prenda.GrabarImagenPrenda(Convert.ToInt32(Datos), Convert.ToInt32(Datos1),Archivos);
        }

    }
}