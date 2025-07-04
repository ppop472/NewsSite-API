using FluentMigrator;

namespace News.Repositories
{
    [Migration(2025021902)]
    public class AddLanguagesTableMigration : Migration
    {
        public override void Up()
        {
            Create.Table("languages")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(200).NotNullable()
                .WithColumn("short").AsString(10).NotNullable()
                .WithColumn("code").AsString(50).NotNullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("updatedAt").AsDateTime().Nullable()
                .WithColumn("deletedAt").AsDateTime().Nullable();

            Insert.IntoTable("languages")
                .Row(new
                {
                    name = "Nederlands",
                    @short = "nl",
                    code = "nl-NL",
                    createdAt = new DateTime(),
                }
                 );

            Insert.IntoTable("languages")
                .Row(new
                {
                    name = "English",
                    @short = "en",
                    code = "en-EN",
                    createdAt = new DateTime(),
                }
                 );

            Insert.IntoTable("languages")
                .Row(new
                {
                    name = "English (UK)",
                    @short = "",
                    code = "en-UK",
                    createdAt = new DateTime(),
                }
                 );

            Insert.IntoTable("languages")
                .Row(new
                {
                    name = "English (US)",
                    @short = "",
                    code = "en-US",
                    createdAt = new DateTime(),
                }
                 );
        }

        public override void Down()
        {
            Delete.Table("Languages");
        }
    }
}