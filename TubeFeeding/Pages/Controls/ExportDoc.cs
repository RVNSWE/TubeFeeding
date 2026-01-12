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
        private readonly string foodName;
        private readonly double cansPerDay;
        private readonly double cansPerDayOne;
        private readonly double cansPerDayTwo;
        private readonly double foodPerMeal;
        private readonly double foodPerMealDayOne;
        private readonly double foodPerMealDayTwo;
        private readonly double waterToAddPerMealDayOne;
        private readonly double waterToAddPerMealDayTwo;
        private readonly double waterToAddPerMeal;
        private readonly double flushPerMeal;
        private readonly double interval;
        private readonly double intervalDayOne;
        private readonly double intervalDayTwo;
        private readonly List<string> scheduleList;
        private readonly List<string> scheduleListDayOne;
        private readonly List<string> scheduleListDayTwo;

        public ExportDoc(FeedingSchedule schedule, string pdfPath)
        {
            feedingSchedule = schedule;
            writer = new PdfWriter(pdfPath);
            pdf = new PdfDocument(writer);
            document = new Document(pdf);

            patientName = $"{feedingSchedule.Patient.PatientName}";
            foodName = $"{feedingSchedule.Patient.FoodName}";
            cansPerDay = feedingSchedule.Patient.CansPerDay;
            cansPerDayOne = feedingSchedule.Patient.CansPerDayOne;
            cansPerDayTwo = feedingSchedule.Patient.CansPerDayTwo;
            foodPerMeal = feedingSchedule.Patient.FoodPerMeal;
            foodPerMealDayOne = feedingSchedule.Patient.FoodPerMealDayOne;
            foodPerMealDayTwo = feedingSchedule.Patient.FoodPerMealDayTwo;
            waterToAddPerMealDayOne = feedingSchedule.Patient.WaterToAddPerMealDayOne;
            waterToAddPerMealDayTwo = feedingSchedule.Patient.WaterToAddPerMealDayTwo;
            waterToAddPerMeal = feedingSchedule.Patient.WaterToAddPerMeal;
            flushPerMeal = feedingSchedule.Patient.VolPerFlush;
            interval = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDay);
            intervalDayOne = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDayOne);
            intervalDayTwo = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDayTwo);
            scheduleList = feedingSchedule.FormattedFeedingTimes;
            scheduleListDayOne = feedingSchedule.FormattedFeedingTimesDayOne;
            scheduleListDayTwo = feedingSchedule.FormattedFeedingTimesDayTwo;

            Compose(document);
        }

        public void Compose(Document document)
        {
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
            p.Add(new Text("Diet: "));
            p.Add(new Text(foodName)
                .SimulateBold());
            document.Add(p
                .SetFontSize(14));

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day One:")
                .SimulateBold()
                .SetUnderline());

            document.Add(PrintFoodPerMealForDay(1));
            document.Add(PrintContainersUsedForDay(1));

            p = new Paragraph();
            p.Add(new Text("Example "
                + intervalDayOne
                + " hourly feeding schedule:"));
            document.Add(p);

            document.Add(PrintSchedule(scheduleListDayOne)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day Two:")
                .SimulateBold()
                .SetUnderline());

            document.Add(PrintFoodPerMealForDay(2));
            document.Add(PrintContainersUsedForDay(2));

            p = new Paragraph();
            p.Add(new Text("Example "
                + intervalDayTwo
                + " hourly feeding schedule:"));
            document.Add(p);

            document.Add(PrintSchedule(scheduleListDayTwo)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day Three Onwards:")
                .SimulateBold()
                .SetUnderline());

            document.Add(PrintFoodPerMealForDay(3));
            document.Add(PrintContainersUsedForDay(3));

            p = new Paragraph();
            p.Add(new Text("Example "
                + interval
                + " hourly feeding schedule:"));
            document.Add(p);

            document.Add(PrintSchedule(scheduleList)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("It is vital you follow the above feeding plan in order to prevent refeeding syndrome. ")
                .SimulateBold());
            p.Add(new Text("Refeeding syndrome is a life-threatening condition caused by reintroducing food too rapidly after prolonged periods of not eating, which is why it must be done gradually over the first three days."));
            document.Add(p);

            document.Add(new Paragraph("You are free to adjust the times at which you feed "
                + patientName
                + " to suit you, but please spread them out as much as possible and allow a minimum of one hour between feeds to avoid overloading the stomach and causing regurgitation."));

            p = new Paragraph();
            p.Add(new Text("Unless advised otherwise by your clinic, fresh water should remain available at all times and you should leave the calculated volume of food in "
                + patientName
                + "'s normal bowl between feeds to allow the opportunity to eat unassisted. If it has all been eaten by the time of their next scheduled feed, offer a fresh bowl of food instead of tube feeding. Otherwise, draw up any remaining uneaten food into a syringe (or the same volume of fresh food if it's looking dry or stale). This is the volume of food you will need to administer to "
                + patientName
                + " this session."));
            document.Add(p);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Preparing the food")
                .SetFontSize(16)
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

            document.Add(new Paragraph("Administering the food")
                .SetFontSize(16)
                .SetUnderline()
                .SimulateBold());

            p = new Paragraph();
            p.Add(new Text("1.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Pinch the feeding tube to prevent food from leaking out when you remove the cap. Attach an empty syringe to the feeding tube port, stop pinching, and gently draw back on the plunger. You should feel some resistance, and the plunger should return to its starting position when you let go of it."));
            p.Add(new Text(" If this does not happen, it may mean the tube has become displaced. STOP immediately, and contact the clinic for advice.")
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
            p.Add(new Text(" If "
                + patientName
                + " starts coughing, gagging, retching, or appearing uncomfortable while flushing, STOP immediately and contact the clinic for advice.")
                .SimulateBold());
            document.Add(p);

            document.Add(new Paragraph(" "));

            p = new Paragraph();
            p.Add(new Text("3.")
                .SetUnderline()
                .SimulateBold());
            p.Add(new Text(" Slowly administer the prepared volume of food through the tube."));
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

        private Paragraph PrintFoodPerMealForDay(int day)
        {
            string waterToAdd = "None";

            double WaterToAddToday = day switch
            {
                1 => waterToAddPerMealDayOne,
                2 => waterToAddPerMealDayTwo,
                _ => waterToAddPerMeal,
            };

            if (WaterToAddToday > 0)
            {
                waterToAdd = WaterToAddToday.ToString() + "ml";
            }

            double foodPerMealToday = day switch
            {
                1 => foodPerMealDayOne,
                2 => foodPerMealDayTwo,
                _ => foodPerMeal,
            };

            double mealsPerDayToday = day switch
            {
                1 => feedingSchedule.Patient.MealsPerDayOne,
                2 => feedingSchedule.Patient.MealsPerDayTwo,
                _ => feedingSchedule.Patient.MealsPerDay,
            };

            Paragraph p = new();

            if (waterToAdd != "None")
            {
                p.Add(new Text("Food per meal: "));
                p.Add(new Text(foodPerMealToday.ToString()
                    + "ml")
                    .SimulateBold());
                p.AddTabStops(new TabStop(10, iText.Layout.Properties.TabAlignment.RIGHT));
                p.Add(new iText.Layout.Element.Tab());
                p.Add(new Text("Additional water per meal: "));
                p.Add(new Text(waterToAdd)
                    .SimulateBold());
                p.AddTabStops(new TabStop(10, iText.Layout.Properties.TabAlignment.RIGHT));
                p.Add(new iText.Layout.Element.Tab());
                p.Add(new Text("Meals per day: "));
                p.Add(new Text(mealsPerDayToday.ToString())
                    .SimulateBold());
            }
            else
            {
                p.Add(new Text("Food per meal: "));
                p.Add(new Text(foodPerMealToday.ToString()
                    + "ml")
                    .SimulateBold());
                p.AddTabStops(new TabStop(10, iText.Layout.Properties.TabAlignment.RIGHT));
                p.Add(new iText.Layout.Element.Tab());
                p.Add(new Text("Meals per day: "));
                p.Add(new Text(mealsPerDayToday.ToString())
                    .SimulateBold());
            }

            return p;
        }

        private Paragraph PrintContainersUsedForDay(int day)
        {
            double cansPerDayToday = day switch
            {
                1 => cansPerDayOne,
                2 => cansPerDayTwo,
                _ => cansPerDay,
            };

            string containersPerDay = cansPerDayToday switch
            {
                > 1 => " containers",
                1 => " container",
                _ => " of a container",
            };

            Paragraph p = new();

            p.Add(new Text("Estimated total food used per day: "));
            p.Add(new Text(cansPerDayToday.ToString()
                + containersPerDay)
                .SimulateBold());

            return p;
        }

        private Paragraph PrintPreparationInstructions()
        {
            bool waterToAddDayOne = waterToAddPerMealDayOne > 0;
            bool waterToAddDayTwo = waterToAddPerMealDayTwo > 0;
            bool waterToAdd = waterToAddPerMeal > 0;

            Paragraph p = new();
            
            p.Add(new Text("Prepare two syringes with "));
            p.Add(new Text(flushPerMeal.ToString()
                + "ml")
            .SimulateBold());
            p.Add(new Text(" of plain tap water in each, which will be used to flush the tube before and after feeding."));

            if (waterToAddDayOne || waterToAddDayTwo || waterToAdd)
            {
                p.Add(new Text(" If "
                    + patientName
                    + " has not been drinking anything, you will also need to mix in additional water as listed in the feeding plan before drawing it up."));
            }

            p.Add(new Text(" If "
                + patientName
                + " has been offered food and eaten some but not all of it, draw the rest of it up into a syringe (or the same amount of fresh food if it's looking dry or stale). If "
                + patientName
                + " has not eaten anything at all since the last feed, you will need to draw up the full volume of food listed for this meal. "));

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