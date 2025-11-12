using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadDirecter.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TargetCountry = table.Column<string>(type: "text", nullable: false),
                    TrafficBuyerName = table.Column<string>(type: "text", nullable: false),
                    TrafficSourceName = table.Column<string>(type: "text", nullable: false),
                    FunnelName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CampaignMacros = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DestinationCrmConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CrmName = table.Column<string>(type: "text", nullable: false),
                    CooperationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeadRegistrationEndpoint = table.Column<string>(type: "text", nullable: false),
                    LeadRegistrationMethod = table.Column<string>(type: "text", nullable: false),
                    LeadRegistrationContentType = table.Column<string>(type: "text", nullable: false),
                    LeadRegistrationHeaders = table.Column<string>(type: "jsonb", nullable: false),
                    LeadRegistrationBodyTemplate = table.Column<string>(type: "jsonb", nullable: false),
                    LeadsRegistrationQueryParams = table.Column<string>(type: "jsonb", nullable: false),
                    LeadsRetrievalEndpoint = table.Column<string>(type: "text", nullable: false),
                    LeadsRetrievalMethod = table.Column<string>(type: "text", nullable: false),
                    LeadRetrievalContentType = table.Column<string>(type: "text", nullable: false),
                    LeadsRetrievalHeaders = table.Column<string>(type: "jsonb", nullable: false),
                    LeadRetrievalBodyTemplate = table.Column<string>(type: "jsonb", nullable: false),
                    LeadsRetrievalQueryParams = table.Column<string>(type: "jsonb", nullable: false),
                    AuthType = table.Column<string>(type: "text", nullable: false),
                    AuthConfig = table.Column<string>(type: "jsonb", nullable: false),
                    ResponseMappingProperty = table.Column<string>(type: "text", nullable: false),
                    ErrorIdentifier = table.Column<string>(type: "jsonb", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationCrmConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DestinationCrmConfigurations_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CountryCodeIso = table.Column<string>(type: "text", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    LeadIdInExternalCrm = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationCrmConfigurationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignMacros = table.Column<string>(type: "jsonb", nullable: false),
                    IsSentSuccessfully = table.Column<bool>(type: "boolean", nullable: false),
                    IsFtd = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerifiedFtd = table.Column<bool>(type: "boolean", nullable: false),
                    FtdAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_DestinationCrmConfigurations_DestinationCrmConfigurat~",
                        column: x => x.DestinationCrmConfigurationId,
                        principalTable: "DestinationCrmConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationCrmConfigurations_CampaignId",
                table: "DestinationCrmConfigurations",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CampaignId",
                table: "Leads",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_DestinationCrmConfigurationId",
                table: "Leads",
                column: "DestinationCrmConfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "DestinationCrmConfigurations");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
