using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;

namespace RBACDemo.Controllers
{
    public class UserController : Controller
    {
        private RbacDB db = new RbacDB();
        public ActionResult Index()
        {
            return View(db.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var user = db.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return Content("未找到要编辑的用户");
            return View(user);
        }

        public ActionResult Save(User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
            return Json(new { code = 200 });
        }

        public ActionResult Delete(int id)
        {
            User user = new User();
            user.Id = id;
            db.Users.Attach(user);

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}