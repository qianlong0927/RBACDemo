using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;
using RBACDemo.ViewModel;

namespace RBACDemo.Controllers
{
    public class UserRoleController : Controller
    {
        private RbacDB db = new RbacDB();
        public ActionResult Index()
        {
            var result = db.Users.Include(r => r.Roles);
            return View(result);
        }

        public ActionResult Create()
        {
            //所有用户的下拉列表项
            ViewBag.UserOptions = from r in db.Users
                                  select new SelectListItem { Text = r.Username, Value = r.Id.ToString() };
            //所有角色的下拉列表
            ViewBag.RoleOptions = from m in db.Roles
                                    select new SelectListItem { Text = m.Name, Value = m.Id.ToString() };
            return View();
        }

        public ActionResult Edit(UserRoleViewModel userRole)
        {
            userRole.UserName = db.Users.FirstOrDefault(r => r.Id == userRole.UserId).Username;
            userRole.RoleName = db.Roles.FirstOrDefault(r => r.Id == userRole.RoleId).Name;

            //所有模块的下拉列表项
            ViewBag.RoleOptions = from r in db.Roles
                                    select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };

            return View(userRole);
        }



        public ActionResult Delete(UserRoleViewModel userRole)
        {
            //先要把要删除权限的用户找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);

            //var role = new Role { Id = roleModule.RoleId};
            //构造一个要删除的角色
            var role= new Role { Id = userRole.RoleId };
            //伪装成从数据库读取出来的一样
            db.Roles.Attach(role);
            //把这个要删除的角色模块，从用户的角色集合中移除
            user.Roles.Remove(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Insert(UserRoleViewModel userRole)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(new {code = 400});
            //}
            //先把要添加的角色的用户找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);
            var role = new Role { Id = userRole.RoleId };
            //伪装成从数据库读取出来的一样
            db.Roles.Attach(role);
            //这一步是关联的关键，把module添加到role的Module
            user.Roles.Add(role);
            //需要把这个角色添加实体集合
            //db.Roles.Add(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }

        public ActionResult Update(UserRoleViewModel userRole)
        {
            

            if (userRole.RoleId == userRole.UpdateRoleId)
            {
                return Json(new { code = 400 });
            }
            //先把要更新的权限的角色找出来
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);

            //构造一个原来的模块
            var role = new Role {Id = userRole.RoleId};
            //伪装成从数据库读取出来的一样
            db.Roles.Attach(role);

            //构造一个要更新的模块

            var updaterole = new Role { Id = userRole.UpdateRoleId };
            //伪装成从数据库读取出来的一样
            db.Roles.Attach(updaterole);
            //这一步是关联的关键，把module添加到role的Module
            user.Roles.Remove(role);
            user.Roles.Add(updaterole);
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