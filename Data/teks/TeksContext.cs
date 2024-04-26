
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.teks;

public partial class TeksContext : DbContext
{
    public TeksContext(DbContextOptions<TeksContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Tek> Teks { get; set; }

    public virtual DbSet<TeksItem> TeksItems { get; set; }

    public virtual DbSet<TeksItemType> TeksItemTypes { get; set; }

    public virtual DbSet<TeksSubject> TeksSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks__3213E83F5692239F");

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
                .HasConstraintName("FK__teks__subject_id__6D6D25A7");
        });

        modelBuilder.Entity<TeksItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_ite__3213E83F4313200F");

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
                .HasConstraintName("FK__teks_item__item___6A90B8FC");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__teks_item__paren__699C94C3");
        });

        modelBuilder.Entity<TeksItemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_ite__3213E83FE4D7045D");

            entity.ToTable("teks_item_types", "teks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<TeksSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__teks_sub__3213E83F9EEBD234");

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
