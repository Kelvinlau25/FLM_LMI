using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using PAB.Helper_Code.Objects;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeModel
{

    #region Menu
    public class SideBarContent
    {

        public int ID_ACL_RESOURCE { get; set; }
        public int RESOURCE_PARENT_ID { get; set; }
        public string RESOURCE_DESC { get; set; }
        public string RESOURCE_name { get; set; }
        public string RESOURCE_VIEW { get; set; }
        public string RESOURCE_CONTROLLER { get; set; }
        public int LAYER { get; set; }
        public int ACTION { get; set; }
    }
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Old Password")]
        public string OLD_PASSWORD { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string NEW_PASSWORD { get; set; }
        [Required]
        [NotMapped]
        [Compare("NEW_PASSWORD")]
        [Display(Name = "Confirm New Password")]
        public string CONFIRM_NEW_PASSWORD { get; set; }


    }
    #endregion
    public class AuthenticatorModel
    {
        public int ID_ACL_USER { get; set; }
        public string USER_ID { get; set; }
        public string USR_EMAIL { get; set; }
        public string COMPANY { get; set; }
        public string EMP_NO { get; set; }
        public string EMP_NAME { get; set; }
        public int ID_ACL_ROLE { get; set; }
        public string ROLE_NAME { get; set; }
        public string ROLE_DESC { get; set; }
        public int ID_ACL_RESOURCE { get; set; }
        public string RESOURCE_NAME { get; set; }
        public string RESOURCE_DESC { get; set; }
        public bool VALID_USER { get; set; }
        [Required(ErrorMessage = "* Please Enter Username.")]
        [Display(Name = "Username")]
        public string LOGIN_ID { get; set; }
        [Required(ErrorMessage = "* Please Enter password.")]
        [Display(Name = "Password")]
        public string PASSWORD { get; set; }

    }
}