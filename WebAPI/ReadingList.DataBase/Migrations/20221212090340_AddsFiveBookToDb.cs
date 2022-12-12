using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadingList.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddsFiveBookToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new [] {"Id", "Name"},
                values: new object[,]
                {
                    {new Guid("88CD5322-9C58-42AB-B3DD-500CDCD2AF02").ToString("D"), "Computers & technology"},
                    {new Guid("1FD67454-27AE-4330-88CE-E42BE11B20D4").ToString("D"), "Cookbooks, food & Wine"},
                    {new Guid("9601BF5A-1910-4267-8B54-4C62C2092CC9").ToString("D"), "Fantasy"},
                    {new Guid("D632F81D-488B-449E-A02B-F7AB204C4289").ToString("D"), "Fiction"},
                    {new Guid("39A100BD-F238-4E2B-8BEE-F1A3F47EF1CA").ToString("D"), "Science"},
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new [] {"Id", "FullName"},
                values: new object[,]
                {
                    {new Guid("93BB3B4C-FE33-4AFC-9D11-28AAA1E41EEF").ToString("D"), "George Orwell"},
                    {new Guid("5286273E-A629-4C6C-96C9-A3F4F28F3936").ToString("D"), "Christopher Negus"},
                    {new Guid("2BDCB518-2D91-4F29-90A5-A5EE68A594F2").ToString("D"), "Dan Abnett"},
                    {new Guid("AB492D3B-15CB-4032-A92C-A76CFE843E1B").ToString("D"), "Chelsea Monroe-Cassel"},
                    {new Guid("11D8E74E-DB86-4A2B-9D36-EE6282EF48EC").ToString("D"), "Albert Einstein"},
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Title", "AuthorId", "CategoryId" },
                values: new object[,]
                {
                    {
                        new Guid("4EE953FC-72EB-418A-AF76-0CFBC6B5D5D8").ToString("D"), 
                        "Linux Bible", 
                        new Guid("5286273E-A629-4C6C-96C9-A3F4F28F3936").ToString("D"), 
                        new Guid("88CD5322-9C58-42AB-B3DD-500CDCD2AF02").ToString("D")
                    },
                    {
                        new Guid("C6383BBA-3820-4EF0-9220-138568A33FAD").ToString("D"),
                        "The Elder Scrolls: The Official Cookbook",
                        new Guid("AB492D3B-15CB-4032-A92C-A76CFE843E1B").ToString("D"),
                        new Guid("1FD67454-27AE-4330-88CE-E42BE11B20D4").ToString("D")
                    },
                    {
                        new Guid("D795F416-5FC1-4F28-BA72-B817ABC35EB4").ToString("D"),
                        "Relativity: The Special and General Theory",
                        new Guid("11D8E74E-DB86-4A2B-9D36-EE6282EF48EC").ToString("D"),
                        new Guid("39A100BD-F238-4E2B-8BEE-F1A3F47EF1CA").ToString("D")
                    },
                    {
                        new Guid("7A223788-4E7F-4195-BAFE-C38A8947630B").ToString("D"),
                        "Animal Farm",
                        new Guid("93BB3B4C-FE33-4AFC-9D11-28AAA1E41EEF").ToString("D"),
                        new Guid("D632F81D-488B-449E-A02B-F7AB204C4289").ToString("D")
                    },
                    {
                        new Guid("B6701F9D-2289-466B-8045-F4E53C457AD2").ToString("D"),
                        "Horus Rising: The Horus Heresy",
                        new Guid("2BDCB518-2D91-4F29-90A5-A5EE68A594F2").ToString("D"),
                        new Guid("9601BF5A-1910-4267-8B54-4C62C2092CC9").ToString("D")
                    },

                });

            migrationBuilder.InsertData(
                table: "BookNotes",
                columns: new[] { "Id", "BookId", "Priority", "Status" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString("D"), 
                        new Guid("C6383BBA-3820-4EF0-9220-138568A33FAD").ToString("D"), 
                        3, 
                        0
                    },
                    {
                        Guid.NewGuid().ToString("D"),
                        new Guid("D795F416-5FC1-4F28-BA72-B817ABC35EB4").ToString("D"),
                        2,
                        1
                    },
                    {
                        Guid.NewGuid().ToString("D"),
                        new Guid("7A223788-4E7F-4195-BAFE-C38A8947630B").ToString("D"),
                        4,
                        1
                    },
                    {
                        Guid.NewGuid().ToString("D"),
                        new Guid("B6701F9D-2289-466B-8045-F4E53C457AD2").ToString("D"),
                        2,
                        3
                    },
                    {
                        Guid.NewGuid().ToString("D"),
                        new Guid("4EE953FC-72EB-418A-AF76-0CFBC6B5D5D8").ToString("D"),
                        4,
                        2
                    },

                });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
