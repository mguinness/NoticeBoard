using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NoticeBoard.Models;

namespace NoticeBoard.Data.Migrations
{
    [DbContext(typeof(ContentContext))]
    partial class ContentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("NoticeBoard.Models.Page", b =>
                {
                    b.Property<string>("Pagename");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("ModifiedBy")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Pagename");

                    b.ToTable("Pages");
                });
        }
    }
}
