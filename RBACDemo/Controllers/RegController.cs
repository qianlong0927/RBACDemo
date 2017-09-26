using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Filters;
using RBACDemo.Models;

namespace RBACDemo.Controllers
{
    [CustomAuthorization(AuthorizationType = AuthorizationType.None)]
    public class RegController : Controller
    {
        private RbacDB db = new RbacDB();
        // GET: Reg
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reg(User regUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            try
            {
                //获得要增加的角色实体
                var role = db.Roles.FirstOrDefault(r => r.Id == 3);
                //给用户增加角色
                regUser.Roles.Add(role);
                //把注册用户添加到用户表
                db.Users.Add(regUser);
                //保存到数据库(持久化数据)
                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(new { code = 404 });
            }
            
            return Json(new {code = 200});
        }
    }
}