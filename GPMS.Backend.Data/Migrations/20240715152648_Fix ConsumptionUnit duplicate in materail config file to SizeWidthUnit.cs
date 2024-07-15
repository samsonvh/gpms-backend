using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixConsumptionUnitduplicateinmaterailconfigfiletoSizeWidthUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 410, DateTimeKind.Utc).AddTicks(9463),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 467, DateTimeKind.Utc).AddTicks(1520));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 402, DateTimeKind.Utc).AddTicks(917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 458, DateTimeKind.Utc).AddTicks(8890));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 381, DateTimeKind.Utc).AddTicks(2529),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 439, DateTimeKind.Utc).AddTicks(7577));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 359, DateTimeKind.Utc).AddTicks(6312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 418, DateTimeKind.Utc).AddTicks(1220));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 346, DateTimeKind.Utc).AddTicks(1395),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 405, DateTimeKind.Utc).AddTicks(4841));

            migrationBuilder.AlterColumn<string>(
                name: "SizeWidthUnit",
                table: "Material",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 336, DateTimeKind.Utc).AddTicks(5954),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 396, DateTimeKind.Utc).AddTicks(1035));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 326, DateTimeKind.Utc).AddTicks(6499),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 385, DateTimeKind.Utc).AddTicks(7881));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 323, DateTimeKind.Utc).AddTicks(7190),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 382, DateTimeKind.Utc).AddTicks(8928));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 309, DateTimeKind.Utc).AddTicks(6795),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 368, DateTimeKind.Utc).AddTicks(5673));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 467, DateTimeKind.Utc).AddTicks(1520),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 410, DateTimeKind.Utc).AddTicks(9463));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 458, DateTimeKind.Utc).AddTicks(8890),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 402, DateTimeKind.Utc).AddTicks(917));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 439, DateTimeKind.Utc).AddTicks(7577),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 381, DateTimeKind.Utc).AddTicks(2529));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 418, DateTimeKind.Utc).AddTicks(1220),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 359, DateTimeKind.Utc).AddTicks(6312));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 405, DateTimeKind.Utc).AddTicks(4841),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 346, DateTimeKind.Utc).AddTicks(1395));

            migrationBuilder.AlterColumn<string>(
                name: "SizeWidthUnit",
                table: "Material",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 396, DateTimeKind.Utc).AddTicks(1035),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 336, DateTimeKind.Utc).AddTicks(5954));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 385, DateTimeKind.Utc).AddTicks(7881),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 326, DateTimeKind.Utc).AddTicks(6499));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 382, DateTimeKind.Utc).AddTicks(8928),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 323, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 11, 38, 26, 368, DateTimeKind.Utc).AddTicks(5673),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 309, DateTimeKind.Utc).AddTicks(6795));
        }
    }
}
