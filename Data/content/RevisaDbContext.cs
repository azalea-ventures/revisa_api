﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.content;

public partial class RevisaDbContext : DbContext
{
    public RevisaDbContext()
    {
    }

    public RevisaDbContext(DbContextOptions<RevisaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ContentDetail> ContentDetails { get; set; }

    public virtual DbSet<ContentGroup> ContentGroups { get; set; }

    public virtual DbSet<ContentTxt> ContentTxts { get; set; }

    public virtual DbSet<ContentType> ContentTypes { get; set; }

    public virtual DbSet<ContentVersion> ContentVersions { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=tcp:revisa-db.database.windows.net,1433;Initial Catalog=revisa_db;Persist Security Info=False;User ID=revisa_admin;Password=EeR8kMiFf@y5SCb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clients__3213E83F2E677153");

            entity.ToTable("clients", "content", tb => tb.HasTrigger("trg_upper_client_name"));

            entity.HasIndex(e => e.ClientName, "UQ__clients__9ADC3B74C6852588").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .HasColumnName("client_name");
        });

        modelBuilder.Entity<ContentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F912332DE");

            entity.ToTable("content_details", "content", tb => tb.HasTrigger("trg_insert_content_version"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");
            entity.Property(e => e.OriginalFilename)
                .HasMaxLength(255)
                .HasColumnName("original_filename");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Client).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__clien__7CE47361");

            entity.HasOne(d => d.Grade).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__grade__7DD8979A");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__owner__7FC0E00C");

            entity.HasOne(d => d.Subject).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__subje__7ECCBBD3");
        });

        modelBuilder.Entity<ContentGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F694E3803");

            entity.ToTable("content_group", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentVersionId).HasColumnName("content_version_id");

            entity.HasOne(d => d.ContentVersion).WithMany(p => p.ContentGroups)
                .HasForeignKey(d => d.ContentVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_g__conte__0E0EFF63");
        });

        modelBuilder.Entity<ContentTxt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F2A8E2045");

            entity.ToTable("content_txt", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentGroupId).HasColumnName("content_group_id");
            entity.Property(e => e.ObjectId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("object_id");
            entity.Property(e => e.Txt)
                .HasColumnType("text")
                .HasColumnName("txt");

            entity.HasOne(d => d.ContentGroup).WithMany(p => p.ContentTxts)
                .HasForeignKey(d => d.ContentGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_t__conte__10EB6C0E");
        });

        modelBuilder.Entity<ContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FA4E9E056");

            entity.ToTable("content_type", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentType1)
                .HasMaxLength(100)
                .HasColumnName("content_type");
        });

        modelBuilder.Entity<ContentVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F3717AB54");

            entity.ToTable("content_versions", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentDetailsId).HasColumnName("content_details_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsLatest)
                .HasDefaultValue((byte)1)
                .HasColumnName("is_latest");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.ContentDetails).WithMany(p => p.ContentVersions)
                .HasForeignKey(d => d.ContentDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_v__conte__076201D4");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentVersions)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_v__owner__066DDD9B");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83FC920337B");

            entity.ToTable("grades", "content");

            entity.HasIndex(e => e.Grade1, "UQ__grades__28A83176D8A5B5B1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade1)
                .HasMaxLength(50)
                .HasColumnName("grade");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subjects__3213E83F147F371E");

            entity.ToTable("subjects", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Subject1)
                .HasMaxLength(100)
                .HasColumnName("subject");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F6D21DE32");

            entity.ToTable("users", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
