﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Zindagi.Infra.Data;

namespace Zindagi.Infra.Data.Migrations
{
    [DbContext(typeof(ZindagiDbContext))]
    partial class ZindagiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("zg")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.HasSequence("global")
                .IncrementsBy(10);

            modelBuilder.Entity("Zindagi.Domain.RequestsAggregate.BloodRequest", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("blood_request_id")
                        .HasAnnotation("Npgsql:HiLoSequenceName", "global")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<long?>("AssigneeId")
                        .HasColumnType("bigint")
                        .HasColumnName("assignee_id");

                    b.Property<int>("BloodGroup")
                        .HasColumnType("integer")
                        .HasColumnName("blood_group");

                    b.Property<int>("DonationType")
                        .HasColumnType("integer")
                        .HasColumnName("donation_type");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("patient_name");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<double>("QuantityInUnits")
                        .HasColumnType("double precision")
                        .HasColumnName("quantity_in_units");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("reason");

                    b.Property<long>("RequestorId")
                        .HasColumnType("bigint")
                        .HasColumnName("requestor_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_blood_requests");

                    b.HasIndex("AssigneeId")
                        .HasDatabaseName("ix_blood_requests_assignee_id");

                    b.HasIndex("RequestorId")
                        .HasDatabaseName("ix_blood_requests_requestor_id");

                    b.ToTable("blood_requests");
                });

            modelBuilder.Entity("Zindagi.Domain.UserAggregate.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id")
                        .HasAnnotation("Npgsql:HiLoSequenceName", "global")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("AlternateMobileNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("alternate_mobile_number");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("auth_id");

                    b.Property<int>("BloodGroup")
                        .HasColumnType("integer")
                        .HasColumnName("blood_group");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<int>("IsActive")
                        .HasColumnType("integer")
                        .HasColumnName("is_active");

                    b.Property<int>("IsDonor")
                        .HasColumnType("integer")
                        .HasColumnName("is_donor");

                    b.Property<int>("IsEmailVerified")
                        .HasColumnType("integer")
                        .HasColumnName("is_email_verified");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("middle_name");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mobile_number");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("picture_url");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasAlternateKey("AuthId")
                        .HasName("ak_users_auth_id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Zindagi.Domain.RequestsAggregate.BloodRequest", b =>
                {
                    b.HasOne("Zindagi.Domain.UserAggregate.User", "Assignee")
                        .WithMany("AssignedBloodRequests")
                        .HasForeignKey("AssigneeId")
                        .HasConstraintName("fk_blood_requests_users_assignee_id");

                    b.HasOne("Zindagi.Domain.UserAggregate.User", "Requestor")
                        .WithMany("CreatedBloodRequests")
                        .HasForeignKey("RequestorId")
                        .HasConstraintName("fk_blood_requests_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Requestor");
                });

            modelBuilder.Entity("Zindagi.Domain.UserAggregate.User", b =>
                {
                    b.Navigation("AssignedBloodRequests");

                    b.Navigation("CreatedBloodRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
