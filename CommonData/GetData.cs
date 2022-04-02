using NewsWebsite.Models;
using NewsWebsite.Repository;
using NewsWebsite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.CommonData
{
    public class GetData
    {
        UnitOfWork db = new UnitOfWork(new Models.NewsDbContext());
        public IEnumerable<ViewModel.ListPostViewModel> GetRelatedPost(int tag, int count = 6)
        {
            Tag tags = db.tagRepository.FindByID(tag);
            List<ListPostViewModel> post = new List<ListPostViewModel>();
            post = (
                from a in db.Context.Tags
                //instance from navigation property
                from b in a.Posts
                join c in db.Context.Posts on b.post_id equals c.post_id
                where a.tag_id == tag
                where c.status == true
                //sắp xếp so khớp
                orderby c.create_date descending
                select new ListPostViewModel
                {
                    post_id = c.post_id,
                    post_title = c.post_title,
                    ViewCount = c.view_count,
                    AvatarImage = c.avatar_image,
                    create_date = c.create_date,
                    slug = c.post_slug
                }
                ).Take(count).ToList();
            return post;
        }
        public List<Models.Post> GetPopularPost()
        {
            return db.postRepository.AllPosts().Take(5)
                .Where(m => m.status == true).OrderByDescending(m => m.view_count).ToList();
        }
        public IEnumerable<ListPostViewModel> GetHighRatedPost()
        {
            List<ListPostViewModel> lstPosts = new List<ListPostViewModel>();

            var x = db.postRepository.AllPosts()
                .Where(m => m.status == true).OrderBy(m => m.rated)
                .Take(4)
                .Select(c => new ListPostViewModel
                {
                    post_id = c.post_id,
                    post_title = c.post_title,
                    create_date = c.create_date,
                    slug = c.post_slug,
                });
            return x.ToList();
        }
        public IEnumerable<ViewModel.ViewPostViewModel> GetPostByTag(int tag, int count = 4)
        {
            Tag tags = db.tagRepository.FindByID(tag);
            List<ViewModel.ViewPostViewModel> post = new List<ViewPostViewModel>();

            post = (
                       from a in db.Context.Tags
                           // instance from navigation property
                            from b in a.Posts
                                //join to bring useful data
                            join c in db.Context.Posts on b.post_id equals c.post_id
                       where a.tag_id == tag
                       where c.status == true
                            // sắp theo so khớp
                            orderby c.create_date descending
                       select new
                       {
                           c.post_id,
                           c.post_title,
                                //c.post_teaser,
                                c.view_count,
                           c.avatar_image,
                           c.create_date,
                           c.post_slug
                       })
               //DISTINCT ĐỂ SAU KHI SELECT ĐỐI TƯỢNG MỚI ĐƯỢC
               //VÌ THẰNG DƯỚI KHÔNG EQUAL HASHCODE
               .Distinct().Take(count).Select(c => new ViewPostViewModel
               {
                   post_id = c.post_id,
                   firstTag = tags.tag_name,
                   post_title = c.post_title,
                        //post_teaser = c.post_teaser,
                        ViewCount = c.view_count,
                   AvatarImage = c.avatar_image,
                   create_date = c.create_date,
                   post_slug = c.post_slug
               })
               .ToList();


            return post;

        }
        public StickyPost[] GetHotPosts()
        {
            StickyPost[] post = db.hotPostRepository.AllPosts().Where(m => m.Post.status == true).OrderBy(m => m.priority).Take(3).ToArray();
            return post;
        }
        public IEnumerable<ViewModel.ListTagViewModel> GetListTags()
        {
            return db.tagRepository.AllTags().Select(
                m => new ViewModel.ListTagViewModel
                {
                    TagID = m.tag_id,
                    TagName = m.tag_name
                }).ToList();
        }
        public IEnumerable<ViewModel.SeriesPostViewModel> GetListSeries()
        {
            return db.seriesRepository.AllSeries().Select(
                m => new ViewModel.SeriesPostViewModel
                {
                    SerieID = m.series_id,
                    SerieName = m.seriesName
                }).ToList();
        }
        public IEnumerable<EnumShowList> GetEnumShowList()
        {
            List<ViewModel.Dynasty> dynasties = GetEnumList<ViewModel.Dynasty>();
            List<EnumShowList> enums = new List<EnumShowList>();
            foreach (var i in dynasties)
            {
                enums.Add(new EnumShowList
                {
                    slug = SlugGenerator.SlugGenerator.GenerateSlug(i.GetDisplayName()),
                    showname = i.GetDisplayName(),
                    value = (int)i,

                });
            }
            return enums;
        }
        private static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
    public class EnumShowList
    {
        public string slug { get; set; }
        public string showname { get; set; }
        public int value { get; set; }

    }
}