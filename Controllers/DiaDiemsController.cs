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
    public class DiaDiemsController : Controller
    {
        private Data db = new Data();

        // GET: DiaDiems
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.DiaDiems.ToList());
        }

        // GET: DiaDiems/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenDD,KhuVuc,GiaCuoc")] DiaDiem diaDiem)
        {
            if (!string.IsNullOrEmpty(diaDiem.TenDD)&&diaDiem.GiaCuoc!=0)
            {
                db.DiaDiems.Add(diaDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của khu vực.";
            return View(diaDiem);
        }

        // GET: DiaDiems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiems.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            return View(diaDiem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenDD,KhuVuc,GiaCuoc")] DiaDiem diaDiem)
        {
            if (!string.IsNullOrEmpty(diaDiem.TenDD) && diaDiem.GiaCuoc != 0)
            {
                db.Entry(diaDiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của khu vực.";
            return View(diaDiem);
        }

        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = false, message = "Chưa chọn đối tượng xóa." },
                         JsonRequestBehavior.AllowGet);
            }
            DiaDiem dichVu = db.DiaDiems.Find(id);
            var listDH = db.DonHangs.Where(g => g.KhuVuc == id).ToList();
            foreach (var item in listDH)
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
            db.DiaDiems.Remove(dichVu);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công.", href = "/DiaDiems/Index" },
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
