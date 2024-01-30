using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using NLog.LayoutRenderers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace GravityFs.Models;

public partial class GravityfsdbContext : DbContext
{
    private IConfiguration config;



    public GravityfsdbContext(DbContextOptions<GravityfsdbContext> options, IConfiguration config) : base(options)
    {
        this.config = config;
    }

    public virtual DbSet<Atchmnfl> Atchmnfls { get; set; }

    public virtual DbSet<AtchmnflGroup> AtchmnflGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(config.GetConnectionString("MariaDBContext"), ServerVersion.AutoDetect(config.GetConnectionString("MariaDBContext")));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Atchmnfl>(entity =>
        {
            entity.HasKey(e => e.AtchmnflId).HasName("PRIMARY");

            entity.ToTable("atchmnfl");

            entity.Property(e => e.AtchmnflId)
                .HasMaxLength(50)
                .HasColumnName("atchmnfl_id");
            entity.Property(e => e.AtchmnflGroupId)
                .HasMaxLength(50)
                .HasColumnName("atchmnfl_group_id");
            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .HasColumnName("content_type");
            entity.Property(e => e.FileSize)
                .HasColumnType("int(11)")
                .HasColumnName("file_size");
            entity.Property(e => e.OriginalFilename)
                .HasMaxLength(50)
                .HasColumnName("original_filename");
            entity.Property(e => e.RegistDt)
                .HasColumnType("datetime")
                .HasColumnName("regist_dt");
            entity.Property(e => e.RegisterId)
                .HasMaxLength(50)
                .HasColumnName("register_id");
            entity.Property(e => e.SaveFilename)
                .HasMaxLength(50)
                .HasColumnName("save_filename");
            entity.Property(e => e.SaveSubPath)
                .HasMaxLength(50)
                .HasColumnName("save_sub_path");
            entity.Property(e => e.UpdateDt)
                .HasColumnType("datetime")
                .HasColumnName("update_dt");
            entity.Property(e => e.UpdaterId)
                .HasMaxLength(50)
                .HasColumnName("updater_id");
        });

        modelBuilder.Entity<AtchmnflGroup>(entity =>
        {
            entity.HasKey(e => e.AtchmnflGroupId).HasName("PRIMARY");

            entity.ToTable("atchmnfl_group");

            entity.Property(e => e.AtchmnflGroupId)
                .HasMaxLength(50)
                .HasColumnName("atchmnfl_group_id");
            entity.Property(e => e.BizKey)
                .HasMaxLength(50)
                .HasColumnName("biz_key");
            entity.Property(e => e.RegistDt)
                .HasColumnType("datetime")
                .HasColumnName("regist_dt");
            entity.Property(e => e.RegisterId)
                .HasMaxLength(50)
                .HasColumnName("register_id");
            entity.Property(e => e.UpdateDt)
                .HasColumnType("datetime")
                .HasColumnName("update_dt");
            entity.Property(e => e.UpdaterId)
                .HasMaxLength(50)
                .HasColumnName("updater_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
