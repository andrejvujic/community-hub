using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Services.Dtos.Issues;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CommunityHub.Application.Services.Issues.Reports
{
    public class IssueReportPdfDocument : IDocument
    {
        private readonly string _reportTitle;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly List<BuildingIssueReportSection> _sections;

        public IssueReportPdfDocument(
            string reportTitle,
            DateTime fromDate,
            DateTime toDate,
            List<BuildingIssueReportSection> sections)
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
                    .Text("Izveštaj o prijavljenim problemima")
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
            container.Text("Nema prijavljenih problema za izabrani period.");
        }
        private void ComposeSection(IContainer container, BuildingIssueReportSection section)
        {
            container.PaddingTop(6).Column(sectionColumn =>
            {
                sectionColumn.Spacing(8);

                sectionColumn.Item()
                    .Text(section.BuildingName)
                    .FontSize(15)
                    .SemiBold()
                    .FontColor(Colors.Black);

                sectionColumn.Item().Element(c => ComposeSectionSummary(c, section));

                sectionColumn.Item()
                    .LineHorizontal(1)
                    .LineColor(Colors.Grey.Lighten2);

                foreach (var item in section.Items)
                {
                    sectionColumn.Item().Element(c => ComposeIssueCard(c, item));
                }
            });
        }
        private void ComposeSectionSummary(IContainer container, BuildingIssueReportSection section)
        {
            var totalCount = section.Items.Count;
            var unresolvedCount = section.Items.Count(x => x.Status == IssueStatus.Open);
            var potentiallyResolvedCount = section.Items.Count(x => x.Status == IssueStatus.PotentiallyResolved);
            var resolvedCount = section.Items.Count(x => x.Status == IssueStatus.Resolved);

            container.Text(text =>
            {
                text.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));

                text.Span("Ukupno: ").SemiBold();
                text.Span(totalCount.ToString());

                text.Span(" | ");

                text.Span("Nerešeni: ").SemiBold();
                text.Span(unresolvedCount.ToString());

                text.Span(" | ");

                text.Span("Potencijalno rešeni: ").SemiBold();
                text.Span(potentiallyResolvedCount.ToString());

                text.Span(" | ");

                text.Span("Rešeni: ").SemiBold();
                text.Span(resolvedCount.ToString());
            });
        }
        private void ComposeIssueCard(IContainer container, IssueDto item)
        {
            container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Background(Colors.Grey.Lighten4)
                .Padding(12)
                .Column(itemColumn =>
                {
                    itemColumn.Spacing(6);

                    itemColumn.Item().Text(text =>
                    {
                        text.Span("Stanar: ").SemiBold();
                        text.Span($"{item.Resident!.Name} {item.Resident.Surname}");
                    });

                    itemColumn.Item().Text(text =>
                    {
                        text.Span("Prijavljeno: ").SemiBold();
                        text.Span($"{item.ReportedAt:dd.MM.yyyy.}");
                    });

                    itemColumn.Item().Text(text =>
                    {
                        text.Span("Prioritet: ").SemiBold();
                        text.Span(IssuePriorityFormatter.Format(item.Priority));
                    });

                    itemColumn.Item().Text(text =>
                    {
                        text.Span("Status: ").SemiBold();
                        text.Span(IssueStatusFormatter.Format(item.Status));
                    });

                    itemColumn.Item().Text(text =>
                    {
                        text.Span("Opis: ").SemiBold();
                        text.Span(item.Description);
                    });
                });
        }
    }
}