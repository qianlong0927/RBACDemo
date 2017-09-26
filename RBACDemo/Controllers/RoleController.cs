using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;

namespace RBACDemo.Controllers
{
    public class RoleController : Controller
    {
        private RbacDB db = new RbacDB();
        public ActionResult Index()
        {
            return View(db.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var role = db.Roles.FirstOrDefault(m => m.Id == id);
            if (role == null) return Content("未找到要编辑的角色");
            return View(role);
        }

        public ActionResult Save(Role role)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Roles.AddOrUpdate(role);
            db.SaveChanges();
            return Json(new { code = 200 });
        }

        public ActionResult Delete(int id)
        {
            Role role = new Role();
            role.Id = id;
            db.Roles.Attach(role);

            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}