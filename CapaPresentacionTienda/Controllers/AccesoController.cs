using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(objeto.Nombre) ? "" : objeto.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(objeto.Apellido) ? "" : objeto.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Email) ? "" : objeto.Email;

            //if (objeto.Clave != objeto.ConfirmarClave)
            //{
            //    ViewBag.Error = "Las contraseñas no coinciden";
            //    return View();
            //}

            resultado = new CN_Usuarios().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index(string correo, string clave) //Login
        {
            Usuario oUsuario = null;

            //Si Usuario existe
            oUsuario = new CN_Usuarios().Listar().Where(item => item.Email == correo && item.Clave == CN_Recursos.ConvertSha256(clave)).FirstOrDefault();

            if (oUsuario == null) 
            {
                //ViewBag solo funciona para compartir información dentro de la misma vista.  ViewBag y TempData son datos temporales
                ViewBag.Error = "Correo o contraseña no son correctas";
                ViewBag.Correo = correo;
                return View();
            }
            else
            {
                //si no reestableció su contraseña, que lo haga
                if (oUsuario.Reestablecer)
                {
                    //para enviar información a otra vista (enviar IdUsuario a CambiarClave).
                    //TempData nos permite guardar información y compartirlo a travez de multiples vistas que están en el mismo controlador.
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave","Acceso");
                }
                else
                {
                    //autenticación de usuario por su correo, Cierra session al cerrar el navegador o ventana aunque no se haya terminado el tiempo
                    FormsAuthentication.SetAuthCookie(oUsuario.Email, false);
                    Session["Usuario"] = oUsuario;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");

                }
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Email == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(oUsuario.IdUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = claveActual; //Conservar clave

                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = claveActual;

                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            ViewData["vClave"] = ""; //Desocupamos y limpiamos el campo

            //Encriptamos la clave
            nuevaClave = CN_Recursos.ConvertSha256(nuevaClave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index"); //Redirige al Login
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();

            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }





    }
}