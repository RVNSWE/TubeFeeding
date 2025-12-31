using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TubeFeeding.Pages.Controls
{
    /*
     * This PDF library only supports desktop apps - TO DO: find one that supports mobile apps too.
     */
    public class ExportDoc : IDocument
    {
        private readonly FeedingSchedule feedingSchedule;

        public ExportDoc(FeedingSchedule schedule)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            feedingSchedule = schedule;
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

                    col.Item().Text($"Patient: {feedingSchedule.Patient.PatientName}          Client: {feedingSchedule.Patient.ClientName}          Species: {feedingSchedule.Patient.Species}          Body weight: {feedingSchedule.Patient.BodyWeight}kg").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Diet to feed: {feedingSchedule.Patient.FoodName}").Bold();

                    col.Item().Text(" ");

                    col.Item().Text($"Number of meals per day: {feedingSchedule.Patient.MealsPerDay}          Estimated packs of food used per day: {feedingSchedule.Patient.CansPerDay}");

                    col.Item().Text(" ");

                    col.Item().Text($"Food per meal: {feedingSchedule.Patient.FoodPerMeal}ml          Water to add to each meal: {WaterToAdd()}");

                    col.Item().Text(" ");

                    col.Item().Text($"Water per flush (before and after each meal): {feedingSchedule.Patient.FlushPerMeal}ml");

                    col.Item().Text(" ");

                    col.Item().Text(PrintSchedule(feedingSchedule.FormattedFeedingTimes));
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

            if (feedingSchedule.Patient.WaterToAddPerMeal > 0)
            {
                waterToAdd = feedingSchedule.Patient.WaterToAddPerMeal.ToString() + "ml";
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