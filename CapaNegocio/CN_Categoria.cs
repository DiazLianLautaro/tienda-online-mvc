using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();

        public List<Categoria> Listar(bool soloActivos)
        {
            return objCapaDato.Listar(soloActivos);
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "la descripción de la Categoría no puede estar vacía";
            }
            


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }



        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "la descripción de la Categoría no puede estar vacía";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
