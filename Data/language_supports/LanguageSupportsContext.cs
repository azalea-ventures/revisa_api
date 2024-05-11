using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.language_supports;

public partial class LanguageSupportsContext : DbContext
{
    public LanguageSupportsContext()
    {
    }

    public LanguageSupportsContext(DbContextOptions<LanguageSupportsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cognate> Cognates { get; set; }

    public virtual DbSet<Iclo> Iclos { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=tcp:revisa-db.database.windows.net,1433;Initial Catalog=revisa_db;Persist Security Info=False;User ID=revisa_admin;Password=EeR8kMiFf@y5SCb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cognate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cognates__3213E83FF3979A04");

            entity.ToTable("cognates", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentTxtId).HasColumnName("content_txt_id");
            entity.Property(e => e.LanguageOriginId).HasColumnName("language_origin_id");
            entity.Property(e => e.LanguageOriginText)
                .HasMaxLength(56)
                .IsUnicode(false)
                .HasColumnName("language_origin_text");
            entity.Property(e => e.LanguageTargetId).HasColumnName("language_target_id");
            entity.Property(e => e.LanguageTargetMeaning)
                .HasColumnType("text")
                .HasColumnName("language_target_meaning");
            entity.Property(e => e.LanguageTargetText)
                .HasMaxLength(56)
                .IsUnicode(false)
                .HasColumnName("language_target_text");

            entity.HasOne(d => d.LanguageOrigin).WithMany(p => p.CognateLanguageOrigins)
                .HasForeignKey(d => d.LanguageOriginId)
                .HasConstraintName("FK__cognates__langua__56A9BBE0");

            entity.HasOne(d => d.LanguageTarget).WithMany(p => p.CognateLanguageTargets)
                .HasForeignKey(d => d.LanguageTargetId)
                .HasConstraintName("FK__cognates__langua__579DE019");
        });

        modelBuilder.Entity<Iclo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__iclos__3213E83F8B5088FC");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
