﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityAccessControl;

#nullable disable

namespace UniversityAccessControl.Migrations
{
    [DbContext(typeof(AccessControlDbContext))]
    partial class AccessControlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("GroupSubject", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubjectsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GroupsId", "SubjectsId");

                    b.HasIndex("SubjectsId");

                    b.ToTable("GroupSubject");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.AccessLogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("AccessedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PassageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PassageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("AccessLogEntries");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Passage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AreaId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Passages");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Allow")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AreaId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PassageId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PassageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("GroupSubject", b =>
                {
                    b.HasOne("UniversityAccessControl.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityAccessControl.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniversityAccessControl.Models.AccessLogEntry", b =>
                {
                    b.HasOne("UniversityAccessControl.Models.Passage", "Passage")
                        .WithMany()
                        .HasForeignKey("PassageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityAccessControl.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passage");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Passage", b =>
                {
                    b.HasOne("UniversityAccessControl.Models.Area", "Area")
                        .WithMany("Passages")
                        .HasForeignKey("AreaId");

                    b.Navigation("Area");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Rule", b =>
                {
                    b.HasOne("UniversityAccessControl.Models.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId");

                    b.HasOne("UniversityAccessControl.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("UniversityAccessControl.Models.Passage", "Passage")
                        .WithMany()
                        .HasForeignKey("PassageId");

                    b.HasOne("UniversityAccessControl.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.Navigation("Area");

                    b.Navigation("Group");

                    b.Navigation("Passage");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("UniversityAccessControl.Models.Area", b =>
                {
                    b.Navigation("Passages");
                });
#pragma warning restore 612, 618
        }
    }
}
