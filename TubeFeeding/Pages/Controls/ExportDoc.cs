using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TubeFeeding.Pages.Controls
{
    public class ExportDoc : IDocument
    {
        private readonly FeedingSchedule _schedule;

        public ExportDoc(FeedingSchedule schedule)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _schedule = schedule;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Header().Text("Tube Feeding Schedule").Bold().FontSize(20);

                page.Content().Column(col =>
                {
                    col.Item().Text($"Patient: {_schedule.Patient.PatientName}          Client: {_schedule.Patient.ClientName}").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Species: {_schedule.Patient.Species}          Body weight (kg): {_schedule.Patient.BodyWeight}").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Food: {_schedule.Food.Name}          ");
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Pg. ");
                    x.CurrentPageNumber();
                });
            });
        }

        private void ComposeBody(PageDescriptor page)
        {
            page.Content().Row(row =>
            {

            });
        }
    }
}