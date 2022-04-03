using NewsWebsite.CommonData;
using NewsWebsite.Models;
using NewsWebsite.Repository;
using NewsWebsite.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace NewsWebsite.Admin.Controllers
{
    //1-admin, 2-editor
    [Authorize(Roles = "1,2")]
    public class AdminController : Controller
    {
        UnitOfWork db = new UnitOfWork(new NewsDbContext());
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        public ActionResult ListPost(string sortOrder, 
                                     string currentSort, 
                                     int? page, 
                                     DateTime? startDate, 
                                     DateTime? endDate)
        {
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<Post> posts = null;
            ViewBag.Sort = "tăng dần";
         
            

            if (!startDate.HasValue && !endDate.HasValue)
            {

                    switch (sortOrder)
                    {
                        case "Title":
                            ViewBag.SortName = "tiêu đề";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                posts = db.postRepository.AllPosts().OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            else
                            {
                                posts = db.postRepository.AllPosts().OrderBy
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "CreateDate":
                            ViewBag.SortName = "ngày tạo";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                posts = db.postRepository.AllPosts().OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }
                            else
                            {
                                posts = db.postRepository.AllPosts().OrderBy
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "ViewCount":
                            ViewBag.SortName = "luợt xem";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                posts = db.postRepository.AllPosts().OrderByDescending
                                    (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }
                            else
                            {
                                posts = db.postRepository.AllPosts().OrderBy
                                    (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                    }

                
            }
            else
            {
                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;

                if (!startDate.HasValue) startDate = DateTime.Now.Date;
                if (!endDate.HasValue) endDate = DateTime.Now.Date;
                if (endDate < startDate) endDate = startDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.SortName = "tiêu đề";
                        if (sortOrder.Equals(currentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate && m.create_date <= endDate).OrderByDescending
                                (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate && m.create_date <= endDate ).OrderBy
                                (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.SortName = "ngày tạo";
                        if (sortOrder.Equals(currentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate && m.create_date <= endDate ).OrderByDescending
                                (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate && m.create_date <= endDate ).OrderBy
                                (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "ViewCount":
                        ViewBag.SortName = "luợt xem";
                        if (sortOrder.Equals(currentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate && m.create_date <= endDate ).OrderByDescending
                                (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            posts = db.postRepository.AllPosts().Where(m => m.create_date >= startDate  && m.create_date <= endDate ).OrderBy
                                (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                }

            }
            return View(posts);
        }

        public ActionResult newPost()
        {
            NewPostViewModel model = new NewPostViewModel
            {
                post_type = PostType.Normal,
                post_tag = PostData.getTagList(),
                Status = true,
                changeAvatar = true
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult newPost(NewPostViewModel model)
        {
            List<Tag> tags = new List<Tag>();
            tags.AddRange(model.post_tag.Where(m => m.Selected)
                .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text }));
            if(ModelState.IsValid && tags.Count > 0)
            {
                //Neu teaser trong hoac qua ngan se dung content thay the
                if(model.post_teaser == null || model.post_teaser.Length <= 20)
                {
                    model.post_teaser = CommonFunction.GetTeaserFromContent(model.post_content, 200);
                }
                bool isSaveSuccessfully = true;
                try
                {
                    if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        string extension = Path.GetExtension(model.avatarFile.FileName);
                        model.AvatarImage = SlugGenerator.SlugGenerator.GenerateSlug(model.post_title) + "-" + new Random().Next(1, 100) + extension;
                        model.avatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.AvatarImage);
                    }
                }
                catch (Exception)
                {
                    isSaveSuccessfully = false;
                }
                if (isSaveSuccessfully)
                {
                    User user = db.userRepository.FindByUsername(User.Identity.Name);
                    Post post = new Post
                    {
                        user_id = user.user_id,
                        create_date = DateTime.Now,
                        dynasty = model.dynasty.ToString(),
                        avatar_image = model.AvatarImage,
                        post_content = model.post_content,
                        post_review = model.post_review,
                        post_title = model.post_title,
                        post_type = (int)model.post_type,
                        view_count = 0,
                        rated = (int)model.Rated,
                        post_teaser = model.post_teaser,
                        status = model.Status,
                        post_tag = model.meta_tag,
                    };
                    foreach (var i in tags)
                    {
                        Tag tag = db.tagRepository.FindByID(i.tag_id);
                        post.Tags.Add(tag);
                        tag.Posts.Add(post);
                    }
                    string slug = SlugGenerator.SlugGenerator.GenerateSlug(post.post_title.ToLower());
                    post.post_slug = slug;
                    if (db.postRepository.AllPosts().Any(m => m.post_slug == slug))
                    {
                        post.post_slug = slug + "-" + 1;
                    }
                    db.postRepository.AddPost(post);
                    db.Commit();
                    if (model.post_type.Equals(PostType.Slide))
                    {
                        // return View("UploadImage", new UploadViewModel { id = post.post_id, title = post.post_title });
                    }

                    post.post_slug += "-" + db.postRepository.FindIdByUsername(post.post_title);
                    db.Commit();
                    return RedirectToAction("ListPost");
                }
            }
           
            return View(model);
        }
        public ActionResult editPost(int id)
        {
            Post post = db.postRepository.FindByID(id);
            if (post == null)
            {
                return RedirectToAction("ListPost");
            }
            NewPostViewModel model = new NewPostViewModel();
            if (post.User.username == User.Identity.Name || User.IsInRole("1"))
            {
                Dynasty dn;
                //dynasty - khu vực 
                Enum.TryParse(post.dynasty, out dn);
                Rated rated;
                Enum.TryParse(post.rated.ToString(), out rated);
                PostType type;
                Enum.TryParse(post.post_type.ToString(), out type);
                List<SelectListItem> tag = PostData.getTagList();
                //string[] oldtag = post.post_tag.Split(',');
                foreach (var x in tag)
                {
                    foreach (var t in post.Tags)
                    {
                        if (x.Value == t.tag_id.ToString())
                        {
                            x.Selected = true;
                        }
                    }
                }
                model = new NewPostViewModel
                {
                    post_id = post.post_id,
                    dynasty = dn,
                    post_content = post.post_content,
                    AvatarImage = post.avatar_image,
                    post_review = post.post_review,
                    changeAvatar = false,
                    imagepath = "/images/slides/" + post.post_id,
                    Rated = rated,
                    Status = post.status,
                    post_tag = tag,
                    post_teaser = post.post_teaser,
                    post_title = post.post_title,
                    post_type = type,
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult editPost(NewPostViewModel model)
        {
            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.post_tag.Where(m => m.Selected)
                .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text })
                );
            List<Tag> untaglist = new List<Tag>();
            untaglist.AddRange(model.post_tag.Where(m => m.Selected == false)
                .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text })
                );
            if (ModelState.IsValid && taglist.Count > 0)
            {
                // Nếu như teaser trống hoặc quá ngắn, sẽ dùng content thay thế từ content bài viết
                if (model.post_teaser == null || model.post_teaser.Length <= 20)
                {
                    model.post_teaser = CommonFunction.GetTeaserFromContent(model.post_content, 200);
                }
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;

                try
                {
                    if (model.changeAvatar == true && model.avatarFile != null && model.avatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        //xóa ảnh cũ
                        bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + model.AvatarImage));
                        if (exist)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Upload/images/" + model.AvatarImage));
                        }
                        // lưu ảnh mới với tên là slug tiêu đề mới
                        string extension = Path.GetExtension(model.avatarFile.FileName);
                        model.AvatarImage = SlugGenerator.SlugGenerator.GenerateSlug(model.post_title) + "-" + model.post_id + extension;
                        model.avatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.AvatarImage);
                    }
                }
                catch (Exception )
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    Post pOST = db.postRepository.FindByID(model.post_id);
                    if (pOST.User.username != User.Identity.Name)
                    {
                        return RedirectToAction("ListPost");
                    }
                    pOST.dynasty = model.dynasty.ToString();
                    pOST.edit_date = DateTime.Now;
                    pOST.avatar_image = model.AvatarImage;
                    pOST.post_content = model.post_content;
                    pOST.post_review = model.post_review;
                    pOST.post_title = model.post_title;
                    pOST.post_type = (int)model.post_type;
                    pOST.rated = (int)model.Rated;
                    pOST.post_teaser = model.post_teaser;
                    pOST.status = model.Status;
                    pOST.post_tag = model.meta_tag;
                    foreach (var i in taglist)
                    {
                        Tag tags = db.tagRepository.FindByID(i.tag_id);
                        if (!pOST.Tags.Contains(tags))
                        {
                            pOST.Tags.Add(tags);
                            tags.Posts.Add(pOST);
                        }
                        //else if(pOST.Tbl_Tags.Contains(tags))
                        //{
                        //    pOST.Tbl_Tags.Remove(tags);
                        //    tags.Tbl_POST.Remove(pOST);
                        //}

                    }
                    foreach (var i in untaglist)
                    {
                        Tag tags = db.tagRepository.FindByID(i.tag_id);
                        if (pOST.Tags.Contains(tags))
                        {
                            pOST.Tags.Remove(tags);
                            tags.Posts.Remove(pOST);
                        }
                    }
                    if (model.UpdateSlug)
                    {
                        string slug = SlugGenerator.SlugGenerator.GenerateSlug(model.post_title.ToLower());
                        pOST.post_slug = slug;
                        if (db.postRepository.AllPosts().Any(m => m.post_slug == slug))
                        {
                            pOST.post_slug = slug + "-" + 1;
                        }
                    }
                    db.postRepository.UpdatePost(pOST);
                    db.Commit();
                    if (model.post_type.Equals(PostType.Slide))
                    {
                        // return View("UploadImage", new UploadViewModel { id = pOST.post_id, title = pOST.post_title });
                    }

                }

                return RedirectToAction("ListPost");

            }
            NewPostViewModel nmodel = new NewPostViewModel
            {
                post_type = PostType.Normal,
                post_tag = PostData.getTagList(),
            };

            return View(nmodel);
        }
        
        //POST: Tbl_POST/Delete/5
        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            Post Tbl_POST = db.postRepository.FindByID(id);
            string title = Tbl_POST.post_title;
            if ((Tbl_POST.User.username == User.Identity.Name) || User.IsInRole("1"))
            {
                db.postRepository.DeletePost(Tbl_POST);
                // if (Tbl_POST.post_type == (int)PostType.Slide)
                // {
                //     string subPath = Server.MapPath("~/Files/images/slides/" + Tbl_POST.post_id);
                //     bool exists = System.IO.Directory.Exists(subPath);
                //     if (exists)
                //     {
                //         System.IO.Directory.Delete(subPath, true);
                //     }
                // }
                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + Tbl_POST.avatar_image));
                if (exist)
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/images/" + Tbl_POST.avatar_image));
                }

                db.Commit();
                return Json(new { reload = true, Message = "Xóa '" + title + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "Không thể xóa '" + title + "' <br /> vì đó không phải bài viết của bạn." }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult addhotPost(int id, int priority)
        //{
        //    StickyPost hotPost = new StickyPost
        //    {
        //        post_id = id,
        //        priority = priority,
        //    };
        //    db.hotPostRepository.AddHotPost(hotPost);
        //    db.hotPostRepository.SaveChanges();
        //    return Json(new { reload = true, Message = "Ghim bài thành công" }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public JsonResult deleteHotPost(int id)
        //{

        //    StickyPost hotPost = db.hotPostRepository.FindByID(id);
        //    string tit = hotPost.Tbl_POST.post_title;
        //    db.hotPostRepository.DeletePost(hotPost);
        //    db.hotPostRepository.SaveChanges();
        //    return Json(new { reload = true, Message = "Gỡ bài ghim '" + tit + "' thành công" }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult MyPost(string sortOrder, string CurrentSort, int? page, string titleStr)
        {
            //DVCPContext db = new DVCPContext();
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<Post> post = null;
            ViewBag.Sort = "tăng dần";
            User user = db.userRepository.FindByUsername(User.Identity.Name);
            if (String.IsNullOrWhiteSpace(titleStr))
            {
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                    (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                    (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }

                        break;
                        //default:
                        //    post = UnitOfWork.postRepository.AllPosts().ToPagedList(pageIndex, pageSize);
                        //    break;
                }
            }
            else
            {
                ViewBag.titleStr = titleStr;
                switch (sortOrder)
                {
                    case "Title":
                        ViewBag.sortname = "tiêu đề";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                    (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        else
                        {
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                      (m => m.post_title).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "CreateDate":
                        ViewBag.sortname = "ngày tạo";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                    (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                   (m => m.create_date).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                    case "ViewCount":
                        ViewBag.sortname = "lượt xem";
                        if (sortOrder.Equals(CurrentSort))
                        {
                            ViewBag.Sort = "giảm dần";
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                    (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }

                        else
                        {
                            post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                     (m => m.view_count).ToPagedList(pageIndex, pageSize);
                        }
                        break;
                        //default:
                        //    post = UnitOfWork.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower())).ToPagedList(pageIndex, pageSize);
                        //    break;
                }
            }
            return View(post);
        }
        [HttpPost]
        public JsonResult changeStatus(int id, bool state = false)
        {
            Post post = db.postRepository.FindByID(id);
            string title = post.post_title;
            post.status = state;
            string prefix = state ?  "Đăng" : "Hủy đăng";
            db.Commit();
            return Json(new { Message = prefix + " \"" + title + "\" thành công" }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles  = "1")]
        public   ActionResult ListTags()
        {
            List<ListTagViewModel> listTags = db.tagRepository.AllTags()
                .Select(m => new ListTagViewModel
                {
                    TagID = m.tag_id,
                    TagName = m.tag_name,
                    PostCount = m.Posts.Count
                }).ToList();
            return View(listTags);
        }
        [HttpPost]
        public JsonResult NewTag(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = 500;
                return Json(new { reload = true, Message = "Chưa nhập tên" }, JsonRequestBehavior.AllowGet);
            }
            Tag tags = new Tag
            {
                tag_name = name,
            };
            db.tagRepository.AddTag(tags);
            db.tagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Thêm '" + tags.tag_name + "' thành công" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTag(int id)
        {
            Tag tags = db.tagRepository.FindByID(id);
            if(tags.Posts.Count > 0)
            {
                Response.StatusCode = 500;
                return Json(new { reload = false,  Message = "Tags còn chứa bài viết, không xóa được!" }, JsonRequestBehavior.AllowGet);
            }
            db.tagRepository.DeleteTag(tags);
            db.tagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Xoá thành công!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTag(int id, string name)
        {
            Tag tags = db.tagRepository.FindByID(id);
            if(String.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = 500;
                return Json(new { reload = false, Message = "Chưa nhập tên!" }, JsonRequestBehavior.AllowGet);
            }
            tags.tag_name = name;
            db.tagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Sửa thành công!" }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "1")]
        public ViewResult HotPostManager()
        {
            List<StickyPost> posts = db.hotPostRepository.AllPosts().ToList();
            return View(posts);
        }

        [HttpPost]
        public JsonResult checkPost(int id)
        {
            Post post = db.postRepository.FindByID(id);
            if(post != null)
            {
                return Json(new { valid = true, Message = post.post_title }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { valid = false, Message = "ID nhập không hợp lệ" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult AddHotPost(int id,  int priority)
        {
            StickyPost hotPost = new StickyPost
            {
                post_id = id,
                priority = priority
            };
            db.hotPostRepository.AddHotPost(hotPost);
            db.hotPostRepository.SaveChanges();
            return Json(new { reload = true, Messenge = "Ghim bài thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteHotPost(int id)
        {
            StickyPost hotPost = db.hotPostRepository.FindByID(id);
            string title = hotPost.Post.post_title;
            db.hotPostRepository.DeletePost(hotPost);
            db.hotPostRepository.SaveChanges();
            return Json(new { reload = true, Message = "Gỡ bài ghim '" + title + "' thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult editHotPost(int id, int priority)
        {
            StickyPost hotPost = db.hotPostRepository.FindByID(id);
            hotPost.priority = priority;
            db.hotPostRepository.SaveChanges();
            return Json(new { reload = true, Message = "Sửa '" + hotPost.Post.post_title + "' thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserPost(int id, string sortOrder, string currentSort, int? page, string titleStr)
        {
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "Title" : sortOrder;
            IPagedList<Post> post = null;
            ViewBag.Sort = "Tăng dần";
            User user = db.userRepository.FindByID(id);
            if (user != null)
            {
                if (String.IsNullOrWhiteSpace(titleStr))
                {
                    switch (sortOrder)
                    {
                        case "Title":
                            ViewBag.sortname = "tiêu đề";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "CreateDate":
                            ViewBag.sortname = "ngày tạo";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            break;
                        case "ViewCount":
                            ViewBag.sortname = "lượt xem";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderByDescending
                                        (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = db.postRepository.AllPosts().Where(u => u.user_id == user.user_id).OrderBy
                                        (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }

                            break;
                            //default:
                            //    post = UnitOfWork.postRepository.AllPosts().ToPagedList(pageIndex, pageSize);
                            //    break;
                    }
                }
                else
                {
                    ViewBag.titleStr = titleStr;
                    switch (sortOrder)
                    {
                        case "Title":
                            ViewBag.sortname = "tiêu đề";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                        (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            else
                            {
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                          (m => m.post_title).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "CreateDate":
                            ViewBag.sortname = "ngày tạo";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                        (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                       (m => m.create_date).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                        case "ViewCount":
                            ViewBag.sortname = "lượt xem";
                            if (sortOrder.Equals(currentSort))
                            {
                                ViewBag.Sort = "giảm dần";
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderByDescending
                                        (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }

                            else
                            {
                                post = db.postRepository.AllPosts().Where(m => m.post_title.ToLower().Contains(titleStr.ToLower()) && m.user_id == user.user_id).OrderBy
                                         (m => m.view_count).ToPagedList(pageIndex, pageSize);
                            }
                            break;
                           
                    }
                }
                return View(post);
            }
            else return RedirectToAction("UserManager", "User");
        }
        
         public ActionResult ListSeries(string name, int? page)
        {
            int pageSize = 100;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Series> sr = null;
            if (!String.IsNullOrWhiteSpace(name))
            {
                ViewBag.name = name;
                sr = db.seriesRepository.AllSeries().Where(m => m.seriesName.Contains(name)).OrderBy(x => x.series_id).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                sr = db.seriesRepository.AllSeries().OrderBy(x => x.series_id).ToPagedList(pageIndex, pageSize);
            }
            return View(sr);
        }
        public ActionResult SerieDetail(int id, string name)
        {
            Series sr = db.seriesRepository.FindByID(id);
            if (sr != null)
            {

                if (!String.IsNullOrWhiteSpace(name))
                {
                    using (NewsDbContext conn = new NewsDbContext())
                    {
                        var result = (
                                // instance from context
                            from a in conn.Series
                                    // instance from navigation property
                            from b in a.Posts
                                    //join to bring useful data
                            join c in conn.Posts on b.post_id equals c.post_id
                                //where a.seriesID == c.Tbl_Series.s
                            where a.series_id.Equals(id)
                            where b.post_title.Contains(name)
                            select new ViewModel.SeriesPost
                                {
                                    post_id = b.post_id,
                                    post_title = b.post_title,
                                    status = b.status,
                                    ViewCount = b.view_count,
                                    userid = b.user_id,
                                    create_date = b.create_date,
                                userfullname = b.User.fullname,
                                username = b.User.username,
                                slug = b.post_slug
                            }).ToList();
                        SeriesPostViewModel postViewModel = new SeriesPostViewModel
                        {
                            SerieID = sr.series_id,
                            SerieName = sr.seriesName,
                            ListPost = result,
                        };
                        return View(postViewModel);

                    }
                }
                else
                {
                    ViewBag.name = name;
                    using (NewsDbContext conn = new NewsDbContext())
                    {
                        var result = (
                            // instance from context
                            from a in conn.Series
                                // instance from navigation property
                            from b in a.Posts
                                //join to bring useful data
                            join c in conn.Posts on b.post_id equals c.post_id
                            //where a.seriesID == c.Tbl_Series.s
                            where a.series_id.Equals(id)
                            select new ViewModel.SeriesPost
                            {
                                post_id = b.post_id,
                                post_title = b.post_title,
                                status = b.status,
                                ViewCount = b.view_count,
                                userid = b.view_count,
                                create_date = b.create_date,
                                userfullname = b.User.fullname,
                                username = b.User.username,

                            }).ToList();
                        SeriesPostViewModel postViewModel = new SeriesPostViewModel
                        {
                            SerieID = sr.series_id,
                            SerieName = sr.seriesName,
                            ListPost = result,
                        };
                        return View(postViewModel);

                    }
                }
                //return View(postlist);
            }
            return RedirectToAction("ListSeries");

        } 
        /// <summary>
        /// Thêm series mới
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult addSerie(string name)
        {
            if(String.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = 500;
                return Json(new { Message = "Không được để trống tiêu đề series" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                db.seriesRepository.AddSeries(new Series
                {
                    seriesName = name,
                });
                db.Commit();
                return Json(new { Message = "Tạo series" + " \"" + name + "\" thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult editSerie(int id,string name)
        {
            if (string.IsNullOrWhiteSpace(name) || id == null)
            {
                Response.StatusCode = 500;
                return Json(new { Message = "Không được để trống tiêu đề series hoặc id" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                Series series = db.seriesRepository.FindByID(id);
                series.seriesName = name;
                db.Commit();
                return Json(new { Message = "Edit series" + " \"" + series.seriesName + "\" thành công" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DeleteSeries(int id)
        {
            Series cate = db.seriesRepository.FindByID(id);
            string title = cate.seriesName;
            db.seriesRepository.Delete(cate);
            db.Commit();
            return Json(new { reload = true, Message = "Xóa '" + title + "' thành công" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddToSerie(int? id,int? seriid)
        {
            if (id == null || seriid == null)
            {
                Response.StatusCode = 500;
                return Json(new { Message = "Lỗi" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                using (NewsDbContext conn = new NewsDbContext())
                {
                    Series sr = conn.Series.Find(seriid);
                    Post post = conn.Posts.Find(id);
                    foreach(var x in sr.Posts)
                    {
                        if(x.post_id.Equals(id))
                        {
                            Response.StatusCode = 500;
                            return Json(new { Message = "Trùng lặp bài viết" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    sr.Posts.Add(post);
                    post.Series.Add(sr);
                    conn.SaveChanges();
                }
                    return Json(new { Message = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RemoveFromSerie(int? id,int? seriid)
        {
            if (id == null || seriid == null)
            {
                Response.StatusCode = 500;
                return Json(new { Message = "Lỗi" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                using (NewsDbContext conn = new NewsDbContext())
                {
                    Series sr = conn.Series.Find(seriid);
                    Post post = conn.Posts.Find(id);
                    sr.Posts.Remove(post);
                    post.Series.Remove(sr);
                    conn.SaveChanges();
                }
                return Json(new { Message = "Remove thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "1")]
        public ActionResult InfoChange()
        {
            WebInfo info = db.infoRepository.FindByID();
            InfoViewModel model = new InfoViewModel
            {
                web_name = info.web_name,
                web_about = info.web_about,
                web_des = info.web_des
            };


            return View(model);
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult InfoChange(InfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                WebInfo info = db.infoRepository.FindByID();
                info.web_des = model.web_des;
                info.web_name = model.web_name;
                info.web_about = model.web_about;
                db.Commit();
            }
            return View();
        }
    }
}