using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NoticeBoard.Models;

namespace NoticeBoard.Data.Migrations
{
    [DbContext(typeof(ContentContext))]
    [Migration("20160822032245_CreateContentSchema")]
    partial class CreateContentSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("NoticeBoard.Models.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Markdown");

                    b.Property<string>("Path")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasIndex("Path");

                    b.ToTable("Content");
                });
        }
    }
}
