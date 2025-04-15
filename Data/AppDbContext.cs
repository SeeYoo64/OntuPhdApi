using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Authorization;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Employees;
using OntuPhdApi.Models.News;
using OntuPhdApi.Models.Documents;

namespace OntuPhdApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<ProgramDocument> ProgramDocuments { get; set; }
        public DbSet<ProgramComponent> ProgramComponents { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<DefenseModel> Defenses { get; set; }

        public DbSet<EmployeeModel> Employees { get; set; }

        public DbSet<NewsModel> News { get; set; }

        public DbSet<DocumentModel> Documents { get; set; }

        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureAuthEntities(modelBuilder);
            ConfigureProgramEntity(modelBuilder);

            ConfigureDefense(modelBuilder);

            ConfigureEmployees(modelBuilder);

            ConfigureNews(modelBuilder);

            ConfigureDocument(modelBuilder);

        }

        private void ConfigureProgramEntity(ModelBuilder modelBuilder)
        {
            ConfigureProgramDocument(modelBuilder);
            ConfigureProgramJobs(modelBuilder);
            ConfigureProgramComponent(modelBuilder);
            ConfigureProgramModel(modelBuilder);
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
                      .HasColumnType("jsonb"); 

                entity.Property(e => e.Speciality)
                      .HasColumnType("jsonb");

                entity.Property(e => e.ProgramCharacteristics)
                      .HasColumnType("jsonb");

                entity.Property(e => e.ProgramCompetence)
                      .HasColumnType("jsonb");

                entity.Property(e => e.Form)
                      .HasColumnType("jsonb");

                entity.Property(e => e.Directions)
                      .HasColumnType("jsonb");

                entity.Property(e => e.ProgramCharacteristics)
                      .HasColumnType("jsonb");

                entity.Property(e => e.ProgramCompetence)
                      .HasColumnType("jsonb");

                entity.Property(e => e.Results)
                      .HasColumnType("jsonb");

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

            modelBuilder.Ignore<FieldOfStudy>();
            modelBuilder.Ignore<Speciality>();
            modelBuilder.Ignore<ShortSpeciality>();
            modelBuilder.Ignore<ProgramCharacteristics>();
            modelBuilder.Ignore<ProgramCompetence>();
            modelBuilder.Ignore<Area>();
            modelBuilder.Ignore<DefenseFile>();
            modelBuilder.Ignore<CompositionOfRada>();
            modelBuilder.Ignore<MemberOfRada>();
        }

        private void ConfigureProgramComponent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramComponent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ComponentType).IsRequired();
                entity.Property(e => e.ComponentName).IsRequired();

                entity.Property(e => e.ControlForm)
                      .HasColumnType("jsonb");
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

        private void ConfigureDefense(ModelBuilder modelBuilder)
        {
            // Конфигурация DefenseModel
            modelBuilder.Entity<DefenseModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CandidateNameSurname);
                entity.Property(e => e.DefenseTitle);
                entity.Property(e => e.ScienceTeachers).HasColumnType("jsonb");
                entity.Property(e => e.DefenseDate);
                entity.Property(e => e.Address);
                entity.Property(e => e.Message);
                entity.Property(e => e.Placeholder);
                entity.Property(e => e.Members).HasColumnType("jsonb");
                entity.Property(e => e.Files).HasColumnType("jsonb");
                entity.Property(e => e.PublicationDate);
                entity.Property(e => e.ProgramId);

                // Связь с Program
                entity.HasOne(d => d.Program)
                      .WithMany()
                      .HasForeignKey(d => d.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }

        private void ConfigureEmployees(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Position).IsRequired();
                entity.Property(e => e.PhotoPath);
            });
        }

        private void ConfigureNews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Summary).IsRequired();
                entity.Property(e => e.MainTag).IsRequired();
                entity.Property(e => e.OtherTags).HasColumnType("jsonb");
                entity.Property(e => e.PublicationDate);
                entity.Property(e => e.ThumbnailPath);
                entity.Property(e => e.PhotoPaths).HasColumnType("jsonb");
                entity.Property(e => e.Body).HasColumnType("text");
            });
        }

        private void ConfigureDocument(ModelBuilder modelBuilder)
        {
            // Конфигурация Document
            modelBuilder.Entity<DocumentModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(d => d.Id).ValueGeneratedOnAdd();
                entity.Property(d => d.ProgramId);
                entity.Property(d => d.Name).IsRequired();
                entity.Property(d => d.Type).IsRequired();
                entity.Property(d => d.Link).IsRequired();
            });
        }




        private void ConfigureAuthEntities(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            ConfigureAccount(modelBuilder);
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
