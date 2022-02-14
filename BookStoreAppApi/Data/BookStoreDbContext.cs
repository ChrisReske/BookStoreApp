using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStoreAppApi.Data
{
    public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
    {
        public BookStoreDbContext()
        {
        }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Entity > Author

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Bio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            #endregion

            #region Entity > Book

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EAD34CF4CE")
                    .IsUnique();

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Summary).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

            #endregion

            #region Entity > Identity Role (Seed)

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = "9539a969-975b-41b1-9aa8-0879efebc322"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Id = "fe7c3b95-8c2b-4886-9b01-e388061add34"
                }
            );

            #endregion

            #region Entity Api - User (Seed)

            var hasher = new PasswordHasher<ApiUser>();

            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {
                    Id = "1b98fd57-a180-4f64-b08f-35dc8f7f7986",
                    Email = "admin@bookstore.com",
                    NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                    UserName = "admin@bookstore.com",
                    NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1")

                },
                new ApiUser
                {
                    Id = "a235814b-ec8f-4d7b-b543-e9587402ad9b",
                    Email = "user@bookstore.com",
                    NormalizedEmail = "USER@BOOKSTORE.COM",
                    UserName = "user@bookstore.com",
                    NormalizedUserName = "USER@BOOKSTORE.COM",
                    FirstName = "System",
                    LastName = "User",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1")
                }
            );

            #endregion

            #region Entities IdentityUserRole

            // User 
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "9539a969-975b-41b1-9aa8-0879efebc322", // User-Role
                    UserId = "a235814b-ec8f-4d7b-b543-e9587402ad9b"
                });

            // Admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "fe7c3b95-8c2b-4886-9b01-e388061add34", // Admin Role
                    UserId = "1b98fd57-a180-4f64-b08f-35dc8f7f7986"
                });    

            #endregion

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
