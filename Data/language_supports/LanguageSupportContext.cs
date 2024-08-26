using System;
using System.Collections.Generic;
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

    public virtual DbSet<SupportPackage> SupportPackages { get; set; }

    public virtual DbSet<ContentTeksSubject> ContentTeksSubjects { get;  set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContentTeksSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F6A017BAA");

            entity.ToTable("content_teks_subjects", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentSubjectId).HasColumnName("content_subject_id");
            entity.Property(e => e.TeksSubjectId).HasColumnName("teks_subject_id");

            entity.HasOne(d => d.ContentSubject).WithMany(p => p.ContentTeksSubjects)
                .HasForeignKey(d => d.ContentSubjectId)
                .HasConstraintName("FK__content_t__conte__015F0FBB");

            entity.HasOne(d => d.TeksSubject).WithMany(p => p.ContentTeksSubjects)
                .HasForeignKey(d => d.TeksSubjectId)
                .HasConstraintName("FK__content_t__teks___025333F4");
        });


        modelBuilder.Entity<Iclo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__iclos__3213E83F7A520BBC");

            entity.ToTable("iclos", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iclo1)
                .HasColumnType("text")
                .HasColumnName("iclo");
            entity.Property(e => e.LessonScheduleId).HasColumnName("lesson_schedule_id");
            entity.Property(e => e.StrategyObjectiveId).HasColumnName("strategy_objective_id");
            entity.Property(e => e.TeksItemId).HasColumnName("teks_item_id");

            entity.HasOne(d => d.LessonSchedule).WithMany(p => p.Iclos)
                .HasForeignKey(d => d.LessonScheduleId)
                .HasConstraintName("FK__iclos__lesson_sc__6D58170E");

            entity.HasOne(d => d.StrategyObjective).WithMany(p => p.Iclos)
                .HasForeignKey(d => d.StrategyObjectiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__iclos__strategy___752E4300");

            entity.HasOne(d => d.TeksItem).WithMany(p => p.Iclos)
                .HasForeignKey(d => d.TeksItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__iclos__teks_item__76226739");
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

        modelBuilder.Entity<SupportPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__support___3213E83F1CFE1778");

            entity.ToTable("support_packages", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentLanguageId).HasColumnName("content_language_id");
            entity.Property(e => e.CrossLinguisticConnection)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cross_linguistic_connection");
            entity.Property(e => e.ElpsStrategyObjectiveId).HasColumnName("elps_strategy_objective_id");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(1)
                .HasColumnName("is_active");
            entity.Property(e => e.LessonScheduleId).HasColumnName("lesson_schedule_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.ContentLanguage).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.ContentLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__conte__2EF0D041");

            entity.HasOne(d => d.ElpsStrategyObjective).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.ElpsStrategyObjectiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__elps___2DFCAC08");

            entity.HasOne(d => d.Grade).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__grade__2FE4F47A");

            entity.HasOne(d => d.LessonSchedule).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.LessonScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__lesso__31CD3CEC");

            entity.HasOne(d => d.Subject).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__subje__30D918B3");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
