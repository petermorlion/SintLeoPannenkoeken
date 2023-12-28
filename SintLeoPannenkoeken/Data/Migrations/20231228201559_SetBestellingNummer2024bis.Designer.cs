﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SintLeoPannenkoeken.Data;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231228201559_SetBestellingNummer2024bis")]
    partial class SetBestellingNummer2024bis
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Bestelling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AantalPakken")
                        .HasColumnType("int");

                    b.Property<int>("BestellingNummer")
                        .HasColumnType("int");

                    b.Property<bool>("Betaald")
                        .HasColumnType("bit");

                    b.Property<string>("Bus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IngaveDatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("LidId")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Opmerkingen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScoutsjaarId")
                        .HasColumnType("int");

                    b.Property<int>("StraatId")
                        .HasColumnType("int");

                    b.Property<int>("TakId")
                        .HasColumnType("int");

                    b.Property<string>("Telefoon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LidId");

                    b.HasIndex("ScoutsjaarId");

                    b.HasIndex("StraatId");

                    b.HasIndex("TakId");

                    b.ToTable("Bestellingen");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Bestuurder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Achternaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bestuurders");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Lid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Achternaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Functie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TakId")
                        .HasColumnType("int");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TakId");

                    b.ToTable("Leden");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Ronde", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BestuurderId")
                        .HasColumnType("int");

                    b.Property<int>("ScoutsjaarId")
                        .HasColumnType("int");

                    b.Property<int>("ZoneId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BestuurderId");

                    b.HasIndex("ScoutsjaarId");

                    b.HasIndex("ZoneId");

                    b.ToTable("Rondes");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Scoutsjaar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Begin")
                        .HasColumnType("int");

                    b.Property<int>("Einde")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Begin")
                        .IsUnique();

                    b.ToTable("Scoutsjaren");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Straat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Gemeente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nummer")
                        .HasColumnType("int");

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostNummer")
                        .HasColumnType("int");

                    b.Property<int>("ZoneId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("Straten");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.StreefCijfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<int?>("ScoutsjaarId")
                        .HasColumnType("int");

                    b.Property<int>("TakId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ScoutsjaarId");

                    b.HasIndex("TakId");

                    b.ToTable("StreefCijfers");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Tak", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Afkorting")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Takken");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bestuurder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gemeente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KaartNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PostNummer")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Bestelling", b =>
                {
                    b.HasOne("SintLeoPannenkoeken.Models.Lid", "Lid")
                        .WithMany("Bestellingen")
                        .HasForeignKey("LidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SintLeoPannenkoeken.Models.Scoutsjaar", null)
                        .WithMany("Bestellingen")
                        .HasForeignKey("ScoutsjaarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SintLeoPannenkoeken.Models.Straat", "Straat")
                        .WithMany()
                        .HasForeignKey("StraatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SintLeoPannenkoeken.Models.Tak", "Tak")
                        .WithMany()
                        .HasForeignKey("TakId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lid");

                    b.Navigation("Straat");

                    b.Navigation("Tak");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Lid", b =>
                {
                    b.HasOne("SintLeoPannenkoeken.Models.Tak", "Tak")
                        .WithMany("Leden")
                        .HasForeignKey("TakId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tak");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Ronde", b =>
                {
                    b.HasOne("SintLeoPannenkoeken.Models.Bestuurder", "Bestuurder")
                        .WithMany()
                        .HasForeignKey("BestuurderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SintLeoPannenkoeken.Models.Scoutsjaar", null)
                        .WithMany("Rondes")
                        .HasForeignKey("ScoutsjaarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SintLeoPannenkoeken.Models.Zone", "Zone")
                        .WithMany()
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bestuurder");

                    b.Navigation("Zone");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Straat", b =>
                {
                    b.HasOne("SintLeoPannenkoeken.Models.Zone", "Zone")
                        .WithMany("Straten")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Zone");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.StreefCijfer", b =>
                {
                    b.HasOne("SintLeoPannenkoeken.Models.Scoutsjaar", null)
                        .WithMany("StreefCijfers")
                        .HasForeignKey("ScoutsjaarId");

                    b.HasOne("SintLeoPannenkoeken.Models.Tak", "Tak")
                        .WithMany()
                        .HasForeignKey("TakId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tak");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Lid", b =>
                {
                    b.Navigation("Bestellingen");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Scoutsjaar", b =>
                {
                    b.Navigation("Bestellingen");

                    b.Navigation("Rondes");

                    b.Navigation("StreefCijfers");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Tak", b =>
                {
                    b.Navigation("Leden");
                });

            modelBuilder.Entity("SintLeoPannenkoeken.Models.Zone", b =>
                {
                    b.Navigation("Straten");
                });
#pragma warning restore 612, 618
        }
    }
}
