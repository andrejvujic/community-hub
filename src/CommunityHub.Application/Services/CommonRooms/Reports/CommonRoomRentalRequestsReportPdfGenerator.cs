using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class CommonRoomRentalRequestsReportPdfGenerator
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");

        public string GenerateToDownloads(
            IReadOnlyList<CommonRoomRentalRequestsReport> reports,
            DateTime from,
            DateTime to
        )
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads"
            );
            Directory.CreateDirectory(downloadsPath);

            var fileName = $"izvjestaj-zahtjevi-zajednicke-prostorije-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
            var filePath = Path.Combine(downloadsPath, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(text => text.FontSize(9));

                    page.Header().Column(header =>
                    {
                        header.Item().Text("Izvještaj o zahtjevima za iznajmljivanje zajedničkih prostorija")
                            .FontSize(16)
                            .SemiBold();
                        header.Item().PaddingTop(4).Text($"Opseg datuma: {FormatDate(from)} - {FormatDate(to)}")
                            .FontSize(10);
                    });

                    page.Content().PaddingTop(20).Column(column =>
                    {
                        column.Spacing(16);

                        if (reports.Count == 0)
                        {
                            column.Item().Text("Nema zahtjeva za izabrani opseg datuma.");
                            return;
                        }

                        foreach (var report in reports)
                        {
                            column.Item().Element(container => ComposeCommonRoomReport(container, report));
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Strana ");
                        text.CurrentPageNumber();
                        text.Span(" od ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf(filePath);

            return filePath;
        }

        private void ComposeCommonRoomReport(IContainer container, CommonRoomRentalRequestsReport report)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text(report.CommonRoom.Name)
                    .FontSize(13)
                    .SemiBold();

                column.Item().Text(
                    $"Ukupno zahtjeva: {report.TotalRequestsCount} | " +
                    $"Odobreno: {report.ApprovedRequestsCount} | " +
                    $"Na čekanju: {report.PendingRequestsCount} | " +
                    $"Odbijeno: {report.RejectedRequestsCount}"
                );

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(34);
                        columns.RelativeColumn(1.4f);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1.2f);
                        columns.RelativeColumn(1.1f);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header.Cell()).Text("ID");
                        HeaderCell(header.Cell()).Text("Stanar");
                        HeaderCell(header.Cell()).Text("Traženi opseg");
                        HeaderCell(header.Cell()).Text("Predloženi opseg");
                        HeaderCell(header.Cell()).Text("Dodijeljeni datum");
                        HeaderCell(header.Cell()).Text("Status");
                    });

                    foreach (var request in report.RentalRequests)
                    {
                        BodyCell(table.Cell()).Text(request.Id.ToString());
                        BodyCell(table.Cell()).Text(FormatResident(request));
                        BodyCell(table.Cell()).Text(FormatDateRange(request.RequestedStartDate, request.RequestedEndDate));
                        BodyCell(table.Cell()).Text(FormatProposedDateRange(request));
                        BodyCell(table.Cell()).Text(FormatAssignedDate(request, report.CommonRoom.RentalType));
                        BodyCell(table.Cell()).Text(FormatStatus(request.Status));
                    }
                });
            });
        }

        private static IContainer HeaderCell(IContainer container)
            => container
                .Background(Colors.Grey.Lighten3)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(4)
                .DefaultTextStyle(text => text.SemiBold());

        private static IContainer BodyCell(IContainer container)
            => container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(4);

        private string FormatDate(DateTime date)
            => date.ToString("dd.MM.yyyy.", _culture);

        private string FormatDateRange(DateTime from, DateTime to)
            => $"{FormatDate(from)} - {FormatDate(to)}";

        private string FormatProposedDateRange(CommonRoomRentalRequest request)
        {
            if (request.ProposedStartDate is null || request.ProposedEndDate is null)
                return "Nema";

            return FormatDateRange(request.ProposedStartDate.Value, request.ProposedEndDate.Value);
        }

        private string FormatAssignedDate(CommonRoomRentalRequest request, RentalType rentalType)
        {
            if (rentalType is RentalType.MultiDay)
                return "/";

            return request.AssignedDate is null
                ? "Nema"
                : FormatDate(request.AssignedDate.Value);
        }

        private static string FormatResident(CommonRoomRentalRequest request)
        {
            if (request.Resident is null)
                return $"ID {request.ResidentId}";

            return $"{request.Resident.Name} {request.Resident.Surname}";
        }

        private static string FormatStatus(RentalRequestStatus status)
            => status switch
            {
                RentalRequestStatus.Approved => "Odobren",
                RentalRequestStatus.Waiting => "Na čekanju",
                RentalRequestStatus.Rejected => "Odbijen",
                RentalRequestStatus.WaitingDateChange => "Čeka promjenu datuma",
                _ => status.ToString()
            };
    }
}
