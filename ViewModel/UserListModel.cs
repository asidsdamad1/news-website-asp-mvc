using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebsite.ViewModel
{
    public class UserListModel
    {
        [Key]
        public int user_id { get; set; }


        [Required]
        [DisplayName("User name")]
        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string password { get; set; }

        [Required]
        [DisplayName("Full name")]
        [StringLength(50)]
        public string fullname { get; set; }

        public int role_id { get; set; }
        public string role_name { get; set; }

        public bool status { get; set; }
        public int post_count { get; set; }
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is not valid.")]
        public string email { get; set; }
        public virtual Role Role { get; set; }
       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}