using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTypeInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Product");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 716, DateTimeKind.Utc).AddTicks(1771),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 151, DateTimeKind.Utc).AddTicks(6917));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 708, DateTimeKind.Utc).AddTicks(1344),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 146, DateTimeKind.Utc).AddTicks(1088));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 689, DateTimeKind.Utc).AddTicks(4234),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 130, DateTimeKind.Utc).AddTicks(4788));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 668, DateTimeKind.Utc).AddTicks(7655),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 115, DateTimeKind.Utc).AddTicks(6392));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 655, DateTimeKind.Utc).AddTicks(9092),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 104, DateTimeKind.Utc).AddTicks(2732));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 646, DateTimeKind.Utc).AddTicks(7726),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 95, DateTimeKind.Utc).AddTicks(2283));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 637, DateTimeKind.Utc).AddTicks(1558),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 84, DateTimeKind.Utc).AddTicks(9397));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 634, DateTimeKind.Utc).AddTicks(2191),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 82, DateTimeKind.Utc).AddTicks(7843));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 620, DateTimeKind.Utc).AddTicks(3961),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 72, DateTimeKind.Utc).AddTicks(6710));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 151, DateTimeKind.Utc).AddTicks(6917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 716, DateTimeKind.Utc).AddTicks(1771));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 146, DateTimeKind.Utc).AddTicks(1088),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 708, DateTimeKind.Utc).AddTicks(1344));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 130, DateTimeKind.Utc).AddTicks(4788),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 689, DateTimeKind.Utc).AddTicks(4234));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 115, DateTimeKind.Utc).AddTicks(6392),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 668, DateTimeKind.Utc).AddTicks(7655));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 104, DateTimeKind.Utc).AddTicks(2732),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 655, DateTimeKind.Utc).AddTicks(9092));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 95, DateTimeKind.Utc).AddTicks(2283),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 646, DateTimeKind.Utc).AddTicks(7726));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 84, DateTimeKind.Utc).AddTicks(9397),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 637, DateTimeKind.Utc).AddTicks(1558));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 82, DateTimeKind.Utc).AddTicks(7843),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 634, DateTimeKind.Utc).AddTicks(2191));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 72, DateTimeKind.Utc).AddTicks(6710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 10, 39, 51, 620, DateTimeKind.Utc).AddTicks(3961));
        }
    }
}
