using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReviewerIdtonullableinproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 865, DateTimeKind.Utc).AddTicks(4154),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 140, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 857, DateTimeKind.Utc).AddTicks(9513),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 119, DateTimeKind.Utc).AddTicks(1875));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 851, DateTimeKind.Utc).AddTicks(3669),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 104, DateTimeKind.Utc).AddTicks(6401));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 839, DateTimeKind.Utc).AddTicks(5531),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 85, DateTimeKind.Utc).AddTicks(6231));

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewerId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 831, DateTimeKind.Utc).AddTicks(4069),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 73, DateTimeKind.Utc).AddTicks(8035));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 829, DateTimeKind.Utc).AddTicks(4304),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 71, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 822, DateTimeKind.Utc).AddTicks(9258),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 63, DateTimeKind.Utc).AddTicks(8062));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 821, DateTimeKind.Utc).AddTicks(9701),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 62, DateTimeKind.Utc).AddTicks(4974));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 820, DateTimeKind.Utc).AddTicks(2832),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 59, DateTimeKind.Utc).AddTicks(4306));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 140, DateTimeKind.Utc).AddTicks(2185),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 865, DateTimeKind.Utc).AddTicks(4154));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 119, DateTimeKind.Utc).AddTicks(1875),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 857, DateTimeKind.Utc).AddTicks(9513));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 104, DateTimeKind.Utc).AddTicks(6401),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 851, DateTimeKind.Utc).AddTicks(3669));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 85, DateTimeKind.Utc).AddTicks(6231),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 839, DateTimeKind.Utc).AddTicks(5531));

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewerId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 73, DateTimeKind.Utc).AddTicks(8035),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 831, DateTimeKind.Utc).AddTicks(4069));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 71, DateTimeKind.Utc).AddTicks(3619),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 829, DateTimeKind.Utc).AddTicks(4304));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 63, DateTimeKind.Utc).AddTicks(8062),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 822, DateTimeKind.Utc).AddTicks(9258));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 62, DateTimeKind.Utc).AddTicks(4974),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 821, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 6, 55, 20, 59, DateTimeKind.Utc).AddTicks(4306),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 820, DateTimeKind.Utc).AddTicks(2832));
        }
    }
}
