﻿// <auto-generated />
using System;
using GymMembershipAPI.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymMembershipAPI.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GymMembershipAPI.Domain.GymMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GymOwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubscriptionEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubscriptionStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GymOwnerId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymOwner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MonthlyEarnings")
                        .HasColumnType("decimal(8,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SuperAdminId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SuperAdminId");

                    b.ToTable("GymOwner");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymSuperAdmin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MonthlyPercentage")
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GymSuperAdmins");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.HealthyTip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HealthyTip");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("GymMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GymOwerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("GymSuperAdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GymMemberId");

                    b.HasIndex("GymOwerId");

                    b.HasIndex("GymSuperAdminId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "SuperAdmin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "GymOwner"
                        },
                        new
                        {
                            Id = 3,
                            Name = "GymMembers"
                        });
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isverified")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TokenExpiration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerificationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerifiedAt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymMember", b =>
                {
                    b.HasOne("GymMembershipAPI.Domain.GymOwner", "GymOwner")
                        .WithMany("GymMembers")
                        .HasForeignKey("GymOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GymOwner");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymOwner", b =>
                {
                    b.HasOne("GymMembershipAPI.Domain.GymSuperAdmin", "GymSuperAdmin")
                        .WithMany("Owners")
                        .HasForeignKey("SuperAdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GymSuperAdmin");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.Payment", b =>
                {
                    b.HasOne("GymMembershipAPI.Domain.GymMember", "GymMember")
                        .WithMany("Payments")
                        .HasForeignKey("GymMemberId");

                    b.HasOne("GymMembershipAPI.Domain.GymOwner", "GymOwner")
                        .WithMany("Payments")
                        .HasForeignKey("GymOwerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymMembershipAPI.Domain.GymSuperAdmin", "GymSuperAdmin")
                        .WithMany()
                        .HasForeignKey("GymSuperAdminId");

                    b.Navigation("GymMember");

                    b.Navigation("GymOwner");

                    b.Navigation("GymSuperAdmin");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.UserRole", b =>
                {
                    b.HasOne("GymMembershipAPI.Domain.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymMembershipAPI.Domain.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymMember", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymOwner", b =>
                {
                    b.Navigation("GymMembers");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.GymSuperAdmin", b =>
                {
                    b.Navigation("Owners");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("GymMembershipAPI.Domain.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
