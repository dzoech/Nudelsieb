using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nudelsieb.Persistence.Relational.Migrations
{
    public partial class InitialNeuronEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Neurons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Information = table.Column<string>(nullable: false),
                    Reminder = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neurons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    NeuronId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => new { x.Name, x.NeuronId });
                    table.ForeignKey(
                        name: "FK_Groups_Neurons_NeuronId",
                        column: x => x.NeuronId,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_NeuronId",
                table: "Groups",
                column: "NeuronId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Neurons");
        }
    }
}
