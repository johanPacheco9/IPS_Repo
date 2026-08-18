using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NombresPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "EPS",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Pacientes");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Profesionales",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "Profesionales",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimerNombre",
                table: "Profesionales",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "Profesionales",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoNombre",
                table: "Profesionales",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EpsId",
                table: "Pacientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Pacientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimerNombre",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoNombre",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Eps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Nit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Eps",
                columns: new[] { "Id", "Activo", "Codigo", "Nit", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "EPS001", "800088702-2", "SURA EPS" },
                    { 2, true, "EPS002", "800251440-6", "SANITAS EPS" },
                    { 3, true, "EPS003", "900156264-2", "NUEVA EPS" },
                    { 4, true, "EPS004", "800130907-4", "SALUD TOTAL EPS" },
                    { 5, true, "EPS005", "860066942-7", "COMPENSAR EPS" },
                    { 6, true, "EPS006", "900226715-3", "COOSALUD EPS" },
                    { 7, true, "EPS007", "000000000-0", "PARTICULAR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_EpsId",
                table: "Pacientes",
                column: "EpsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Eps_EpsId",
                table: "Pacientes",
                column: "EpsId",
                principalTable: "Eps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Eps_EpsId",
                table: "Pacientes");

            migrationBuilder.DropTable(
                name: "Eps");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_EpsId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "PrimerNombre",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "SegundoNombre",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "EpsId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "PrimerNombre",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "SegundoNombre",
                table: "Pacientes");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Profesionales",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Profesionales",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Pacientes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPS",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
