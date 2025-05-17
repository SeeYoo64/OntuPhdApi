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
using OntuPhdApi.Models.ApplyDocuments;
using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Models;
using System.Reflection.Metadata;
using OntuPhdApi.Models.Roadmap;

namespace OntuPhdApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<ProgramCharacteristics> ProgramCharacteristics { get; set; }
        public DbSet<ProgramCompetence> ProgramCompetences { get; set; }
        public DbSet<ProgramComponent> ProgramComponents { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<OverallCompetence> OverallCompetences { get; set; }
        public DbSet<SpecialCompetence> SpecialCompetences { get; set; }
        public DbSet<ControlForm> ControlForms { get; set; }
        public DbSet<LinkFaculty> LinkFaculties { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<ProgramDocument> ProgramDocuments { get; set; }
        public DbSet<Institute> Institutes { get; set; }

        public DbSet<RoadmapModel> Roadmaps { get; set; }

        public DbSet<DefenseModel> Defenses { get; set; }

        public DbSet<EmployeeModel> Employees { get; set; }

        public DbSet<NewsModel> News { get; set; }

        public DbSet<DocumentModel> Documents { get; set; }

        public DbSet<ApplyDocumentModel> ApplyDocuments { get; set; }

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

            ConfigureApplyDocument(modelBuilder);

            ConfigureRoadmap(modelBuilder);
        }

        private void ConfigureProgramEntity(ModelBuilder modelBuilder)
        {
            ConfigureProgramModel(modelBuilder);
            ConfigureProgramCharacteristics(modelBuilder);
            ConfigureProgramCompetence(modelBuilder);
            ConfigureProgramComponent(modelBuilder);
            ConfigureControlForm(modelBuilder);
            ConfigureProgramDocument(modelBuilder);
        }

        private void ConfigureProgramModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Degree).IsRequired();

                entity.Property(e => e.Form).HasColumnType("jsonb");
                entity.Property(e => e.Directions).HasColumnType("jsonb");
                entity.Property(e => e.Results).HasColumnType("jsonb");

                modelBuilder.Entity<ProgramModel>()
                    .HasOne(p => p.Speciality)
                    .WithMany(s => s.Programs)
                    .HasForeignKey(p => p.SpecialityId)
                    .OnDelete(DeleteBehavior.SetNull);

                modelBuilder.Entity<ProgramModel>()
                    .HasOne(p => p.FieldOfStudy)
                    .WithMany(f => f.Programs)
                    .HasForeignKey(p => p.FieldOfStudyId)
                    .OnDelete(DeleteBehavior.SetNull);

                modelBuilder.Entity<Speciality>()
                    .HasOne(s => s.FieldOfStudy)
                    .WithMany(f => f.Specialities)
                    .HasForeignKey(s => s.FieldOfStudyId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: ProgramModel → Jobs
                entity.HasMany(p => p.Jobs)
                      .WithOne(j => j.ProgramModel)
                      .HasForeignKey(j => j.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: ProgramModel → LinkFaculties
                entity.HasMany(p => p.LinkFaculties)
                      .WithOne(lf => lf.ProgramModel)
                      .HasForeignKey(lf => lf.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-one: ProgramModel → ProgramCharacteristics
                entity.HasOne(p => p.ProgramCharacteristics)
                      .WithOne(pc => pc.Program)
                      .HasForeignKey<ProgramCharacteristics>(pc => pc.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-one: ProgramModel → ProgramCompetence
                entity.HasOne(p => p.ProgramCompetence)
                      .WithOne(pc => pc.Program)
                      .HasForeignKey<ProgramCompetence>(pc => pc.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: ProgramModel → ProgramComponents
                entity.HasMany(p => p.ProgramComponents)
                      .WithOne(pc => pc.ProgramModel)
                      .HasForeignKey(pc => pc.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-one: ProgramModel → ProgramDocument
                entity.HasOne(p => p.ProgramDocument)
                      .WithOne(pd => pd.Program)
                      .HasForeignKey<ProgramModel>(p => p.ProgramDocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Many-to-one: ProgramModel → Institute
                entity.HasOne(p => p.Institute)
                      .WithMany()
                      .HasForeignKey(p => p.InstituteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureProgramCharacteristics(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramCharacteristics>()
                .HasOne(pc => pc.Area)
                .WithOne(a => a.ProgramCharacteristics)
                .HasForeignKey<Area>(a => a.ProgramCharacteristicsId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureProgramCompetence(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramCompetence>()
                .HasMany(pc => pc.OverallCompetences)
                .WithOne(oc => oc.ProgramCompetence)
                .HasForeignKey(oc => oc.ProgramCompetenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramCompetence>()
                .HasMany(pc => pc.SpecialCompetences)
                .WithOne(sc => sc.ProgramCompetence)
                .HasForeignKey(sc => sc.ProgramCompetenceId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureProgramComponent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramComponent>()
                .HasOne(pc => pc.ProgramModel)
                .WithMany(pm => pm.ProgramComponents)
                .HasForeignKey(pc => pc.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureControlForm(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControlForm>()
                .HasOne(cf => cf.ProgramComponent)
                .WithMany(pc => pc.ControlForms)
                .HasForeignKey(cf => cf.ProgramComponentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureProgramDocument(ModelBuilder modelBuilder)
        {
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
                
                modelBuilder.Entity<DefenseModel>()
                .Property(d => d.DefenseDate)
                .HasConversion(
                    v => v.ToUniversalTime(), 
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc) 
                );

                modelBuilder.Entity<DefenseModel>()
                    .Property(d => d.PublicationDate)
                    .HasConversion(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
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

        private void ConfigureApplyDocument(ModelBuilder modelBuilder)
        {
            // Конфигурация ApplyDocument
            modelBuilder.Entity<ApplyDocumentModel>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).ValueGeneratedOnAdd();
                entity.Property(d => d.Name).IsRequired();
                entity.Property(d => d.Description).IsRequired();
                entity.Property(d => d.Requirements).HasColumnType("jsonb").IsRequired();
                entity.Property(d => d.OriginalsRequired).HasColumnType("jsonb").IsRequired();
            });
        }


        private void ConfigureRoadmap(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoadmapModel>(entity =>
            {

                entity.HasKey(r => r.Id);


                entity.Property(r => r.DataStart)
                      .IsRequired();

                entity.Property(r => r.DataEnd)
                      .IsRequired(false);

                entity.Property(r => r.AdditionalTime)
                      .HasMaxLength(100);

                entity.Property(r => r.Description)
                      .IsRequired(false);

                // Status is computed, not mapped
                entity.Ignore(r => r.Status);
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

            modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

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
