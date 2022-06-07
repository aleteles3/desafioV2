﻿// <auto-generated />
using System;
using Infra.Data.Users.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Data.Users.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20220607010020_Alter_UserIdColumn")]
    partial class Alter_UserIdColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.User.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Use_UserId");

                    b.Property<DateTimeOffset>("DateAlter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateInc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Use_Login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Use_Password");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean")
                        .HasColumnName("Use_Status");

                    b.HasKey("Id")
                        .HasName("PK_Use_User");

                    b.HasIndex("Login")
                        .HasDatabaseName("IX_Use_Login");

                    b.ToTable("Use_User", "User");
                });
#pragma warning restore 612, 618
        }
    }
}
