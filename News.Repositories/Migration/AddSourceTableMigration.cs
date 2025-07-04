using FluentMigrator;
using FluentMigrator.Runner;
using News.Repositories.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection.Emit;
namespace News.Repositories
{
    [Migration(2025021903)]
    public class AddSourceTableMigration : Migration
    {
        public override void Up()
        {
            Create.Table("source")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(200).NotNullable()
                .WithColumn("typeId").AsString(255).NotNullable()
                .WithColumn("url").AsString(1024).Nullable()
                .WithColumn("module").AsString(200).Nullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("updatedAt").AsDateTime().Nullable()
                .WithColumn("deletedAt").AsDateTime().Nullable();

            Insert.IntoTable("source")
                .Row(new
                {
                    name = "NOS Algemeen",
                    typeId = "Rss",
                    url = "https://feeds.nos.nl/nosnieuwsalgemeen",
                    createdAt = new DateTime(),
                }
                 );

            Insert.IntoTable("source")
                .Row(new
                {
                    name = "NOS Formule-1",
                    typeId = "Rss",
                    url = "https://feeds.nos.nl/nossportformule1",
                    createdAt = new DateTime(),
                }
                 );

            Insert.IntoTable("source")
                .Row(new
                {
                    name = "New York Times",
                    typeId = "Rss",
                    url = "https://www.nytimes.com/svc/collections/v1/publish/https://www.nytimes.com/section/world/rss.xml",
                    createdAt = new DateTime(),
                }
                 );
        }
        public override void Down()
        {
            Delete.Table("Source");
        }
    }
}