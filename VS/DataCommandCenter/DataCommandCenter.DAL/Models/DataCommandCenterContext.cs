using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataCommandCenter.DAL.Models
{
    public partial class DataCommandCenterContext : DbContext
    {
        public DataCommandCenterContext()
        {
        }

        public DataCommandCenterContext(DbContextOptions<DataCommandCenterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ObjectSearch> ObjectSearches { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<ServerType> ServerTypes { get; set; } = null!;
        public virtual DbSet<SqlColumn> SqlColumns { get; set; } = null!;
        public virtual DbSet<SqlDatabase> SqlDatabases { get; set; } = null!;
        public virtual DbSet<SqlDatabaseHistory> SqlDatabaseHistories { get; set; } = null!;
        public virtual DbSet<SqlMetadatum> SqlMetadata { get; set; } = null!;
        public virtual DbSet<SqlObject> SqlObjects { get; set; } = null!;
        public virtual DbSet<SqlTableHistory> SqlTableHistories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=XPS17;Database=DataCommandCenter;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjectSearch>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ObjectSearch", "meta");

                entity.Property(e => e.DisplayText).HasMaxLength(1263);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ObjectType).HasMaxLength(4000);

                entity.Property(e => e.SearchText).HasMaxLength(869);
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("Server", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ServerInstance).HasMaxLength(50);

                entity.Property(e => e.ServerName).HasMaxLength(255);

                entity.Property(e => e.ServerTypeId).HasColumnName("ServerTypeID");

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.HasOne(d => d.ServerType)
                    .WithMany(p => p.Servers)
                    .HasForeignKey(d => d.ServerTypeId)
                    .HasConstraintName("FK_Server_Server");
            });

            modelBuilder.Entity<ServerType>(entity =>
            {
                entity.ToTable("ServerType", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ServerType1)
                    .HasMaxLength(255)
                    .HasColumnName("ServerType");
            });

            modelBuilder.Entity<SqlColumn>(entity =>
            {
                entity.ToTable("SQL_Column", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ColumnName).HasMaxLength(255);

                entity.Property(e => e.DataType).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.SqlColumns)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("FK_SQL_Column_SQL_Object");
            });

            modelBuilder.Entity<SqlDatabase>(entity =>
            {
                entity.ToTable("SQL_Database", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Access)
                    .HasMaxLength(60)
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.Collation).HasMaxLength(128);

                entity.Property(e => e.CreatedDatetime).HasColumnType("datetime");

                entity.Property(e => e.DataSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("DataSizeMB");

                entity.Property(e => e.DatabaseName).HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.LogSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("LogSizeMB");

                entity.Property(e => e.Recovery)
                    .HasMaxLength(60)
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.ServerId).HasColumnName("ServerID");

                entity.HasOne(d => d.Server)
                    .WithMany(p => p.SqlDatabases)
                    .HasForeignKey(d => d.ServerId)
                    .HasConstraintName("FK_Database_Server");
            });

            modelBuilder.Entity<SqlDatabaseHistory>(entity =>
            {
                entity.ToTable("SQL_DatabaseHistory", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CheckDatetime).HasColumnType("datetime");

                entity.Property(e => e.DataSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("DataSizeMB");

                entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");

                entity.Property(e => e.LogSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("LogSizeMB");

                entity.HasOne(d => d.Database)
                    .WithMany(p => p.SqlDatabaseHistories)
                    .HasForeignKey(d => d.DatabaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SQL_DatabaseHistory_SQL_Database");
            });

            modelBuilder.Entity<SqlMetadatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("SQL_Metadata", "meta");

                entity.Property(e => e.Access)
                    .HasMaxLength(60)
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.Collation).HasMaxLength(128);

                entity.Property(e => e.ColumnName).HasMaxLength(255);

                entity.Property(e => e.CreatedDatetime).HasColumnType("datetime");

                entity.Property(e => e.DataSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("DataSizeMB");

                entity.Property(e => e.DataType).HasMaxLength(50);

                entity.Property(e => e.DatabaseName).HasMaxLength(128);

                entity.Property(e => e.LogSizeMb)
                    .HasColumnType("numeric(38, 6)")
                    .HasColumnName("LogSizeMB");

                entity.Property(e => e.ObjectName).HasMaxLength(255);

                entity.Property(e => e.ObjectType).HasMaxLength(50);

                entity.Property(e => e.Recovery)
                    .HasMaxLength(60)
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.SchemaName).HasMaxLength(50);

                entity.Property(e => e.ServerInstance).HasMaxLength(50);

                entity.Property(e => e.ServerName).HasMaxLength(255);

                entity.Property(e => e.ServerType).HasMaxLength(255);

                entity.Property(e => e.SizeMb).HasColumnName("SizeMB");

                entity.Property(e => e.Version).HasMaxLength(50);
            });

            modelBuilder.Entity<SqlObject>(entity =>
            {
                entity.ToTable("SQL_Object", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ObjectName).HasMaxLength(255);

                entity.Property(e => e.ObjectType).HasMaxLength(50);

                entity.Property(e => e.SchemaName).HasMaxLength(50);

                entity.Property(e => e.SizeMb).HasColumnName("SizeMB");

                entity.HasOne(d => d.Database)
                    .WithMany(p => p.SqlObjects)
                    .HasForeignKey(d => d.DatabaseId)
                    .HasConstraintName("FK_SQL_Object_SQL_Database");
            });

            modelBuilder.Entity<SqlTableHistory>(entity =>
            {
                entity.ToTable("SQL_TableHistory", "meta");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CheckDatetime).HasColumnType("datetime");

                entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

                entity.Property(e => e.SizeMb).HasColumnName("SizeMB");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.SqlTableHistories)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("FK_SQL_TableHistory_SQL_Object");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
