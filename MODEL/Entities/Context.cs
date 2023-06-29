using Microsoft.EntityFrameworkCore;
using MODEL.Interceptors;

namespace MODEL.Entities
{
    public partial class Context : DbContext
    {
        #region constructors
        public Context() { }
        public Context(DbContextOptions<Context> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }
        #endregion

        #region DbSets
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Commerce> Commerces { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<FavoriteArticle> FavoriteArticles { get; set; } = null!;
        public virtual DbSet<FavoriteCommerce> FavoriteCommerces { get; set; } = null!;
        public virtual DbSet<GeoPoint> Geopints { get; set; } = null!;
        public virtual DbSet<Impression> Impressions { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Shop> Shops { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        #endregion

        #region configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.RoleId, "acc_has_r_fk");

                entity.HasIndex(e => e.Id, "account_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("rol_id");

                entity.Property(e => e.Salt)
                    .HasColumnName("salt");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_account_acc_has_r_role");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article");

                entity.HasIndex(e => e.Name, "article_name_asc");

                entity.HasIndex(e => e.Id, "article_pk")
                    .IsUnique();

                entity.HasIndex(e => e.CurId, "of_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.CurId).HasColumnName("cur_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .HasColumnName("description");


                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NewPrice)
                    .HasPrecision(4, 2)
                    .HasColumnName("new_price");

                entity.Property(e => e.OldPrice)
                    .HasPrecision(4, 2)
                    .HasColumnName("old_price");

                entity.Property(e => e.Sale)
                    .HasPrecision(3, 2)
                    .HasColumnName("sale");

                entity.Property(e => e.ValidDate).HasColumnName("valid_date");

                entity.HasOne(d => d.Cur)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CurId);

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Arts)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleInCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("Id").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_article__article_i_category"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("ArtId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_article__article_i_article"),
                        j =>
                        {
                            j.HasKey("ArtId", "Id").HasName("pk_article_in_category");

                            j.ToTable("article_in_category");

                            j.HasIndex(new[] { "Id" }, "article_in_category_fk");

                            j.HasIndex(new[] { "ArtId" }, "article_in_category_fk2");

                            j.HasIndex(new[] { "ArtId", "Id" }, "article_in_category_pk").IsUnique();

                            j.IndexerProperty<Guid>("ArtId").HasColumnName("art_id");

                            j.IndexerProperty<Guid>("Id").HasColumnName("id");
                        });

                entity.HasMany(d => d.Shops)
                    .WithMany(p => p.Articles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleInShop",
                        l => l.HasOne<Shop>().WithMany().HasForeignKey("Id").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_article__article_i_shop"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("ArtId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_article__article_i_article"),
                        j =>
                        {
                            j.HasKey("ArtId", "Id").HasName("pk_article_in_shop");

                            j.ToTable("article_in_shop");

                            j.HasIndex(new[] { "Id" }, "article_in_shop_fk");

                            j.HasIndex(new[] { "ArtId" }, "article_in_shop_fk2");

                            j.HasIndex(new[] { "ArtId", "Id" }, "article_in_shop_pk").IsUnique();

                            j.IndexerProperty<Guid>("ArtId").HasColumnName("art_id");

                            j.IndexerProperty<Guid>("Id").HasColumnName("id");
                        });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.HasIndex(e => e.Id, "category_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.DisplaySeq).HasColumnName("display_seq");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.CountryId, "city_fk");

                entity.HasIndex(e => e.Id, "city_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(3)
                    .HasColumnName("cou_id")
                    .IsFixedLength();

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_city_city_country");
            });

            modelBuilder.Entity<Commerce>(entity =>
            {
                entity.ToTable("commerce");

                entity.HasIndex(e => e.AccountId, "co_has_acc_fk");

                entity.HasIndex(e => e.Id, "commerce_pk")
                    .IsUnique();

                entity.HasIndex(e => e.CityId, "residence_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AccountId).HasColumnName("acc_id");

                entity.Property(e => e.CityId).HasColumnName("cit_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .HasColumnName("logo");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Commerces)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_commerce_co_has_ac_account");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Commerces)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_commerce_residence_city");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.HasIndex(e => e.Id, "country_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(3)
                    .HasColumnName("id")
                    .IsFixedLength();

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("currency");

                entity.HasIndex(e => e.Id, "currency_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .HasColumnName("code");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(120)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<FavoriteArticle>(entity =>
            {
                entity.ToTable("favorite_article");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.HasIndex(e => e.ArticleId, "favorite_article_fk");

                entity.HasIndex(e => e.UserId, "favorite_article_fk2");

                entity.HasIndex(e => e.Id, "favorite_article_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.ArticleId).HasColumnName("art_id");

                entity.Property(e => e.UserId).HasColumnName("use_id");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.FavoriteArticles)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_favorite_favorite__article");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteArticles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_favorite_favorite__user");
            });

            modelBuilder.Entity<FavoriteCommerce>(entity =>
            {
                entity.ToTable("favorite_commerce");

                entity.HasIndex(e => e.CommerceId, "favorite_commerce_fk");

                entity.HasIndex(e => e.UserId, "favorite_commerce_fk2");

                entity.HasIndex(e => e.Id, "favorite_commerce_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CommerceId).HasColumnName("com_id");

                entity.Property(e => e.UserId).HasColumnName("use_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.HasOne(d => d.Commerce)
                    .WithMany(p => p.FavoriteCommerces)
                    .HasForeignKey(d => d.CommerceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_favorite_favorite__commerce");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteCommerces)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_favorite_favorite__user");
            });

            modelBuilder.Entity<GeoPoint>(entity =>
            {
                entity.ToTable("geopoint");

                entity.HasIndex(e => e.Id, "geopint_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.Address)
                    .HasColumnName("address");

                entity.Property(e => e.Modified).HasColumnName("modified");
            });

            modelBuilder.Entity<Impression>(entity =>
            {
                entity.ToTable("impression");

                entity.HasIndex(e => e.Id, "impression_pk")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "leaves_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Content)
                    .HasMaxLength(1024)
                    .HasColumnName("content");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.UserId).HasColumnName("use_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Impressions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_impressi_leaves_user");
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("picture");

                entity.HasIndex(e => e.ArticleId, "ar_has_pic_fk");

                entity.HasIndex(e => e.Id, "picture_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.ArticleId).HasColumnName("art_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.PicUrl)
                    .HasMaxLength(512)
                    .HasColumnName("pic")
                    .IsFixedLength();

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_picture_ar_has_pi_article");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.Id, "role_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .HasColumnName("code");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("shop");

                entity.HasIndex(e => e.GeoId, "address_fk");

                entity.HasIndex(e => e.CommerceId, "co_has_sh_fk");

                entity.HasIndex(e => e.CityId, "lokacija_fk");

                entity.HasIndex(e => e.Id, "shop_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CityId).HasColumnName("cit_id");

                entity.Property(e => e.CommerceId).HasColumnName("com_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.GeoId).HasColumnName("geo_id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_shop_lokacija_city");

                entity.HasOne(d => d.Commerce)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.CommerceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_shop_co_has_sh_commerce");

                entity.HasOne(d => d.GeoPoint)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.GeoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_shop_address_geopint");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.AccountId, "u_has_acc_fk");

                entity.HasIndex(e => e.Id, "user_pk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AccountId).HasColumnName("acc_id");

                entity.Property(e => e.Birthdate).HasColumnName("birthdate");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Modified).HasColumnName("modified");

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .HasColumnName("sex");

                entity.Property(e => e.Surname)
                    .HasMaxLength(25)
                    .HasColumnName("surname");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_user_u_has_acc_account");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new DbSaveChangesInterceptor());
        }
        #endregion
    }
}
