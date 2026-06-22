using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace CommunityHub.Application.Services.Events
{
    public class EventReportService
    {
        private static readonly string AccentBrush = "#A0B99B";

        static EventReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateEventReport(CommunityEventDto eventData, string citizenName)
        {
            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Column(headerCol =>
                        {
                            headerCol.Spacing(5);
                            headerCol.Item().Text("IZVEŠTAJ O DOGAĐAJU").SemiBold().FontSize(24).FontColor(AccentBrush).AlignCenter();
                            headerCol.Item().Text(eventData.Name).SemiBold().FontSize(18).AlignCenter();
                            headerCol.Item().LineHorizontal(1).LineColor(AccentBrush);
                        });

                    page.Content().PaddingVertical(1, Unit.Centimetre)
                        .Column(content =>
                        {
                            content.Spacing(15);

                            content.Item().Column(info =>
                            {
                                info.Spacing(5);
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Datum: ").SemiBold();
                                    txt.Span(eventData.EventStart.ToString("dd.MM.yyyy"));
                                });
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Vreme: ").SemiBold();
                                    txt.Span(eventData.EventStart.ToString("HH:mm"));
                                });
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Trajanje: ").SemiBold();
                                    txt.Span($"{eventData.DurationMinutes} minuta");
                                });
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Status: ").SemiBold();
                                    txt.Span(GetStatusString(eventData.Status));
                                });
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Organizator: ").SemiBold();
                                    txt.Span($"{eventData.Organizer?.Name} {eventData.Organizer?.Surname}");
                                });
                                info.Item().Text(txt =>
                                {
                                    txt.Span("Kvart: ").SemiBold();
                                    txt.Span(eventData.Neighborhood?.Name ?? "Nije navedeno");
                                });
                            });

                            content.Item().LineHorizontal(1).LineColor(AccentBrush);

                            if (!string.IsNullOrEmpty(eventData.Description))
                            {
                                content.Item().Column(desc =>
                                {
                                    desc.Spacing(5);
                                    desc.Item().Text("Opis događaja:").SemiBold().FontSize(14);
                                    desc.Item().Text(eventData.Description);
                                });
                                content.Item().LineHorizontal(1).LineColor(AccentBrush);
                            }

                            content.Item().Column(volunteer =>
                            {
                                volunteer.Spacing(5);
                                volunteer.Item().Text("VOLONTERI").SemiBold().FontSize(14);
                                volunteer.Item().Text(txt =>
                                {
                                    txt.Span("Potrebno volontera: ").SemiBold();
                                    txt.Span(eventData.MinVolunteers.ToString());
                                });
                                volunteer.Item().Text(txt =>
                                {
                                    txt.Span("Prijavljeno volontera: ").SemiBold();
                                    txt.Span(eventData.RegisteredCount.ToString());
                                });
                            });

                            if (eventData.Registrations?.Any() == true)
                            {
                                content.Item().Column(reg =>
                                {
                                    reg.Spacing(5);
                                    reg.Item().Text("Prijavljeni volonteri:").SemiBold().FontSize(14);

                                    reg.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(3);
                                            columns.RelativeColumn(1);
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Ime i prezime").SemiBold();
                                            header.Cell().Text("Prisustvo").SemiBold().AlignCenter();
                                        });

                                        foreach (var registration in eventData.Registrations)
                                        {
                                            table.Cell().Text($"{registration.Citizen?.Name} {registration.Citizen?.Surname}");
                                            table.Cell().Text(registration.Attended == true ? "+" :registration.Attended == false ? "-" :"?").AlignCenter();
                                        }
                                    });
                                });
                                content.Item().LineHorizontal(1).LineColor(AccentBrush);
                            }

                            if (eventData.Items?.Any() == true)
                            {
                                content.Item().Column(items =>
                                {
                                    items.Spacing(5);
                                    items.Item().Text("POTREBNE STAVKE").SemiBold().FontSize(14);

                                    foreach (var item in eventData.Items)
                                    {
                                        items.Item().Text($"• {item.Name}");
                                    }
                                });
                            }
                        });

                    page.Footer().AlignRight()
                        .Text(text =>
                        {
                            text.Span("Izveštaj generisan: ").FontSize(10);
                            text.Span(DateTime.Now.ToString("d.M.yyyy. HH:mm")).FontSize(10);
                            text.Span($" | Korisnik: {citizenName}").FontSize(10);
                        });
                });
            })
            .GeneratePdf(stream);

            return stream.ToArray();
        }

        private string GetStatusString(EventStatus status)
        {
            return status switch
            {
                EventStatus.Preparation => "U pripremi",
                EventStatus.Scheduled => "Zakazan",
                EventStatus.Finished => "Završen",
                EventStatus.Cancelled => "Otkazan",
                _ => status.ToString()
            };
        }

        public void SaveReportToFile(byte[] pdfData, string filePath)
        {
            File.WriteAllBytes(filePath, pdfData);
        }
    }
}
