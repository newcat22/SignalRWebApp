using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SignalRWebApp.Models;
using SignalRWebApp.Service;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalRWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _user;
        public HomeController(ILogger<HomeController> logger, IUserService user)
        {
            _logger = logger;
            _user = user;
        }

        /// <summary>
        /// 初始化数据库 
        /// </summary>
        /// <returns></returns>
        public bool CodeFirst()
        {
            return _user.CodeFirst();
        }

        /// <summary>
        /// 主界面：默认界面：WPF前后端分离不调，直接调用Login登录即可
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("userName");
            var Id = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(userName))
            {
                //说明用户信息不存在，未登录
                return Redirect("/Home/Login");
            }

            // 创建Claims列表
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Id)
                // 根据需要添加更多 Claims
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            ViewBag.UserName = userName;
            ViewBag.Id = Id;
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult loginUser(string name, string password)
        {
            return View();
        }



        /// <summary>
        /// 登出页面：WPF前后端分离不调
        /// </summary>
        /// <returns></returns>
        public IActionResult LogOut()
        {
            // 清除Session和Cookie
            HttpContext.Session.Remove("userName");
            Response.Cookies.Delete("userName");
            // 重定向到登录页
            return Redirect("/Home/Login");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Submit(string name, string password)
        {
            UserInfo userInfo = _user.GetUser(name, password);
            if (string.IsNullOrEmpty(userInfo.Id))
            {
                return Redirect("/Home/Error");
            }
            HttpContext.Session.SetString("Id", userInfo.Id);
            HttpContext.Session.SetString("userName", userInfo.Name);            
            Response.Cookies.Append("Id", userInfo.Id);
            Response.Cookies.Append("userName", userInfo.Name);
            return Redirect("/");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// 历史消息，WPF前后端分离不调
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetMessages(int pageIndex, int pageSize)
        {
            return Json(_user.GetMessages(pageIndex, pageSize));
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public ResultBean registerUser(string name, string password)
        {
            List<UserInfo> list = _user.getUserInfos();
            var result = new ResultBean
            {
                Success = true,
                Message = "Operation successful.",
                Data = list
            };
            return result;
        }


        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <returns></returns>
        public ResultBean selectUser()
        {
            List<UserInfo> list = _user.getUserInfos();
            var result = new ResultBean
            {
                Success = true,
                Message = "Operation successful.",
                Data = list
            };
            return result;
        }


        /// <summary>
        /// 按姓名查询用户
        /// </summary>
        /// <returns></returns>
        public ResultBean findUserByName(String userName)
        {
            List<UserInfo> list = _user.getUserInfosByName(userName);
            var result = new ResultBean
            {
                Success = true,
                Message = "Operation successful.",
                Data = list
            };
            return result;
        }


        /// <summary>
        /// 添加好友
        /// </summary>
        /// <returns></returns>
        public ResultBean addFriend(String userId, String friendId)
        {
            int insertResult = _user.addFriend(userId, friendId);
            var result = new ResultBean
            {
                Success = true,
                Message = "Operation successful.",
                Data = insertResult
            };
            return result;
        }


        /// <summary>
        /// 查询好友列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>        
        public ResultBean selectFriend(String userId)
        {
            List<UserInfo> list = _user.getFriendInfos(userId);
            var result = new ResultBean
            {
                Success = true,
                Message = "Operation successful.",
                Data = list
            };
            return result;
        }

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <returns></returns>
        public String removeFriend(String userId, String friendId)
        {

            return "test";
        }
    }
}