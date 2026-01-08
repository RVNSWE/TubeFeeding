using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace TubeFeeding.Pages.Controls
{
    public class ExportDoc
    {
        private readonly FeedingSchedule feedingSchedule;
        private readonly PdfWriter writer;
        private readonly PdfDocument pdf;
        private readonly Document document;
        private readonly string patientName;

        public ExportDoc(FeedingSchedule schedule, string pdfPath)
        {
            feedingSchedule = schedule;
            writer = new PdfWriter(pdfPath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);

            patientName = $"{feedingSchedule.Patient.PatientName}";

            Compose(document);
        }

        public void Compose(Document document)
        {
            string foodName = $"{feedingSchedule.Patient.FoodName}";
            string cansPerDay = $"{feedingSchedule.Patient.CansPerDay}";
            string foodPerMeal = $"{feedingSchedule.Patient.FoodPerMeal}";
            string flushPerMeal = $"{feedingSchedule.Patient.VolPerFlush}";
            string mealsPerDay = $"{feedingSchedule.Patient.MealsPerDay}";
            string interval = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDay).ToString();
            List<string> schedule = feedingSchedule.FormattedFeedingTimes;

            Paragraph p = new();
            p.Add(new Text("Tube Feeding Instructions for "
                + PrintPatientName()));
            document.Add(p
                .SetFontSize(18)
                .SetUnderline()
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            
            p = new Paragraph();
            p.Add(new Text("Diet: ")
                .SimulateBold());
            p.Add(new Text(foodName));
            p.AddTabStops(new TabStop(1000, iText.Layout.Properties.TabAlignment.RIGHT));
            p.Add(new iText.Layout.Element.Tab());
            p.Add(new Text("Estimated cans of food per day: ")
                .SimulateBold());
            p.Add(new Text(cansPerDay));
            document.Add(p);

            p = new Paragraph();
            p.Add(new Text("Food to administer each meal: ")
                .SimulateBold());
            p.Add(new Text(foodPerMeal
                + "ml"));
            p.AddTabStops(new TabStop(1000, iText.Layout.Properties.TabAlignment.RIGHT));
            p.Add(new iText.Layout.Element.Tab());
            p.Add(new Text("Water to add to each meal: ")
                .SimulateBold());
            p.Add(new Text(PrintWaterToAdd()));
            document.Add(p);

            p = new Paragraph();
            p.Add(new Text("Volume of water per flush: ")
                .SimulateBold());
            p.Add(new Text(flushPerMeal
                + "ml"));
            document.Add(p);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Feeding Schedule:")
                .SetFontSize(14)
                .SetUnderline()
                .SimulateBold());

            document.Add(PrintScheduleInstructions());

            p = new Paragraph();
            p.Add(new Text("Unless advised otherwise by your clinic, you should leave a fresh "
                + foodPerMeal.ToString()
                + "ml of food available for "
                + patientName
                + " between each feeding session in case they are able to eat unassisted. If they have eaten it all by the time the next feed is due, you can skip this feed and offer them another "
                + foodPerMeal.ToString()
                + "ml of food. Otherwise, draw up the remaining uneaten food into a syringe (or the same volume of fresh food). This is the volume of food you will need to administer to "
                + patientName
                + " this session."));
            document.Add(p);

            p = new Paragraph();
            p.Add(new Text("The following times are offered as a suggested schedule for feeding every "
                + interval
                + " hours:"));
            document.Add(p);

            document.Add(PrintSchedule(schedule)
                .SetUnderline());

            document.Add(new Paragraph("You are free to adjust these times to suit you, but please allow a minimum of one hour between feeds to avoid overloading the stomach and causing regurgitation."));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Preparing the food:")
                .SetFontSize(14)
                .SetUnderline()
                .SimulateBold());

            document.Add(PrintPreparationInstructions());
            p = new Paragraph();
            p.Add(new Text("Place the filled syringes into a jug of "));
            p.Add(new Text("warm")
                .SimulateBold());
            p.Add(new Text(" (not hot!) water until they reach body temperature. "));
            p.Add(new Text("DO NOT MICROWAVE,")
                .SimulateBold());
            p.Add(new Text(" as this can create pockets of hot liquid that may scald "
                + patientName
                + "."));
            document.Add(p);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Administering the food:")
                .SetFontSize(14)
                .SetUnderline()
                .SimulateBold());

            p = new Paragraph();
            p.Add(new Text("1.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Pinch the feeding tube to prevent food from leaking out when you remove the cap. Attach an empty syringe to the feeding tube port, stop pinching, and then gently draw back on the plunger. You should feel some resistance, and the plunger should return to its starting position when you let go of it."));
            document.Add(p);
            p = new Paragraph();
            p.Add(new Text("If this does not happen, it may mean the tube has become displaced. STOP immediately, and contact the clinic for advice.")
                .SimulateBold());
            document.Add(p);

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("2.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Slowly")
                .SimulateBold());
            p.Add(new Text(" flush the feeding tube with "));
            p.Add(new Text(flushPerMeal
                + "ml")
                .SimulateBold());
            p.Add(new Text(" of water before administering any food."));
            document.Add(p);
            p = new Paragraph();
            p.Add(new Text("If "
                + patientName
                + " starts coughing, gagging, retching, or otherwise showing signs of discomfort while flushing, STOP immediately and contact the clinic for advice.")
                .SimulateBold());
            document.Add(p);

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("3.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Slowly administer the prepared volume of food through the tube."));
            document.Add(p);
            p = new Paragraph();
            p.Add(new Text("You may notice "
                + patientName
                + " swallowing as you do this. This is normal, as the food is being administered into the oesophagus rather than directly into the stomach. If they regurgitate, slow down the rate of administration. "));
            p.Add(new Text("If regurgitation continues, stop feeding and contact the clinic for advice.")
                .SimulateBold());
            document.Add(p);

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("4.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Slowly flush the tube with "));
            p.Add(new Text(flushPerMeal
                + "ml")
                .SimulateBold());
            p.Add(new Text(" of water again to clear it of any residual food."));
            document.Add(p);

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("5.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Place the cap back on the feeding tube and wipe away any food from the outside of the tube with a clean, damp cloth. Ensure the outside of the tube is dry before tucking it away again. Rinse the used syringes with water to clean them ready for the next feed."));
            document.Add(p);

            document.Close();
        }

        private string PrintPatientName()
        {
            string name = patientName + " " + $"{feedingSchedule.Patient.ClientName}";

            return name;
        }

        private string PrintWaterToAdd()
        {
            string waterToAdd = "None";

            if (feedingSchedule.Patient.WaterToAddPerMeal > 0)
            {
                waterToAdd = feedingSchedule.Patient.WaterToAddPerMeal.ToString() + "ml";
            }

            return waterToAdd;
        }

        private Paragraph PrintScheduleInstructions()
        {
            Paragraph p = new();
            double cansPerDay = feedingSchedule.Patient.CansPerDay;
            string foodName = feedingSchedule.Patient.FoodName;
            double mealsPerDay = feedingSchedule.Patient.MealsPerDay;
            double foodPerMeal = feedingSchedule.Patient.FoodPerMeal;

            p.Add(new Text(patientName
                + " will need to consume "));
            p.Add(new Text(foodPerMeal.ToString()
                + "ml")
                .SimulateBold());
            p.Add(new Text(" of "));
            p.Add(new Text(foodName)
                .SimulateBold());
            p.Add(new Text(" food "));
            p.Add(new Text(mealsPerDay.ToString())
                .SimulateBold());
            p.Add(new Text(" times per day in order to meet their nutritional needs. This is expected to equal around "));
            p.Add(new Text(cansPerDay.ToString())
                .SimulateBold());

            if (cansPerDay > 1)
            {
                p.Add(new Text(" containers of "));
            }
            else if (cansPerDay == 1)
            {
                p.Add(new Text(" container of "));
            }
            else
            {
                p.Add(new Text(" of a container of "));
            }

            p.Add(new Text("food per day."));

            return p;
        }

        private Paragraph PrintPreparationInstructions()
        {
            Paragraph p = new();

            double waterToAddPerMeal = feedingSchedule.Patient.WaterToAddPerMeal;
            double foodPerMeal = feedingSchedule.Patient.FoodPerMeal;
            double flushPerMeal = feedingSchedule.Patient.VolPerFlush;
            string foodName = feedingSchedule.Patient.FoodName;

            if (feedingSchedule.Patient.WaterToAddPerMeal > 0)
            {
                p.Add(new Text("Mix "));
                p.Add(new Text(waterToAddPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of water into "));
                p.Add(new Text(foodPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of food, and draw this up into a syringe. Prepare two other syringes with "));
                p.Add(new Text(flushPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of plain tap water in each, which will be used to flush the tube before and after feeding."));
            }
            else
            {
                p.Add(new Text("Prepare two syringes with "));
                p.Add(new Text(flushPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of plain tap water in each, which will be used to flush the tube before and after feeding. Prepare a separate syringe containing any offered "));
                p.Add(new Text(foodName)
                .SimulateBold());
                p.Add(new Text(" food left uneaten since the last scheduled feed (or the same volume of fresh food). If "
                    + patientName
                    + " has eaten nothing at all since the last scheduled feed, this will be "));
                p.Add(new Text(foodPerMeal
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of food."));
            }

                return p;
        }

        private static Paragraph PrintSchedule(List<string> schedule)
        {
            Paragraph p = new();
            p.AddTabStops(new TabStop(10, iText.Layout.Properties.TabAlignment.RIGHT));

            foreach (string item in schedule)
            {
                p.Add(new Text(item));
                p.Add(new iText.Layout.Element.Tab());
            }

            return p;
        }
    }
}