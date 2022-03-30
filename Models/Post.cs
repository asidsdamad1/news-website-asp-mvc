namespace NewsWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            StickyPosts = new HashSet<StickyPost>();
            Tags = new HashSet<Tag>();
            Series = new HashSet<Series>();
        }

        [Key]
        public int post_id { get; set; }

        public int? user_id { get; set; }

        [Required]
        [StringLength(200)]
        public string post_title { get; set; }

        [StringLength(200)]
        public string post_slug { get; set; }

        [Required]
        [StringLength(500)]
        public string post_teaser { get; set; }

        [StringLength(500)]
        public string post_review { get; set; }

        [Column(TypeName = "ntext")]
        public string post_content { get; set; }

        public int post_type { get; set; }

        [StringLength(200)]
        public string post_tag { get; set; }

        public DateTime? create_date { get; set; }

        public DateTime? edit_date { get; set; }

        [StringLength(50)]
        public string dynasty { get; set; }

        public int view_count { get; set; }

        public int rated { get; set; }

        [StringLength(200)]
        public string avatar_image { get; set; }

        public bool status { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StickyPost> StickyPosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Series> Series { get; set; }
    }
}
