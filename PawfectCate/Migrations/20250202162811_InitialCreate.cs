using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawfectCate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    adminId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admins__AD0500A6AFD735CC", x => x.adminId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__703FF80E641F2B33", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    serviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(name: "description ", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__455070DF5B3C7223", x => x.serviceId);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    feedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__2613FD24E56FCA71", x => x.feedbackId);
                    table.ForeignKey(
                        name: "FK_Feedback_ToTable",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId");
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    petId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    breed = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__DDF85079F0C061E0", x => x.petId);
                    table.ForeignKey(
                        name: "FK_Pets_ToTable",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    appointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    petId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__D06765FE230842D9", x => x.appointmentId);
                    table.ForeignKey(
                        name: "FK_Table_ToTable",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId");
                    table.ForeignKey(
                        name: "FK_Table_ToTable_1",
                        column: x => x.petId,
                        principalTable: "Pets",
                        principalColumn: "petId");
                });

            migrationBuilder.CreateTable(
                name: "AppointmentServices",
                columns: table => new
                {
                    appointmentId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Table_ToTable7",
                        column: x => x.appointmentId,
                        principalTable: "Appointments",
                        principalColumn: "appointmentId");
                    table.ForeignKey(
                        name: "FK_Table_ToTable8",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "serviceId");
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    billId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointmentId = table.Column<int>(type: "int", nullable: false),
                    billAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bill__6D903F03F4F462BA", x => x.billId);
                    table.ForeignKey(
                        name: "FK_Bill_ToTable",
                        column: x => x.appointmentId,
                        principalTable: "Appointments",
                        principalColumn: "appointmentId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    paymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    billId = table.Column<int>(type: "int", nullable: false),
                    cvvno = table.Column<int>(type: "int", nullable: false),
                    cardno = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "completed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__A0D9EFC60A229E82", x => x.paymentId);
                    table.ForeignKey(
                        name: "FK_Payments_ToTable",
                        column: x => x.billId,
                        principalTable: "Bill",
                        principalColumn: "billId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_customerId",
                table: "Appointments",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_petId",
                table: "Appointments",
                column: "petId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_appointmentId",
                table: "AppointmentServices",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_serviceId",
                table: "AppointmentServices",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_appointmentId",
                table: "Bill",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_customerId",
                table: "Feedback",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_billId",
                table: "Payments",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_customerId",
                table: "Pets",
                column: "customerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AppointmentServices");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
