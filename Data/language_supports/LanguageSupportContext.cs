using Microsoft.EntityFrameworkCore;
using revisa_api.Data.content;
using revisa_api.Data.elps;
using Grade = revisa_api.Data.content.Grade;

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

    public virtual DbSet<AttrType> AttrTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ContentDetail> ContentDetails { get; set; }

    public virtual DbSet<ContentFile> ContentFiles { get; set; }

    public virtual DbSet<ContentGroup> ContentGroups { get; set; }

    public virtual DbSet<ContentLanguage> ContentLanguages { get; set; }

    public virtual DbSet<ContentRichTxt> ContentRichTxts { get; set; }

    public virtual DbSet<ContentStatus> ContentStatuses { get; set; }

    public virtual DbSet<ContentTranslation> ContentTranslations { get; set; }

    public virtual DbSet<ContentTxt> ContentTxts { get; set; }

    public virtual DbSet<ContentType> ContentTypes { get; set; }

    public virtual DbSet<ContentVersion> ContentVersions { get; set; }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<DomainLevel> DomainLevels { get; set; }

    public virtual DbSet<DomainLvlAttr> DomainLvlAttrs { get; set; }

    public virtual DbSet<DomainLvlAttrItem> DomainLvlAttrItems { get; set; }

    public virtual DbSet<DomainObjective> DomainObjectives { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LearningStrategiesMod> LearningStrategiesMods { get; set; }

    public virtual DbSet<LearningStrategy> LearningStrategies { get; set; }

    public virtual DbSet<LessonSchedule> LessonSchedules { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<StrategyObjective> StrategiesObjectives { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SupportPackage> SupportPackages { get; set; }

    public virtual DbSet<TranslationPair> TranslationPairs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttrType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__attr_typ__3213E83FD6BC78D3");

            entity.ToTable("attr_type", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Typ).HasColumnName("typ");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clients__3213E83F99ECF343");

            entity.ToTable("clients", "content", tb => tb.HasTrigger("trg_upper_client_name"));

            entity.HasIndex(e => e.ClientName, "UQ__clients__9ADC3B74ADFDD228").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .HasColumnName("client_name");
        });

        modelBuilder.Entity<ContentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FD5B946B5");

            entity.ToTable("content_details", "content", tb => tb.HasTrigger("trg_insert_content_version"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.FileId)
                .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"))
                .HasColumnName("file_id");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");
            entity.Property(e => e.LanguageId)
                .HasDefaultValue(1)
                .HasColumnName("language_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(0)
                .HasColumnName("status_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Client).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__clien__31432D07");

            entity.HasOne(d => d.File).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("FK_content_d_file");

            entity.HasOne(d => d.Grade).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__grade__32375140");

            entity.HasOne(d => d.Language).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_content_d_lang");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__owner__341F99B2");

            entity.HasOne(d => d.Status).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__content_d__statu__61B15A38");

            entity.HasOne(d => d.Subject).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__subje__332B7579");
        });

        modelBuilder.Entity<ContentFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F6D69A64E");

            entity.ToTable("content_file", "content");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.FileName).HasColumnName("file_name");
            entity.Property(e => e.SourceFileId).HasColumnName("source_file_id");
        });

        modelBuilder.Entity<ContentGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83FD4F75830");

            entity.ToTable("content_group", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentVersionId).HasColumnName("content_version_id");

            entity.HasOne(d => d.ContentVersion).WithMany(p => p.ContentGroups)
                .HasForeignKey(d => d.ContentVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_g__conte__454A25B4");
        });

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

        modelBuilder.Entity<ContentRichTxt>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("content_rich_txt", "content");

            entity.Property(e => e.ContentTxtId).HasColumnName("content_txt_id");
            entity.Property(e => e.RichTxt).HasColumnName("rich_txt");

            entity.HasOne(d => d.ContentTxt).WithMany()
                .HasForeignKey(d => d.ContentTxtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_r__rich___4D755761");
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

        modelBuilder.Entity<Grade>(entity =>
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

        modelBuilder.Entity<StrategyObjective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__strategi__3213E83F0EC8FB95");

            entity.ToTable("strategies_objectives", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainObjectiveId).HasColumnName("domain_objective_id");
            entity.Property(e => e.StrategyModId).HasColumnName("strategy_mod_id");

            entity.HasOne(d => d.DomainObjective).WithMany(p => p.StrategyObjectives)
                .HasForeignKey(d => d.DomainObjectiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__strategie__domai__382534C0");

            entity.HasOne(d => d.StrategyMod).WithMany(p => p.StrategyObjectives)
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

        modelBuilder.Entity<TranslationPair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__translat__3213E83FFD76787D");

            entity.ToTable("translation_pairs", "language_supports");

            entity.Property(e => e.Id).HasColumnName("id");
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

            entity.HasOne(d => d.LanguageOrigin).WithMany(p => p.TranslationPairLanguageOrigins)
                .HasForeignKey(d => d.LanguageOriginId)
                .HasConstraintName("FK__translati__langu__49A4C67D");

            entity.HasOne(d => d.LanguageTarget).WithMany(p => p.TranslationPairLanguageTargets)
                .HasForeignKey(d => d.LanguageTargetId)
                .HasConstraintName("FK__translati__langu__4A98EAB6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
