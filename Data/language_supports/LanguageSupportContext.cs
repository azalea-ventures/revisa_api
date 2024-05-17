using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.language_supports;

public partial class LanguageSupportContext : DbContext
{
    public LanguageSupportContext()
    {
    }

    public LanguageSupportContext(DbContextOptions<LanguageSupportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Iclo> Iclos { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LessonSchedule> LessonSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Iclo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__iclos__3213E83F7A520BBC");

            entity.ToTable("iclos", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iclo1)
                .HasColumnType("text")
                .HasColumnName("iclo");
            entity.Property(e => e.StrategyObjectiveId).HasColumnName("strategy_objective_id");
            entity.Property(e => e.TeksItemId).HasColumnName("teks_item_id");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__language__3213E83F0C30CFAC");

            entity.ToTable("languages", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("language_name");
            entity.Property(e => e.LanguageShort)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("language_short");
        });

        modelBuilder.Entity<LessonSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lesson_s__3213E83FA79D06E5");

            entity.ToTable("lesson_schedule", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.LessonOrder).HasColumnName("lesson_order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
