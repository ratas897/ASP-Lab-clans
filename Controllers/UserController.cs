 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASP_Lab_clans.Models;

namespace ASP_Lab_clans.Controllers
{
    public class UserController : Controller
    {
        #region Registration action and POST action
        [HttpGet]
          public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Registration(User user)
        {
            bool Status = false;
            string message = "";
            //
            //Model validation
            if (ModelState.IsValid)
            {
                #region Email already exists
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(user);
                }
                #endregion
                #region User already exists
                var isExistUser = IsUserExist(user.Username);
                if (isExistUser)
                {
                    ModelState.AddModelError("UserExist", "User already exists");
                    return View(user);
                }
                #endregion
                #region Passsword hashing
                user.Password = Crypt.Hash(user.Password);
                user.ConfirmPassword = Crypt.Hash(user.ConfirmPassword);
                #endregion
                #region Saving to database
                using (Database1Entities database = new Database1Entities())
                {
                    database.Users.Add(user);
                    database.SaveChanges();
                }
                #endregion
                Status = true;
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }
        #endregion
        #region Login action and post action
        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Users.Where(a => a.Username == login.Username).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypt.Hash(login.Password),v.Password) == 0)
                    {
                        int timeout = login.Remember ? 526000 : 20; //Laikas sausainiui
                        var ticket = new FormsAuthenticationTicket(login.Username,login.Remember, timeout);//Sukuria prisijungimo bilieta
                        string encrypted = FormsAuthentication.Encrypt(ticket); //Encryptina bilieta
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted); //Sukuria "Cookie"
                        cookie.Expires = DateTime.Now.AddMinutes(timeout); //Prideda kada sausainis dingsta
                        cookie.HttpOnly = true; //Apsauga nuo kliento
                        Response.Cookies.Add(cookie); //Prideda sausani prie atsakymo

                        if (Url.IsLocalUrl(ReturnUrl))//Gražintų į pagrindinį puslapį, kada prisijungia prie svetainės
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid username or password";
                    }
                }
                else
                {
                    message = "Invalid username or password";
                }
            }

            ViewBag.Message = message;
            return View();
        }
        #endregion
        #region Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        #endregion
        #region Register checks
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Users.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }
        public bool IsUserExist(string User)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Users.Where(a => a.Username == User).FirstOrDefault();
                return v != null;
            }
        }
        #endregion
    }

}