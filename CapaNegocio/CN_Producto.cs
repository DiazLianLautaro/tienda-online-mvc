using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Productos objCapaDato = new CD_Productos();

        public List<Producto> Listar(bool soloActivos)
        {
            return objCapaDato.Listar(soloActivos);
        }

        public Producto Detalle(int Id)
        {
            return objCapaDato.Detalle(Id);
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción del Producto no puede estar vacía";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe selecionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe selecionar una Categoría";
            }
            else if (obj.oDetalle.PrecioVenta == 0)
            {
                Mensaje = "Debe ingresar el Precio del producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del producto";
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

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción del Producto no puede estar vacía";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe selecionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe selecionar una Categoría";
            }
            else if (obj.oDetalle.PrecioVenta == 0)
            {
                Mensaje = "Debe ingresar el Precio del producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del producto";
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

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(oProducto, out Mensaje );
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        
    }
}
