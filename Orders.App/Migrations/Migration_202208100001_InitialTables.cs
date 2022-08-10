using FluentMigrator;

namespace Orders.App.Migrations;

[Migration(202208100001)]
public class Migration_202208100001_InitialTables : Migration
{
    public override void Up()
    {
        Create.Table("Orders")
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefaultValue(Guid.NewGuid())
            .WithColumn("Status").AsByte().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Orders");
    }
}

