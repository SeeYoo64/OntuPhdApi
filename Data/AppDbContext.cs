using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Models.Authorization;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<ProgramDocument> ProgramDocuments { get; set; }
        public DbSet<ProgramComponent> ProgramComponents { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureAuthEntities(modelBuilder);

            ConfigureProgramModel(modelBuilder);

            ConfigureProgramDocument(modelBuilder);
            ConfigureProgramJobs(modelBuilder);
            ConfigureProgramComponent(modelBuilder);    
        }

        private void ConfigureProgramModel(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Конфигурация ProgramModel
            modelBuilder.Entity<ProgramModel>(entity =>
            {
                // Первичный ключ
                entity.HasKey(e => e.Id);

                // Обязательные поля
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Degree).IsRequired();

                // Поля с типом jsonb
                entity.Property(e => e.FieldOfStudy)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<FieldOfStudy>(v, new JsonSerializerOptions()));

                entity.Property(e => e.Speciality)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<Speciality>(v, new JsonSerializerOptions()));

                entity.Property(e => e.Form)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));

                entity.Property(e => e.Directions)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));

                entity.Property(e => e.ProgramCharacteristics)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<ProgramCharacteristics>(v, new JsonSerializerOptions()));

                entity.Property(e => e.ProgramCompetence)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<ProgramCompetence>(v, new JsonSerializerOptions()));

                entity.Property(e => e.Results)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));

                // Связь один-к-одному с ProgramDocument
                entity.HasOne(e => e.ProgramDocument)
                      .WithOne()
                      .HasForeignKey<ProgramModel>(e => e.ProgramDocumentId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Связь один-ко-многим с ProgramComponent
                entity.HasMany(e => e.Components)
                      .WithOne(e => e.ProgramModel)
                      .HasForeignKey(e => e.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Связь один-ко-многим с Job
                entity.HasMany(e => e.Jobs)
                      .WithOne(e => e.ProgramModel)
                      .HasForeignKey(e => e.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureProgramComponent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramComponent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ComponentType).IsRequired();
                entity.Property(e => e.ComponentName).IsRequired();

                entity.Property(e => e.ControlForm)
                      .HasColumnType("jsonb")
                      .HasConversion(
                          v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));
            });
        }

        private void ConfigureProgramJobs(ModelBuilder modelBuilder)
        {
            // Конфигурация для Job
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.Title).IsRequired();

                // Поле ProgramModel в Job не нужно хранить в базе, оно уже связано через ProgramId
                entity.Ignore(e => e.ProgramModel);
            });
        }

        private void ConfigureProgramDocument(ModelBuilder modelBuilder)
        {
            // Конфигурация ProgramDocument
            modelBuilder.Entity<ProgramDocument>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired();
                entity.Property(e => e.FilePath).IsRequired();
                entity.Property(e => e.UploadDate).IsRequired();
                entity.Property(e => e.FileSize).IsRequired();
                entity.Property(e => e.ContentType).IsRequired();
            });
        }


        private void ConfigureAuthEntities(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            ConfigureAccount(modelBuilder);
            ConfigureSession(modelBuilder);
            ConfigureVerificationToken(modelBuilder);
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Поля
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(255);
            });
        }

        private void ConfigureAccount(ModelBuilder modelBuilder)
        {
            // Конфигурация Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Поля
                entity.Property(e => e.Type).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Provider).HasMaxLength(255).IsRequired();
                entity.Property(e => e.ProviderAccountId).HasMaxLength(255).IsRequired();

                // Связь с User
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Accounts)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureSession(ModelBuilder modelBuilder)
        {
            // Конфигурация Session
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Поля
                entity.Property(e => e.SessionToken).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Expires).IsRequired();

                // Связь с User
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Sessions)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureVerificationToken(ModelBuilder modelBuilder)
        {
            // Конфигурация VerificationToken
            modelBuilder.Entity<VerificationToken>(entity =>
            {
                // Составной ключ (identifier, token)
                entity.HasKey(e => new { e.Identifier, e.Token });

                // Все поля обязательные
                entity.Property(e => e.Identifier).IsRequired();
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.Expires).IsRequired();
            });
        }


    }
}
