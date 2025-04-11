using System.Collections.Generic;
using System.Reflection.Emit;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureAuthEntities(modelBuilder);

            ConfigureProgramModel(modelBuilder);

            ConfigureProgramDocument(modelBuilder);

        }

        private void ConfigureProgramModel(ModelBuilder modelBuilder)
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
        }

        private void ConfigureProgramDocument(ModelBuilder modelBuilder)
        {
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
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
                entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
                entity.Property(e => e.Image).HasColumnName("image");
            });
        }

        private void ConfigureAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Type).HasColumnName("type").IsRequired().HasMaxLength(255);
                entity.Property(e => e.Provider).HasColumnName("provider").IsRequired().HasMaxLength(255);
                entity.Property(e => e.ProviderAccountId).HasColumnName("provider_account_id").IsRequired().HasMaxLength(255);
                entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
                entity.Property(e => e.AccessToken).HasColumnName("access_token");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.IdToken).HasColumnName("id_token");
                entity.Property(e => e.Scope).HasColumnName("scope");
                entity.Property(e => e.SessionState).HasColumnName("session_state");
                entity.Property(e => e.TokenType).HasColumnName("token_type");
                entity.HasOne(a => a.User)
                      .WithMany(u => u.Accounts)
                      .HasForeignKey(a => a.UserId)
                      .HasConstraintName("fk_auth_accounts_user_id")
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(a => new { a.Provider, a.ProviderAccountId })
                      .HasDatabaseName("idx_auth_accounts_provider_provider_account_id")
                      .IsUnique();
            });
        }

        private void ConfigureSession(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Expires).HasColumnName("expires").IsRequired();
                entity.Property(e => e.SessionToken).HasColumnName("session_token").IsRequired().HasMaxLength(255);
                entity.HasOne(s => s.User);
          //            .WithMany(u => s.Sessions)
         //             .HasForeignKey(s => s.UserId)
         //             .HasConstraintName("fk_auth_sessions_user_id")
        //              .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(s => s.SessionToken)
                      .HasDatabaseName("idx_auth_sessions_session_token")
                      .IsUnique();
            });
        }

        private void ConfigureVerificationToken(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VerificationToken>(entity =>
            {
                entity.ToTable("verification_token");
                entity.HasKey(vt => new { vt.Identifier, vt.Token });
                entity.Property(vt => vt.Identifier).HasColumnName("identifier").IsRequired();
                entity.Property(vt => vt.Token).HasColumnName("token").IsRequired();
                entity.Property(vt => vt.Expires).HasColumnName("expires").IsRequired();
            });
        }


    }
}
