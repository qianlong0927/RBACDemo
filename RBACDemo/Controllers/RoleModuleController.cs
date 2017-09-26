using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;
using System.Data.Entity;
using RBACDemo.ViewModel;

namespace RBACDemo.Controllers
{
    public class RoleModuleController : Controller
    {
        private RbacDB db = new RbacDB();
        public ActionResult Index()
        {
            var result = db.Roles.Include(r =>r.Modules);
            return View(result);
        }

        public ActionResult Create()
        {
            //所有角色的下拉列表项
            ViewBag.RoleOptions = from r in db.Roles
                select new SelectListItem {Text = r.Name, Value = r.Id.ToString()};
            //所有模块的下拉列表
            ViewBag.ModuleOptions = from m in db.Modules
                                  select new SelectListItem { Text = m.Name, Value = m.Id.ToString() };
            return View();
        }

        public ActionResult Edit(RoleModuleViewModel roleModule)
        {
            roleModule.RoleName = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId).Name;
            roleModule.ModuleName = db.Modules.FirstOrDefault(r => r.Id == roleModule.ModuleId).Name;

            //所有模块的下拉列表项
            ViewBag.ModuleOptions = from r in db.Modules
                                  select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };

            return View(roleModule);
        }

       

        public ActionResult Delete(RoleModuleViewModel roleModule)
        {
            //先要把要删除权限的角色找出来
            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);
            //var role = new Role { Id = roleModule.RoleId};
            //构造一个要删除的模块
            var module = new Module { Id = roleModule.ModuleId };
            //伪装成从数据库读取出来的一样
            db.Modules.Attach(module);
            //把这个要删除的权限模块，从脚色的模块集合中移除
            role.Modules.Remove(module);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Insert(RoleModuleViewModel roleModule)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(new {code = 400});
            //}
            //先把要添加的权限的角色找出来
            var role = db.Roles.FirstOrDefault(r=>r.Id==roleModule.RoleId);
            var module = new Module {Id = roleModule.ModuleId };
            //伪装成从数据库读取出来的一样
            db.Modules.Attach(module);
            //这一步是关联的关键，把module添加到role的Module
            role.Modules.Add(module);
            //需要把这个角色添加实体集合
            //db.Roles.Add(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new {code = 400});
            }
            return Json(new {code = 200});
        }

        public ActionResult Update(RoleModuleViewModel roleModule)
        {
            if (roleModule.ModuleId == roleModule.UpdateModuleId)
            {
                return Json(new {code = 400});
            }
            //先把要更新的权限的角色找出来
            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);

            //构造一个原来的模块
            var module = new Module { Id = roleModule.ModuleId };
            //伪装成从数据库读取出来的一样
            db.Modules.Attach(module);

            //构造一个要更新的模块
            var updatemodule = new Module { Id = roleModule.UpdateModuleId };
            //伪装成从数据库读取出来的一样
            db.Modules.Attach(updatemodule);

            //这一步是关联的关键，把module添加到role的Module
            role.Modules.Remove(module);
            role.Modules.Add(updatemodule);
            //需要把这个角色添加实体集合
            //db.Roles.Add(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}