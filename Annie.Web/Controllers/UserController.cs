using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annie.Web.Services;
using Annie.Web.ViewModels.User;
using Annie.Web.Services.UserServices;
using Annie.Web.Services.UserServices.Models;
using Annie.Web.Services.AuthServices;
using Annie.Web.Services.AuthServices.Models;
using Microsoft.AspNetCore.Http;
using Annie.Authorization.Filter;
using Annie.Data;
using Annie.Model;
using Annie.Data.Static;
using Annie.Web.Models.Core;

namespace Annie.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IStaticRepository _staticRepository;

        public UserController(IUserService userService, IAuthService authService, IStaticRepository staticRepository)
        {
            _userService = userService;
            _authService = authService;
            _staticRepository = staticRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToRoute("HomePage");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddMessage(new SystemMessages.LoginMessage(new LoginStatus() { Status = LoginStatus.UserLoginStatus.DataIsNotValid }, Url.Action("ResendConfirmRegistrationEmail", "User")));
                return RedirectToAction("Login");
            }

            var loginStatus = await _authService.LoginAsync(new LoginData(viewModel));

            if (loginStatus.Status != LoginStatus.UserLoginStatus.Success)
            {
                TempData.AddMessage(new SystemMessages.LoginMessage(loginStatus, Url.Action("ResendConfirmRegistrationEmail", "User")));
                return RedirectToAction("Login");
            }

            return RedirectToRoute("HomePage");
        }

        [HttpGet, Authorize()]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToRoute("HomePage");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToRoute("HomePage");

            var viewModel = new SignUp()
            {
                Roles = _staticRepository.Roles.Where(r => r.Id != (int)Roles.Global && r.Id != (int)Roles.Admin).ToList(),
                Disciplines = _staticRepository.Disciplines
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddMessage(new SystemMessages.SignUpMessage(new SignUpStatus() { Status = SignUpStatus.UserSignUpStatus.DataIsNotValid }));
                return RedirectToAction("SignUp", "User", viewModel);
            }

            var signUpStatus = await _userService.SignUpUserAsync(new SignUpData(viewModel));
            TempData.AddMessage(new SystemMessages.SignUpMessage(signUpStatus));

            if (signUpStatus.Status != SignUpStatus.UserSignUpStatus.Success)
                return RedirectToAction("SignUp", "User", viewModel);

            return RedirectToAction("Login", "User");
        }

    }
}
