using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Parcial2.Models;

namespace Parcial2.Clases
{
    public class clsPrenda
    {
        private DBExamenEntities bdExam = new DBExamenEntities();
        private clsCliente cliente = new clsCliente();
        public Prenda prenda { get; set; }

        public string Insertar(string doc, string nombre, string email, string celular)
        {
           if(cliente.ConsultarXDocumento(prenda.Cliente) == null){
                RedirigirCliente(doc,nombre,email,celular);
            }

            try
            {
                bdExam.Prendas.Add(prenda); //Agrega un objeto empleado a la lista de "empleadoes"
                bdExam.SaveChanges(); // Guardar los cambios en la base de datos
                return "Prenda guardada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar la prenda " + ex.Message;
            }

        }

        public void RedirigirCliente(string doc, string nombre, string email, string celular)
        {
            var Cliente = new Cliente
            {
                Celular = celular,
                Documento = doc,
                Email = email,
                Nombre = nombre
            };
            bdExam.Clientes.Add(Cliente);
            bdExam.SaveChanges();
        }

        public string GrabarImagenPrenda(int idPrenda,int idfoto, List<string> Imagenes)
        {
            try
            {
                foreach (string imagen in Imagenes)
                {
                    FotoPrenda imagenProducto = new FotoPrenda();
                    imagenProducto.idPrenda = idPrenda;
                    imagenProducto.idFoto=idfoto;
                    imagenProducto.FotoPrenda1 = imagen;
                    bdExam.FotoPrendas.Add(imagenProducto);
                    bdExam.SaveChanges();

                }
                return "Se guardó la información en la base de datos :)";
            }
            catch (Exception ex)
            {
                return "Error :( " + ex.Message;
            }
        }

    }
}