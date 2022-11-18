﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Randomizer.Shared.Models;

namespace Randomizer.Shared.Migrations
{
    [DbContext(typeof(RandomizerContext))]
    partial class RandomizerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Randomizer.Shared.Models.GeneratedRom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("GeneratorVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<string>("RomPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Seed")
                        .HasColumnType("TEXT");

                    b.Property<string>("Settings")
                        .HasColumnType("TEXT");

                    b.Property<string>("SpoilerPath")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("GeneratedRoms");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerBossState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BossName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Defeated")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerBossStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerDungeonState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Cleared")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasManuallyClearedTreasure")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("MarkedMedallion")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MarkedReward")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("RemainingTreasure")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("RequiredMedallion")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Reward")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerDungeonStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerHistoryEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsImportant")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsUndone")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocationName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ObjectName")
                        .HasColumnType("TEXT");

                    b.Property<double>("Time")
                        .HasColumnType("REAL");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerHistoryEvents");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerItemState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrackingState")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerItemStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerLocationState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Cleared")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("Item")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("MarkedItem")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerLocationStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerMarkedLocation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemName")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerMarkedLocations");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerRegionState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("Medallion")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Reward")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackerStateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TypeName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TrackerStateId");

                    b.ToTable("TrackerRegionStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PercentageCleared")
                        .HasColumnType("INTEGER");

                    b.Property<double>("SecondsElapsed")
                        .HasColumnType("REAL");

                    b.Property<DateTimeOffset>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UpdatedDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TrackerStates");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.GeneratedRom", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany()
                        .HasForeignKey("TrackerStateId");

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerBossState", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("BossStates")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerDungeonState", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("DungeonStates")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerHistoryEvent", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("History")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerItemState", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("ItemStates")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerLocationState", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("LocationStates")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerMarkedLocation", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("MarkedLocations")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerRegionState", b =>
                {
                    b.HasOne("Randomizer.Shared.Models.TrackerState", "TrackerState")
                        .WithMany("RegionStates")
                        .HasForeignKey("TrackerStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TrackerState");
                });

            modelBuilder.Entity("Randomizer.Shared.Models.TrackerState", b =>
                {
                    b.Navigation("BossStates");

                    b.Navigation("DungeonStates");

                    b.Navigation("History");

                    b.Navigation("ItemStates");

                    b.Navigation("LocationStates");

                    b.Navigation("MarkedLocations");

                    b.Navigation("RegionStates");
                });
#pragma warning restore 612, 618
        }
    }
}
