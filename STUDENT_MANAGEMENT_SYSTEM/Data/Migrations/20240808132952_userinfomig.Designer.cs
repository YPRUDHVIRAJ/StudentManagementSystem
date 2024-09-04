﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StudentManagementSystem.Context;

#nullable disable

namespace StudentManagementSystem.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240808132952_userinfomig")]
    partial class userinfomig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StudentManagementSystem.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CourseID"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("integer");

                    b.HasKey("CourseID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StudentID"));

                    b.Property<int>("CourseID")
                        .HasColumnType("integer");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("integer");

                    b.Property<byte[]>("FingerprintData")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("YearOfJoiningID")
                        .HasColumnType("integer");

                    b.HasKey("StudentID");

                    b.HasIndex("CourseID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("YearOfJoiningID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.YearOfJoining", b =>
                {
                    b.Property<int>("YearOfJoiningID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("YearOfJoiningID"));

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("YearOfJoiningID");

                    b.ToTable("YearsOfJoining");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Course", b =>
                {
                    b.HasOne("StudentManagementSystem.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Student", b =>
                {
                    b.HasOne("StudentManagementSystem.Models.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagementSystem.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagementSystem.Models.YearOfJoining", "YearOfJoining")
                        .WithMany("Students")
                        .HasForeignKey("YearOfJoiningID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Department");

                    b.Navigation("YearOfJoining");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Course", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.Department", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("StudentManagementSystem.Models.YearOfJoining", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
