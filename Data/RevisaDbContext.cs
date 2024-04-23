using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data;

public partial class RevisaDbContext : DbContext
{
    public RevisaDbContext()
    {
    }

    public RevisaDbContext(DbContextOptions<RevisaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttrType> AttrTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ContentDetail> ContentDetails { get; set; }

    public virtual DbSet<ContentTxt> ContentTxts { get; set; }

    public virtual DbSet<ContentType> ContentTypes { get; set; }

    public virtual DbSet<ContentVersion> ContentVersions { get; set; }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<DomainLevel> DomainLevels { get; set; }

    public virtual DbSet<DomainLvlAttr> DomainLvlAttrs { get; set; }

    public virtual DbSet<DomainLvlAttrItem> DomainLvlAttrItems { get; set; }

    public virtual DbSet<DomainObjective> DomainObjectives { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Grade1> Grades1 { get; set; }

    public virtual DbSet<LearningStrategy> LearningStrategies { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=tcp:revisa-db.database.windows.net,1433;Initial Catalog=revisa_db;Persist Security Info=False;User ID=revisa_admin;Password=EeR8kMiFf@y5SCb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttrType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__attr_typ__3213E83F8A073583");

            entity.ToTable("attr_type", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Typ).HasColumnName("typ");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clients__3213E83F3C9E1E1F");

            entity.ToTable("clients", "content", tb => tb.HasTrigger("trg_upper_client_name"));

            entity.HasIndex(e => e.ClientName, "UQ__clients__9ADC3B74C8CDBFBF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .HasColumnName("client_name");
        });

        modelBuilder.Entity<ContentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F5662A25A");

            entity.ToTable("content_details", "content", tb => tb.HasTrigger("trg_insert_content_version"));

            entity.HasIndex(e => e.OriginalFilename, "UQ__content___D24CB641EE26E4CD").IsUnique();

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
                .HasConstraintName("FK__content_d__clien__7B4643B2");

            entity.HasOne(d => d.Grade).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__grade__7C3A67EB");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__owner__7E22B05D");

            entity.HasOne(d => d.Subject).WithMany(p => p.ContentDetails)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_d__subje__7D2E8C24");
        });

        modelBuilder.Entity<ContentTxt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F84D88FC0");

            entity.ToTable("content_txt", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
            entity.Property(e => e.ContentVersionId).HasColumnName("content_version_id");

            entity.HasOne(d => d.ContentType).WithMany(p => p.ContentTxts)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_t__conte__0D64F3ED");

            entity.HasOne(d => d.ContentVersion).WithMany(p => p.ContentTxts)
                .HasForeignKey(d => d.ContentVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_t__conte__0C70CFB4");
        });

        modelBuilder.Entity<ContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F1BD70EBB");

            entity.ToTable("content_type", "content");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ContentType1)
                .HasMaxLength(100)
                .HasColumnName("content_type");
        });

        modelBuilder.Entity<ContentVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__content___3213E83F4EB5F501");

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
                .HasConstraintName("FK__content_v__conte__05C3D225");

            entity.HasOne(d => d.Owner).WithMany(p => p.ContentVersions)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__content_v__owner__04CFADEC");
        });

        modelBuilder.Entity<Domain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domains__3213E83F7D810A39");

            entity.ToTable("domains", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83FB229BDDD");

            entity.ToTable("domain_levels", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.LevelId).HasColumnName("level_id");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainLevels)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK__domain_le__domai__1975C517");

            entity.HasOne(d => d.Level).WithMany(p => p.DomainLevels)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK__domain_le__level__1A69E950");
        });

        modelBuilder.Entity<DomainLvlAttr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83FB3EA6CC6");

            entity.ToTable("domain_lvl_attr", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attr).HasColumnName("attr");
            entity.Property(e => e.AttrTypeId).HasColumnName("attr_type_id");
            entity.Property(e => e.DomainLevelId).HasColumnName("domain_level_id");
            entity.Property(e => e.GradeId).HasColumnName("grade_id");

            entity.HasOne(d => d.AttrType).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.AttrTypeId)
                .HasConstraintName("FK__domain_lv__attr___2116E6DF");

            entity.HasOne(d => d.DomainLevel).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.DomainLevelId)
                .HasConstraintName("FK__domain_lv__domai__1F2E9E6D");

            entity.HasOne(d => d.Grade).WithMany(p => p.DomainLvlAttrs)
                .HasForeignKey(d => d.GradeId)
                .HasConstraintName("FK__domain_lv__grade__2022C2A6");
        });

        modelBuilder.Entity<DomainLvlAttrItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_l__3213E83F1206E4B7");

            entity.ToTable("domain_lvl_attr_item", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainLvlAttrId).HasColumnName("domain_lvl_attr_id");
            entity.Property(e => e.Item).HasColumnName("item");

            entity.HasOne(d => d.DomainLvlAttr).WithMany(p => p.DomainLvlAttrItems)
                .HasForeignKey(d => d.DomainLvlAttrId)
                .HasConstraintName("FK__domain_lv__domai__23F3538A");
        });

        modelBuilder.Entity<DomainObjective>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domain_o__3213E83F50A4B573");

            entity.ToTable("domain_objectives", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.Label)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.Objective).HasColumnName("objective");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainObjectives)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK__domain_ob__domai__12C8C788");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83FC3351689");

            entity.ToTable("grades", "content");

            entity.HasIndex(e => e.Grade1, "UQ__grades__28A83176A807DC0C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade1)
                .HasMaxLength(50)
                .HasColumnName("grade");
        });

        modelBuilder.Entity<Grade1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83F78E0A02C");

            entity.ToTable("grades", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("grade");
        });

        modelBuilder.Entity<LearningStrategy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__learning__3213E83F274E5600");

            entity.ToTable("learning_strategies", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Label)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("label");
            entity.Property(e => e.Objective).HasColumnName("objective");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__levels__3213E83FD2E33A50");

            entity.ToTable("levels", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Lvl)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("lvl");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subjects__3213E83FBE310ABA");

            entity.ToTable("subjects", "content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Subject1)
                .HasMaxLength(100)
                .HasColumnName("subject");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F833C8A2A");

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
