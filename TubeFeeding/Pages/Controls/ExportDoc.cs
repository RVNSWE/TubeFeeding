using iText.Kernel.Pdf;
using iText.Layout;

namespace TubeFeeding.Pages.Controls
{
    /*
     * This PDF library only supports desktop apps - TO DO: find one that supports mobile apps too.
     */
    public class ExportDoc
    {
        private readonly FeedingSchedule feedingSchedule;

        public ExportDoc(FeedingSchedule schedule)
        {
            feedingSchedule = schedule;
        }

        public void Compose(Document document)
        {

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
    }
}