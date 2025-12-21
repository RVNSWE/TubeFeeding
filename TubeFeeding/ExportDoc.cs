using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TubeFeeding
{
    class ExportDoc : IDocument
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

                page.Content().Column(col =>
                {
                    // Title
                    col.Item().Text($"Tube Feeding Schedule").FontSize(20).Bold();

                    col.Item().Text(" "); // Empty line

                    col.Item().Text($"Food: {_schedule.Food.Name}").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Patient: {_schedule.Schedule.PatientName}          Client: {_schedule.Schedule.ClientName}");
                });
            });
        }
    }
}
