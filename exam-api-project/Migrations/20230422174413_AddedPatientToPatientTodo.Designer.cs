﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using exam_api_project.Repositories.Context;

#nullable disable

namespace exam_api_project.Migrations
{
    [DbContext(typeof(ExamContext))]
    [Migration("20230422174413_AddedPatientToPatientTodo")]
    partial class AddedPatientToPatientTodo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("exam_api_project.models.Entities.DepartmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.DeviceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(264)
                        .HasColumnType("character varying(264)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserModelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.MedicineModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ActiveSubstance")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<decimal>("PricePrMg")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientJournalModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("PatientModelId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PatientModelId");

                    b.ToTable("PatientJournals");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientMedicineModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MedicineModelId")
                        .HasColumnType("integer");

                    b.Property<int>("PatientModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MedicineModelId");

                    b.HasIndex("PatientModelId");

                    b.ToTable("PatientMedicines");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SocialSecurityNumber")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientTodoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Done")
                        .HasColumnType("boolean");

                    b.Property<int>("PatientMedicineModelId")
                        .HasColumnType("integer");

                    b.Property<int>("PatientModelId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PlannedTimeAtDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PatientMedicineModelId");

                    b.HasIndex("PatientModelId");

                    b.HasIndex("UserModelId");

                    b.ToTable("PatientTodos");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DepartmentModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentModelId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.DeviceModel", b =>
                {
                    b.HasOne("exam_api_project.models.Entities.UserModel", "User")
                        .WithMany("Devices")
                        .HasForeignKey("UserModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientJournalModel", b =>
                {
                    b.HasOne("exam_api_project.models.Entities.PatientModel", "Patient")
                        .WithMany("PatientJournals")
                        .HasForeignKey("PatientModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientMedicineModel", b =>
                {
                    b.HasOne("exam_api_project.models.Entities.MedicineModel", "Medicine")
                        .WithMany()
                        .HasForeignKey("MedicineModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("exam_api_project.models.Entities.PatientModel", "Patient")
                        .WithMany("PatientMedicines")
                        .HasForeignKey("PatientModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientTodoModel", b =>
                {
                    b.HasOne("exam_api_project.models.Entities.PatientMedicineModel", "PatientMedicine")
                        .WithMany("Todos")
                        .HasForeignKey("PatientMedicineModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("exam_api_project.models.Entities.PatientModel", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("exam_api_project.models.Entities.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserModelId");

                    b.Navigation("Patient");

                    b.Navigation("PatientMedicine");

                    b.Navigation("User");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.UserModel", b =>
                {
                    b.HasOne("exam_api_project.models.Entities.DepartmentModel", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentModelId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.DepartmentModel", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientMedicineModel", b =>
                {
                    b.Navigation("Todos");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.PatientModel", b =>
                {
                    b.Navigation("PatientJournals");

                    b.Navigation("PatientMedicines");
                });

            modelBuilder.Entity("exam_api_project.models.Entities.UserModel", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
