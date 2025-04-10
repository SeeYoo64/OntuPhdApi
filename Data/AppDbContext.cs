using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Models.Programs;

namespace OntuPhdApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<ProgramDocument> ProgramDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для ProgramModel
            modelBuilder.Entity<ProgramModel>(entity =>
            {
                entity.ToTable("program");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Degree).HasColumnName("degree");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.NameCode).HasColumnName("name_code");
                entity.Property(e => e.FieldOfStudy).HasColumnName("field_of_study").HasColumnType("jsonb");
                entity.Property(e => e.Speciality).HasColumnName("speciality").HasColumnType("jsonb");
                entity.Property(e => e.Form).HasColumnName("form").HasColumnType("jsonb");
                entity.Property(e => e.Objects).HasColumnName("objects");
                entity.Property(e => e.Directions).HasColumnName("directions").HasColumnType("jsonb");
                entity.Property(e => e.Descriptions).HasColumnName("descriptions");
                entity.Property(e => e.Purpose).HasColumnName("purpose");
                entity.Property(e => e.Years).HasColumnName("years");
                entity.Property(e => e.Credits).HasColumnName("credits");
                entity.Property(e => e.ProgramCharacteristics).HasColumnName("program_characteristics").HasColumnType("jsonb");
                entity.Property(e => e.ProgramCompetence).HasColumnName("program_competence").HasColumnType("jsonb");
                entity.Property(e => e.Results).HasColumnName("results").HasColumnType("jsonb");
                entity.Property(e => e.LinkFaculty).HasColumnName("link_faculty");
                entity.Property(e => e.ProgramDocumentId).HasColumnName("programdocumentid");
                entity.Property(e => e.Accredited).HasColumnName("accredited");

                // Связь с ProgramDocument
                entity.HasOne(p => p.ProgramDocument)
                      .WithMany()
                      .HasForeignKey(p => p.ProgramDocumentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Конфигурация для ProgramDocument
            modelBuilder.Entity<ProgramDocument>(entity =>
            {
                entity.ToTable("programdocuments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.FileName).HasColumnName("filename");
                entity.Property(e => e.FilePath).HasColumnName("filepath");
                entity.Property(e => e.UploadDate).HasColumnName("uploaddate");
                entity.Property(e => e.FileSize).HasColumnName("filesize");
                entity.Property(e => e.ContentType).HasColumnName("contenttype");
            });
        }
    }
}
