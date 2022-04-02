
using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Repository
{
    public class UnitOfWork
    {
        private NewsDbContext context = new NewsDbContext();
        private UserRepository _userRepository;
        private InfoRepository _infoRepository;
        private PostRepository _postRepository;
        private SeriesRepository _seriesRepository;
        private TagRepository _tagRepository;
        private RoleRepository _roleRepository;
        private HotPostRepository _hotPostRepository;

        public UnitOfWork(NewsDbContext _context)
        {
            this.context = _context;
        }
        public NewsDbContext Context
        {
            get
            {
                return context;
            }
        }
        public HotPostRepository hotPostRepository
        {
            get
            {
                if (_hotPostRepository == null)
                {
                    _hotPostRepository = new HotPostRepository(context);

                }
                return _hotPostRepository;
            }
        }
        public TagRepository tagRepository
        {
            get
            {
                if (_tagRepository == null)
                {
                    _tagRepository = new TagRepository(context);

                }
                return _tagRepository;
            }
        }
        public RoleRepository roleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new RoleRepository(context);

                }
                return _roleRepository;
            }
        }
        public SeriesRepository seriesRepository
        {
            get
            {
                if (_seriesRepository == null)
                {
                    _seriesRepository = new SeriesRepository(context);

                }
                return _seriesRepository;
            }
        }
        public PostRepository postRepository
        {
            get
            {
                if (_postRepository == null)
                {
                    _postRepository = new PostRepository(context);

                }
                return _postRepository;
            }
        }
        public InfoRepository infoRepository
        {
            get
            {
                if (_infoRepository == null)
                {
                    _infoRepository = new InfoRepository(context);

                }
                return _infoRepository;
            }
        }
        public UserRepository userRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(context);

                }
                return _userRepository;
            }
        }

        public object InfoRepository { get; internal set; }

        public void Commit()
        {
            context.SaveChanges();
        }
    }

   
}