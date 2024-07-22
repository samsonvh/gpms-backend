using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixrelationshipbetweenWarehouseRequestandProductionRequirementtomanymany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 773, DateTimeKind.Utc).AddTicks(9379),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 50, DateTimeKind.Utc).AddTicks(2757));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 769, DateTimeKind.Utc).AddTicks(2425),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 42, DateTimeKind.Utc).AddTicks(5719));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 765, DateTimeKind.Utc).AddTicks(4672),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 34, DateTimeKind.Utc).AddTicks(2012));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 758, DateTimeKind.Utc).AddTicks(1579),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 22, DateTimeKind.Utc).AddTicks(7243));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 753, DateTimeKind.Utc).AddTicks(512),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 3, DateTimeKind.Utc).AddTicks(2586));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 751, DateTimeKind.Utc).AddTicks(8054),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 1, DateTimeKind.Utc).AddTicks(930));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 747, DateTimeKind.Utc).AddTicks(5845),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 993, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 746, DateTimeKind.Utc).AddTicks(9256),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 993, DateTimeKind.Utc).AddTicks(878));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 744, DateTimeKind.Utc).AddTicks(5451),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 991, DateTimeKind.Utc).AddTicks(5017));

            migrationBuilder.CreateTable(
                name: "WarehouseRequestRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    WarehouseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionRequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseRequestRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRequirements_ProductionRequirement_ProductionRequirementId",
                        column: x => x.ProductionRequirementId,
                        principalTable: "ProductionRequirement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRequirements_WarehouseRequest_WarehouseRequestId",
                        column: x => x.WarehouseRequestId,
                        principalTable: "WarehouseRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRequirements_ProductionRequirementId",
                table: "WarehouseRequestRequirements",
                column: "ProductionRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRequirements_WarehouseRequestId",
                table: "WarehouseRequestRequirements",
                column: "WarehouseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest",
                column: "ProductionRequirementId",
                principalTable: "ProductionRequirement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest");

            migrationBuilder.DropTable(
                name: "WarehouseRequestRequirements");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 50, DateTimeKind.Utc).AddTicks(2757),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 773, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 42, DateTimeKind.Utc).AddTicks(5719),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 769, DateTimeKind.Utc).AddTicks(2425));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 34, DateTimeKind.Utc).AddTicks(2012),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 765, DateTimeKind.Utc).AddTicks(4672));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 22, DateTimeKind.Utc).AddTicks(7243),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 758, DateTimeKind.Utc).AddTicks(1579));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 3, DateTimeKind.Utc).AddTicks(2586),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 753, DateTimeKind.Utc).AddTicks(512));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 28, 1, DateTimeKind.Utc).AddTicks(930),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 751, DateTimeKind.Utc).AddTicks(8054));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 993, DateTimeKind.Utc).AddTicks(9340),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 747, DateTimeKind.Utc).AddTicks(5845));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 993, DateTimeKind.Utc).AddTicks(878),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 746, DateTimeKind.Utc).AddTicks(9256));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 21, 16, 44, 27, 991, DateTimeKind.Utc).AddTicks(5017),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 744, DateTimeKind.Utc).AddTicks(5451));

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest",
                column: "ProductionRequirementId",
                principalTable: "ProductionRequirement",
                principalColumn: "Id");
        }
    }
}
