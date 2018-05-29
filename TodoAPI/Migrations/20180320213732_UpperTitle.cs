using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TodoAPI.Migrations
{
    public partial class UpperTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpperTitle",
                table: "Todos",
                nullable: true,
                computedColumnSql: "dbo.func([Title])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpperTitle",
                table: "Todos");
        }
    }
}
