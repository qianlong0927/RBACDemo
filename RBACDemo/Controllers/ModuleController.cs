using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;

namespace RBACDemo.Controllers
{
    public class ModuleController : Controller
    {
        // GET: Module
        private RbacDB db = new RbacDB(); 
        public ActionResult Index()
        {
            return View(db.Modules);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var module = db.Modules.FirstOrDefault(m => m.Id == id);
            if (module == null) return Content("未找到要编辑的模块");
            return View(module);
        }

        public ActionResult Save(Module module)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {code = 400});
            }
            db.Modules.AddOrUpdate(module);
            db.SaveChanges();
            return Json(new {code = 200});
        }

        public ActionResult Delete(int id)
        {
            Module module = new Module();
            module.Id = id;
            db.Modules.Attach(module);

            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}