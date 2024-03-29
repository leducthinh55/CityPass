﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(CityPassContext))]
    [Migration("20210316022044_update-user-3")]
    partial class updateuser3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Entities.Attraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000)
                        .IsUnicode(true);

                    b.Property<bool>("IsTemporarityClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CityId");

                    b.ToTable("Attraction");
                });

            modelBuilder.Entity("Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Core.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("City");
                });

            modelBuilder.Entity("Core.Entities.Collection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxConstrain")
                        .HasColumnType("int");

                    b.Property<Guid>("PassId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PassId");

                    b.ToTable("Collection");
                });

            modelBuilder.Entity("Core.Entities.Pass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ChildrenPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000)
                        .IsUnicode(true);

                    b.Property<int>("ExpireDuration")
                        .HasColumnType("int");

                    b.Property<bool>("IsSelling")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Pass");
                });

            modelBuilder.Entity("Core.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UsedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserPassId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TicketTypeId");

                    b.HasIndex("UserPassId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("Core.Entities.TicketType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("AdultPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AtrractionId")
                        .HasColumnType("int");

                    b.Property<decimal?>("ChildrenPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200)
                        .IsUnicode(true);

                    b.Property<string>("UrlImage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AtrractionId");

                    b.ToTable("TicketType");
                });

            modelBuilder.Entity("Core.Entities.TicketTypeInCollection", b =>
                {
                    b.Property<Guid>("TicketTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TicketTypeId", "CollectionId");

                    b.HasIndex("CollectionId");

                    b.ToTable("TicketTypeInCollection");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Property<string>("Uid")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Uid");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Core.Entities.UserPass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BoughtAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Feedback")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsChildren")
                        .HasColumnType("bit");

                    b.Property<Guid>("PassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PriceWhenBought")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<DateTime>("WillExpireAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PassId");

                    b.ToTable("UserPass");
                });

            modelBuilder.Entity("Core.Entities.WorkingTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AtrractionId")
                        .HasColumnType("int");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<double>("EndTime")
                        .HasColumnType("float");

                    b.Property<double>("StartTime")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AtrractionId");

                    b.ToTable("WorkingTime");
                });

            modelBuilder.Entity("Core.Entities.Attraction", b =>
                {
                    b.HasOne("Core.Entities.Category", "Category")
                        .WithMany("Atrractions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.City", "City")
                        .WithMany("Atrractions")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.Collection", b =>
                {
                    b.HasOne("Core.Entities.Pass", "Passes")
                        .WithMany("Collections")
                        .HasForeignKey("PassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.Ticket", b =>
                {
                    b.HasOne("Core.Entities.TicketType", "TicketType")
                        .WithMany("Tickets")
                        .HasForeignKey("TicketTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.UserPass", "UserPass")
                        .WithMany("UsingTickets")
                        .HasForeignKey("UserPassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.TicketType", b =>
                {
                    b.HasOne("Core.Entities.Attraction", "Atrraction")
                        .WithMany("TicketTypes")
                        .HasForeignKey("AtrractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.TicketTypeInCollection", b =>
                {
                    b.HasOne("Core.Entities.Collection", "Collection")
                        .WithMany("TicketTypeInCollections")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.TicketType", "TicketType")
                        .WithMany("TicketTypeInCollections")
                        .HasForeignKey("TicketTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.UserPass", b =>
                {
                    b.HasOne("Core.Entities.Pass", "Pass")
                        .WithMany("UserPasses")
                        .HasForeignKey("PassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.WorkingTime", b =>
                {
                    b.HasOne("Core.Entities.Attraction", "Atrraction")
                        .WithMany("WorkingTimes")
                        .HasForeignKey("AtrractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
