using CapaEntidad;
using CapaNegocio;
using CapaPresentacionTienda.Filtros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar(false).Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }

            return View(oProducto);
        }



        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar(true);

            return Json(new { data = lista },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();

            lista = new CN_Marca().ListarMarcaPorCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductos(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar(true).Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                oDetalle = p.oDetalle,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                NombreImagen = p.NombreImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen,p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p => 
                //Si es 0 trae todas las categorías, sino trae la que recibió por parámetro. Lo mismo en marca. También stock > 0 y activo == true
                p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
                p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonResult = Json(new { data = lista}, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;

        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            bool existe = new CN_Carrito().ExisteCarrito(idUsuario, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            //si no existe, lo agrega al carrito.
            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idUsuario, idproducto, true, out mensaje);
            }
            
            return Json(new { respuesta =  respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
        }


        // --- Carrito ---
        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            int cantidad = new CN_Carrito().CantidadEnCarrito(idUsuario);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidarSesion]
        public JsonResult ListarProductosCarrito()
        {
            int idUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idUsuario).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    oDetalle = oc.oProducto.oDetalle,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //Método para operar las cantidades de un producto en el carrito
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idUsuario, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idUsuario, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        //--- UBICACION ---
        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();

            oLista = new CN_Ubicacion().ObtenerDepartamento();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string idDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();

            oLista = new CN_Ubicacion().ObtenerProvincia(idDepartamento);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string idProvincia, string idDepartamento)
        {
            List<Distrito> oLista = new List<Distrito>();

            oLista = new CN_Ubicacion().ObtenerDistrito(idProvincia, idDepartamento);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        //Vista
        public ActionResult Carrito() 
        {
            return View();
        }


        //--- Pago ---
        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;
            DataTable detalleVenta = new DataTable();
            detalleVenta.Locale = new CultureInfo("es-US");

            detalleVenta.Columns.Add("IdProducto", typeof(string));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("Total", typeof(decimal));

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.oDetalle.PrecioVenta;

                total += subtotal;

                detalleVenta.Rows.Add( new object[]
                {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IdUsuario = ((Usuario)Session["Usuario"]).IdUsuario;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalleVenta;

            return Json(new { Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string idTransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;

            if (status) {
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalleVenta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idTransaccion;

                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalleVenta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();

        }

    }
}