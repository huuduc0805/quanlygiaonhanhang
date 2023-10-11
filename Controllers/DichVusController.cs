using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using quanlygiaonhanhang.Models;

namespace quanlygiaonhanhang.Controllers
{
    public class DichVusController : Controller
    {
        private Data db = new Data();

        // GET: DichVus
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.DichVus.ToList());
        }

        // GET: DichVus/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenDV,PhiDV")] DichVu dichVu)
        {
            if (!string.IsNullOrEmpty(dichVu.TenDV) && dichVu.PhiDV!=0)
            {
                db.DichVus.Add(dichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của dịch vụ";
            return View(dichVu);
        }

        // GET: DichVus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DichVu dichVu = db.DichVus.Find(id);
            if (dichVu == null)
            {
                return HttpNotFound();
            }
            return View(dichVu);
        }

        // POST: DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenDV,PhiDV")] DichVu dichVu)
        {
            if (!string.IsNullOrEmpty(dichVu.TenDV) && dichVu.PhiDV != 0)
            {
                db.Entry(dichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của dịch vụ";
            return View(dichVu);
        }

        // GET: DichVus/Delete/5
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = false, message = "Chưa chọn đối tượng xóa." },
                         JsonRequestBehavior.AllowGet);
            }
            DichVu dichVu = db.DichVus.Find(id);
            var listDH = db.DonHangs.Where(g => g.IdDV == id).ToList();
            foreach(var item in listDH)
            {
                var pc = db.PhanCongVCs.FirstOrDefault(g => g.IdDH == item.Id);
                db.PhanCongVCs.Remove(pc);
            }
            db.DonHangs.RemoveRange(listDH);
            if (dichVu == null)
            {
                return Json(new { status = false, message = "Không tìm thấy dịch vụ." },
                        JsonRequestBehavior.AllowGet);
            }
            db.DichVus.Remove(dichVu);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công.",href="/DichVus/Index" },
                        JsonRequestBehavior.AllowGet);
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
