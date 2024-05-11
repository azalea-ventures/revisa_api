using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.teks;

public partial class TeksContext : DbContext
{
    public TeksContext()
    {
    }

    public TeksContext(DbContextOptions<TeksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tek> Teks { get; set; }

    public virtual DbSet<TeksItem> TeksItems { get; set; }

    public virtual DbSet<TeksItemType> TeksItemTypes { get; set; }

    public virtual DbSet<TeksSubject> TeksSubjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks__3213E83F095A5906");

            entity.ToTable("teks", "teks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AdoptionStatus).HasColumnName("adoption_status");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EffectiveYear).HasColumnName("effective_year");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OfficialSourceUrl).HasColumnName("official_source_url");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Subject).WithMany(p => p.Teks)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__teks__subject_id__5FDE205F");
        });

        modelBuilder.Entity<TeksItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_ite__3213E83F80BCBD75");

            entity.ToTable("teks_items", "teks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FullStatement).HasColumnName("full_statement");
            entity.Property(e => e.HumanCodingScheme).HasColumnName("human_coding_scheme");
            entity.Property(e => e.ItemTypeId).HasColumnName("item_type_id");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.LastChangeTea)
                .HasColumnType("datetime")
                .HasColumnName("last_change_tea");
            entity.Property(e => e.ListEnumeration).HasColumnName("list_enumeration");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.UploadedAt)
                .HasColumnType("datetime")
                .HasColumnName("uploaded_at");

            entity.HasOne(d => d.ItemType).WithMany(p => p.TeksItems)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK__teks_item__item___5D01B3B4");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__teks_item__paren__5C0D8F7B");
        });

        modelBuilder.Entity<TeksItemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_ite__3213E83F6085634D");

            entity.ToTable("teks_item_types", "teks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<TeksSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_sub__3213E83FF01291B0");

            entity.ToTable("teks_subjects", "teks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
