using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nudelsieb.Persistence.Relational.Migrations
{
    public partial class AddUserIdDiscriminatorToAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Neurons_SubjectId",
                table: "Reminders");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reminders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Neurons",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Groups",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Neurons_SubjectId",
                table: "Reminders",
                column: "SubjectId",
                principalTable: "Neurons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Neurons_SubjectId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Neurons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Reminders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Neurons_SubjectId",
                table: "Reminders",
                column: "SubjectId",
                principalTable: "Neurons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
