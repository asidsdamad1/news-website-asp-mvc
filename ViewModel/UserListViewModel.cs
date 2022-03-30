using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebsite.ViewModel
{
    public class UserListViewModel
    {
        public int userid { get; set; }

        [Required(ErrorMessage = "Chưa nhập username, max 20 ký tự")]
        [StringLength(20)]
        public string username { get; set; }

        [Required(ErrorMessage = "Chưa nhập password")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Bạn nhập mật khẩu không trùng nhau!")]
        public string repassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập fullname")]
        [StringLength(30)]
        public string fullname { get; set; }

        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is not valid.")]
        public string email { get; set; }


        public int userrole { get; set; }
    }
}