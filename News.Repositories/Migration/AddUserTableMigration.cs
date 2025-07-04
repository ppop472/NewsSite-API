using FluentMigrator;
namespace News.Repositories
{
    [Migration(2025021904)]
    public class AddUserTableMigration : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("mail").AsString(int.MaxValue).NotNullable()
                .WithColumn("password").AsString(int.MaxValue).NotNullable()
                .WithColumn("sourceId").AsInt32().Nullable()
                .WithColumn("firstName").AsString(200).NotNullable()
                .WithColumn("lastName").AsString(200).NotNullable()
                .WithColumn("telephoneNumber").AsInt32().Nullable()
                .WithColumn("createdAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("updatedAt").AsDateTime().Nullable();
        }
        public override void Down()
        {
            Delete.Table("User");
        }
    }
}