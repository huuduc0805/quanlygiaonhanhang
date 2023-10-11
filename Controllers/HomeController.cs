using quanlygiaonhanhang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quanlygiaonhanhang.Controllers
{
    public class HomeController : Controller
    {
        Data db = new Data();
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user1 = db.NhanViens.FirstOrDefault(u => u.TaiKhoan.Equals(TaiKhoan) && u.MatKhau.Equals(MatKhau));
                if (user1 != null)
                {
                    var newCookie = new HttpCookie("myCookieadmin", user1.Id.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Session["ID_TKadmin"] = user1.Id;
                    Session["PQ"] = user1.PhanQuyen;
                    Session["Ten"] = user1.HoTen;
                    Response.AppendCookie(newCookie);
                    return Redirect("/Home/Index");
                }

                else
                {
                    ViewBag.error = "Thông tin đăng nhập không hợp lệ!";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            if (Request.Cookies["myCookieadmin"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookieadmin"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return RedirectToAction("/Login");
        }
    }
}