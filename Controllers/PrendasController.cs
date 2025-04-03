using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Parcial2.Clases;
using Parcial2.Models;
using System.IO;

namespace Parcial2.Controllers
{
    [RoutePrefix("api/Prendas")]
    public class PrendasController : ApiController
    {
        [HttpPut]
        [Route("Insertar")]
        public string Insertar([FromBody] Prenda prenda,string doc, string nombre, string email, string celular)
        {
            clsPrenda Prenda = new clsPrenda();
            Prenda.prenda = prenda;

            return Prenda.Insertar(doc, nombre, email, celular);
        }


        [HttpGet]
        [Route("ObtenerPrendas")]
        public IHttpActionResult ObtenerPrendas()
        {
            using (var context = new DBExamenEntities())
            {
                var prendas = from P in context.Set<Prenda>()
                              join C in context.Set<Cliente>() on P.Cliente equals C.Documento
                              join FP in context.Set<FotoPrenda>() on P.IdPrenda equals FP.idPrenda
                              select new
                              {
                                  Documento = C.Documento,
                                  Nombre = C.Nombre,
                                  Email = C.Email,
                                  Celular = C.Celular,
                                  TipoPrenda = P.TipoPrenda,
                                  Descripcion = P.Descripcion,
                                  Valor = P.Valor,
                                  FotoPrenda = FP.FotoPrenda1
                              };

                return Ok(prendas.ToList());
            }
        }

        [HttpDelete]
        [Route("EliminarImagen")]
        public IHttpActionResult EliminarImagen(int idImagen)
        {
            using (var bdExam = new DBExamenEntities())
            {
                var imagen = bdExam.FotoPrendas.Find(idImagen);
                if (imagen == null)
                {
                    return NotFound();
                }

                string rutaImagen = HttpContext.Current.Server.MapPath("~/Archivos/" + imagen.FotoPrenda1);

                if (File.Exists(rutaImagen))
                {
                    File.Delete(rutaImagen);
                }

                bdExam.FotoPrendas.Remove(imagen);
                bdExam.SaveChanges();

                return Ok("Imagen eliminada.");
            }
        }



    }




}