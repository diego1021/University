using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.web.App_Start;
using University.BL.DTOs;
using University.BL.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using University.BL.Services.Implements;
using Microsoft.Owin.Security;


namespace University.web.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser {UserName = registerDTO.Email, Email = registerDTO.Email};
                var result = await UserManager.CreateAsync(user, registerDTO.Password);

                if (result.Succeeded)
                {
                    await SendEmailConfirmationTokenAsync( user.Id, user.UserName, user.Email, "Por favor valida tu cuenta de University");

                    ViewBag.Message = "Check your Email and confirm your Account , you must be confirmed "
                        + "before you can log in.";

                    //ViewBag.Message = string.Format("User {0} was created successfully!", user.UserName);

                    return View("Info", "_LayoutAccount");
                }

                AddErrors(result);
            }

            return View(registerDTO);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return View(loginDTO);

            var result = await SignInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password,
                loginDTO.RememberMe,
                shouldLockout: false);

            var user = await UserManager.FindByNameAsync(loginDTO.Email);
            if(user != null)
            {
                if(!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    await SendEmailConfirmationTokenAsync(user.Id, user.UserName, user.Email, "Por favor valida tu cuenta de University");

                    ViewBag.errorMessage = "You must have a confirmed email to log on. "
                        + "The confirmation token has been resent to your email account.";

                    return View("Error", "_LayoutAccount");
                }
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                    return View("Lockout", "_LayoutAccount");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(loginDTO);
            }

        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        #region Helpers

        public async Task SendEmailConfirmationTokenAsync(string userID,
            string userName,
            string userEmail,
            string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userid = userID, code = code }, protocol: Request.Url.Scheme);

            EmailService emailService = new EmailService();
            var data = new { UserName = userName, CallbackUrl = callbackUrl };
            var basePathTemplate = Server.MapPath("~/Resources/Templates/ConfirmEmail.html");
            var content = emailService.GetHtml(basePathTemplate, data);
            List<Documents> documents = new List<Documents>();

            documents.Add(new Documents
            {
                ContentId = "angular_logo",
                Disposition = Disposition.inline.ToString(),
                Type = "image/png",
                Filename = "angular_logo",
                Path = Server.MapPath("~/Resources/Templates/Images/angular_logo.png")
            });

            await emailService.SendNotification(documents, userEmail, subject, content);
        }

        #endregion

    }
}