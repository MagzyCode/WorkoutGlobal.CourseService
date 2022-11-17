using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutGlobal.CourseService.Api.Migrations
{
    public partial class DataRedundancyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorFullName",
                table: "Courses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoDescription",
                table: "CourseLessons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoTitle",
                table: "CourseLessons",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorFullName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "VideoDescription",
                table: "CourseLessons");

            migrationBuilder.DropColumn(
                name: "VideoTitle",
                table: "CourseLessons");
        }
    }
}
