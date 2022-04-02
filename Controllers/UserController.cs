using NewsWebsite.Models;
using NewsWebsite.Repository;
using NewsWebsite.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static NewsWebsite.MvcApplication;

namespace NewsWebsite.Admin.Controllers
{
    public class UserController : Controller
    {
        UnitOfWork UnitOfWork = new UnitOfWork(new NewsDbContext());
        // GET: User

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
       

        public ActionResult UserManager()
        {
            List<UserListModel> listUsers = UnitOfWork.userRepository.AllUsers()
                .Select(m => new UserListModel
                {
                    user_id = m.user_id,
                    username = m.username,
                    fullname = m.fullname,
                    role_id = m.role_id,
                    email = m.email,
                    status = m.status,
                    Posts = m.Posts,
                    role_name = m.Role.role_name,
                    post_count = m.Posts.Count
                }).ToList();
            return View(listUsers);
        }
        public ActionResult List()
        {
            List<UserListModel> listUsers = UnitOfWork.userRepository.AllUsers()
                .Select(m => new UserListModel
                {
                    user_id = m.user_id,
                    username = m.username,
                    fullname = m.fullname,
                    role_id = m.role_id,
                    email = m.email,
                    status  = m.status,
                    Posts =  m.Posts,
                    role_name = m.Role.role_name,
                    post_count = m.Posts.Count
                }).ToList();
            return View(listUsers);
        }

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                User user = UnitOfWork.userRepository.FindByUsername(model.Username);
                if (user != null)
                {

                    if (user.password == CommonData.CommonFunction.CalculateMD5Hash(model.Password) && user.status == true)
                    {
                        setCookie(user.username, model.RememberMe, user.role_id.ToString());
                        if (ReturnUrl != null)
                            return Redirect(ReturnUrl);
                        return RedirectToAction("Index", "Admin");
                    }

                    ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";

                    return View();
                }
            }

            ViewBag.Error = "Wrong username or password!";
            return View();
        }

    
        public void setCookie(string username, bool rememberme = false, string role = "2")
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               rememberme,
                               role
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [Authorize(Roles = "1")]
        public ActionResult createUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createUser(UserListViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = UnitOfWork.userRepository.FindByUsername(model.username);
                if (user == null)
                {
                    User nuser = new User
                    {
                        username = model.username,
                        fullname = model.fullname,
                        status = true,
                        role_id = 2,
                        password = CommonData.CommonFunction.CalculateMD5Hash(model.password)
                    };
                    UnitOfWork.userRepository.Add(nuser);
                    UnitOfWork.Commit();
                    return RedirectToAction("UserManager");
                }
                else
                {
                    ViewBag.anno = "Tên người dùng này đã tồn tại";
                    return View();
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.oldpassword == model.password)
                {
                    ViewBag.anno = "Mật khẩu mới không được trùng mật khẩu cũ !";
                    return View();
                }
                else
                {
                    User user = UnitOfWork.userRepository.FindByUsername(User.Identity.Name);
                    if (user != null)
                    {
                        user.password = CommonData.CommonFunction.CalculateMD5Hash(model.password);
                        UnitOfWork.Commit();
                        return RedirectToAction("Logout");
                    }
                }
            }
            return View();
        }
        [Authorize(Roles = "1")]
        public JsonResult changeStatus(int id, bool state = true)
        {
            string prefix = state ? "Đã bỏ cấm" : "Đã cấm";
            User u = UnitOfWork.userRepository.FindByID(id);
            if (u.role_id != 1)
            {
                u.status = state;
                UnitOfWork.Commit();
                return Json(new { Message = prefix + " \"" + u.username + "\"" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "Không được cấm admin" }, JsonRequestBehavior.AllowGet);
        }

        NewsDbContext db = new NewsDbContext();
        public ActionResult Edit(string username)
        {
            if(String.IsNullOrWhiteSpace(username))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            User user = UnitOfWork.userRepository.FindByUsername(username);
            UserListModel model = new UserListModel();
            if (user == null)
            {
                return HttpNotFound();
            }
            model = new UserListModel
            {
                user_id = user.user_id,
                username = user.username,
                password = user.password,
                email = user.email,
                fullname = user.fullname,
                status = user.status,
                Role = user.Role,
                Posts = user.Posts,
                role_id =  user.role_id
            };
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name");
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(UserListModel user)
        {
            //User userUpdate = new Models.User();
            //userUpdate.AddRange(model.post_tag.Where(m => m.Selected)
            //    .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text })
            //    );
            //List<Tag> untaglist = new List<Tag>();
            //untaglist.AddRange(model.post_tag.Where(m => m.Selected == false)
            //    .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text })
            //    );
            if (ModelState.IsValid)
            {
                User entity = UnitOfWork.userRepository.FindByUsername(user.username);
                entity.username = user.username;
                entity.email = user.email;
                entity.role_id = user.role_id;
                entity.fullname = user.fullname;
                
                UnitOfWork.userRepository.Update(entity);
                UnitOfWork.Commit();
                return RedirectToAction("UserManager");
            }
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            return View(user);
        }

    }
}