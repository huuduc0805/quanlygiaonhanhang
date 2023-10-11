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
    public class DonHangsController : Controller
    {
        private Data db = new Data();

        // GET: DonHangs
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var donHangs = db.DonHangs.Include(d => d.DichVu).Include(d => d.DiaDiem);
            return View(donHangs.ToList());
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.IdDV = new SelectList(db.DichVus, "Id", "TenDV");
            ViewBag.KhuVuc = new SelectList(db.DiaDiems, "Id", "TenDD");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenKH,DiaChi,SDT,TenMH,SoKG,IdDV,PhiVC,TinhTrang,KhuVuc")] DonHang donHang)
        {
            if (!string.IsNullOrEmpty(donHang.TenKH)&& donHang.KhuVuc!=0 && !string.IsNullOrEmpty(donHang.SDT)&&!string.IsNullOrEmpty(donHang.DiaChi))
            {
                donHang.TinhTrang = 1;
                var phidv = db.DichVus.FirstOrDefault(g => g.Id == donHang.IdDV).PhiDV;
                var phivc = db.DiaDiems.FirstOrDefault(g => g.Id == donHang.KhuVuc).GiaCuoc;
                var phikg = 0;
                if (donHang.SoKG > 0)
                {
                    phikg +=Convert.ToInt32(donHang.SoKG)*10000 ;
                }
                donHang.PhiVC = phidv + phivc + phikg;
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của đơn hàng.";
            ViewBag.IdDV = new SelectList(db.DichVus, "Id", "TenDV", donHang.IdDV);
            ViewBag.KhuVuc = new SelectList(db.DiaDiems, "Id", "TenDD", donHang.DiaChi);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDV = new SelectList(db.DichVus, "Id", "TenDV", donHang.IdDV);
            ViewBag.KhuVuc = new SelectList(db.DiaDiems, "Id", "TenDD", donHang.KhuVuc);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenKH,DiaChi,SDT,TenMH,SoKG,IdDV,PhiVC,TinhTrang,KhuVuc")] DonHang donHang)
        {
            if (!string.IsNullOrEmpty(donHang.TenKH) && donHang.KhuVuc != 0 && !string.IsNullOrEmpty(donHang.SDT)&& !string.IsNullOrEmpty(donHang.DiaChi))
            {
                donHang.TinhTrang = 1;
                var phidv = db.DichVus.FirstOrDefault(g => g.Id == donHang.IdDV).PhiDV;
                var phivc = db.DiaDiems.FirstOrDefault(g => g.Id == donHang.KhuVuc).GiaCuoc;
                var phikg = 0;
                if (donHang.SoKG > 0)
                {
                    phikg += Convert.ToInt32(donHang.SoKG) * 10000;
                }
                donHang.PhiVC = phidv + phivc + phikg;
                db.DonHangs.AddOrUpdate(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TB = "Yêu cầu nhập đủ thông tin của đơn hàng.";
            ViewBag.IdDV = new SelectList(db.DichVus, "Id", "TenDV", donHang.IdDV);
            ViewBag.KhuVuc = new SelectList(db.DiaDiems, "Id", "TenDD", donHang.KhuVuc);
            return View(donHang);
        }
        public ActionResult PhanCong(int? id)
        {
            ViewBag.IdDH = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhanCong([Bind(Include = "Id,HoTenTX,IdDH,SDT")] PhanCongVC donHang)
        {
            if (!string.IsNullOrEmpty(donHang.HoTenTX) && !string.IsNullOrEmpty(donHang.SDT))
            {
                donHang.IdNV = Convert.ToInt32(Session["ID_TKadmin"].ToString());
                db.PhanCongVCs.Add(donHang);
                DonHang dh = db.DonHangs.Find(donHang.IdDH);
                dh.TinhTrang = 2;
                db.DonHangs.AddOrUpdate(dh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDH = donHang.IdDH;
            ViewBag.TB = "Yêu cầu nhập đủ thông tin phân công.";
            return View(donHang);
        }
        public ActionResult HoanThanh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            donHang.TinhTrang = 3;
            if (donHang == null)
            {
                return HttpNotFound();
            }
            db.DonHangs.AddOrUpdate(donHang);
            db.SaveChanges();
            return RedirectToAction("Index","DonHangs");
        }
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = false, message = "Chưa chọn đối tượng xóa." },
                         JsonRequestBehavior.AllowGet);
            }
            DonHang dichVu = db.DonHangs.Find(id);
            if (dichVu == null)
            {
                return Json(new { status = false, message = "Không tìm thấy dịch vụ." },
                        JsonRequestBehavior.AllowGet);
            }
            db.DonHangs.Remove(dichVu);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công.", href = "/DonHangs/Index" },
                        JsonRequestBehavior.AllowGet);
        }
        // GET: DonHangs/Delete/5

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
