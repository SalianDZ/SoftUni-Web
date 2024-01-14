using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class AddTasksAndBoardAndSeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "In Progress" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Done" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { new Guid("36d6d8f8-99a1-4c13-b9df-72dfdaff1913"), 2, new DateTime(2023, 11, 28, 13, 31, 27, 874, DateTimeKind.Utc).AddTicks(92), "Create Desktop Client App for the RESTFUL TaskBoard service", "cb630144-a847-4491-894b-9331a30b5401", "Desktop Client App" },
                    { new Guid("4bee7504-1e4e-4647-bd12-bea10dab11b5"), 1, new DateTime(2023, 7, 28, 13, 31, 27, 874, DateTimeKind.Utc).AddTicks(88), "Create Android Client App for the RESTFUL TaskBoard service", "0b6217b6-7438-4534-b4d1-eea092246f0e", "Android Client App" },
                    { new Guid("8ff331fe-e3d9-4238-b7e1-606dc64f8ca2"), 2, new DateTime(2024, 12, 28, 13, 31, 27, 874, DateTimeKind.Utc).AddTicks(94), "Implement [Create Task] page for adding tasks", "cb630144-a847-4491-894b-9331a30b5401", "Create Tasks" },
                    { new Guid("f9431e67-18de-469a-ac35-be66f59ffacc"), 1, new DateTime(2023, 6, 11, 13, 31, 27, 874, DateTimeKind.Utc).AddTicks(70), "Implement better styling for all public pages", "23f8a8ff-3c73-436c-a66f-45b245666a2f", "Improve CSS styles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
