using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeDepartmentIDinStaffNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 140, DateTimeKind.Utc).AddTicks(2185),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 410, DateTimeKind.Utc).AddTicks(9463));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 119, DateTimeKind.Utc).AddTicks(1875),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 402, DateTimeKind.Utc).AddTicks(917));

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 104, DateTimeKind.Utc).AddTicks(6401),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 381, DateTimeKind.Utc).AddTicks(2529));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 85, DateTimeKind.Utc).AddTicks(6231),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 359, DateTimeKind.Utc).AddTicks(6312));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 73, DateTimeKind.Utc).AddTicks(8035),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 346, DateTimeKind.Utc).AddTicks(1395));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 71, DateTimeKind.Utc).AddTicks(3619),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 336, DateTimeKind.Utc).AddTicks(5954));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 63, DateTimeKind.Utc).AddTicks(8062),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 326, DateTimeKind.Utc).AddTicks(6499));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 62, DateTimeKind.Utc).AddTicks(4974),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 323, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 59, DateTimeKind.Utc).AddTicks(4306),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 309, DateTimeKind.Utc).AddTicks(6795));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 410, DateTimeKind.Utc).AddTicks(9463),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 140, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 402, DateTimeKind.Utc).AddTicks(917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 119, DateTimeKind.Utc).AddTicks(1875));

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "Staffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 381, DateTimeKind.Utc).AddTicks(2529),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 104, DateTimeKind.Utc).AddTicks(6401));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 359, DateTimeKind.Utc).AddTicks(6312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 85, DateTimeKind.Utc).AddTicks(6231));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 346, DateTimeKind.Utc).AddTicks(1395),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 73, DateTimeKind.Utc).AddTicks(8035));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 336, DateTimeKind.Utc).AddTicks(5954),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 71, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 326, DateTimeKind.Utc).AddTicks(6499),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 63, DateTimeKind.Utc).AddTicks(8062));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 323, DateTimeKind.Utc).AddTicks(7190),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 62, DateTimeKind.Utc).AddTicks(4974));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 15, 26, 47, 309, DateTimeKind.Utc).AddTicks(6795),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 59, DateTimeKind.Utc).AddTicks(4306));
        }
    }
}
