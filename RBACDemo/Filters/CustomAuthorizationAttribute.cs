using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBACDemo.Models;

namespace RBACDemo.Filters
{
    /// <summary>
    /// 授权过滤器认证类型枚举
    /// None，无限制，不认证，比如登录，注册
    /// Identity，仅身份认证，比如首页，导航，头部
    /// Authorize，授权认证
    /// </summary>
    public enum AuthorizationType
    {
        None,Identity,Authorization
    }

    public class CustomAuthorizationAttribute:FilterAttribute,IAuthorizationFilter
    {
        /// <summary>
        /// 授权过滤器认证类型属性，默认是授权认证
        /// </summary>
        public AuthorizationType AuthorizationType { get;set; } = AuthorizationType.Authorization;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //1.无限制
            if (AuthorizationType == AuthorizationType.None) return;
            //2.身份认证
            if (filterContext.HttpContext.Session["user"] == null)
            {
                RedirectToLogin(filterContext);
                return;
            }
            if (AuthorizationType == AuthorizationType.Identity) return;
            //3.授权认证
            //3-1获取当前请求的控制器名称
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //3-2从Session里拿到用户登录的角色
            var role = filterContext.HttpContext.Session["role"] as Role;
            //3-3如果角色为空的，说明用户信息不完整，所以返回。
            if (role == null)
            {
                RedirectToLogin(filterContext);
                return;
            }
            //3-4查找角色模块里的控制器是否存在 我们请求的控制器
            var module = role.Modules.FirstOrDefault(m => m.Controller == controller);
            //3-5如果不存在，就重定向到登录页
            if (module == null)
            {
                RedirectToLogin(filterContext);
                return;
            }

            //Func<Module, bool> funcl = module =>
            //{
            //    return module.Controller != controller;
            //};

            //if (role.Modules.All(m => m.Controller != controller))
            //{
            //    RedirectToLogin(filterContext);
            //}
        }
        /// <summary>
        /// 重定向到登录页
        /// </summary>
        /// <param name="filterContext"></param>
        public void RedirectToLogin(AuthorizationContext filterContext)
        {
            //实例化一个UrlHelper对象
            var url = new UrlHelper(filterContext.RequestContext);
            //设置返回结果为重定向到登录页
            filterContext.Result = new RedirectResult(url.Action("Index", "Login"));
        }
    }
}