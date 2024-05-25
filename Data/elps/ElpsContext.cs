using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace revisa_api.Data.elps;

public partial class ElpsContext : DbContext
{
    public ElpsContext()
    {
    }

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

    public virtual DbSet<LearningStrategiesMod> LearningStrategiesMods { get; set; }

    public virtual DbSet<LearningStrategy> LearningStrategies { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<StrategyObjective> StrategyObjectives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttrType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__attr_typ__3213E83FD6BC78D3");

            entity.ToTable("attr_type", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Typ).HasColumnName("typ");
        });

        modelBuilder.Entity<Domain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__domains__3213E83F40239699");

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

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainObjectives)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK__domain_ob__domai__232A17DA");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__grades__3213E83FCBC377F2");

            entity.ToTable("grades", "elps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade1)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("grade");
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

            entity.HasOne(d => d.DomainObjective).WithMany(p => p.StrategiesObjectives)
                .HasForeignKey(d => d.DomainObjectiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__strategie__domai__382534C0");

            entity.HasOne(d => d.StrategyMod).WithMany(p => p.StrategiesObjectives)
                .HasForeignKey(d => d.StrategyModId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__strategie__strat__37311087");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
