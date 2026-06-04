using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms.Reports;
using MigraDoc;
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
            IReadOnlyList<CommonRoomRentalRequestsReportDto> reports,
            DateTime from,
            DateTime to
        )
        {
            ConfigureFont();

            var downloadsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads"
            );

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
            IReadOnlyList<CommonRoomRentalRequestsReportDto> reports,
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
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1.5);

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
            if (normal is null) return;

            normal.Font.Name = ReportFontResolver.FontFamilyName;
            normal.Font.Size = 8;

            var title = document.Styles.AddStyle("Title", "Normal");
            title.Font.Size = 14;
            title.Font.Bold = true;
            title.ParagraphFormat.SpaceAfter = Unit.FromPoint(6);

            var subtitle = document.Styles.AddStyle("Subtitle", "Normal");
            subtitle.Font.Size = 9;
            subtitle.ParagraphFormat.SpaceAfter = Unit.FromPoint(12);

            var commonRoomTitle = document.Styles.AddStyle("CommonRoomTitle", "Normal");
            commonRoomTitle.Font.Size = 12;
            commonRoomTitle.Font.Bold = true;
            commonRoomTitle.ParagraphFormat.SpaceBefore = Unit.FromPoint(8);
            commonRoomTitle.ParagraphFormat.SpaceAfter = Unit.FromPoint(4);

            var information = document.Styles.AddStyle("Information", "Normal");
            information.Font.Size = 9;
            information.ParagraphFormat.SpaceAfter = Unit.FromPoint(4);
        }

        private static void ConfigureFont()
        {
            if (GlobalFontSettings.FontResolver is null)
                GlobalFontSettings.FontResolver = new ReportFontResolver();

            PredefinedFontsAndChars.ErrorFontName = ReportFontResolver.FontFamilyName;
        }

        private void AddCommonRoomReport(Section section, CommonRoomRentalRequestsReportDto report)
        {
            var title = section.AddParagraph(report.CommonRoom.Name);
            title.Style = "CommonRoomTitle";
            title.Format.KeepWithNext = true;

            var information = section.AddParagraph(
                $"Tip iznajmljivanja: {FormatRentalType(report.CommonRoom.RentalType)}\n" +
                $"Ukupno zahtjeva: {report.TotalRequestsCount} | " +
                $"Odobreno: {report.ApprovedRequestsCount} | " +
                $"Na čekanju: {report.PendingRequestsCount} | " +
                $"Odbijeno: {report.RejectedRequestsCount}"
            );
            information.Style = "Information";
            information.Format.KeepWithNext = true;

            var table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.Gray;
            table.Format.Font.Size = 8;
            table.Rows.LeftIndent = 0;

            table.AddColumn(Unit.FromCentimeter(3.0));
            table.AddColumn(Unit.FromCentimeter(3.5));
            table.AddColumn(Unit.FromCentimeter(3.5));
            table.AddColumn(Unit.FromCentimeter(3.0));
            table.AddColumn(Unit.FromCentimeter(3.0));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.HeadingFormat = true;
            header.Format.Font.Bold = true;
            AddCell(header, 0, "Stanar");
            AddCell(header, 1, "Traženi opseg");
            AddCell(header, 2, "Predloženi opseg");
            AddCell(header, 3, "Dodijeljeni datum");
            AddCell(header, 4, "Status");

            if (report.TotalRequestsCount == 0)
            {
                var emptyRow = table.AddRow();
                emptyRow.Cells[0].MergeRight = 4;
                AddCell(emptyRow, 0, "Nema zahtjeva za ovu zajedničku prostoriju u izabranom opsegu.");
                section.AddParagraph().Format.SpaceAfter = Unit.FromPoint(8);
                return;
            }

            foreach (var request in report.RentalRequests)
            {
                var row = table.AddRow();
                AddCell(row, 0, FormatResident(request));
                AddCell(row, 1, FormatDateRange(request.RequestedStartDate, request.RequestedEndDate));
                AddCell(row, 2, FormatProposedDateRange(request));
                AddCell(row, 3, FormatAssignedDate(request, report.CommonRoom.RentalType));
                AddCell(row, 4, FormatStatus(request.Status));
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

        private string FormatProposedDateRange(CommonRoomRentalRequestDto request)
        {
            if (request.ProposedStartDate is null || request.ProposedEndDate is null)
                return "Nema";

            return FormatDateRange(request.ProposedStartDate.Value, request.ProposedEndDate.Value);
        }

        private string FormatAssignedDate(CommonRoomRentalRequestDto request, RentalType rentalType)
        {
            return request.AssignedDate is null || rentalType is RentalType.MultiDay
                ? "Nema"
                : FormatDate(request.AssignedDate.Value);
        }

        private static string FormatResident(CommonRoomRentalRequestDto request)
        {
            if (request.Resident is null)
                return "/";

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

        private static string FormatRentalType(RentalType type)
            => type switch
            {
                RentalType.OneDay => "Jednodnevno",
                RentalType.MultiDay => "Višednevno",
                _ => type.ToString()
            };
    }


}
