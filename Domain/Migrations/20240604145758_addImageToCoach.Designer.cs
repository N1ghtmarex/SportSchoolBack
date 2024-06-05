﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240604145758_addImageToCoach")]
    partial class addImageToCoach
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClientSection", b =>
                {
                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("section_id");

                    b.HasKey("ClientId", "SectionId")
                        .HasName("pk_client_section");

                    b.HasIndex("SectionId")
                        .HasDatabaseName("ix_client_section_section_id");

                    b.ToTable("ClientSection", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid")
                        .HasColumnName("external_id");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("text")
                        .HasColumnName("image_file_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_clients");

                    b.ToTable("clients", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Coach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("EducationForm")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("education_form");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid")
                        .HasColumnName("external_id");

                    b.Property<string>("Faculty")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("faculty");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("text")
                        .HasColumnName("image_file_name");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("institution");

                    b.Property<string>("Job")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job");

                    b.Property<string>("JobPeriod")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_period");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_title");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("qualification");

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("speciality");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_coachs");

                    b.ToTable("coachs", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.IndividualEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<Guid>("CoachId")
                        .HasColumnType("uuid")
                        .HasColumnName("coach_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid")
                        .HasColumnName("room_id");

                    b.Property<Guid>("SportId")
                        .HasColumnType("uuid")
                        .HasColumnName("sport_id");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("pk_individual_events");

                    b.HasIndex("ClientId")
                        .HasDatabaseName("ix_individual_events_client_id");

                    b.HasIndex("CoachId")
                        .HasDatabaseName("ix_individual_events_coach_id");

                    b.HasIndex("RoomId")
                        .HasDatabaseName("ix_individual_events_room_id");

                    b.HasIndex("SportId")
                        .HasDatabaseName("ix_individual_events_sport_id");

                    b.ToTable("individual_events", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_rooms");

                    b.ToTable("rooms", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CoachId")
                        .HasColumnType("uuid")
                        .HasColumnName("coach_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid")
                        .HasColumnName("room_id");

                    b.Property<Guid>("SportId")
                        .HasColumnType("uuid")
                        .HasColumnName("sport_id");

                    b.HasKey("Id")
                        .HasName("pk_sections");

                    b.HasIndex("CoachId")
                        .HasDatabaseName("ix_sections_coach_id");

                    b.HasIndex("RoomId")
                        .HasDatabaseName("ix_sections_room_id");

                    b.HasIndex("SportId")
                        .HasDatabaseName("ix_sections_sport_id");

                    b.ToTable("sections", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.SectionEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer")
                        .HasColumnName("day_of_week");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("end_time");

                    b.Property<DateOnly>("Period")
                        .HasColumnType("date")
                        .HasColumnName("period");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("section_id");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("start_time");

                    b.HasKey("Id")
                        .HasName("pk_section_events");

                    b.HasIndex("SectionId")
                        .HasDatabaseName("ix_section_events_section_id");

                    b.ToTable("section_events", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Sport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_sports");

                    b.ToTable("sports", (string)null);
                });

            modelBuilder.Entity("ClientSection", b =>
                {
                    b.HasOne("Domain.Entities.Client", null)
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_client_section_clients_client_id");

                    b.HasOne("Domain.Entities.Section", null)
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_client_section_sections_section_id");
                });

            modelBuilder.Entity("Domain.Entities.IndividualEvent", b =>
                {
                    b.HasOne("Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .HasConstraintName("fk_individual_events_clients_client_id");

                    b.HasOne("Domain.Entities.Coach", "Coach")
                        .WithMany()
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_individual_events_coachs_coach_id");

                    b.HasOne("Domain.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_individual_events_rooms_room_id");

                    b.HasOne("Domain.Entities.Sport", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_individual_events_sports_sport_id");

                    b.Navigation("Client");

                    b.Navigation("Coach");

                    b.Navigation("Room");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("Domain.Entities.Section", b =>
                {
                    b.HasOne("Domain.Entities.Coach", "Coach")
                        .WithMany("Section")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sections_coachs_coach_id");

                    b.HasOne("Domain.Entities.Room", "Room")
                        .WithMany("Section")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sections_rooms_room_id");

                    b.HasOne("Domain.Entities.Sport", "Sport")
                        .WithMany("Section")
                        .HasForeignKey("SportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sections_sports_sport_id");

                    b.Navigation("Coach");

                    b.Navigation("Room");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("Domain.Entities.SectionEvent", b =>
                {
                    b.HasOne("Domain.Entities.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_section_events_sections_section_id");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Domain.Entities.Coach", b =>
                {
                    b.Navigation("Section");
                });

            modelBuilder.Entity("Domain.Entities.Room", b =>
                {
                    b.Navigation("Section");
                });

            modelBuilder.Entity("Domain.Entities.Sport", b =>
                {
                    b.Navigation("Section");
                });
#pragma warning restore 612, 618
        }
    }
}
