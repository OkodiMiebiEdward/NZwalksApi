using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalksAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataForDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3844aa94-a7c0-4ad7-b3d0-15886afa1fdc"), "Easy" },
                    { new Guid("3b6a34c8-29be-44eb-95bb-c171fb6a020f"), "Hard" },
                    { new Guid("b02deb7f-733f-49ad-8dd4-8a24d443e505"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("32bd0cba-48ad-4d72-a1f4-d9e38f927693"), "NTL", "Northland", null },
                    { new Guid("4632dfaa-24db-4009-ba0e-866771b5d78d"), "AKL", "Auckland", "https://source.unsplash.com/random/300x200?sig=123" },
                    { new Guid("abd0d56c-a7ad-44ec-9ae7-d777a18fb2f4"), "BOP", "Bay Of Plenty", null },
                    { new Guid("ad789b77-f798-4acf-b2dc-270517bd9d3f"), "STL", "Southland", "https://source.unsplash.com/random/300x200?sig=789" },
                    { new Guid("b6f4b32f-d3ab-4d2d-a97c-69a80da92379"), "NSN", "Nelson", null },
                    { new Guid("f922ea03-3baa-4e11-8f6f-9e715946225e"), "WGN", "Wellington", "https://source.unsplash.com/random/300x200?sig=456" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("3844aa94-a7c0-4ad7-b3d0-15886afa1fdc"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("3b6a34c8-29be-44eb-95bb-c171fb6a020f"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b02deb7f-733f-49ad-8dd4-8a24d443e505"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("32bd0cba-48ad-4d72-a1f4-d9e38f927693"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("4632dfaa-24db-4009-ba0e-866771b5d78d"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("abd0d56c-a7ad-44ec-9ae7-d777a18fb2f4"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("ad789b77-f798-4acf-b2dc-270517bd9d3f"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("b6f4b32f-d3ab-4d2d-a97c-69a80da92379"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f922ea03-3baa-4e11-8f6f-9e715946225e"));
        }
    }
}
