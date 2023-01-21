using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_Project.DataAccess.Migrations
{
    public partial class AddStoreProcedureToCoverTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Save
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Create
                                 @Name varchar(50)
                                 AS
                                 insert CoverTypes values(@Name)");
            //Update
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Update
                                 @Id int,
                                 @Name varchar(50)
                                 AS
                                 update CoverTypes set Name=@Name where Id=@Id");
            //Delete
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Delete
                                 @Id int
                                 AS
                                 Delete CoverTypes where Id=@Id");
            //Display
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_GetCoverTypes
                                 AS
                                 Select * from CoverTypes");
            //Find
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_GetCoverType
                                 @Id int
                                 AS
                                 Select * from CoverTypes where Id=@Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE SP_GetCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_GetCoverTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_Create");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_GetCoverType_Update");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_GetCoverType_Delete");
        }
    }
}
