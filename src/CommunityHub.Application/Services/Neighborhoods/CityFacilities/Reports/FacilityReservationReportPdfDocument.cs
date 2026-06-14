using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationsReportPdfDocument : IDocument
    {
        private static readonly string DarkBlue = "#1e3a5f";
        private static readonly string AccentBlue = "#3b7dd8";
        private static readonly string LightBg = "#f7f9fc";

        private readonly string _neighborhoodName;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly List<FacilityReservationDto> _reservations;
        private readonly FacilityReservationsReportFormatter _formatter = new();

        public FacilityReservationsReportPdfDocument(
            string neighborhoodName,
            DateTime fromDate,
            DateTime toDate,
            List<FacilityReservationDto> reservations)
        {
            _neighborhoodName = neighborhoodName;
            _fromDate = fromDate;
            _toDate = toDate;
            _reservations = reservations;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(ComposePage);
        }

        private void ComposePage(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.MarginLeft(0);
            page.MarginRight(0);
            page.MarginTop(0);
            page.MarginBottom(0);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x
                .FontFamily("Arial")
                .FontSize(11)
                .FontColor(Colors.Grey.Darken4));

            page.Header().ShowOnce().Element(ComposeHeader);
            page.Content().PaddingHorizontal(36).PaddingTop(24).Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        }

        private void ComposeHeader(IContainer container)
        {
            container
                .Background(DarkBlue)
                .PaddingLeft(36)
                .PaddingRight(36)
                .PaddingTop(28)
                .PaddingBottom(22)
                .Column(column =>
                {
                    column.Spacing(4);

                    column.Item()
                        .Text("Izveštaj o rezervacijama gradskih objekata")
                        .FontSize(17)
                        .Bold()
                        .FontColor(Colors.White);

                    column.Item().Height(12);

                    column.Item().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(10).FontColor("#a8c4e0"));
                        text.Span("Kvart: ");
                        text.Span(_neighborhoodName).Bold().FontColor(Colors.White);
                    });

                    column.Item().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(10).FontColor("#a8c4e0"));
                        text.Span("Period: ");
                        text.Span(_formatter.FormatDateRange(_fromDate, _toDate)).Bold().FontColor(Colors.White);
                    });

                    column.Item().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(10).FontColor("#a8c4e0"));
                        text.Span("Ukupno rezervacija: ");
                        text.Span(_reservations.Count.ToString()).Bold().FontColor(Colors.White);
                        text.Span("   |   ").FontColor("#a8c4e0");
                        text.Span("Generisano: ");
                        text.Span(_formatter.FormatGeneratedAt()).Bold().FontColor(Colors.White);
                    });
                });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(18);

                if (_reservations.Count == 0)
                {
                    column.Item()
                        .PaddingTop(12)
                        .Text("Nema rezervacija gradskih objekata za izabrani period.")
                        .FontColor(Colors.Grey.Darken1);
                    return;
                }

                var grouped = _reservations
                    .GroupBy(r => r.Facility.Name)
                    .OrderBy(g => g.Key);

                foreach (var group in grouped)
                {
                    column.Item().Element(c => ComposeFacilitySection(c, group.Key, group.ToList()));
                }
            });
        }

        private void ComposeFacilitySection(IContainer container, string facilityName, List<FacilityReservationDto> reservations)
        {
            container.Column(column =>
            {
                column.Spacing(6);

                column.Item()
                    .BorderLeft(3)
                    .BorderColor(DarkBlue)
                    .PaddingLeft(8)
                    .Text(facilityName.ToUpper())
                    .FontSize(11)
                    .Bold()
                    .FontColor(DarkBlue)
                    .LetterSpacing(0.07f);

                column.Item()
                    .PaddingLeft(11)
                    .Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Medium));
                        text.Span("Broj poseta: ");
                        text.Span(reservations.Count.ToString());
                    });

                foreach (var reservation in reservations.OrderBy(r => r.StartDatetime))
                {
                    column.Item().Element(c => ComposeReservationCard(c, reservation));
                }
            });
        }

        private void ComposeReservationCard(IContainer container, FacilityReservationDto reservation)
        {
            container
                .Background(LightBg)
                .BorderLeft(3)
                .BorderColor(AccentBlue)
                .Padding(9)
                .PaddingLeft(12)
                .Column(column =>
                {
                    column.Spacing(2);

                    AddRow(column, "Datum posete", _formatter.FormatDate(reservation.StartDatetime));
                    AddRow(column, "Početak / Kraj",
                        $"{_formatter.FormatTime(reservation.StartDatetime)} – {_formatter.FormatTime(reservation.EndDatetime)}");
                    AddRow(column, "Trajanje", _formatter.FormatDuration(reservation.StartDatetime, reservation.EndDatetime));
                });
        }

        private static void AddRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Text(label).FontSize(10).FontColor("#555555");
                row.RelativeItem().AlignRight().Text(value).FontSize(10).Bold().FontColor("#111111");
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container
                .BorderTop(2)
                .BorderColor(DarkBlue)
                .PaddingHorizontal(36)
                .PaddingVertical(8)
                .Row(row =>
                {
                    row.RelativeItem()
                        .AlignLeft()
                        .Text("CommunityHub")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);

                    row.RelativeItem()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(x => x.FontSize(10).Bold().FontColor(DarkBlue));
                            text.Span("Strana ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
        }
    }
}