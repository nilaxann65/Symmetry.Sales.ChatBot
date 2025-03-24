using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Symmetry.Sales.ChatBot.Web.Migrations;

/// <inheritdoc />
public partial class PaymentMethod : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: "Business",
      columns: table => new
      {
        Id = table
          .Column<int>(type: "integer", nullable: false)
          .Annotation(
            "Npgsql:ValueGenerationStrategy",
            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
          ),
        Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
        Description = table.Column<string>(type: "text", nullable: false),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Business", x => x.Id);
      }
    );

    migrationBuilder.CreateTable(
      name: "Chats",
      columns: table => new
      {
        Id = table
          .Column<int>(type: "integer", nullable: false)
          .Annotation(
            "Npgsql:ValueGenerationStrategy",
            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
          ),
        TenantId = table.Column<int>(type: "integer", nullable: false),
        Origin = table.Column<int>(type: "integer", nullable: false),
        ContactId = table.Column<string>(
          type: "character varying(64)",
          maxLength: 64,
          nullable: false
        ),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Chats", x => x.Id);
      }
    );

    migrationBuilder.CreateTable(
      name: "Contact",
      columns: table => new
      {
        ContactId = table.Column<string>(type: "text", nullable: false),
        Name = table.Column<string>(type: "text", nullable: false),
        ContactOrigin = table.Column<int>(type: "integer", nullable: false),
        BusinessId = table.Column<int>(type: "integer", nullable: true),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Contact", x => x.ContactId);
        table.ForeignKey(
          name: "FK_Contact_Business_BusinessId",
          column: x => x.BusinessId,
          principalTable: "Business",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade
        );
      }
    );

    migrationBuilder.CreateTable(
      name: "PaymentMethod",
      columns: table => new
      {
        Id = table
          .Column<int>(type: "integer", nullable: false)
          .Annotation(
            "Npgsql:ValueGenerationStrategy",
            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
          ),
        Name = table.Column<string>(type: "text", nullable: false),
        IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
        PaymentMethodType = table.Column<int>(type: "integer", nullable: false),
        PaymentDetails = table.Column<string>(type: "text", nullable: false),
        BusinessId = table.Column<int>(type: "integer", nullable: true),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_PaymentMethod", x => x.Id);
        table.ForeignKey(
          name: "FK_PaymentMethod_Business_BusinessId",
          column: x => x.BusinessId,
          principalTable: "Business",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade
        );
      }
    );

    migrationBuilder.CreateTable(
      name: "Conversation",
      columns: table => new
      {
        Id = table
          .Column<int>(type: "integer", nullable: false)
          .Annotation(
            "Npgsql:ValueGenerationStrategy",
            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
          ),
        IsActive = table.Column<bool>(type: "boolean", nullable: false),
        ChatId = table.Column<int>(type: "integer", nullable: true),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Conversation", x => x.Id);
        table.ForeignKey(
          name: "FK_Conversation_Chats_ChatId",
          column: x => x.ChatId,
          principalTable: "Chats",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade
        );
      }
    );

    migrationBuilder.CreateTable(
      name: "Message",
      columns: table => new
      {
        Id = table
          .Column<int>(type: "integer", nullable: false)
          .Annotation(
            "Npgsql:ValueGenerationStrategy",
            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
          ),
        Date = table.Column<DateTime>(type: "timestamp", nullable: false),
        Content = table.Column<string>(type: "text", nullable: false),
        Sender = table.Column<int>(type: "integer", nullable: false),
        ConversationId = table.Column<int>(type: "integer", nullable: true),
      },
      constraints: table =>
      {
        table.PrimaryKey("PK_Message", x => x.Id);
        table.ForeignKey(
          name: "FK_Message_Conversation_ConversationId",
          column: x => x.ConversationId,
          principalTable: "Conversation",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade
        );
      }
    );

    migrationBuilder.CreateIndex(
      name: "IX_Contact_BusinessId",
      table: "Contact",
      column: "BusinessId"
    );

    migrationBuilder.CreateIndex(
      name: "IX_Conversation_ChatId",
      table: "Conversation",
      column: "ChatId"
    );

    migrationBuilder.CreateIndex(
      name: "IX_Message_ConversationId",
      table: "Message",
      column: "ConversationId"
    );

    migrationBuilder.CreateIndex(
      name: "IX_PaymentMethod_BusinessId",
      table: "PaymentMethod",
      column: "BusinessId"
    );
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(name: "Contact");

    migrationBuilder.DropTable(name: "Message");

    migrationBuilder.DropTable(name: "PaymentMethod");

    migrationBuilder.DropTable(name: "Conversation");

    migrationBuilder.DropTable(name: "Business");

    migrationBuilder.DropTable(name: "Chats");
  }
}
