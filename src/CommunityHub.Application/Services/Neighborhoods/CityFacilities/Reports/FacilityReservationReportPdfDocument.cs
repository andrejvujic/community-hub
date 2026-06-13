using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities.Reports
{
    public class FacilityReservationReportPdfDocument : IDocument
    {
        private readonly string _reportTitle;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly List<FacilityReservationReportSection> _sections;

        public FacilityReservationReportPdfDocument(
            string reportTitle,
            DateTime fromDate,
            DateTime toDate,
            List<FacilityReservationReportSection> sections)
        {
            _reportTitle = reportTitle;
            _fromDate = fromDate;
            _toDate = toDate;
            _sections = sections;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(ComposePage);
        }

        private void ComposePage(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.MarginLeft(30);
            page.MarginRight(30);
            page.MarginTop(30);
            page.MarginBottom(10);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x
                .FontFamily("Lato")
                .FontSize(11)
                .FontColor(Colors.Grey.Darken4));

            page.Header().ShowOnce().Element(ComposeHeader);
            page.Content().PaddingTop(20).Element(ComposeContent);
            page.Footer().AlignCenter().Text(text =>
            {
                text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken1));
                text.Span("Strana ");
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(6);

                column.Item()
                    .Text("Izveštaj o rezervacijama gradskih objekata")
                    .FontSize(20)
                    .SemiBold()
                    .FontColor(Colors.Black);

                column.Item().Height(10);

                column.Item().Text(text =>
                {
                    text.Span("Obuhvat: ").SemiBold();
                    text.Span(_reportTitle);
                });

                column.Item().Text(text =>
                {
                    text.Span("Period: ").SemiBold();
                    text.Span($"{_fromDate:dd.MM.yyyy.} - {_toDate:dd.MM.yyyy.}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Generisano: ").SemiBold();
                    text.Span($"{DateTime.Now:dd.MM.yyyy. HH:mm}h");
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(14);

                if (_sections.Count == 0)
                {
                    column.Item().Element(ComposeEmptyState);
                    return;
                }

                foreach (var section in _sections)
                {
                    if (section.Items == null || section.Items.Count == 0)
                        continue;

                    column.Item().Element(c => ComposeSection(c, section));
                }
            });
        }

        private void ComposeEmptyState(IContainer container)
        {
            container.Text("Nema rezervacija za izabrani period.");
        }

        private void ComposeSection(IContainer container, FacilityReservationReportSection section)
        {
            container.PaddingTop(6).Column(sectionColumn =>
            {
                sectionColumn.Spacing(8);

                sectionColumn.Item()
                    .Text(section.FacilityName)
                    .FontSize(15)
                    .SemiBold()
                    .FontColor(Colors.Black);

                sectionColumn.Item().Element(c => ComposeSectionSummary(c, section));

                sectionColumn.Item()
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);

                foreach (var item in section.Items)
                {
                    sectionColumn.Item().Element(c => ComposeReservationCard(c, item));
                }
            });
        }

        private void ComposeSectionSummary(IContainer container, FacilityReservationReportSection section)
        {
            var totalCount = section.Items.Count;

            container.Text(text =>
            {
                text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));

                text.Span("Ukupno: ").SemiBold();
                text.Span(totalCount.ToString());
            });
        }

        private void ComposeReservationCard(IContainer container, FacilityReservationDto item)
        {
            container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Background(Colors.Grey.Lighten4)
                .Padding(12)
                .Column(column =>
                {
                    column.Spacing(6);

                    AddLabeledText(column, "Objekat: ", item.Facility.Name);
                    AddLabeledText(column, "Kvart: ", item.Neighborhood.Name);
                    AddLabeledText(column, "Početak: ", item.StartDatetime.ToString("dd.MM.yyyy. HH:mm"));
                    AddLabeledText(column, "Kraj: ", item.EndDatetime.ToString("dd.MM.yyyy. HH:mm"));
                });
        }

        private static void AddLabeledText(ColumnDescriptor column, string label, string value)
        {
            column.Item().Text(text =>
            {
                text.Span(label).SemiBold();
                text.Span(value);
            });
        }
    }
}
