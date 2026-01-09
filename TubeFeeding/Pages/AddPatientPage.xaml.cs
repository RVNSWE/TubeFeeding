using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class AddPatientPage : ContentPage
{
    private const int MAX_ML_PER_KG_DAY_ONE = 10;
    private const int MAX_ML_PER_KG_DAY_TWO = 15;
    private const int MAX_ML_PER_KG = 20;

    public AddPatientPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        btnSave.Clicked += async (s, e) => await SaveNewSchedule(
            newPatientName.Text,
            newClientName.Text,
            newSpecies.Text,
            newBodyWeight.Text,
            newFoodName.Text,
            newKcal.Text,
            newNetWeight.Text,
            newWaterPercentage.Text
            );

        btnCancel.Clicked += (s, e) => Globals.GoToList();
    }

    public async Task SaveNewSchedule(
        string patientName,
        string clientName,
        string species,
        string rawBodyWeight,
        string foodName,
        string rawKcal,
        string rawNetWeight,
        string rawWaterPercentage
        )
    {
        double bodyWeight = 0;
        double kcal = 0;
        double netWeight = 0;
        double waterPercentage = 0;

        patientName = Globals.FormatString(patientName);
        clientName = Globals.FormatString(clientName);
        species = Globals.FormatString(species);
        rawBodyWeight = Globals.FormatString(rawBodyWeight);
        foodName = Globals.FormatString(foodName);
        rawKcal = Globals.FormatString(rawKcal);
        rawNetWeight = Globals.FormatString(rawNetWeight);
        rawWaterPercentage = Globals.FormatString(rawWaterPercentage);

        if (Globals.IsStringEmpty(patientName))
        {
            await DisplayAlertAsync("No patient name entered", "Please enter the patient's name.", "OK");
        }
        else if (Globals.IsStringEmpty(clientName))
        {
            await DisplayAlertAsync("No client name entered", "Please enter the client's name.", "OK");
        }
        else if (Globals.IsStringEmpty(species))
        {
            await DisplayAlertAsync("No species entered", "Please enter the species.", "OK");
        }
        else if (rawBodyWeight.Length > 0 && !double.TryParse(rawBodyWeight, out bodyWeight))
        {
            await DisplayAlertAsync("Invalid body weight", "Please enter the patient body weight as either a whole number or decimal.", "OK");
        }
        else if (Globals.IsStringEmpty(foodName))
        {
            await DisplayAlertAsync("No food name entered", "Please enter the name of the food.", "OK");
        }
        else if (rawKcal.Length > 0 && !double.TryParse(rawKcal, out kcal))
        {
            await DisplayAlertAsync("Invalid kcal", "Please enter the total kcal per container of food as either a whole number or decimal.", "OK");
        }
        else if (rawNetWeight.Length > 0 && !double.TryParse(rawNetWeight, out netWeight))
        {
            await DisplayAlertAsync("Invalid net weight", "Please enter the net (total) weight (g) or volume (ml) of food per container as either a whole number or decimal.", "OK");
        }
        else if (rawWaterPercentage.Length > 0 && !double.TryParse(rawWaterPercentage, out waterPercentage))
        {
            await DisplayAlertAsync("Invalid dry weight", "Please enter the dry (dehydrated) weight (g) or volume (ml) of food per container as either a whole number or decimal.", "OK");
        }
        else
        {
            Globals.GoToList(); // So "back" from the details page navigates back to the list instead of here

            double kcalPerMl = kcal * 0.001;
            double waterContent = waterPercentage * 0.01;
            double rER = Globals.CalculateRER(bodyWeight, species);
            double flushPerMeal = Globals.GetFlushPerMeal(bodyWeight);
            double foodPerDay = rER / kcalPerMl;
            double foodPerDayOne = foodPerDay * 0.33;
            double foodPerDayTwo = foodPerDay * 0.66;

            double waterPerDay = Globals.CalculateTotalFluidRequirement(
                    bodyWeight,
                    species,
                    foodPerDay,
                    waterContent
                    );
            double waterPerDayOne = Globals.CalculateTotalFluidRequirement(
                    bodyWeight,
                    species,
                    foodPerDayOne,
                    waterContent
                    );
            double waterPerDayTwo = Globals.CalculateTotalFluidRequirement(
                    bodyWeight,
                    species,
                    foodPerDayTwo,
                    waterContent
                    );

            double totalFoodAndWaterPerDay = foodPerDay + waterPerDay;
            double totalFoodAndWaterPerDayOne = foodPerDayOne + waterPerDayOne;
            double totalFoodAndWaterPerDayTwo = foodPerDayTwo + waterPerDayTwo;
            double maxTotalVolumePerMeal = bodyWeight * MAX_ML_PER_KG;
            double maxTotalVolumePerMealDayOne = bodyWeight * MAX_ML_PER_KG_DAY_ONE;
            double maxTotalVolumePerMealDayTwo = bodyWeight * MAX_ML_PER_KG_DAY_TWO;

            int mealsPerDay = (int)Math.Round(totalFoodAndWaterPerDay / maxTotalVolumePerMeal, 0, MidpointRounding.AwayFromZero);
            int mealsPerDayOne = (int)Math.Round(totalFoodAndWaterPerDayOne / maxTotalVolumePerMealDayOne, 0, MidpointRounding.AwayFromZero);
            int mealsPerDayTwo = (int)Math.Round(totalFoodAndWaterPerDayTwo / maxTotalVolumePerMealDayTwo, 0, MidpointRounding.AwayFromZero);

            double foodPerMeal = Globals.CalculateFoodPerMeal(mealsPerDay, foodPerDay);
            double foodPerMealDayOne = Globals.CalculateFoodPerMeal(mealsPerDayOne, foodPerDayOne);
            double foodPerMealDayTwo = Globals.CalculateFoodPerMeal(mealsPerDayTwo, foodPerDayTwo);
            double waterPerMeal = Globals.CalculateWaterPerMeal(mealsPerDay, waterPerDay);
            double waterPerMealDayOne = Globals.CalculateWaterPerMeal(mealsPerDayOne, waterPerDayOne);
            double waterPerMealDayTwo = Globals.CalculateWaterPerMeal(mealsPerDayTwo, waterPerDayTwo);
            double waterToAddPerMeal = Globals.CalculateWaterToAddPerMeal(waterPerMeal, flushPerMeal);
            double waterToAddPerMealDayOne = Globals.CalculateWaterToAddPerMeal(waterPerMealDayOne, flushPerMeal);
            double waterToAddPerMealDayTwo = Globals.CalculateWaterToAddPerMeal(waterPerMealDayTwo, flushPerMeal);
            double totalVolumePerMeal = Globals.CalculateTotalVolumePerMeal(foodPerMeal, flushPerMeal, waterToAddPerMeal);
            double totalVolumePerMealDayOne = Globals.CalculateTotalVolumePerMeal(foodPerMealDayOne, flushPerMeal, waterToAddPerMealDayOne);
            double totalVolumePerMealDayTwo = Globals.CalculateTotalVolumePerMeal(foodPerMealDayTwo, flushPerMeal, waterToAddPerMealDayTwo);

            while (totalVolumePerMeal > maxTotalVolumePerMeal)
            {
                mealsPerDay += 1;
                foodPerMeal = Globals.CalculateFoodPerMeal(mealsPerDay, foodPerDay);
                waterPerMeal = Globals.CalculateWaterPerMeal(mealsPerDay, waterPerDay);
                waterToAddPerMeal = Globals.CalculateWaterToAddPerMeal(waterPerMeal, flushPerMeal);
                totalVolumePerMeal = Globals.CalculateTotalVolumePerMeal(foodPerMeal, flushPerMeal, waterToAddPerMeal);
                if (mealsPerDay > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMealDayOne > maxTotalVolumePerMealDayOne)
            {
                mealsPerDayOne += 1;
                foodPerMealDayOne = Globals.CalculateFoodPerMeal(mealsPerDayOne, foodPerDayOne);
                waterPerMealDayOne = Globals.CalculateWaterPerMeal(mealsPerDayOne, waterPerDayOne);
                waterToAddPerMealDayOne = Globals.CalculateWaterToAddPerMeal(waterPerMealDayOne, flushPerMeal);
                totalVolumePerMealDayOne = Globals.CalculateTotalVolumePerMeal(foodPerMealDayOne, flushPerMeal, waterToAddPerMealDayOne);
                if (mealsPerDayOne > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMealDayTwo > maxTotalVolumePerMealDayTwo)
            {
                mealsPerDayTwo += 1;
                foodPerMealDayTwo = Globals.CalculateFoodPerMeal(mealsPerDayTwo, foodPerDayTwo);
                waterPerMealDayTwo = Globals.CalculateWaterPerMeal(mealsPerDayTwo, waterPerDayTwo);
                waterToAddPerMealDayTwo = Globals.CalculateWaterToAddPerMeal(waterPerMealDayTwo, flushPerMeal);
                totalVolumePerMealDayTwo = Globals.CalculateTotalVolumePerMeal(foodPerMealDayTwo, flushPerMeal, waterToAddPerMealDayTwo);
                if (mealsPerDayTwo > 23)
                {
                    break;
                }
            }

            double cansPerDay = foodPerDay / netWeight;
            double cansPerDayOne = foodPerDayOne / netWeight;
            double cansPerDayTwo = foodPerDayTwo / netWeight;

            await App.Repo.AddNewPatient(
                foodName,
                Math.Round(kcalPerMl, 3, MidpointRounding.AwayFromZero),
                waterContent,
                patientName,
                clientName,
                species,
                bodyWeight,
                Math.Round(maxTotalVolumePerMeal, 1, MidpointRounding.AwayFromZero),
                Math.Round(maxTotalVolumePerMealDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(maxTotalVolumePerMealDayTwo, 1, MidpointRounding.AwayFromZero),
                Math.Round(foodPerMeal, 1, MidpointRounding.AwayFromZero),
                Math.Round(foodPerDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(foodPerDayTwo, 1, MidpointRounding.AwayFromZero),
                flushPerMeal / 2,
                Math.Round(waterToAddPerMeal, 1, MidpointRounding.AwayFromZero),
                Math.Round(waterToAddPerMealDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(waterToAddPerMealDayTwo, 1, MidpointRounding.AwayFromZero),
                mealsPerDay,
                mealsPerDayOne,
                mealsPerDayTwo,
                Math.Round(cansPerDay, 1, MidpointRounding.AwayFromZero),
                Math.Round(cansPerDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(cansPerDayTwo, 1, MidpointRounding.AwayFromZero)
                );

            Globals.GoToView();
        }
    }
}