using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EPS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RegimenSalud = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesionales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroRethus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesionales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfesionalId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MotivoConsulta = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Profesionales_ProfesionalId",
                        column: x => x.ProfesionalId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfesionalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DuracionMinutosCita = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disponibilidades_Profesionales_ProfesionalId",
                        column: x => x.ProfesionalId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admisiones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaMedicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoContrato = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EPS = table.Column<string>(type: "text", nullable: false),
                    NumeroAutorizacion = table.Column<string>(type: "text", nullable: false),
                    ValorConsulta = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CopagoOCuotaModeradora = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorNeto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FechaAdmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegistradoPorUsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admisiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admisiones_Citas_CitaMedicaId",
                        column: x => x.CitaMedicaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admisiones_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoriasClinicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfesionalId = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaMedicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EsInmutable = table.Column<bool>(type: "boolean", nullable: false),
                    MotivoConsulta = table.Column<string>(type: "text", nullable: false),
                    EnfermedadActual = table.Column<string>(type: "text", nullable: false),
                    RevisionPorSistemas = table.Column<string>(type: "text", nullable: false),
                    AntecedentesPersonales = table.Column<string>(type: "text", nullable: false),
                    AntecedentesFamiliares = table.Column<string>(type: "text", nullable: false),
                    ExamenFisico = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriasClinicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Citas_CitaMedicaId",
                        column: x => x.CitaMedicaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Profesionales_ProfesionalId",
                        column: x => x.ProfesionalId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdmisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroFactura = table.Column<string>(type: "text", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValorBruto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorCopago = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorDescuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorNeto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CUFE = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturas_Admisiones_AdmisionId",
                        column: x => x.AdmisionId,
                        principalTable: "Admisiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facturas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosticosConsulta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoriaClinicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoCIE11 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    TipoDiagnostico = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EsPrincipal = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosticosConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosticosConsulta_HistoriasClinicas_HistoriaClinicaId",
                        column: x => x.HistoriaClinicaId,
                        principalTable: "HistoriasClinicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotasAclaratorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoriaClinicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfesionalId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasAclaratorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasAclaratorias_HistoriasClinicas_HistoriaClinicaId",
                        column: x => x.HistoriaClinicaId,
                        principalTable: "HistoriasClinicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotasAclaratorias_Profesionales_ProfesionalId",
                        column: x => x.ProfesionalId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedimientosConsulta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoriaClinicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoCUPS = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreProcedimiento = table.Column<string>(type: "text", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedimientosConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedimientosConsulta_HistoriasClinicas_HistoriaClinicaId",
                        column: x => x.HistoriaClinicaId,
                        principalTable: "HistoriasClinicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RipsTransacciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FacturaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CUV = table.Column<string>(type: "text", nullable: false),
                    JsonRips = table.Column<string>(type: "jsonb", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstadoValidacion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RipsTransacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RipsTransacciones_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admisiones_CitaMedicaId",
                table: "Admisiones",
                column: "CitaMedicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admisiones_PacienteId",
                table: "Admisiones",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ProfesionalId",
                table: "Citas",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticosConsulta_HistoriaClinicaId",
                table: "DiagnosticosConsulta",
                column: "HistoriaClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_ProfesionalId",
                table: "Disponibilidades",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_AdmisionId",
                table: "Facturas",
                column: "AdmisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_PacienteId",
                table: "Facturas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_CitaMedicaId",
                table: "HistoriasClinicas",
                column: "CitaMedicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_PacienteId",
                table: "HistoriasClinicas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_ProfesionalId",
                table: "HistoriasClinicas",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasAclaratorias_HistoriaClinicaId",
                table: "NotasAclaratorias",
                column: "HistoriaClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasAclaratorias_ProfesionalId",
                table: "NotasAclaratorias",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_TipoDocumento_NumeroDocumento",
                table: "Pacientes",
                columns: new[] { "TipoDocumento", "NumeroDocumento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcedimientosConsulta_HistoriaClinicaId",
                table: "ProcedimientosConsulta",
                column: "HistoriaClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesionales_NumeroRethus",
                table: "Profesionales",
                column: "NumeroRethus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RipsTransacciones_FacturaId",
                table: "RipsTransacciones",
                column: "FacturaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagnosticosConsulta");

            migrationBuilder.DropTable(
                name: "Disponibilidades");

            migrationBuilder.DropTable(
                name: "NotasAclaratorias");

            migrationBuilder.DropTable(
                name: "ProcedimientosConsulta");

            migrationBuilder.DropTable(
                name: "RipsTransacciones");

            migrationBuilder.DropTable(
                name: "HistoriasClinicas");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Admisiones");

            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Profesionales");
        }
    }
}
