using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyAPI.Migrations
{
    /// <inheritdoc />
    public partial class ResponseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<string>(type: "text", nullable: false),
                    IdChoice = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Responses_Choices_IdChoice",
                        column: x => x.IdChoice,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responses_IdChoice",
                table: "Responses",
                column: "IdChoice");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_IdUser",
                table: "Responses",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responses");
        }
    }
}
