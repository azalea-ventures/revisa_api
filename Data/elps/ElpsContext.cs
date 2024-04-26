
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.elps;

public partial class ElpsContext : DbContext
{

    public ElpsContext(DbContextOptions<ElpsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttrType> AttrTypes { get; set; }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<DomainLevel> DomainLevels { get; set; }

    public virtual DbSet<DomainLvlAttr> DomainLvlAttrs { get; set; }

    public virtual DbSet<DomainLvlAttrItem> DomainLvlAttrItems { get; set; }

    public virtual DbSet<DomainObjective> DomainObjectives { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<LearningStrategy> LearningStrategies { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83F78E0A02C");

            entity.ToTable("grades", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade1)
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
