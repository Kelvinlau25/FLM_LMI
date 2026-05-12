using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HomeModel;
using MIB_FILM_CLD_MM_MVC.Extensions;
using MIB_FILM_CLD_MM_MVC.Filters;
using PAB.Helper_Code.Objects;
using PAB.Repository;

using System.Security.Cryptography;

namespace ACL_System.Controllers
{

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public CommonRepo CommonRepo = new CommonRepo();
        public ACLRepo ACLRepo = new ACLRepo();

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {

            string userAD = string.Empty;
            string vUserAD = string.Empty;
            string systemName = string.Empty;

            AuthenticatorModel model = new AuthenticatorModel();
            //userAD = Environment.UserName;
            userAD = HttpContext.User.Identity?.Name;// "mint_teoh";//
            systemName = _configuration["AppSettings:SystemName"];
            
            // Check if userAD is null or empty before attempting to split
            if (!string.IsNullOrEmpty(userAD))
            {
                string[] splitWords = userAD.Split('\\');
                vUserAD = splitWords[splitWords.Length - 1];

                model = ACLRepo.ValidateUserInfo(vUserAD, systemName);
                ViewBag.Validate = model.VALID_USER;
                ViewBag.userAD = userAD;
                ViewBag.userAD2 = vUserAD;
                if (model.VALID_USER == true)
                {
                    HttpContext.Session.SetObject("AclUser", new ACL_UserObj
                    {
                        ID_ACL_USER = model.ID_ACL_USER,
                        ID_ACL_ROLE = model.ID_ACL_ROLE,
                        ID_ACL_RESOURCE = model.ID_ACL_RESOURCE,
                        USER_ID = model.USER_ID,
                        USR_EMAIL = model.USR_EMAIL,
                        COMPANY = model.COMPANY,
                        EMP_NO = model.EMP_NO,
                        EMP_NAME = model.EMP_NAME,
                        ROLE_NAME = model.ROLE_NAME,
                        ROLE_DESC = model.ROLE_DESC,
                        RESOURCE_NAME = model.RESOURCE_NAME,
                        RESOURCE_DESC = model.RESOURCE_DESC
                    });
                }
            }
            else
            {
                // User is not authenticated, redirect to login
                return RedirectToAction("Login");
            }

            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthenticatorModel model)
        {
            if (ModelState.IsValid)  //checking model is valid or not
            {
                string systemName = string.Empty;
                systemName = _configuration["AppSettings:SystemName"];
                string passwordTMP = model.PASSWORD;
                // debug use only
                //    HttpContext.Session.SetObject("AclUser", new ACL_UserObj
                //    {
                //        ID_ACL_USER = model.ID_ACL_USER,
                //        ID_ACL_ROLE = model.ID_ACL_ROLE,
                //        ID_ACL_RESOURCE = model.ID_ACL_RESOURCE,
                //        USER_ID = model.USER_ID,
                //        USR_EMAIL = model.USR_EMAIL,
                //        COMPANY = model.COMPANY,
                //        EMP_NO = model.EMP_NO,
                //        EMP_NAME = "Admin",
                //        ROLE_NAME = model.ROLE_NAME,
                //        ROLE_DESC = model.ROLE_DESC,
                //        RESOURCE_NAME = model.RESOURCE_NAME,
                //        RESOURCE_DESC = model.RESOURCE_DESC
                //    });
                //    return RedirectToAction("Menu", model);
                //}

                // Debug use only
                model = ACLRepo.ValidateUserInfo(model.LOGIN_ID, systemName);

                if (VerifyHashedPassword(model.PASSWORD, passwordTMP))
                {
                    model.VALID_USER = true;
                }
                else
                {
                    model.VALID_USER = false;
                }

                ViewBag.Validate = model.VALID_USER;

                ModelState.Clear();
                if (model.VALID_USER == true)
                {
                    HttpContext.Session.SetObject("AclUser", new ACL_UserObj
                    {
                        ID_ACL_USER = model.ID_ACL_USER,
                        ID_ACL_ROLE = model.ID_ACL_ROLE,
                        ID_ACL_RESOURCE = model.ID_ACL_RESOURCE,
                        USER_ID = model.USER_ID,
                        USR_EMAIL = model.USR_EMAIL,
                        COMPANY = model.COMPANY,
                        EMP_NO = model.EMP_NO,
                        EMP_NAME = model.EMP_NAME,
                        ROLE_NAME = model.ROLE_NAME,
                        ROLE_DESC = model.ROLE_DESC,
                        RESOURCE_NAME = model.RESOURCE_NAME,
                        RESOURCE_DESC = model.RESOURCE_DESC
                    });
                    return RedirectToAction("Menu", model);
                }
                else
                {
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError("", "Error in saving data");
                return View();
            }

        }
        [SessionExpireFilter]
        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult SideBar()
        {
            int roleID = HttpContext.Session.GetObject<ACL_UserObj>("AclUser").ID_ACL_ROLE;
            string systemName = _configuration["AppSettings:SystemName"];
            var dt = ACLRepo.sideBarDB(roleID, systemName);
            List<SideBarContent> SideBarModel = CommonRepo.ConvertToList<SideBarContent>(dt);
            return View(SideBarModel);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel ChangePasswordModel)
        {
            var aclUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            DataTable dt = ACLRepo.oldPassword(aclUser.ID_ACL_USER);
            //DataTable dt = db.oldPassword(25);
            bool a = VerifyHashedPassword(dt.Rows[0][0].ToString(), ChangePasswordModel.OLD_PASSWORD);
            if (VerifyHashedPassword(dt.Rows[0][0].ToString(), ChangePasswordModel.OLD_PASSWORD))
            {
                if (ACLRepo.NewPassWord(aclUser.ID_ACL_USER, HashPassword(ChangePasswordModel.NEW_PASSWORD)) == "Y")
                {
                    ViewData["Message"] = "Successful";
                    ViewData["MessageType"] = "Y";
                }
                else
                {
                    ViewData["Message"] = "Failed";
                    ViewData["MessageType"] = "E";
                }
            }
            else
            {
                ViewData["Message"] = "Failed";
                ViewData["MessageType"] = "E";
            }
            ModelState.Clear();
            return View();
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            //if ((src.Length != 0x31) || (src[0] != 0))
            //{
            //    return false;
            //}
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return buffer3.SequenceEqual(buffer4);
        }

        public ActionResult SearchDetail(string[] search)
        {
            foreach (var col in search)
            {
                string[] c = col.Split(new Char[] { '/' });
            }
            return View();
        }

        public ActionResult AdvancedSearch(string actionRequested)
        {
            string action = actionRequested.Split('/')[0];
            string controller = actionRequested.Split('/')[1];
            Type myType = typeof(PAB.Models.SearchSource);
            string[] searchSource = (string[])myType.GetField(action).GetValue(null);
            ViewData["searchSource"] = searchSource;
            ViewData["Action"] = action;
            ViewData["Controller"] = controller;

            return View();
        }

        public ActionResult AddNew(string act, string ctrl)
        {
            return RedirectToAction(act, ctrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View();
        }

    }
}
