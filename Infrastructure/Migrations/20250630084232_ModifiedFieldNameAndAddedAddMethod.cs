using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudMvc.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedFieldNameAndAddedAddMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagineFileName",
                table: "Products",
                newName: "ImageFileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Products",
                newName: "ImagineFileName");
        }
    }
}
