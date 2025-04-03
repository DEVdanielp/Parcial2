using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Parcial2.Models;

namespace Parcial2.Clases
{
    public class clsCliente
    {
        private DBExamenEntities bdExam = new DBExamenEntities();
        private Cliente cliente { get; set; }



        public Cliente ConsultarXDocumento(string Documento)
        {
            return bdExam.Clientes.FirstOrDefault(e => e.Documento == Documento);
        }

        public string Insertar()
        {
            try
            {
                bdExam.Clientes.Add(cliente); //Agrega un objeto empleado a la lista de "empleadoes"
                bdExam.SaveChanges(); // Guardar los cambios en la base de datos
                return "Cliente guardado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar el cliente " + ex.Message;
            }
        }

        public IQueryable ListarImagenesXCliente(int idCliente)
        {
            using (var context = new DBExamenEntities())
            {
                return from P in context.Set<Prenda>()
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
            }
        }

    }


}
