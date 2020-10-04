using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nudelsieb.Persistence.Relational.Migrations
{
    public partial class AddReminderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminder",
                table: "Neurons");

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: true),
                    At = table.Column<DateTimeOffset>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Neurons_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_SubjectId",
                table: "Reminders",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.AddColumn<DateTime>(
                name: "Reminder",
                table: "Neurons",
                type: "datetime2",
                nullable: true);
        }
    }
}
