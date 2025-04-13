﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;

#nullable disable

namespace OntuPhdApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OntuPhdApi.Models.Authorization.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<long?>("ExpiresAt")
                        .HasColumnType("bigint");

                    b.Property<string>("IdToken")
                        .HasColumnType("text");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ProviderAccountId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<string>("Scope")
                        .HasColumnType("text");

                    b.Property<string>("SessionState")
                        .HasColumnType("text");

                    b.Property<string>("TokenType")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Authorization.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("EmailVerified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("MustChangePassword")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Authorization.VerificationToken", b =>
                {
                    b.Property<string>("Identifier")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Identifier", "Token");

                    b.ToTable("VerificationTokens");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Defense.DefenseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("CandidateNameSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DefenseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DefenseTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<DefenseFile>>("Files")
                        .HasColumnType("jsonb");

                    b.Property<List<CompositionOfRada>>("Members")
                        .HasColumnType("jsonb");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Placeholder")
                        .HasColumnType("text");

                    b.Property<int>("ProgramId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.PrimitiveCollection<List<string>>("ScienceTeachers")
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("ProgramId");

                    b.ToTable("Defenses");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Employees.EmployeeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "code");

                    b.Property<int>("ProgramId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProgramModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.HasKey("Id");

                    b.HasIndex("ProgramModelId");

                    b.ToTable("Jobs");

                    b.HasAnnotation("Relational:JsonPropertyName", "jobs");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramComponent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ComponentCredits")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "componentCredits");

                    b.Property<int?>("ComponentHours")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "componentHours");

                    b.Property<string>("ComponentName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "componentName");

                    b.Property<string>("ComponentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "componentType");

                    b.PrimitiveCollection<List<string>>("ControlForm")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "controlForm");

                    b.Property<int?>("ProgramId")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "programId");

                    b.HasKey("Id");

                    b.HasIndex("ProgramId");

                    b.ToTable("ProgramComponents");

                    b.HasAnnotation("Relational:JsonPropertyName", "components");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ProgramDocuments");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Accredited")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "accredited");

                    b.Property<int?>("Credits")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "credits");

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "degree");

                    b.Property<string>("Descriptions")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "descriptions");

                    b.Property<List<string>>("Directions")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "directions");

                    b.Property<FieldOfStudy>("FieldOfStudy")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "fieldOfStudy");

                    b.Property<List<string>>("Form")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "form");

                    b.Property<string>("LinkFaculty")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "linkFaculty");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("NameCode")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nameCode");

                    b.Property<string>("Objects")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "objects");

                    b.Property<ProgramCharacteristics>("ProgramCharacteristics")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "programCharacteristics");

                    b.Property<ProgramCompetence>("ProgramCompetence")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "programCompetence");

                    b.Property<int?>("ProgramDocumentId")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "programDocumentId");

                    b.Property<int?>("ProgramDocumentId1")
                        .HasColumnType("integer");

                    b.Property<string>("Purpose")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "purpose");

                    b.Property<List<string>>("Results")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "results");

                    b.Property<Speciality>("Speciality")
                        .HasColumnType("jsonb")
                        .HasAnnotation("Relational:JsonPropertyName", "speciality");

                    b.Property<int?>("Years")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "years");

                    b.HasKey("Id");

                    b.HasIndex("ProgramDocumentId")
                        .IsUnique();

                    b.HasIndex("ProgramDocumentId1")
                        .IsUnique();

                    b.ToTable("Programs");

                    b.HasAnnotation("Relational:JsonPropertyName", "programDocument");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Authorization.Account", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Authorization.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Defense.DefenseModel", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramModel", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Program");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.Job", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramModel", null)
                        .WithMany("Jobs")
                        .HasForeignKey("ProgramModelId");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramComponent", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramModel", "ProgramModel")
                        .WithMany("Components")
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ProgramModel");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramModel", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramDocument", "ProgramDocument")
                        .WithOne()
                        .HasForeignKey("OntuPhdApi.Models.Programs.ProgramModel", "ProgramDocumentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("OntuPhdApi.Models.Programs.ProgramDocument", null)
                        .WithOne("Program")
                        .HasForeignKey("OntuPhdApi.Models.Programs.ProgramModel", "ProgramDocumentId1");

                    b.Navigation("ProgramDocument");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Authorization.User", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramDocument", b =>
                {
                    b.Navigation("Program");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramModel", b =>
                {
                    b.Navigation("Components");

                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
