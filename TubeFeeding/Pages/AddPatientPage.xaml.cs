using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class AddPatientPage : ContentPage
{
    private const int MAX_ML_PER_KG = 10;
    double foodPerMeal;
    double waterPerMeal;
    double flushPerMeal;
    double waterToAddPerMeal;
    double totalVolumePerMeal;

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
            Globals.GoToList();

            double kcalPerMl = Math.Round(kcal * 0.001, 3, MidpointRounding.ToZero);
            double waterContent = waterPercentage * 0.01;
            double rER = Globals.CalculateRER(bodyWeight, species);
            double totalFluidsPerDay = 2 * bodyWeight * 24;
            double foodPerDay = rER / kcalPerMl;

            System.Diagnostics.Debug.WriteLine("foodPerDay = " + foodPerDay);

            double waterPerDay = totalFluidsPerDay - (foodPerDay * waterContent);
            if (waterPerDay < 0)
            {
                waterPerDay = 0; // TO DO: CHECK ALTERNATIVE FLUID CALCS, POSS RECOMMEND DIFFERENT FOOD
            }

            double totalFoodAndWaterPerDay = foodPerDay + waterPerDay;
            double maxTotalVolumePerMeal = bodyWeight * MAX_ML_PER_KG;
            int mealsPerDay = (int)Math.Round(totalFoodAndWaterPerDay / maxTotalVolumePerMeal, 0, MidpointRounding.AwayFromZero);

            CalculateMeals(foodPerDay, mealsPerDay, waterPerDay, bodyWeight);

            while (totalVolumePerMeal > maxTotalVolumePerMeal)
            {
                mealsPerDay += 1;
                CalculateMeals(foodPerDay, mealsPerDay, waterPerDay, bodyWeight);
            }

            double cansPerDay = Math.Round(foodPerDay / netWeight, 1, MidpointRounding.AwayFromZero);

            await App.Repo.AddNewPatient(
                foodName,
                kcalPerMl,
                waterContent,
                patientName,
                clientName,
                species,
                bodyWeight,
                Math.Round(maxTotalVolumePerMeal, 2, MidpointRounding.AwayFromZero),
                foodPerMeal,
                flushPerMeal / 2,
                waterToAddPerMeal,
                mealsPerDay,
                cansPerDay
                );

            Globals.GoToView();
        }
    }

    private void CalculateMeals(double foodPerDay, int mealsPerDay, double waterPerDay, double bodyWeight)
    {
        foodPerMeal = foodPerDay / mealsPerDay;

        waterPerMeal = waterPerDay / mealsPerDay;
        if (waterPerMeal < 0)
        {
            waterPerMeal = 0;
        }

        switch (bodyWeight)
        {
            case < 1:
                flushPerMeal = 2;
                break;
            case < 3:
                flushPerMeal = 4;
                break;
            case < 4:
                flushPerMeal = 5;
                break;
            case < 8:
                flushPerMeal = 10;
                break;
            case < 20:
                flushPerMeal = 12;
                break;
            default:
                flushPerMeal = 20;
                break;
        }

        waterToAddPerMeal = Math.Round(waterPerMeal - flushPerMeal, 1, MidpointRounding.AwayFromZero);
        if (waterToAddPerMeal < 0)
        {
            waterToAddPerMeal = 0;
        }

        foodPerMeal = Math.Round(foodPerMeal, 1, MidpointRounding.AwayFromZero);
        waterPerMeal = Math.Round(waterPerMeal, 1, MidpointRounding.AwayFromZero);

        totalVolumePerMeal = Math.Round(foodPerMeal + flushPerMeal + waterToAddPerMeal, 1, MidpointRounding.AwayFromZero);
    }
}