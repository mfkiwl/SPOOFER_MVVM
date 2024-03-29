﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spoofer.Data;

namespace Spoofer.Migrations
{
    [DbContext(typeof(CoordinatesContext))]
    [Migration("20220509112924_initializerize")]
    partial class initializerize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Spoofer.Models.Coordinates", b =>
                {
                    b.Property<string>("CoorfianteId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<double?>("Height")
                        .HasColumnType("float");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumberInOrder")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("CoorfianteId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("Spoofer.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool?>("IsAuthenticated")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Spoofer.Models.Coordinates", b =>
                {
                    b.HasOne("Spoofer.Models.User", "User")
                        .WithMany("Coordinates")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Coordinates_User");
                });
#pragma warning restore 612, 618
        }
    }
}
