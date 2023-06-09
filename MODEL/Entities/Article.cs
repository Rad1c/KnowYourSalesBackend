﻿namespace MODEL.Entities
{
    public partial class Article : BaseEntity
    {
        public Article()
        {
            FavoriteArticles = new HashSet<FavoriteArticle>();
            Pictures = new HashSet<Picture>();
            Categories = new HashSet<Category>();
            Shops = new HashSet<Shop>();
        }

        public Guid CurId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Sale { get; set; }
        public DateTime ValidDate { get; set; }

        public virtual Currency? Cur { get; set; } = null!;
        public virtual ICollection<FavoriteArticle> FavoriteArticles { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
