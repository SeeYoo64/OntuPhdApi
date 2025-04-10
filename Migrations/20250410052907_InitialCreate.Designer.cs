﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;

#nullable disable

namespace OntuPhdApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250410052907_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OntuPhdApi.Models.Programs.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ProgramModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProgramModelId");

                    b.ToTable("Job");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramComponent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ComponentCredits")
                        .HasColumnType("integer");

                    b.Property<int?>("ComponentHours")
                        .HasColumnType("integer");

                    b.Property<string>("ComponentName")
                        .HasColumnType("text");

                    b.Property<string>("ComponentType")
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("ControlForm")
                        .HasColumnType("text[]");

                    b.Property<int?>("ProgramId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProgramModelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProgramModelId");

                    b.ToTable("ProgramComponent");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ContentType");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FileName");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FilePath");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("FileSize");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Programdocuments", (string)null);
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Accredited")
                        .HasColumnType("boolean")
                        .HasColumnName("Accredited");

                    b.Property<int?>("Credits")
                        .HasColumnType("integer")
                        .HasColumnName("Credits");

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Degree");

                    b.Property<string>("Descriptions")
                        .HasColumnType("text")
                        .HasColumnName("Descriptions");

                    b.Property<List<string>>("Directions")
                        .HasColumnType("jsonb")
                        .HasColumnName("Directions");

                    b.Property<FieldOfStudy>("FieldOfStudy")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Field_Of_Study");

                    b.Property<List<string>>("Form")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Form");

                    b.Property<string>("LinkFaculty")
                        .HasColumnType("jsonb")
                        .HasColumnName("Link_Faculty");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<string>("NameCode")
                        .HasColumnType("text")
                        .HasColumnName("Name_Code");

                    b.Property<string>("Objects")
                        .HasColumnType("text")
                        .HasColumnName("Objects");

                    b.Property<ProgramCharacteristics>("ProgramCharacteristics")
                        .HasColumnType("jsonb")
                        .HasColumnName("Program_Characteristics");

                    b.Property<ProgramCompetence>("ProgramCompetence")
                        .HasColumnType("jsonb")
                        .HasColumnName("Program_Competence");

                    b.Property<int?>("ProgramDocumentId")
                        .HasColumnType("integer")
                        .HasColumnName("ProgramDocumentId");

                    b.Property<string>("Purpose")
                        .HasColumnType("text")
                        .HasColumnName("Purpose");

                    b.Property<List<string>>("Results")
                        .HasColumnType("jsonb")
                        .HasColumnName("Results");

                    b.Property<Speciality>("Speciality")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Speciality");

                    b.Property<int?>("Years")
                        .HasColumnType("integer")
                        .HasColumnName("Years");

                    b.HasKey("Id");

                    b.HasIndex("ProgramDocumentId");

                    b.ToTable("Program", (string)null);
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.Job", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramModel", null)
                        .WithMany("Jobs")
                        .HasForeignKey("ProgramModelId");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramComponent", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramModel", null)
                        .WithMany("Components")
                        .HasForeignKey("ProgramModelId");
                });

            modelBuilder.Entity("OntuPhdApi.Models.Programs.ProgramModel", b =>
                {
                    b.HasOne("OntuPhdApi.Models.Programs.ProgramDocument", "ProgramDocument")
                        .WithMany()
                        .HasForeignKey("ProgramDocumentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ProgramDocument");
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
