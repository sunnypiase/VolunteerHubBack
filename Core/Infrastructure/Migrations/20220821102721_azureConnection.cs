using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class azureConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolunteerPostPostId = table.Column<int>(type: "int", nullable: false),
                    NeedfulPostPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostConnections_Posts_NeedfulPostPostId",
                        column: x => x.NeedfulPostPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_PostConnections_Posts_VolunteerPostPostId",
                        column: x => x.VolunteerPostPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostsPostId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostsPostId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagId", "Name" },
                values: new object[,]
                {
                    { 1, "Житло" },
                    { 2, "Медицина" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "Name", "Password", "PhoneNumber", "Role", "Surname" },
                values: new object[,]
                {
                    { 1, "Admin street", "admin@example.com", "Admin", new byte[] { 218, 49, 142, 22, 217, 252, 247, 124, 109, 0, 230, 127, 13, 159, 153, 241, 198, 18, 231, 211, 121, 153, 219, 80, 72, 249, 255, 91, 137, 192, 210, 60, 237, 223, 205, 159, 252, 193, 17, 250, 226, 239, 169, 34, 40, 168, 23, 154, 1, 18, 145, 73, 83, 35, 91, 153, 4, 16, 72, 77, 43, 55, 31, 66 }, "88005553535", 2, "Test" },
                    { 2, "Volunteer street", "volunteer@example.com", "Volunteer", new byte[] { 99, 75, 46, 160, 118, 173, 166, 203, 149, 66, 168, 76, 228, 217, 62, 28, 122, 239, 119, 97, 193, 144, 144, 174, 126, 40, 85, 241, 126, 172, 52, 124, 193, 254, 164, 70, 89, 111, 57, 116, 123, 166, 87, 127, 182, 160, 34, 229, 49, 74, 86, 30, 159, 87, 242, 34, 173, 108, 90, 106, 103, 22, 75, 104 }, "88005553535", 0, "Test" },
                    { 3, "Needful street", "needful@example.com", "Needful", new byte[] { 27, 212, 237, 60, 65, 101, 16, 189, 249, 137, 227, 146, 38, 28, 79, 23, 79, 224, 126, 171, 1, 61, 77, 13, 88, 35, 116, 198, 177, 117, 213, 74, 181, 201, 67, 124, 241, 109, 76, 40, 253, 125, 122, 90, 238, 171, 83, 163, 68, 178, 141, 242, 133, 196, 176, 7, 207, 13, 137, 162, 94, 122, 182, 234 }, "88005553535", 1, "Test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostConnections_NeedfulPostPostId",
                table: "PostConnections",
                column: "NeedfulPostPostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostConnections_VolunteerPostPostId",
                table: "PostConnections",
                column: "VolunteerPostPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsTagId",
                table: "PostTag",
                column: "TagsTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostConnections");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
