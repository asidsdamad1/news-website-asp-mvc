using NewsWebsite.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;

namespace NewsWebsite.Controllers
{
    public class BaseController : Controller
    {
        // //initilizing culture on controller initialization
        // protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        // {
        //     base.Initialize(requestContext);
        //     if (Session[CommonConstants.CurrentCulture] != null)
        //     {
        //         Thread.CurrentThread.CurrentCulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
        //         Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
        //     }
        //     else
        //     {
        //         Session[CommonConstants.CurrentCulture] = "vi";
        //         Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
        //         Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
        //     }
        // }
        //
        // // changing culture
        // public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        // {
        //     Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
        //     Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);
        //
        //     Session[CommonConstants.CurrentCulture] = ddlCulture;
        //     return Redirect(returnUrl);
        // }
        public ActionResult Index(string language)
        {
            if (!String.IsNullOrEmpty(language))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }

            HttpCookie cookie = new HttpCookie("Languages");
            cookie.Value = language;
            Response.Cookies.Add(cookie);
            return View();
        }
    }
}