using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }




        //Seccion Categoria

        #region Categoria
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }
            //               Este JSON lo recibe "succeess: function (data)" 
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion



        //Seccion Marca

        #region Marca
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();

            oLista = new CN_Marca().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
		#endregion



		//Seccion Producto

		#region Producto
		[HttpGet]
		public JsonResult ListarProductos()
		{
			List<Producto> oLista = new List<Producto>();

			oLista = new CN_Producto().Listar();

			return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
		{
			string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExito = true;


            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto); //convierte el string en objeto


            decimal precio;
                 //intenta convertir texto en decimal, los decimales son puntos,       region cultural argentina y por ultimo lo guarda en la variable
            if(decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-US"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ####.##" },JsonRequestBehavior.AllowGet);
            }



            //Registrar nuevo
            if (oProducto.IdProducto == 0) 
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                    oProducto.IdProducto = idProductoGenerado;
                else
                    operacionExitosa = false;
            }
            //Editar
            else 
            {
				operacionExitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }
			

            //si se guardó el producto, validamos para guardar la imagen
            if(operacionExitosa)
            {
                if(archivoImagen != null)
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extrension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(oProducto.IdProducto.ToString(), extrension);

                    //Guarda la Ruta y Nombre de la imagen 
                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardarImagenExito = false;
                    }

                    //Guardar Imagen, sinó, mensaje de error
                    if(guardarImagenExito)
                    {
                        oProducto.RutaImagen = rutaGuardar;
                        oProducto.NombreImagen = nombreImagen;
                        bool respuesta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el Producto pero hubo problemas con la Imagen";
                    }
                }
            }

			return Json(new { operacionExitosa = operacionExitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

		}


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            //convertimos la imagen del id que trajimos en Base64 (se usa para enviar una imagen por JSON)
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },

            JsonRequestBehavior.AllowGet //Importante!
                                         //Para poder permitir que una acción responda con JSON mediante GET
                                         //(Corroborar si solo sirve en GET)
            );
        }


		[HttpPost]
		public JsonResult EliminarProducto(int id)
		{
			bool respuesta = false;
			string mensaje = string.Empty;

			respuesta = new CN_Producto().Eliminar(id, out mensaje);

			return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

		}
		#endregion
	}
}