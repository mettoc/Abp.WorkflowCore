using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkflowDemo.Migrations
{
    public partial class addWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    Version = table.Column<int>(nullable: false, defaultValue: 1),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    Color = table.Column<string>(maxLength: 50, nullable: true),
                    Group = table.Column<string>(maxLength: 100, nullable: true),
                    Inputs = table.Column<string>(nullable: true),
                    Nodes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDefinitions", x => new { x.Id, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "WorkflowEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    EventName = table.Column<string>(maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(maxLength: 200, nullable: true),
                    EventData = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExecutionErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkflowId = table.Column<string>(maxLength: 100, nullable: true),
                    ExecutionPointerId = table.Column<string>(maxLength: 100, nullable: true),
                    ErrorTime = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExecutionErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkflowId = table.Column<string>(maxLength: 200, nullable: true),
                    StepId = table.Column<int>(nullable: false),
                    ExecutionPointerId = table.Column<string>(maxLength: 200, nullable: true),
                    EventName = table.Column<string>(maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(maxLength: 200, nullable: true),
                    SubscribeAsOf = table.Column<DateTime>(nullable: false),
                    SubscriptionData = table.Column<string>(nullable: true),
                    ExternalToken = table.Column<string>(maxLength: 200, nullable: true),
                    ExternalWorkerId = table.Column<string>(maxLength: 200, nullable: true),
                    ExternalTokenExpiry = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    WorkflowDefinitionId = table.Column<string>(maxLength: 100, nullable: true),
                    Version = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Reference = table.Column<string>(maxLength: 200, nullable: true),
                    NextExecution = table.Column<long>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CreateUserIdentityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workflows_WorkflowDefinitions_WorkflowDefinitionId_Version",
                        columns: x => new { x.WorkflowDefinitionId, x.Version },
                        principalTable: "WorkflowDefinitions",
                        principalColumns: new[] { "Id", "Version" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExecutionPointers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WorkflowId = table.Column<Guid>(nullable: false),
                    StepId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SleepUntil = table.Column<DateTime>(nullable: true),
                    PersistenceData = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    EventName = table.Column<string>(maxLength: 100, nullable: true),
                    EventKey = table.Column<string>(maxLength: 100, nullable: true),
                    EventPublished = table.Column<bool>(nullable: false),
                    EventData = table.Column<string>(nullable: true),
                    StepName = table.Column<string>(maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(nullable: false),
                    Children = table.Column<string>(nullable: true),
                    ContextItem = table.Column<string>(nullable: true),
                    PredecessorId = table.Column<string>(maxLength: 100, nullable: true),
                    Outcome = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Scope = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExecutionPointers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowExecutionPointers_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersistedWorkflowAuditors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: false),
                    ExecutionPointerId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserIdentityName = table.Column<string>(nullable: true),
                    UserHeadPhoto = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedWorkflowAuditors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersistedWorkflowAuditors_WorkflowExecutionPointers_ExecutionPointerId",
                        column: x => x.ExecutionPointerId,
                        principalTable: "WorkflowExecutionPointers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersistedWorkflowAuditors_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExtensionAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionPointerId = table.Column<string>(nullable: true),
                    AttributeKey = table.Column<string>(maxLength: 100, nullable: true),
                    AttributeValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExtensionAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowExtensionAttributes_WorkflowExecutionPointers_ExecutionPointerId",
                        column: x => x.ExecutionPointerId,
                        principalTable: "WorkflowExecutionPointers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedWorkflowAuditors_ExecutionPointerId",
                table: "PersistedWorkflowAuditors",
                column: "ExecutionPointerId");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedWorkflowAuditors_WorkflowId",
                table: "PersistedWorkflowAuditors",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowExecutionPointers_WorkflowId",
                table: "WorkflowExecutionPointers",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowExtensionAttributes_ExecutionPointerId",
                table: "WorkflowExtensionAttributes",
                column: "ExecutionPointerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_WorkflowDefinitionId_Version",
                table: "Workflows",
                columns: new[] { "WorkflowDefinitionId", "Version" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedWorkflowAuditors");

            migrationBuilder.DropTable(
                name: "WorkflowEvents");

            migrationBuilder.DropTable(
                name: "WorkflowExecutionErrors");

            migrationBuilder.DropTable(
                name: "WorkflowExtensionAttributes");

            migrationBuilder.DropTable(
                name: "WorkflowSubscriptions");

            migrationBuilder.DropTable(
                name: "WorkflowExecutionPointers");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "WorkflowDefinitions");
        }
    }
}
