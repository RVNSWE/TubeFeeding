using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace TubeFeeding.Pages.Controls
{
    /*
     * This PDF library only supports desktop apps - TO DO: find one that supports mobile apps too.
     */
    public class ExportDoc
    {
        private readonly FeedingSchedule feedingSchedule;
        private PdfWriter writer;
        private PdfDocument pdf;
        private Document document;

        public ExportDoc(FeedingSchedule schedule, string pdfPath)
        {
            feedingSchedule = schedule;
            writer = new PdfWriter(pdfPath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);

            Compose(document);
        }

        public void Compose(Document document)
        {
            string foodName = $"{feedingSchedule.Patient.FoodName}";
            string cansPerDay = $"{feedingSchedule.Patient.CansPerDay}";
            string foodPerMeal = $"{feedingSchedule.Patient.FoodPerMeal}";
            string waterToAddPerMeal = $"{feedingSchedule.Patient.WaterToAddPerMeal}";
            string flushPerMeal = $"{feedingSchedule.Patient.FlushPerMeal}";
            string mealsPerDay = $"{feedingSchedule.Patient.MealsPerDay}";
            string interval = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDay).ToString();
            List<string> schedule = feedingSchedule.FormattedFeedingTimes;

            document.Add(new Paragraph($"Tube Feeding Instructions for " + PatientName())
                .SetFontSize(16)
                .SetUnderline()
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Diet: " + foodName + "                Estimated cans of food per day: " + cansPerDay)
                .SimulateBold());
            document.Add(new Paragraph(foodName + " food to administer each meal: " + foodPerMeal + "ml" + "        Water to add to each meal: " + WaterToAdd())
                .SimulateBold());
            document.Add(new Paragraph("Volume of water per flush: " + flushPerMeal)
                .SimulateBold());
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Feeding Schedule:")
                .SetFontSize(14)
                .SetUnderline()
                .SimulateBold());
            document.Add(new Paragraph("Your pet will need to be fed roughly every "
                + interval
                + " hours, for a total of "
                + mealsPerDay
                + " times per day."));
            document.Add(new Paragraph("A schedule has been estimated as follows:"));
            document.Add(new Paragraph(PrintSchedule(schedule)));
            document.Add(new Paragraph("You may wish to adjust these times to suit you, but please ensure a minimum of one hour between each feed to avoid overloading the stomach and causing regurgitation."));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Preparing the food:")
                .SetFontSize(14)
                .SetUnderline()
                .SimulateBold());
            document.Close();
        }

        private string PatientName()
        {
            string name = $"{feedingSchedule.Patient.PatientName}" + $"{feedingSchedule.Patient.ClientName}";

            return name;
        }

        private string WaterToAdd()
        {
            string waterToAdd = "None";

            if (feedingSchedule.Patient.WaterToAddPerMeal > 0)
            {
                waterToAdd = feedingSchedule.Patient.WaterToAddPerMeal.ToString() + "ml";
            }

            return waterToAdd;
        }

        private string PreparationInstructions()
        {
            string instructions = "";



            return instructions;
        }

        private string PrintSchedule(List<string> schedule)
        {
            string listOfHours = "";

            foreach (string item in schedule)
            {
                listOfHours = listOfHours + item + "    ";
            }

            return listOfHours;
        }
    }
}