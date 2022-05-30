using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Obs.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Obs.Attributes
{
    public class LoadUserRole : ActionFilterAttribute
    {
        private UserManager<AppUser> userManager;
        private RoleManager<AppRole> roleManager;
        private SignInManager<AppUser> signInManager;

        public LoadUserRole(UserManager<AppUser> usrMgr, RoleManager<AppRole> rleManager,
            SignInManager<AppUser> sgnInManager)
        {
            userManager = usrMgr;
            roleManager = rleManager;
            signInManager = sgnInManager;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = context.HttpContext.User.Identity.Name;
                var user = await userManager.FindByNameAsync(userName);
                var roles = await userManager.GetRolesAsync(user);
                var controller = context.Controller as Controller;
                controller.ViewBag.Roles = roles;
                controller.ViewBag.UserRole = roles.First();
                controller.ViewBag.UserName = userName;
                controller.ViewBag.UserDomain = user.Domain;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}