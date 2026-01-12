using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Filtros
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Usuario"] == null)
            {
                // Si es AJAX, devolvemos algo que JS pueda manejar
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //var url = new UrlHelper(filterContext.RequestContext)
                    //            .Action("Index", "Acceso");

                    //var url = new UrlHelper(filterContext.RequestContext)
                    //            .Action("Index", "Acceso", null, filterContext.HttpContext.Request.Url.Scheme);

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