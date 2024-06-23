using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutOfOfficeSolution.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Subdivision = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Position = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        PeoplePartner = table.Column<int>(type: "int", nullable: true),
            //        OutOfOfficeBalance = table.Column<int>(type: "int", nullable: true),
            //        Photo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Employee__3214EC27A6A53A22", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Employees_PeoplePartner",
            //            column: x => x.PeoplePartner,
            //            principalTable: "Employees",
            //            principalColumn: "ID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "LeaveRequests",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeID = table.Column<int>(type: "int", nullable: true),
            //        AbsenceReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        StartDate = table.Column<DateOnly>(type: "date", nullable: false),
            //        EndDate = table.Column<DateOnly>(type: "date", nullable: false),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Новий")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__LeaveReq__3214EC272255D7A2", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_LeaveRequests_EmployeeID",
            //            column: x => x.EmployeeID,
            //            principalTable: "Employees",
            //            principalColumn: "ID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Projects",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProjectType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        StartDate = table.Column<DateOnly>(type: "date", nullable: false),
            //        EndDate = table.Column<DateOnly>(type: "date", nullable: true),
            //        ProjectManagerID = table.Column<int>(type: "int", nullable: true),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Projects__3214EC27E306AD0A", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Projects_ProjectManagerID",
            //            column: x => x.ProjectManagerID,
            //            principalTable: "Employees",
            //            principalColumn: "ID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ApprovalRequests",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ApproverID = table.Column<int>(type: "int", nullable: true),
            //        LeaveRequestID = table.Column<int>(type: "int", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Новий"),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Approval__3214EC27553357A7", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_ApprovalRequests_ApproverID",
            //            column: x => x.ApproverID,
            //            principalTable: "Employees",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_ApprovalRequests_LeaveRequestID",
            //            column: x => x.LeaveRequestID,
            //            principalTable: "LeaveRequests",
            //            principalColumn: "ID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EmployeeProjects",
            //    columns: table => new
            //    {
            //        EmployeeID = table.Column<int>(type: "int", nullable: false),
            //        ProjectID = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmployeeProjects", x => new { x.EmployeeID, x.ProjectID });
            //        table.ForeignKey(
            //            name: "FK_EmployeeProjects_EmployeeID",
            //            column: x => x.EmployeeID,
            //            principalTable: "Employees",
            //            principalColumn: "ID");
            //        table.ForeignKey(
            //            name: "FK_EmployeeProjects_ProjectID",
            //            column: x => x.ProjectID,
            //            principalTable: "Projects",
            //            principalColumn: "ID");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApprovalRequests_ApproverID",
            //    table: "ApprovalRequests",
            //    column: "ApproverID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ__Approval__6094218F8C819CE5",
            //    table: "ApprovalRequests",
            //    column: "LeaveRequestID",
            //    unique: true,
            //    filter: "[LeaveRequestID] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmployeeProjects_ProjectID",
            //    table: "EmployeeProjects",
            //    column: "ProjectID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_PeoplePartner",
            //    table: "Employees",
            //    column: "PeoplePartner");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveRequests_EmployeeID",
            //    table: "LeaveRequests",
            //    column: "EmployeeID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Projects_ProjectManagerID",
            //    table: "Projects",
            //    column: "ProjectManagerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ApprovalRequests");

            //migrationBuilder.DropTable(
            //    name: "EmployeeProjects");

            //migrationBuilder.DropTable(
            //    name: "LeaveRequests");

            //migrationBuilder.DropTable(
            //    name: "Projects");

            //migrationBuilder.DropTable(
            //    name: "Employees");
        }
    }
}
