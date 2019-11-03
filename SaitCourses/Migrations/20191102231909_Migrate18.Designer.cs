﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SaitCourses.Models;

namespace SaitCourses.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20191102231909_Migrate18")]
    partial class Migrate18
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SaitCourses.Models.Achievement", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("achievementRequirements");

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("achievements");
                });

            modelBuilder.Entity("SaitCourses.Models.AchievementsUsers", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("achievementRequirements");

                    b.Property<int>("achievementid");

                    b.Property<string>("image");

                    b.Property<int>("progressUser");

                    b.Property<string>("userid");

                    b.HasKey("id");

                    b.ToTable("achievementsUsers");
                });

            modelBuilder.Entity("SaitCourses.Models.Basket", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("amount");

                    b.Property<string>("dataOfPurchase");

                    b.Property<string>("nameShirt");

                    b.Property<bool>("purchaseStatus");

                    b.Property<string>("sex");

                    b.Property<int>("shirtid");

                    b.Property<string>("size");

                    b.Property<string>("userId");

                    b.HasKey("id");

                    b.HasIndex("shirtid");

                    b.HasIndex("userId");

                    b.ToTable("baskets");
                });

            modelBuilder.Entity("SaitCourses.Models.Image", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description");

                    b.Property<string>("image");

                    b.Property<string>("name");

                    b.Property<int>("tShirtId");

                    b.HasKey("id");

                    b.ToTable("images");
                });

            modelBuilder.Entity("SaitCourses.Models.Rating", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("shirtid");

                    b.Property<int>("userId");

                    b.Property<string>("userId1");

                    b.Property<int>("value");

                    b.HasKey("id");

                    b.HasIndex("shirtid");

                    b.HasIndex("userId1");

                    b.ToTable("ratings");
                });

            modelBuilder.Entity("SaitCourses.Models.Shirt", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Sex");

                    b.Property<string>("createDate");

                    b.Property<string>("description");

                    b.Property<string>("image");

                    b.Property<string>("name");

                    b.Property<int>("themeId");

                    b.Property<string>("userId");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("tshirts");
                });

            modelBuilder.Entity("SaitCourses.Models.Tag", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("SaitCourses.Models.TagInTShirt", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("shirtid");

                    b.Property<int>("tagid");

                    b.HasKey("id");

                    b.HasIndex("shirtid");

                    b.HasIndex("tagid");

                    b.ToTable("tagInTShirts");
                });

            modelBuilder.Entity("SaitCourses.Models.Theme", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("themes");
                });

            modelBuilder.Entity("SaitCourses.Models.Topic", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nameTopic");

                    b.HasKey("id");

                    b.ToTable("topics");
                });

            modelBuilder.Entity("SaitCourses.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("Sex");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SaitCourses.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SaitCourses.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SaitCourses.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SaitCourses.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SaitCourses.Models.Basket", b =>
                {
                    b.HasOne("SaitCourses.Models.Shirt", "shirt")
                        .WithMany()
                        .HasForeignKey("shirtid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SaitCourses.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("SaitCourses.Models.Rating", b =>
                {
                    b.HasOne("SaitCourses.Models.Shirt", "shirt")
                        .WithMany()
                        .HasForeignKey("shirtid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SaitCourses.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId1");
                });

            modelBuilder.Entity("SaitCourses.Models.Shirt", b =>
                {
                    b.HasOne("SaitCourses.Models.User", "users")
                        .WithMany()
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("SaitCourses.Models.TagInTShirt", b =>
                {
                    b.HasOne("SaitCourses.Models.Shirt", "shirt")
                        .WithMany()
                        .HasForeignKey("shirtid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SaitCourses.Models.Tag", "tag")
                        .WithMany()
                        .HasForeignKey("tagid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
