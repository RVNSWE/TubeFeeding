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
            string cansPerDayOne = $"{feedingSchedule.Patient.CansPerDayOne}";
            string cansPerDayTwo = $"{feedingSchedule.Patient.CansPerDayTwo}";
            string foodPerMeal = $"{feedingSchedule.Patient.FoodPerMeal}";
            string foodPerMealDayOne = $"{feedingSchedule.Patient.FoodPerMealDayOne}";
            string foodPerMealDayTwo = $"{feedingSchedule.Patient.FoodPerMealDayTwo}";
            string flushPerMeal = $"{feedingSchedule.Patient.VolPerFlush}";
            string interval = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDay).ToString();
            string intervalDayOne = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDayOne).ToString();
            string intervalDayTwo = Globals.CalculateInterval(feedingSchedule.Patient.MealsPerDayTwo).ToString();
            List<string> schedule = feedingSchedule.FormattedFeedingTimes;
            List<string> scheduleDayOne = feedingSchedule.FormattedFeedingTimesDayOne;
            List<string> scheduleDayTwo = feedingSchedule.FormattedFeedingTimesDayTwo; // TO DO: ADD REFEEDING SCHEDULES INTO PDF

            Paragraph p = new();
            p.Add(new Text("Tube Feeding Instructions for "
                + PrintPatientName()));
            document.Add(p
                .SetFontSize(20)
                .SetUnderline()
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Feeding Schedule")
                .SetFontSize(16)
                .SetUnderline()
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            p = new Paragraph();
            p.Add(new Text("Diet: "));
            p.Add(new Text(foodName)
                .SimulateBold());
            document.Add(p);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day One")
                .SetUnderline()
                .SimulateBold());

            document.Add(PrintFoodUsedForDay(1));
            document.Add(PrintFoodPerMealForDay(1));
            document.Add(PrintWaterAddedPerMealForDay(1));

            p = new Paragraph();
            p.Add(new Text("Example schedule for feeding every "
                + intervalDayOne
                + " hours:"));
            document.Add(p);

            document.Add(PrintSchedule(scheduleDayOne)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day Two")
                .SetUnderline()
                .SimulateBold());

            document.Add(PrintFoodUsedForDay(2));
            document.Add(PrintFoodPerMealForDay(2));
            document.Add(PrintWaterAddedPerMealForDay(2));

            p = new Paragraph();
            p.Add(new Text("Example schedule for feeding every "
                + intervalDayTwo
                + " hours:"));
            document.Add(p);

            document.Add(PrintSchedule(scheduleDayTwo)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Day Three Onwards")
                .SetUnderline()
                .SimulateBold());

            document.Add(PrintFoodUsedForDay(3));
            document.Add(PrintFoodPerMealForDay(3));
            document.Add(PrintWaterAddedPerMealForDay(3));

            p = new Paragraph();
            p.Add(new Text("Example schedule for feeding every "
                + interval
                + " hours:"));
            document.Add(p);

            document.Add(PrintSchedule(schedule)
                .SimulateBold());

            document.Add(new Paragraph(" "));

            document.Add(PrintScheduleInstructions());

            p = new Paragraph();
            p.Add(new Text("Unless advised otherwise by your clinic, you should leave a fresh "
                + foodPerMeal.ToString()
                + "ml of food available for "
                + patientName
                + " between each feeding session in case they are able to eat unassisted. If it has all been eaten by the time the next feed is due, you can skip this feed and offer another "
                + foodPerMeal.ToString()
                + "ml of food. Otherwise, draw up the remaining uneaten food into a syringe (or the same volume of fresh food). This is the volume of food you will need to administer to "
                + patientName
                + " this session."));
            document.Add(p);

            document.Add(new Paragraph("You are free to adjust these times to suit you, but please allow a minimum of one hour between feeds to avoid overloading the stomach and causing regurgitation."));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Preparing the food")
                .SetFontSize(16)
                .SetUnderline()
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

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
                .SimulateBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

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

        private Paragraph PrintFoodPerMealForDay(int day)
        {
            double foodPerMeal = day switch
            {
                1 => feedingSchedule.Patient.FoodPerMealDayOne,
                2 => feedingSchedule.Patient.FoodPerMealDayTwo,
                _ => feedingSchedule.Patient.FoodPerMeal,
            };

            double cansPerDay = day switch
            {
                1 => feedingSchedule.Patient.CansPerDayOne,
                2 => feedingSchedule.Patient.CansPerDayTwo,
                _ => feedingSchedule.Patient.CansPerDay,
            };

            Paragraph p = new();

            p.Add(new Text("Food volume per meal: "));
            p.Add(new Text(foodPerMeal.ToString()
                + "ml")
                .SimulateBold());

            return p;
        }

        private Paragraph PrintWaterAddedPerMealForDay(int day)
        {
            string waterToAdd = "None";

            if (feedingSchedule.Patient.WaterToAddPerMeal > 0)
            {
                waterToAdd = day switch
                {
                    1 => feedingSchedule.Patient.WaterToAddPerMealDayOne.ToString() + "ml",
                    2 => feedingSchedule.Patient.WaterToAddPerMealDayTwo.ToString() + "ml",
                    _ => feedingSchedule.Patient.WaterToAddPerMeal.ToString() + "ml",
                };
            }

            Paragraph p = new();

            p.Add(new Text("Water to add per meal: "));
            p.Add(new Text(waterToAdd)
                .SimulateBold());

            return p;
        }

        private Paragraph PrintFoodUsedForDay(int day)
        {
            double cansPerDay = day switch
            {
                1 => feedingSchedule.Patient.CansPerDayOne,
                2 => feedingSchedule.Patient.CansPerDayTwo,
                _ => feedingSchedule.Patient.CansPerDay,
            };

            string description = day switch
            {
                1 => "Estimated containers of food used on day one: ",
                2 => "Estimated containers of food used on day two: ",
                _ => "Estimated containers of food used from day three onwards: ",
            };

            Paragraph p = new();

            p.Add(new Text(description));
            p.Add(new Text(cansPerDay.ToString())
                .SimulateBold());

            return p;
        }

        private Paragraph PrintScheduleInstructions()
        {
            double cansPerDay = feedingSchedule.Patient.CansPerDay;
            string foodName = feedingSchedule.Patient.FoodName;
            double mealsPerDay = feedingSchedule.Patient.MealsPerDay;
            double foodPerMeal = feedingSchedule.Patient.FoodPerMeal;

            Paragraph p = new();

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
            double waterToAddPerMeal = feedingSchedule.Patient.WaterToAddPerMeal;
            double foodPerMeal = feedingSchedule.Patient.FoodPerMeal;
            double flushPerMeal = feedingSchedule.Patient.VolPerFlush;
            string foodName = feedingSchedule.Patient.FoodName;

            Paragraph p = new();

            if (waterToAddPerMeal > 0)
            {
                p.Add(new Text("Mix "));
                p.Add(new Text(waterToAddPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of water into "));
                p.Add(new Text(foodPerMeal.ToString()
                    + "ml")
                .SimulateBold());
                p.Add(new Text(" of food, then draw it up into a syringe. Prepare two other syringes with "));
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