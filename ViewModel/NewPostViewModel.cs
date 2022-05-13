﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.ViewModel
{
    public class RequiredSelectListItem :  ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as List<SelectListItem>;
            if(list != null)
            {
                return list.Where(x => x.Selected == true).Count() > 0;
            }
            return false;
        }
    }

    public enum PostType: int
    {
        [Display(Name = "Bài viết bình thường")]
        Normal = 1,
        [Display(Name = "Slide hình ảnh")]
        Slide = 2,
        [Display(Name = "Thảo luận")]
        Discuss = 3,
    }

    public enum Rated: int
    {
        [Display(Name = "Bài Viết Bình Thường")]
        Normal = 3,
        [Display(Name = "Đề Xuất Cao")]
        HighRated = 2,
        [Display(Name = "Đề Xuất Quan Trọng")]
        HighestRated = 1,
    }
      
    public enum Dynasty
    {
        
        [Display(Name = "Trong nước")]
        TrongNuoc = 0,
        [Display(Name = "Quốc tế")]
        QuocTe = 1,
        [Display(Name = "Khác")]
        Khac = 2,
    }
    public class NewPostViewModel
    {
        [RequiredSelectListItem(ErrorMessage = "Vui lòng chọn ít nhất 1 tag")]
        public List<SelectListItem>  post_tag { get; set; }
        public int post_id { get; set; }
        public int? userid { get; set; }

        [StringLength(200)]
        [MinLength(10, ErrorMessage ="Ít nhất 10 ký tự")]
        [Required(ErrorMessage ="Vui lòng nhập tiêu đề!")]
        public string post_title { get; set; }

        [StringLength(500)]
        [Display(Name = "PostTeaser")]
        public string post_teaser { get; set; }
        [StringLength(500)]
        public  string AvatarImage { get; set; }
        [StringLength(200)]
        public string post_review { get; set; }


        [AllowHtml]
        [Required(ErrorMessage = "Hãy nhập nội dung cho bài viết")]
        public string post_content { get; set; }

        [Range(1, 3, ErrorMessage = "Vui lòng chọn đúng kiểu bài viết!")]
        [Required(ErrorMessage = "Vui lòng chọn kiểu bài viết!")]
        public PostType post_type { get; set; }


        [StringLength(200)]
        public string meta_tag { get; set; }

        public DateTime? create_date { get; set; }

        public DateTime? edit_date { get; set; }

        public string imagepath { get; set; }

        public bool changeAvatar { get; set; } = false;

        [DataType(DataType.Upload)]
        public HttpPostedFileBase avatarFile { get; set; }

        public Dynasty dynasty { get; set; }

        public Rated Rated { get; set; } = Rated.Normal;

        public bool Status { get; set; }
        public bool UpdateSlug { get; set; } = false;
    }
}