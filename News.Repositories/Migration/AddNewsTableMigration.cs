using FluentMigrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using News.Repositories.Interfaces;
namespace News.Repositories
{
    [Migration(2025021901)]
    public class AddNewsTableMigration : Migration
    {
        public override void Up()
        {
            Create.Table("newsitems")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("guid").AsString(250).Nullable()
                .WithColumn("sourceid").AsInt32().NotNullable()
                .WithColumn("title").AsString(100).Nullable().Unique()
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("Content").AsCustom("TEXT").Nullable().Unique()
                .WithColumn("htmlContent").AsCustom("TEXT").Nullable()
                .WithColumn("headline").AsCustom("TEXT").Nullable()
                .WithColumn("linkUrl").AsString(1024).Nullable()
                .WithColumn("imageUrl").AsString(1024).Nullable()
                .WithColumn("thumbUrl").AsString(1024).Nullable()
                .WithColumn("languageId").AsInt32().NotNullable()
                .WithColumn("publishedAt").AsDateTime().Nullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("updatedAt").AsDateTime().Nullable()
                .WithColumn("deletedAt").AsDateTime().Nullable()
                .WithColumn("sent").AsBoolean().Nullable().WithDefaultValue("0");
        }
        public override void Down()
        {
            Delete.Table("News");
        }
    }
}