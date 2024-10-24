using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using revisa_api.Data.elps;

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


    public virtual DbSet<ContentLanguage> ContentLanguages { get; set; }

    public virtual DbSet<ContentTranslation> ContentTranslations { get; set; }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<DomainObjective> DomainObjectives { get; set; }

    public virtual DbSet<content.Grade> Grades { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LearningStrategiesMod> LearningStrategiesMods { get; set; }

    public virtual DbSet<LearningStrategy> LearningStrategies { get; set; }

    public virtual DbSet<LessonSchedule> LessonSchedules { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<StrategiesObjective?> StrategiesObjectives { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SupportPackage> SupportPackages { get; set; }

    public virtual DbSet<TranslationPvrRule> TranslationPvrRules { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContentLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FB4472063");

            entity.ToTable("content_language", "content");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Language)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("language");
        });

        modelBuilder.Entity<ContentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F0639AA97");

            entity.ToTable("content_status", "content");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<ContentTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FF95BEB08");

            entity.ToTable("content_translations", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentGradeId).HasColumnName("content_grade_id");
            entity.Property(e => e.ContentLanguageId).HasColumnName("content_language_id");
            entity.Property(e => e.ContentSubjectId).HasColumnName("content_subject_id");
            entity.Property(e => e.TargetLanguageId).HasColumnName("target_language_id");

            entity.HasOne(d => d.ContentGrade).WithMany(p => p.ContentTranslations)
                .HasForeignKey(d => d.ContentGradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_content_d_grade");

            entity.HasOne(d => d.ContentLanguage).WithMany(p => p.ContentTranslationContentLanguages)
                .HasForeignKey(d => d.ContentLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_content_d_clang");

            entity.HasOne(d => d.ContentSubject).WithMany(p => p.ContentTranslations)
                .HasForeignKey(d => d.ContentSubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_content_d_subj");

            entity.HasOne(d => d.TargetLanguage).WithMany(p => p.ContentTranslationTargetLanguages)
                .HasForeignKey(d => d.TargetLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_content_d_tlang");
        });

        modelBuilder.Entity<ContentTxt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F2E2BCD82");

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
                .HasConstraintName("FK__content_t__conte__4826925F");
        });

        modelBuilder.Entity<ContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FD88A4F11");

            entity.ToTable("content_type", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentType1)
                .HasMaxLength(100)
                .HasColumnName("content_type");
        });

        modelBuilder.Entity<ContentVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F4C0F251E");

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
                .HasConstraintName("FK__content_v__conte__3BC0BB7A");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentVersions)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_v__owner__3ACC9741");
        });

        modelBuilder.Entity<Domain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domains__3213E83F40239699");

            entity.ToTable("domains", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ColorHexCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("color_hex_code");
            entity.Property(e => e.Domain1)
                .IsUnicode(false)
                .HasColumnName("domain");
            entity.Property(e => e.Label)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("label");
        });

        modelBuilder.Entity<DomainLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83F54992CFA");

            entity.ToTable("domain_levels", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.LevelId).HasColumnName("level_id");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainLevels)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK__domain_le__domai__29D71569");

            entity.HasOne(d => d.Level).WithMany(p => p.DomainLevels)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK__domain_le__level__2ACB39A2");
        });

        modelBuilder.Entity<DomainLvlAttr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83F79E395BF");

            entity.ToTable("domain_lvl_attr", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attr)
                .IsUnicode(false)
                .HasColumnName("attr");
            entity.Property(e => e.AttrTypeId).HasColumnName("attr_type_id");
            entity.Property(e => e.DomainLevelId).HasColumnName("domain_level_id");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");

            entity.HasOne(d => d.AttrType).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.AttrTypeId)
                .HasConstraintName("FK__domain_lv__attr___31783731");

            entity.HasOne(d => d.DomainLevel).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.DomainLevelId)
                .HasConstraintName("FK__domain_lv__domai__2F8FEEBF");

            entity.HasOne(d => d.Grade).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.GradeId)
                .HasConstraintName("FK__domain_lv__grade__308412F8");
        });

        modelBuilder.Entity<DomainLvlAttrItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83FA3F55447");

            entity.ToTable("domain_lvl_attr_item", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainLvlAttrId).HasColumnName("domain_lvl_attr_id");
            entity.Property(e => e.Item)
                .IsUnicode(false)
                .HasColumnName("item");

            entity.HasOne(d => d.DomainLvlAttr).WithMany(p => p.DomainLvlAttrItems)
                .HasForeignKey(d => d.DomainLvlAttrId)
                .HasConstraintName("FK__domain_lv__domai__3454A3DC");
        });

        modelBuilder.Entity<DomainObjective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_o__3213E83F05212433");

            entity.ToTable("domain_objectives", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.Label)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.Objective).HasColumnName("objective");
            entity.Property(e => e.ObjectiveRichText).HasColumnName("objective_rich_text");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainObjectives)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK__domain_ob__domai__232A17DA");
        });

        modelBuilder.Entity<content.Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83FB55E4571");

            entity.ToTable("grades", "content");

            entity.HasIndex(e => e.Grade1, "UQ__grades__28A8317604FE4043").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade1)
                .HasMaxLength(50)
                .HasColumnName("grade");
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

        modelBuilder.Entity<LearningStrategiesMod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__learning__3213E83FA0787ED2");

            entity.ToTable("learning_strategies_mods", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageFileId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_file_id");
            entity.Property(e => e.LearningStrategyId).HasColumnName("learning_strategy_id");
            entity.Property(e => e.Strategy).HasColumnName("strategy");
            entity.Property(e => e.StrategyFileId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("strategy_file_id");
            entity.Property(e => e.StrategyRichText).HasColumnName("strategy_rich_text");

            entity.HasOne(d => d.LearningStrategy).WithMany(p => p.LearningStrategiesMods)
                .HasForeignKey(d => d.LearningStrategyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learning___learn__1E6562BD");
        });

        modelBuilder.Entity<LearningStrategy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__learning__3213E83FBA4068D5");

            entity.ToTable("learning_strategies", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Label)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.Strategy).HasColumnName("strategy");
        });

        modelBuilder.Entity<LessonSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lesson_s__3213E83F719B7811");

            entity.ToTable("lesson_schedule", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CycleNumber).HasColumnName("cycle_number");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.LessonOrder).HasColumnName("lesson_order");
            entity.Property(e => e.WeekNumber).HasColumnName("week_number");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__levels__3213E83F1D1223CE");

            entity.ToTable("levels", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Lvl)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("lvl");
        });

        modelBuilder.Entity<StrategiesObjective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__strategi__3213E83F0EC8FB95");

            entity.ToTable("strategies_objectives", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainObjectiveId).HasColumnName("domain_objective_id");
            entity.Property(e => e.StrategyModId).HasColumnName("strategy_mod_id");

            entity.HasOne(d => d.DomainObjective).WithMany(p => p.StrategiesObjectives)
                .HasForeignKey(d => d.DomainObjectiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__strategie__domai__382534C0");

            entity.HasOne(d => d.StrategyMod).WithMany(p => p.StrategiesObjectives)
                .HasForeignKey(d => d.StrategyModId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__strategie__strat__37311087");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subjects__3213E83F17C8DB2D");

            entity.ToTable("subjects", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Subject1)
                .HasMaxLength(100)
                .HasColumnName("subject");
        });

        modelBuilder.Entity<SupportPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__support___3213E83FEF67B781");

            entity.ToTable("support_packages", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentLanguageId).HasColumnName("content_language_id");
            entity.Property(e => e.CrossLinguisticConnection).HasColumnName("cross_linguistic_connection");
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
                .HasConstraintName("FK__support_p__conte__43EBED27");

            entity.HasOne(d => d.ElpsStrategyObjective).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.ElpsStrategyObjectiveId)
                .HasConstraintName("FK__support_p__elps___42F7C8EE");

            entity.HasOne(d => d.Grade).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__grade__44E01160");

            entity.HasOne(d => d.LessonSchedule).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.LessonScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__lesso__46C859D2");

            entity.HasOne(d => d.Subject).WithMany(p => p.SupportPackages)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__support_p__subje__45D43599");
        });

        modelBuilder.Entity<TranslationPvrRule>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("translation_pvr_rules", "language_supports");

            entity.Property(e => e.ContentTranslationId).HasColumnName("content_translation_id");
            entity.Property(e => e.PreviewBody).HasColumnName("preview_body");
            entity.Property(e => e.PreviewNotes).HasColumnName("preview_notes");
            entity.Property(e => e.ReviewBody).HasColumnName("review_body");
            entity.Property(e => e.ReviewNotes).HasColumnName("review_notes");
            entity.Property(e => e.StudentTitle).HasColumnName("student_title");
            entity.Property(e => e.TeacherContent).HasColumnName("teacher_content");
            entity.Property(e => e.TeacherTitle).HasColumnName("teacher_title");
            entity.Property(e => e.ViewBody).HasColumnName("view_body");
            entity.Property(e => e.ViewNotes).HasColumnName("view_notes");

            entity.HasOne(d => d.ContentTranslation).WithMany()
                .HasForeignKey(d => d.ContentTranslationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__conte__654CE0F2");

            entity.HasOne(d => d.PreviewBodyNavigation).WithMany()
                .HasForeignKey(d => d.PreviewBody)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__previ__691D71D6");

            entity.HasOne(d => d.PreviewNotesNavigation).WithMany()
                .HasForeignKey(d => d.PreviewNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__previ__6A11960F");

            entity.HasOne(d => d.ReviewBodyNavigation).WithMany()
                .HasForeignKey(d => d.ReviewBody)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__revie__6CEE02BA");

            entity.HasOne(d => d.ReviewNotesNavigation).WithMany()
                .HasForeignKey(d => d.ReviewNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__revie__6DE226F3");

            entity.HasOne(d => d.StudentTitleNavigation).WithMany()
                .HasForeignKey(d => d.StudentTitle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__stude__67352964");

            entity.HasOne(d => d.TeacherContentNavigation).WithMany()
                .HasForeignKey(d => d.TeacherContent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__teach__68294D9D");

            entity.HasOne(d => d.TeacherTitleNavigation).WithMany()
                .HasForeignKey(d => d.TeacherTitle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__teach__6641052B");

            entity.HasOne(d => d.ViewBodyNavigation).WithMany()
                .HasForeignKey(d => d.ViewBody)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__view___6B05BA48");

            entity.HasOne(d => d.ViewNotesNavigation).WithMany()
                .HasForeignKey(d => d.ViewNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__translati__view___6BF9DE81");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
