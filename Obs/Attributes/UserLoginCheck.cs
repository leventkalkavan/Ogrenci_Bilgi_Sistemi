using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Obs.Attributes
{
    public class UserLoginCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // string AnnouncementNo = context.HttpContext.Request.Cookies["AnnouncementNo"];
            ulong TCNo = parseUlongWithDefault(context.HttpContext.Request.Cookies["TCNo"], 0);
            string Password = context.HttpContext.Request.Cookies["Password"];
            //string.IsNullOrEmpty(AnnouncementNo) || TCNo == 0 || string.IsNullOrEmpty(Password)
            if (TCNo == 0 || string.IsNullOrEmpty(Password))
            {
                DeleteCookiesAndRedirectToLogin(context);
            }
            else
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var controller = context.Controller as Controller;
                    DeleteCookiesAndRedirectToLogin(context);
                }
            }
            base.OnActionExecuting(context);
        }

        private void DeleteCookiesAndRedirectToLogin(ActionExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Delete("AnnouncementNo");
            context.HttpContext.Response.Cookies.Delete("TCNo");
            context.HttpContext.Response.Cookies.Delete("Password");
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "Controller", "Apply" },
                    { "Action", "Login" }
                });
        }

        private ulong parseUlongWithDefault(String number, ulong defaultVal)
        {
            try
            {
                return ulong.Parse(number);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return defaultVal;
            }
        }
    }
}