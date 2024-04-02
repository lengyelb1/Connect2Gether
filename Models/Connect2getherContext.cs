using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Models;

public partial class Connect2getherContext : DbContext
{
    public Connect2getherContext()
    {
    }

    public Connect2getherContext(DbContextOptions<Connect2getherContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alertmessage> Alertmessages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<LikedPost> LikedPosts { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPost> UserPosts { get; set; }

    public virtual DbSet<UserSuspiciou> UserSuspicious { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("SERVER=localhost;PORT=3306;DATABASE=connect2gether;USER=root;PASSWORD=;SSL MODE=none;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alertmessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alertmessage");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Title).HasMaxLength(128);
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Alertmessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("alertmessage_ibfk_1");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comment");

            entity.HasIndex(e => e.CommentId, "CommentId");

            entity.HasIndex(e => e.PostId, "PostId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CommentId).HasColumnType("int(11)");
            entity.Property(e => e.PostId).HasColumnType("int(11)");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("uploadDate");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("comment_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("comment_ibfk_1");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Image1).HasColumnType("mediumblob");
            entity.Property(e => e.Image2).HasColumnType("mediumblob");
            entity.Property(e => e.Image3).HasColumnType("mediumblob");
            entity.Property(e => e.Image4).HasColumnType("mediumblob");
        });

        modelBuilder.Entity<LikedPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("liked_posts");

            entity.HasIndex(e => e.PostId, "PostID");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("PostID");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.LikedPosts)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("liked_posts_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.LikedPosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("liked_posts_ibfk_1");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("permissions");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ranks");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MaxPont)
                .HasColumnType("int(11)")
                .HasColumnName("Max_pont");
            entity.Property(e => e.MinPont)
                .HasColumnType("int(11)")
                .HasColumnName("Min_pont");
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.PermissionId, "PermissionId");

            entity.HasIndex(e => e.RankId, "RankId");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Hash)
                .HasMaxLength(128)
                .HasColumnName("HASH");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PermissionId)
                .HasComment("Felhasználói szint")
                .HasColumnType("int(11)");
            entity.Property(e => e.Point)
                .HasComment("Pontszám")
                .HasColumnType("int(11)");
            entity.Property(e => e.RankId)
                .HasComment("Pontszámhoz kötött rangok")
                .HasColumnType("int(11)");
            entity.Property(e => e.RegistrationDate).HasColumnType("date");
            entity.Property(e => e.Username).HasMaxLength(128);

            entity.HasOne(d => d.Permission).WithMany(p => p.Users)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Rank).WithMany(p => p.Users)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("user_ibfk_3");
        });

        modelBuilder.Entity<UserPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_post");

            entity.HasIndex(e => e.ImageId, "ImageId");

            entity.HasIndex(e => new { e.ImageId, e.UserId }, "ImageId_2").IsUnique();

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ImageId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)");
            entity.Property(e => e.Like).HasColumnType("bigint(20)");
            entity.Property(e => e.Title).HasMaxLength(128);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("uploadDate");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Image).WithMany(p => p.UserPosts)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_post_ibfk_4");

            entity.HasOne(d => d.User).WithMany(p => p.UserPosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_post_ibfk_3");
        });

        modelBuilder.Entity<UserSuspiciou>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_suspicious");

            entity.HasIndex(e => e.UserId, "UserId").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithOne(p => p.UserSuspiciou)
                .HasForeignKey<UserSuspiciou>(d => d.UserId)
                .HasConstraintName("user_suspicious_ibfk_1");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_token");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Token).HasColumnType("text");
            entity.Property(e => e.TokenExpireDate)
                .HasColumnType("datetime")
                .HasColumnName("Token_expire_date");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_token_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
