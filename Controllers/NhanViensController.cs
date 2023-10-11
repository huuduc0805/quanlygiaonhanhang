using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using quanlygiaonhanhang.Models;

namespace quanlygiaonhanhang.Controllers
{
    public class NhanViensController : Controller
    {
        private Data db = new Data();

        // GET: NhanViens
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.NhanViens.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HoTen,SDT,DiaChi,TaiKhoan,MatKhau")] NhanVien nhanVien)
        {
            if (!string.IsNullOrEmpty(nhanVien.HoTen)&& !string.IsNullOrEmpty(nhanVien.SDT) && !string.IsNullOrEmpty(nhanVien.DiaChi) && !string.IsNullOrEmpty(nhanVien.TaiKhoan))
            {
                if (nhanVien.SDT.Length != 10)
                {
                    ViewBag.TB = "Số điện thoại không đúng định dạng.";
                    return View(nhanVien);
                }
                nhanVien.MatKhau = nhanVien.SDT;
                nhanVien.PhanQuyen = 2;
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của nhân viên.";
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HoTen,SDT,DiaChi,TaiKhoan,MatKhau")] NhanVien nhanVien)
        {
            if (!string.IsNullOrEmpty(nhanVien.HoTen) && !string.IsNullOrEmpty(nhanVien.SDT) && !string.IsNullOrEmpty(nhanVien.DiaChi) && !string.IsNullOrEmpty(nhanVien.TaiKhoan))
            {
                if (nhanVien.SDT.Length != 10)
                {
                    ViewBag.TB = "Số điện thoại không đúng định dạng.";
                    return View(nhanVien);
                }
                nhanVien.MatKhau = nhanVien.SDT;
                nhanVien.PhanQuyen = 2;
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của nhân viên.";
            return View(nhanVien);
        }

        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = false, message = "Chưa chọn đối tượng xóa." },
                         JsonRequestBehavior.AllowGet);
            }
            NhanVien dichVu = db.NhanViens.Find(id);
            if (dichVu == null)
            {
                return Json(new { status = false, message = "Không tìm thấy dịch vụ." },
                        JsonRequestBehavior.AllowGet);
            }
            db.NhanViens.Remove(dichVu);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công.", href = "/NhanViens/Index" },
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoiMK(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMK([Bind(Include = "Id,HoTen,SDT,DiaChi,TaiKhoan,PhanQuyen")] NhanVien nhanVien)
        {
            var pass = Request["MKMoi"];
            if (!string.IsNullOrEmpty(pass))
            { 
                nhanVien.MatKhau = pass;
                db.NhanViens.AddOrUpdate(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TB = "Yêu cầu nhập mật khẩu mới.";
            return View(nhanVien);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
