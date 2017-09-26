using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Filters;
using RBACDemo.Models;

namespace RBACDemo.Controllers
{
    [CustomAuthorization(AuthorizationType = AuthorizationType.Identity)]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 头部的分部视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            //不选择角色，只是用默认
            var user = Session["user"] as User;
            var role = Session["role"] as Role;

            ViewBag.UserName = user.Username;
            ViewBag.RoleName = role.Name;

            return PartialView();
        }
        /// <summary>
        /// 导航栏的分部视图
        /// 负责查询你所选择的角色的所有模块
        /// 并且把所有的模块渲染出来
        /// </summary>
        /// <returns></returns>
        public ActionResult Nav(int roleId=0)
        {
            /*
            //获取Session里的用户角色模块表
            var roleModules = Session["roleModules"] as Dictionary<int, ICollection<Module>>;
            //获取Session里默认的角色
            var role = Session["role"] as Role;
            var roles = Session["roles"] as List<Role>;
            //根据参数里roleId，获取某一个用户角色的模块表
            List<Module> modules;
            if (roleModules.ContainsKey(roleId))
            {
                modules = roleModules[roleId].ToList();
            }
            else
            {
                modules = roleModules[role.Id].ToList();
            }*/

            //第二种方式，只是用默认角色，不选择
            //获取Session里默认的角色
            var role = Session["role"] as Role;
            //从默认角色里获取模块
            var modules = role.Modules;

            
            return PartialView(modules);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}