using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntityFrameworkCore10Sample.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "City", "FirstName", "IsDeleted", "LastName", "TenantId" },
                values: new object[,]
                {
                    { new Guid("302dc1b9-6220-425b-b243-7b7fcbbc63ea"), "Paperopoli", "Scrooge", false, "McDuck", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("39b186d5-44b4-417a-9f37-d959b45de89d"), "Paperopoli", "Donald", false, "Duck", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5d4194d4-3905-4646-bf33-432afeef867e"), "Topolinia", "Clarabelle", true, "Cow", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("776ebb4a-c658-4892-8ec3-4ec2d09fadeb"), "Topolinia", "Mickey", false, "Mouse", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("9ae944bb-bba4-4ac6-9c4d-248c221f627b"), "Topolinia", "Pluto", true, "Dog", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b4ab0f01-510d-4d3b-b8f6-af75cb20b777"), "Taggia", "Marco", false, "Minerva", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("baffeb2b-2257-48c4-8f45-ec20a300c922"), "Topolinia", "Minnie", true, "Mouse", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("c98b3e97-5c55-4f11-bdd8-aecfa0610718"), "Topolinia", "Goofy", false, "Goof", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("e8f2b8dc-c1ca-44a2-b7d2-e31a19012e79"), "Paperopoli", "Huey", false, "Duck", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("f1b45738-f211-46e0-b95c-24b894a1f429"), "Paperopoli", "Daisy", false, "Duck", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("f31fb820-b524-4efe-b6f2-7415080f5d7b"), "Paperopoli", "Dewey", false, "Duck", new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
