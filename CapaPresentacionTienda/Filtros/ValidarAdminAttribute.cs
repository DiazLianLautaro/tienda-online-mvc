using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Filtros
{
    public class ValidarAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usuario = filterContext.HttpContext.Session["Usuario"] as Usuario;

            if (usuario.EsAdmin != true || usuario == null)
            {
                // Si es AJAX, devolvemos algo que JS pueda manejar
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var url = "/Tienda/Index";

                    filterContext.Result = new JsonResult
                    {
                        Data = new { redirect = url },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    // Petición normal → redirección normal
                    filterContext.Result =
                        new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary
                            {
                                { "controller", "Tienda" },
                                { "action", "Index" }
                            });
                }
            }
        }
    }
}