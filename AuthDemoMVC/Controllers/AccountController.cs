using AuthDemoMVC.ViewModels;
using DAL;
using DAL.Entities;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace AuthDemoMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (AuthDBContext dBContext = new AuthDBContext())
            {
                var IsValidUser = dBContext.Users.Any(u => u.UserName.ToLower() == model.UserName.ToLower()
                                                           && u.Password == model.Password);

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View();
                }
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (AuthDBContext dBContext = new AuthDBContext())
            {
                var IfAlreadyPresent = dBContext.Users.Any(u => u.UserName.ToLower() == model.UserName.ToLower());
                if (IfAlreadyPresent)
                {
                    ModelState.AddModelError("", $"Username {model.UserName} is already taken try again!");
                    return View(model);
                }
                else
                {
                    User user = new User();
                    user.UserName = model.UserName;
                    user.Password = model.Password;

                    dBContext.Users.Add(user);
                    dBContext.SaveChanges();
                }
            }
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return View("Login");
        }
    }
}