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
    public class LoginController : Controller
    {
        private RbacDB db = new RbacDB();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User loginUser)
        {
            if (!ModelState.IsValid)
            {
                //绑定模型验证失败
                return Json(new {code = 400});

            }
            //查找用户
            var user = db.Users.FirstOrDefault(
                u => u.Username == loginUser.Username && u.Password == loginUser.Password
                );
            //如果没找到，就返回404
            if (user == null) return Json(new {code = 404});
            Session["user"] = user;

            /*
            //获取所有角色的所有模块，转换成字典类，key是角色的id，value是角色所有用的模块集合
            var roleModules = user.Roles.ToDictionary(r => r.Id, r => r.Modules);
            //存入到Session,之后再复用，不用再去查询数据库。
            Session["roleModules"] = roleModules;

            //获取用户列表
            var roles = user.Roles.ToList();
            //存入到Session以便之后复用
            Session["roles"] = roles;
            //设置默认角色为用户角色表里的第一个
            Session["role"] = roles[0];
              */

            //Func<T,T....Tresult>
            //Func<Role, bool> funcl = delegate(Role role1)
            //{
            //    if (role1.Id == 3) return true;
            //    return false;
            //};

            //Func<Role, bool> funcl = role1 =>
            //{
            //    if (role1.Id == 3) return true;
            //    return false;
            //};


            //r=>true就是只拿一条
            var role = user.Roles.FirstOrDefault(rolel=>true);

            Session["role"] = role;
            return Json(new {code = 200});
        }

        private bool Funcl(Role role1)
        {
            if (role1.Id == 3) return true;
            return false;
        }
      
    }
}