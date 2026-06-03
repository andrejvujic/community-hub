using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
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
            ConfigureFonts();

            var downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads"
            );
            Directory.CreateDirectory(downloadsPath);

            var fileName = $"izvjestaj-zahtjevi-zajednicke-prostorije-{DateTime.Now:yyyyMMdd-HHmmss}.pdf";
            var filePath = Path.Combine(downloadsPath, fileName);

            var document = CreateDocument(reports, from, to);
            var renderer = new PdfDocumentRenderer
            {
                Document = document
            };

            renderer.RenderDocument();
            renderer.PdfDocument.Save(filePath);

            return filePath;
        }

        private Document CreateDocument(
            IReadOnlyList<CommonRoomRentalRequestsReport> reports,
            DateTime from,
            DateTime to
        )
        {
            var document = new Document();
            document.Info.Title = "Izvještaj o zahtjevima za iznajmljivanje zajedničkih prostorija";
            DefineStyles(document);

            var section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.TopMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1.3);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1.3);

            var title = section.AddParagraph("Izvještaj o zahtjevima za iznajmljivanje zajedničkih prostorija");
            title.Style = "Title";

            var dateRange = section.AddParagraph($"Opseg datuma: {FormatDate(from)} - {FormatDate(to)}");
            dateRange.Style = "Subtitle";

            if (reports.Count == 0)
            {
                section.AddParagraph("Nema zahtjeva za izabrani opseg datuma.");
                return document;
            }

            foreach (var report in reports)
                AddCommonRoomReport(section, report);

            return document;
        }

        private static void DefineStyles(Document document)
        {
            var normal = document.Styles["Normal"];
            normal.Font.Name = ReportFontResolver.FontFamilyName;
            normal.Font.Size = 8;

            var title = document.Styles.AddStyle("Title", "Normal");
            title.Font.Size = 14;
            title.Font.Bold = true;
            title.ParagraphFormat.SpaceAfter = Unit.FromPoint(6);

            var subtitle = document.Styles.AddStyle("Subtitle", "Normal");
            subtitle.Font.Size = 9;
            subtitle.ParagraphFormat.SpaceAfter = Unit.FromPoint(12);

            var roomTitle = document.Styles.AddStyle("RoomTitle", "Normal");
            roomTitle.Font.Size = 11;
            roomTitle.Font.Bold = true;
            roomTitle.ParagraphFormat.SpaceBefore = Unit.FromPoint(8);
            roomTitle.ParagraphFormat.SpaceAfter = Unit.FromPoint(4);

            var statistics = document.Styles.AddStyle("Statistics", "Normal");
            statistics.Font.Size = 8;
            statistics.ParagraphFormat.SpaceAfter = Unit.FromPoint(4);
        }

        private static void ConfigureFonts()
        {
            if (GlobalFontSettings.FontResolver is null)
                GlobalFontSettings.FontResolver = new ReportFontResolver();

            PredefinedFontsAndChars.ErrorFontName = ReportFontResolver.FontFamilyName;
        }

        private void AddCommonRoomReport(Section section, CommonRoomRentalRequestsReport report)
        {
            var roomTitle = section.AddParagraph(report.CommonRoom.Name);
            roomTitle.Style = "RoomTitle";
            roomTitle.Format.KeepWithNext = true;

            var statistics = section.AddParagraph(
                $"Ukupno zahtjeva: {report.TotalRequestsCount} | " +
                $"Odobreno: {report.ApprovedRequestsCount} | " +
                $"Na čekanju: {report.PendingRequestsCount} | " +
                $"Odbijeno: {report.RejectedRequestsCount}"
            );
            statistics.Style = "Statistics";
            statistics.Format.KeepWithNext = true;

            var table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.Gray;
            table.Format.Font.Size = 7;
            table.Rows.LeftIndent = 0;

            table.AddColumn(Unit.FromCentimeter(1.0));
            table.AddColumn(Unit.FromCentimeter(3.0));
            table.AddColumn(Unit.FromCentimeter(3.1));
            table.AddColumn(Unit.FromCentimeter(3.1));
            table.AddColumn(Unit.FromCentimeter(2.8));
            table.AddColumn(Unit.FromCentimeter(2.8));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.HeadingFormat = true;
            header.Format.Font.Bold = true;
            AddCell(header, 0, "ID");
            AddCell(header, 1, "Stanar");
            AddCell(header, 2, "Traženi opseg");
            AddCell(header, 3, "Predloženi opseg");
            AddCell(header, 4, "Dodijeljeni datum");
            AddCell(header, 5, "Status");

            if (report.RentalRequests.Count == 0)
            {
                var emptyRow = table.AddRow();
                emptyRow.Cells[0].MergeRight = 5;
                AddCell(emptyRow, 0, "Nema zahtjeva za ovu zajedničku prostoriju u izabranom opsegu.");
                section.AddParagraph().Format.SpaceAfter = Unit.FromPoint(8);
                return;
            }

            foreach (var request in report.RentalRequests)
            {
                var row = table.AddRow();
                AddCell(row, 0, request.Id.ToString());
                AddCell(row, 1, FormatResident(request));
                AddCell(row, 2, FormatDateRange(request.RequestedStartDate, request.RequestedEndDate));
                AddCell(row, 3, FormatProposedDateRange(request));
                AddCell(row, 4, FormatAssignedDate(request, report.CommonRoom.RentalType));
                AddCell(row, 5, FormatStatus(request.Status));
            }

            section.AddParagraph().Format.SpaceAfter = Unit.FromPoint(8);
        }

        private static void AddCell(Row row, int index, string text)
        {
            var paragraph = row.Cells[index].AddParagraph(text);
            paragraph.Format.SpaceBefore = Unit.FromPoint(2);
            paragraph.Format.SpaceAfter = Unit.FromPoint(2);
            row.Cells[index].VerticalAlignment = VerticalAlignment.Center;
        }

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
