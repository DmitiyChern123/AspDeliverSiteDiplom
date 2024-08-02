using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    img = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    fullimg = table.Column<string>(type: "TEXT", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courier",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fio = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "status_order",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    login = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    password = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true),
                    role = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: true),
                    bonusPoints = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, nullable: true),
                    opis = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<int>(type: "INTEGER", nullable: true),
                    Grams = table.Column<string>(type: "TEXT", nullable: true),
                    Calories = table.Column<string>(type: "TEXT", nullable: true),
                    id_category = table.Column<int>(type: "INTEGER", nullable: true),
                    img = table.Column<string>(type: "text", nullable: true),
                    Is_hidden = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_category",
                        column: x => x.id_category,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    adress = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: false),
                    pay_type = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    status_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Is_need_devices = table.Column<bool>(type: "INTEGER", nullable: false),
                    courier_id = table.Column<int>(type: "INTEGER", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_courier",
                        column: x => x.courier_id,
                        principalTable: "courier",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_status_order",
                        column: x => x.status_id,
                        principalTable: "status_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 20, nullable: false),
                    price = table.Column<int>(type: "INTEGER", nullable: false),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Is_delated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_type_product",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "korzina",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    productType_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_korzina", x => x.id);
                    table.ForeignKey(
                        name: "FK_korzina_product_type1",
                        column: x => x.productType_id,
                        principalTable: "product_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_korzina_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "korzinaInOrder",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_korzinaInOrder", x => x.id);
                    table.ForeignKey(
                        name: "FK_korzinaInOrder_order",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_korzinaInOrder_product_type",
                        column: x => x.product_id,
                        principalTable: "product_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    opis = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    imageText = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    type_id = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_product_type",
                        column: x => x.type_id,
                        principalTable: "product_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_korzina_productType_id",
                table: "korzina",
                column: "productType_id");

            migrationBuilder.CreateIndex(
                name: "IX_korzina_user_id",
                table: "korzina",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_korzinaInOrder_order_id",
                table: "korzinaInOrder",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_korzinaInOrder_product_id",
                table: "korzinaInOrder",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_courier_id",
                table: "order",
                column: "courier_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_id",
                table: "order",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_id_category",
                table: "product",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_product_type_product_id",
                table: "product_type",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_type_id",
                table: "promotion",
                column: "type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "korzina");

            migrationBuilder.DropTable(
                name: "korzinaInOrder");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "product_type");

            migrationBuilder.DropTable(
                name: "courier");

            migrationBuilder.DropTable(
                name: "status_order");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
