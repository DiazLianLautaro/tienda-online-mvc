using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                //ViewBag solo funciona para compartir información dentro de la misma vista.  ViewBag y TempData son datos temporales
                ViewBag.Error = "Correo o contraseña incorrecta.";
                return View();
            }
            else
            {
                //si es la primera vez del usuario, reestablecerá su clave 
                if (oUsuario.Reestablecer)
                {
                    //para enviar información a otra vista (enviar IdUsuario a CambiarClave).
                    //TempData nos permite guardar información y compartirlo a travez de multiples vistas que están en el mismo controlador.
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    
                    return RedirectToAction("CambiarClave");
                }

                //autenticación de usuario por su correo
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index","Home");

            }

        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmar)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if(oUsuario.Clave != CN_Recursos.ConvertSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = claveActual; //Conservar clave

                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmar)
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

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

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
                return RedirectToAction("Index","Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Acceso");
        }
    }
}