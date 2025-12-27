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
                    col.Item().Text(" ");

                    col.Item().Text($"Patient: {_schedule.Patient.PatientName}          Client: {_schedule.Patient.ClientName}          Species: {_schedule.Patient.Species}          Body weight: {_schedule.Patient.BodyWeight}kg").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Diet to feed: {_schedule.Patient.FoodName}").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Number of meals per day: {_schedule.Patient.MealsPerDay}          Estimated packs of food used per day: {_schedule.Patient.CansPerDay}");

                    col.Item().Text(" ");

                    col.Item().Text($"Food per meal: {_schedule.Patient.FoodPerMeal}ml          Water to add per meal: {WaterToAdd()}");

                    col.Item().Text(" ");

                    col.Item().Text($"Water per flush (before and after each meal): {_schedule.Patient.FlushPerMeal}ml");

                    col.Item().Text(" ");

                    col.Item().Text(PrintSchedule(_schedule.FormattedFeedingTimes));
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Pg. ");
                    x.CurrentPageNumber();
                });
            });
        }

        public string WaterToAdd()
        {
            string waterToAdd = "None";

            if (_schedule.Patient.WaterToAddPerMeal > 0)
            {
                waterToAdd = _schedule.Patient.WaterToAddPerMeal.ToString() + "ml";
            }

            return waterToAdd;
        }

        public string PrintSchedule(List<string> schedule)
        {
            string listOfHours = "";

            foreach (string item in schedule)
            {
                listOfHours = listOfHours + item + "    ";
            }

            return listOfHours;
        }

        private void ComposeBody(PageDescriptor page)
        {
            page.Content().Row(row =>
            {

            });
        }
    }
}