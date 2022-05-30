using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obs;
using Obs.Attributes;
using Obs.Models;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Obs.Controllers
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    

    public class UserRegisterForm
    {
        [Required(ErrorMessage = "Bir ad girmek zorundasınız!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bir numara girmek zorundasınız!")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Bir şifre girmek zorundasınız!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bir Bölüm Id'si girmek zorundasınız!")]
        public int BolumId { get; set; }
        [Required(ErrorMessage = "Bir dönem yılı girmek zorundasınız!")]
        public int YariyilId { get; set; }
    }
    public class AddAkademisyenForm
    {
        [Required(ErrorMessage = "Bir ad girmek zorundasınız!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bir şifre girmek zorundasınız!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bir Bölüm Id'si girmek zorundasınız!")]
        public int BolumId { get; set; }
        
    }
    public class AddBolumForm
    {
        [Required(ErrorMessage = "Bir Bölüm adı girmek zorundasınız!")]
        public string Name { get; set; }
        public string AkademisyenName { get; set; }

    }
    public class AddDonemForm
    {
        [Required(ErrorMessage = "Bir dönem adı girmek zorundasınız!")]
        public string Name { get; set; }

    }
    public class AddDersForm
    {
        [Required(ErrorMessage = "Bir Ders adı girmek zorundasınız!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bir Öğretim Görevlisi Id'si girmek zorundasınız!")]
        public int AkademisyenId { get; set; }
        [Required(ErrorMessage = "Bir Bölüm Id'si girmek zorundasınız!")]
        public int BolumId { get; set; }

    }
    public class AddNotForm
    {
        [Required(ErrorMessage = "Bir Öğrenci Id'si girmek zorundasınız!")]
        
        public int OgrenciId { get; set; }
        [Required(ErrorMessage = "Bir Ders Id'si girmek zorundasınız!")]
        public int DersId { get; set; }
        [Required(ErrorMessage = "Bir Vize Puanı girmek zorundasınız!")]
        public string VizeN { get; set; }
        [Required(ErrorMessage = "Bir Final Puanı girmek zorundasınız!")]
        public string FinalN { get; set; }

    }
    

    [Authorize(Roles = "Super,Akademisyen,Admin,Ogrenci")]
    [ServiceFilter(typeof(LoadUserRole))]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<AppRole> roleManager;
        private SignInManager<AppUser> signInManager;
        private string password;
        private object db;
        private readonly INotyfService _notyf;

        public AdminController(UserManager<AppUser> usrMgr, RoleManager<AppRole> rleManager,
            SignInManager<AppUser> sgnInManager, INotyfService notyfService)
        {
            userManager = usrMgr;
            roleManager = rleManager;
            signInManager = sgnInManager;
            _notyf = notyfService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return LocalRedirect("/Admin/Index");
           
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewBag.ErrorMessages = new List<String>();
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(req.UserName.Trim(), req.Password.Trim(),
                    req.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //SaveLog("Login", req.UserName, DateTime.Now);
                    return LocalRedirect("/Admin/Index");

                }
                
                else
                {
                    ViewBag.ErrorMessages.Add("Geçersiz kullanıcı bilgileri.");
                    return View();
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ViewBag.ErrorMessages =
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            }

            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Admin","Super","Ogrenci");
        }

        [AllowAnonymous]
        public async Task<JsonResult> Register()
        {
            AppUser appUser = new AppUser
            {
                UserName = "admin",
                Domain = UserDomain.Admin
            };
            AppRole adminRole = new AppRole
            {
                Name = "Admin"
            };
            IdentityResult adminRoleResult = await roleManager.CreateAsync(adminRole);
            if (adminRoleResult.Succeeded)
            {
                IdentityResult result = await userManager.CreateAsync(appUser, "abC.1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, adminRole.Name);
                    return Json("Kayit Basarili");
                }
            }

            return Json("Kayit Basarisiz");
        }
        [AllowAnonymous]
        public async Task<JsonResult> Akademisyen()
        {
            AppUser appUser = new AppUser
            {
                UserName = "super",
                Domain = UserDomain.Admin
            };
            AppRole adminRole = new AppRole
            {
                Name = "Super"
            };
            IdentityResult adminRoleResult = await roleManager.CreateAsync(adminRole);
            if (adminRoleResult.Succeeded)
            {
                IdentityResult result = await userManager.CreateAsync(appUser, "abC.1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, adminRole.Name);
                    return Json("Kayit Basarili");
                }
            }

            return Json("Kayit Basarisiz");
        }
        [AllowAnonymous]
        public async Task<JsonResult> Ogrenci()
        {
            AppUser appUser = new AppUser
            {
                UserName = "ogrenci",
                Domain = UserDomain.Admin
            };
            AppRole adminRole = new AppRole
            {
                Name = "Ogrenci"
            };
            IdentityResult adminRoleResult = await roleManager.CreateAsync(adminRole);
            if (adminRoleResult.Succeeded)
            {
                IdentityResult result = await userManager.CreateAsync(appUser, "abC.1234!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, adminRole.Name);
                    return Json("Kayit Basarili");
                }
            }

            return Json("Kayit Basarisiz");
        }



        [Authorize(Roles = "Super,Admin,Ogrenci")]
        public async Task<IActionResult> Users()
        {
            var users = await userManager.GetUsersInRoleAsync("User");
            if (users != null)
            {
                ViewBag.Users = users.ToList();
                return View();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Super,Admin,Ogrenci")]
        [HttpGet("/Admin/AddUser")]
        public IActionResult AddUser()
        {
            return View();
        }
        
        public IActionResult AddBolum()
        {
            return View();
        }
        [HttpGet("/Admin/AddDers")]
        public IActionResult AddDers()
        {
            return View();
        }
        
        [HttpGet("/Admin/AddAkademisyen")]
        public IActionResult AddAkademisyen()
        {
            return View();
        }

        [HttpGet("/Admin/AddDonem")]
        public IActionResult AddDonem()
        {
            return View();
        }
        [HttpGet("/Admin/DeleteDonem")]
        public IActionResult DeleteDonem()
        {
            return View();
        }
        [HttpGet("/Admin/AddNot")]
        public IActionResult AddNot()
        {
            return View();
        }
        //ÖĞRENCİLER
        
        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddUser")]
        public async Task<ActionResult> AddUser(UserRegisterForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Öğrenci eklenirken hata oluştu");
                return View();
            }
            using (var db = new DatabaseContext())
            {
                User yeniOgrenci = new User() { Name = form.UserName,Password=form.Password, Number=form.Number,BolumId=form.BolumId,YariyilId=form.YariyilId};
                db.Ogrenciler.Add(yeniOgrenci);
                db.SaveChanges();
                _notyf.Success("Öğrenci başarıyla eklendi");
                return RedirectToAction("Ogrenciler");

            }

        }
        //AKADEMİSYENLER

        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddAkademisyen")]
        public async Task<ActionResult> AddAkademisyen(AddAkademisyenForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Akademisyen eklenirken hata oluştu!");
                return View();
            }
            using (var db = new DatabaseContext())
            {
                Akademisyen yeniaka = new Akademisyen() { Name = form.UserName,Password=form.Password, BolumId = form.BolumId };
                db.Akademisyenler.Add(yeniaka);
                db.SaveChanges();
                _notyf.Success("Akademisyen başarıyla eklendi");
                //return RedirectToAction("Akademisyenler");

            }           
            AppRole role = new AppRole
            {
                Name = "Akademisyen"
            };
            var ide = await roleManager.RoleExistsAsync(role.Name);
            if (!ide) await roleManager.CreateAsync(role);
            AppUser newAka = new AppUser()
            {
                UserName = form.UserName,
                Domain = UserDomain.User
            };
            var password = (new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true,
                includeSpecial: true, passwordLength: 16)).Next();
            IdentityResult userResult = await userManager.CreateAsync(newAka, password);
            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newAka, role.Name);

                _notyf.Success("Akademisyen başarıyla eklendi.");
               
            }


            return RedirectToAction("Akademisyenler");
        }

        //BÖLÜMLER
        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddBolum")]
        public async Task<ActionResult> AddBolum(AddBolumForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Bölüm eklenirken hata oluştu!");
                return View();
            }

            using (var db = new DatabaseContext())
            {
                Bolum yeniBolum = new Bolum() { Name = form.Name};
                db.Bolumler.Add(yeniBolum);
                db.SaveChanges();
                _notyf.Success("Bölüm başarıyla eklendi");
                return RedirectToAction("Bolumler");

            }
        }
       

        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddNot")]
        public async Task<ActionResult> AddNot(AddNotForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Not eklenirken hata oluştu!");
                return View();
            }

            using (var db = new DatabaseContext())
            {
                // OgrenciId olacak length sayısala olsun diye
                // dersId 
                Not yeniNot = new Not() { OgrenciId = form.OgrenciId, DersId = form.DersId, VizeN =form.VizeN, FinalN=form.FinalN };
                db.Notlar.Add(yeniNot);
                db.SaveChanges();
                _notyf.Success("Not başarıyla eklendi");
                return RedirectToAction("Notlar");

            }
        }
        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddDonem")]
        public async Task<ActionResult> AddDonem(AddDonemForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Dönem eklenirken hata oluştu!");
                return View();
            }

            using (var db = new DatabaseContext())
            {
                Yariyil yeniDonem = new Yariyil() { Name = form.Name };
                db.Yariyillar.Add(yeniDonem);
                db.SaveChanges();
                _notyf.Success("Dönem başarıyla eklendi");
                return RedirectToAction("Yariyillar");

            }
        }
        public async Task<ActionResult> DeleteDonem(int id)
        {
            using (var db = new DatabaseContext())
            {
                var yeniDonem = db.Yariyillar.Find(id);
                db.Yariyillar.Remove(yeniDonem);
                db.SaveChanges();
                _notyf.Success("Dönem başarıyla silindi.");
                return RedirectToAction("Yariyillar");

            }

        }
        [Authorize(Roles = "Super,Admin")]
        [HttpPost("/Admin/AddDers")]
        public async Task<ActionResult> AddDers(AddDersForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _notyf.Error("Ders eklenirken hata oluştu!");
                return View();
            }

            using (var db = new DatabaseContext())
            {
                Ders yeniDers = new Ders() { Name = form.Name, BolumId = form.BolumId,AkademisyenId=form.AkademisyenId };
                db.Dersler.Add(yeniDers);
                db.SaveChanges();
                _notyf.Success("Ders başarıyla eklendi");
                return RedirectToAction("Dersler");

            }
        }

    

        public IActionResult Bolumler()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Bolumler = db.Bolumler.ToList();
            }
            return View();
        }
        
        public async Task<IActionResult> Akademisyenler()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Akademisyenler = db.Akademisyenler.Include(akademisyen => akademisyen.Bolum).ToList();
                // Lazy => tembel => BolumId => Bolum ! X
                // Eager => BolumId => Bolum 
            }
            var foo = await userManager.GetUsersInRoleAsync("User");
            ViewBag.akl = foo.ToList();

            return View();
        }


        public async Task<IActionResult> Ogrenciler()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Ogrenciler = db.Ogrenciler.Include(ogrenci => ogrenci.Bolum).ToList();
            }

            return View();
        }

        public IActionResult Dersler()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Dersler = db.Dersler.ToList();
               
            }
            return View();
        }
        public IActionResult Notlar()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Notlar = db.Notlar.ToList();
            }
            return View();
        }
        public IActionResult Yariyillar()
        {
            using (var db = new DatabaseContext())
            {
                ViewBag.Yariyillar = db.Yariyillar.ToList();
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }


        //public void SaveLog(string LogType, string LogUser, DateTime LogTime)
        //{
        //    try
        //    {
        //        using (var db = new DatabaseContext())
        //        {
        //            Log l = new Log();
        //            l.LogType = LogType;
        //            l.LogUser = LogUser;
        //            l.LogTime = LogTime;
        //            db.Logs.Add(l);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        public async Task<IActionResult> UpdateUserPassword(string username)
        {
            var userToUpdate = await userManager.FindByNameAsync(username);
            var token = await userManager.GeneratePasswordResetTokenAsync(userToUpdate);
            var newPassword = (new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true,
                includeSpecial: true, passwordLength: 16)).Next();
            var result = await userManager.ResetPasswordAsync(userToUpdate, token, newPassword);

            if (result.Succeeded)
            {
                _notyf.Success("Şifre başarıyla güncellendi.");
            }
            else
            {
                _notyf.Error("Şifre güncellenirken hata!");
            }


            return RedirectToAction("Users");
        }
    }
}