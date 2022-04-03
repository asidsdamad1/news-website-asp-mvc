using NewsWebsite.CommonData;
using NewsWebsite.Models;
using NewsWebsite.Repository;
using NewsWebsite.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork db = new UnitOfWork(new Models.NewsDbContext());
        public ActionResult Index()
        {
            ViewBag.Title = db.infoRepository.FindByID(1).web_name;
            return View(db.postRepository.AllPosts().Where(m => m.status == true)
                .OrderByDescending(m => m.create_date).Take(6).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ViewPost(string title)
        {
            if (!String.IsNullOrWhiteSpace(title))
            {
                Post p = db.postRepository.FindBySlug(title);
                if (p != null)
                {
                    p.view_count++;
                    db.Commit();
                    List<TagList> tagLists = p.Tags.Select(m => new TagList
                    {
                        id = m.tag_id,
                        name = m.tag_name,
                        slug = SlugGenerator.SlugGenerator.GenerateSlug(m.tag_name) + "-" + m.tag_id
                    }).ToList();
                    return View(new ViewPostViewModel
                    {
                        post_id = p.post_id,
                        dynasty = p.dynasty,
                        create_date = p.create_date,
                        //firstTag = tagLists.FirstOrDefault().name,
                        post_review = p.post_review,
                        post_tag = p.post_tag,
                        AvatarImage = p.avatar_image,
                        edit_date = p.edit_date,
                        post_content = p.post_content,
                        post_teaser = p.post_teaser,
                        post_title = p.post_title,
                        post_type = p.post_type,
                        Rated = p.rated,
                        userid = p.user_id,
                        ViewCount = p.view_count,
                        tagLists = tagLists,

                    });
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
        public ActionResult Category(int? id, int? page)
        {
            if (id != null)
            {
                int pageSize = 15;
                int pageIndex = 1;
                //IPagedList<Tbl_POST> post = null;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                Tag tag = db.tagRepository.FindByID(id.Value);
                if (tag != null)
                {
                    using (NewsDbContext conn = db.Context)
                    {
                        var result = (
                            // instance from context
                            from a in conn.Tags
                                // instance from navigation property
                            from b in a.Posts
                                //join to bring useful data
                            join c in conn.Posts on b.post_id equals c.post_id
                            where a.tag_id == id && b.status == true
                            orderby b.create_date descending
                            select new ListPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.view_count,
                                AvatarImage = c.avatar_image,
                                create_date = c.create_date,
                                slug = c.post_slug
                            }).ToPagedList(pageIndex, pageSize);
                        ViewBag.catname = tag.tag_name;
                        return View((PagedList<ListPostViewModel>)result);
                    }
                }
                return HttpNotFound();
            }

            return View("CategoryAll");

        }
        public ActionResult Series(int? id, int? page)
        {
            if (id != null)
            {
                int pageSize = 15;
                int pageIndex = 1;
                //IPagedList<Tbl_POST> post = null;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                Series series = db.seriesRepository.FindByID(id.Value);
                if (series != null)
                {
                    using (NewsDbContext conn = db.Context)
                    {
                        var result = (
                            // instance from context
                            from a in conn.Series
                            // instance from navigation property
                            from b in a.Posts
                            //join to bring useful data
                            join c in conn.Posts on b.post_id equals c.post_id
                            where a.series_id == id && b.status == true
                            orderby b.create_date descending
                            select new ListPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.view_count,
                                AvatarImage = c.avatar_image,
                                create_date = c.create_date,
                                slug = c.post_slug
                            }).ToPagedList(pageIndex, pageSize);
                        ViewBag.catname = series.seriesName;
                        return View((PagedList<ListPostViewModel>)result);
                    }
                }
                return HttpNotFound();
            }

            return View("SeriesAll");

        }
        public ActionResult Dynasty(int? dynasty, int? page)
        {
            if(dynasty != null)
            {
                int pageSize = 10;
                int pageIndex = 1;
                IPagedList<ListPostViewModel> post = null;
                Dynasty d = (Dynasty) dynasty;
                ViewBag.catname = d.GetDisplayName();
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                post = db.postRepository.AllPosts()
                    .Where(m => m.status)
                    .Where(m => m.dynasty.Equals(d.ToString()))
                    .OrderByDescending(m => m.create_date)
                     .Select(c => new ListPostViewModel
                     {
                         post_id = c.post_id,
                         post_title = c.post_title,
                         post_teaser = c.post_teaser,
                         ViewCount = c.view_count,
                         AvatarImage = c.avatar_image,
                         create_date = c.create_date,
                         tagsname = c.Tags.FirstOrDefault().tag_name,
                         slug = c.post_slug
                     }).ToPagedList(pageIndex, pageSize);
                return View((PagedList<ListPostViewModel>)post);
            }
            return View("DynastyAll");
        }

        public ActionResult Search(SearchViewModel model, int? page)
        {
            int pageIndex = 1;
            int pageSize = 8;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ListPostViewModel> post = new List<ListPostViewModel>().ToPagedList(pageIndex, pageSize);
            List<Tag> tagList = new List<Tag>();
            tagList.AddRange(model.post_tag.Where(m => m.Selected)
                .Select(m => new Tag { tag_id = int.Parse(m.Value), tag_name = m.Text })
                );
            ViewBag.title = model.title;
            bool title = String.IsNullOrEmpty(model.title);
            bool tag = tagList.Count == 0;
            bool dynasty = model.Dynasty == null;
            var check = 0;
            if(title)
            {
                if (tag)
                {
                    if (dynasty) check = 1; // ko chọn  
                    else check = 5;         // tìm phạm vi
                }
                else
                {
                    if (dynasty) check = 4; // chỉ tag 
                    else check = 8;         // phạm vi vs tag 
                }
            }
            if (!title)
            {
                if (tag)
                {
                    if (dynasty) check = 3; // chỉ tiêu đề
                    else check = 7;         // phạm vi vs tiêu đề 
                }
                else
                {
                    if (dynasty) check = 6; // tiêu đề vs phạm vi
                    else check = 2;         // tìm cả 3 
                }
            }
            //if (title && tag && dynasty)
            //{
            //    check = 1;
            //}
            //else if (!title && !tag && !dynasty)
            //{
            //    // cả 3 cái đều ko null
            //    check = 2;
            //}
            //else if (!title && tag && dynasty)
            //{
            //    // chỉ title
            //    check = 3;
            //}
            //else if (title && !tag && dynasty)
            //{
            //    // chỉ tag
            //    check = 4;
            //}
            //else if (title && tag && !dynasty)
            //{
            //    // chỉ DN
            //    check = 5;
            //}
            //else if (!title && !tag && dynasty)
            //{
            //    // title và tag
            //    check = 6;
            //}
            //else if (!title && tag && !dynasty)
            //{
            //    // title và dn
            //    check = 7;
            //}
            //else if (title && !tag && !dynasty)
            //{
            //    // tag và dn
            //    check = 8;
            //}
            switch(check)
            {
                default:
                // để trống 
                case 1: 
                    IQueryable<Post> x = db.postRepository.AllPosts()
                        .Where(m => m.status)
                        .OrderByDescending(m => m.create_date);
                    post = x.Select(m => new ListPostViewModel
                    {
                        post_id = m.post_id,
                        post_title = m.post_title,
                        post_teaser = m.post_teaser,
                        ViewCount = m.view_count,
                        AvatarImage = m.avatar_image,
                        create_date = m.create_date,
                        tagsname = m.Tags.FirstOrDefault().tag_name,
                        slug = m.post_slug
                    }).ToPagedList(pageIndex, pageSize);
                    break;
                // tìm cả 3 TH
                case 2: 
                    using(NewsDbContext conn = db.Context)
                    {
                        var query = (
                            from z in tagList
                            join a in conn.Tags on z.tag_id equals a.tag_id
                            from b in a.Posts
                            join c in conn.Posts on b.post_id equals c.post_id
                            where c.status = true
                            where c.dynasty == model.Dynasty.ToString()
                            where c.post_title.ToLower().Contains(model.title.ToLower())
                            orderby c.rated
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.view_count,
                                c.avatar_image,
                                c.create_date,
                                c.Tags.FirstOrDefault().tag_name,
                                c.post_slug
                            }).Distinct();
                        post = query.Select(c => new ListPostViewModel
                        {
                            post_id = c.post_id,
                            post_title = c.post_title,
                            post_teaser = c.post_teaser,
                            ViewCount = c.view_count,
                            AvatarImage = c.avatar_image,
                            create_date = c.create_date,
                            tagsname = c.tag_name,
                            slug = c.post_slug
                        }).ToPagedList(pageIndex, pageSize);
                    }
                    break;
                // tìm theo title
                case 3:
                    var p = db.postRepository.AllPosts()
                        .Where(m => m.status)
                        .Where(m => m.post_title.Contains(model.title))
                        .OrderBy(m => m.post_title.Contains(model.title));
                    post =
                        p.Select(m => new ListPostViewModel
                        {
                            post_id = m.post_id,
                            post_title = m.post_title,
                            post_teaser = m.post_teaser,
                            ViewCount = m.view_count,
                            AvatarImage = m.avatar_image,
                            create_date = m.create_date,
                            tagsname = m.Tags.FirstOrDefault().tag_name,
                            slug = m.post_slug
                        }).ToPagedList(pageIndex, pageSize);
                    break;
                // tìm theo tag
                case 4:
                   using (NewsDbContext conn = db.Context)
                    {
                        post = (
                            from z in tagList
                            join a in conn.Tags on z.tag_id equals a.tag_id
                            from b in a.Posts
                            join c in conn.Posts on b.post_id equals c.post_id
                            where c.status == true
                            orderby b.create_date descending
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.view_count,
                                c.avatar_image,
                                c.create_date,
                                c.Tags.FirstOrDefault().tag_name,
                                c.post_slug
                            })
                            .Distinct().Select(c => new ListPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.view_count,
                                AvatarImage = c.avatar_image,
                                create_date = c.create_date,
                                tagsname = c.tag_name,
                                slug = c.post_slug
                            }
                            ).ToPagedList(pageIndex, pageSize);
                    }
                    break;
                    // tìm theo phạm vi
                case 5:
                    post = db.postRepository.AllPosts()
                        .Where(m => m.status)
                        .Where(m => m.dynasty.Contains(model.Dynasty.ToString()))
                        .OrderByDescending(m => m.create_date)
                        .Select(m => new ListPostViewModel
                        {
                            post_id = m.post_id,
                            post_title = m.post_title,
                            post_teaser = m.post_teaser,
                            ViewCount = m.view_count,
                            AvatarImage = m.avatar_image,
                            create_date = m.create_date,
                            tagsname = m.Tags.FirstOrDefault().tag_name,
                            slug = m.post_slug
                        }).ToPagedList(pageIndex, pageSize);
                    break;
                    // tìm theo tag vs title 
                case 6:
                    using (NewsDbContext conn = db.Context)
                    {
                        post = (
                            //join tag list
                            from z in tagList
                            join a in conn.Tags on z.tag_id equals a.tag_id
                            // join post list 
                            from b in a.Posts
                            join c in conn.Posts on b.post_id equals c.post_id
                            // conditions 
                            where c.post_title.ToLower().Contains(model.title.ToLower())
                            where c.status == true
                            orderby c.post_title.Contains(model.title)
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.view_count,
                                c.avatar_image,
                                c.create_date,
                                c.Tags.FirstOrDefault().tag_name,
                                c.post_slug
                            })
                            .Distinct().Select(c => new ListPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.view_count,
                                AvatarImage = c.avatar_image,
                                create_date = c.create_date,
                                tagsname = c.tag_name,
                                slug = c.post_slug
                            }
                            ).ToPagedList(pageIndex, pageSize);
                    }
                    break;
                    // tìm theo title vs phạm vi 
                case 7:
                    post = db.postRepository.AllPosts()
                        .Where(m => m.status)
                        .Where(m => m.post_title.Contains(model.title))
                        .Where(m => m.dynasty.Contains(model.Dynasty.ToString()))
                        .OrderByDescending(m => m.create_date)
                        .Select(m => new ListPostViewModel
                        {
                            post_id = m.post_id,
                            post_title = m.post_title,
                            post_teaser = m.post_teaser,
                            ViewCount = m.view_count,
                            AvatarImage = m.avatar_image,
                            create_date = m.create_date,
                            tagsname = m.Tags.FirstOrDefault().tag_name,
                            slug = m.post_slug
                        }).ToPagedList(pageIndex, pageSize);
                    break;
                    // tìm theo tag vs phạm vi 
                case 8:
                    using (NewsDbContext conn = db.Context)
                    {
                        post = (
                            //join tag list
                            from z in tagList
                            join a in conn.Tags on z.tag_id equals a.tag_id
                            // join post list 
                            from b in a.Posts
                            join c in conn.Posts on b.post_id equals c.post_id
                            // conditions 
                            where c.dynasty == model.Dynasty.ToString()
                            where c.status 
                            orderby c.create_date descending
                            select new
                            {
                                c.post_id,
                                c.post_title,
                                c.post_teaser,
                                c.view_count,
                                c.avatar_image,
                                c.create_date,
                                c.Tags.FirstOrDefault().tag_name,
                                c.post_slug
                            })
                            //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
                            //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
                            .Distinct().Select(c => new ListPostViewModel
                            {
                                post_id = c.post_id,
                                post_title = c.post_title,
                                post_teaser = c.post_teaser,
                                ViewCount = c.view_count,
                                AvatarImage = c.avatar_image,
                                create_date = c.create_date,
                                tagsname = c.tag_name,
                                slug = c.post_slug
                            }
                            ).ToPagedList(pageIndex, pageSize);
                    }
                    break;
            }
            return View((PagedList<ListPostViewModel>)post);
        }
    }
}