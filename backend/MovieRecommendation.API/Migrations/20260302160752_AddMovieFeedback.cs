using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendation.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalMovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Liked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieFeedbacks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieFeedbacks");
        }
    }
}
