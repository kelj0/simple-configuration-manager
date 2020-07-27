using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SimpleConfigurationManager.Models.DbModels;
using OperatingSystem = SimpleConfigurationManager.Models.DbModels.OperatingSystem;

namespace SimpleConfigurationManager.Infrastructure.Database
{
    public partial class SimpleConfigurationManagerContext : DbContext
    {
        public SimpleConfigurationManagerContext()
        {
        }

        public SimpleConfigurationManagerContext(DbContextOptions<SimpleConfigurationManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<ConfigurationReview> ConfigurationReview { get; set; }
        public virtual DbSet<OperatingSystem> OperatingSystem { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<ServerConfiguration> ServerConfiguration { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Infrastructure.Settings.Constants.CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(e => e.IdConfiguration);

                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Configur__737584F673FE1369")
                    .IsUnique();

                entity.Property(e => e.ConfigurationScript).IsRequired();

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.FullDescription).HasMaxLength(2000);

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.TimeOfCreation)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeOfLastUpdate).HasColumnType("datetime");

                entity.HasOne(d => d.OperatingSystem)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.OperatingSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_OperatingSystem");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_User");
            });

            modelBuilder.Entity<ConfigurationReview>(entity =>
            {
                entity.HasKey(e => e.IdConfigurationReview);

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.TimeOfCreation)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.ConfigurationReview)
                    .HasForeignKey(d => d.ConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationReview_Configuration");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ConfigurationReview)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationReview_User");
            });

            modelBuilder.Entity<OperatingSystem>(entity =>
            {
                entity.HasKey(e => e.IdOperatingSystem);

                entity.HasIndex(e => e.OperatingSystemName)
                    .HasName("UQ__Operatin__59602E2716DFE742")
                    .IsUnique();

                entity.Property(e => e.OperatingSystemName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.IdServer);

                entity.HasIndex(e => e.IpAddress)
                    .HasName("UQ__Server__30C707A393831EB2")
                    .IsUnique();

                entity.HasIndex(e => e.ServerName)
                    .HasName("UQ__Server__97BAE5EB475231DA")
                    .IsUnique();

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeOfLastPing).HasColumnType("datetime");

                entity.HasOne(d => d.OperatingSystem)
                    .WithMany(p => p.Server)
                    .HasForeignKey(d => d.OperatingSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Server_OperatingSystem");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Server)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Server_User");
            });

            modelBuilder.Entity<ServerConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.ServerId, e.ConfigurationId })
                    .HasName("PK_UserGroup");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.ServerConfiguration)
                    .HasForeignKey(d => d.ConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerConfiguration_Configuration");

                entity.HasOne(d => d.Server)
                    .WithMany(p => p.ServerConfiguration)
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServerConfiguration_Server");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__User__A9D10534AC45CBC9")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__User__C9F2845662BB8FC5")
                    .IsUnique();

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeOfLastLogin).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(150);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
