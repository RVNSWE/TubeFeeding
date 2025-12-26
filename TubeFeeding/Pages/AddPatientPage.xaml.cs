namespace TubeFeeding.Pages;

public partial class AddPatientPage : ContentPage
{
    private const int MAX_ML_PER_KG = 10;
    double foodPerMeal;
    double waterPerMeal;
    int flushPerMeal;
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
            newWaterPercentage.Text,
            newMealsPerDay.Text
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
        string rawWaterPercentage,
        string rawMealsPerDay
        )
    {
        double bodyWeight = 0;
        double kcal = 0;
        double netWeight = 0;
        double waterPercentage = 0;
        int mealsPerDay = 0;

        patientName = Globals.FormatString(patientName);
        clientName = Globals.FormatString(clientName);
        species = Globals.FormatString(species);
        rawBodyWeight = Globals.FormatString(rawBodyWeight);
        foodName = Globals.FormatString(foodName);
        rawKcal = Globals.FormatString(rawKcal);
        rawNetWeight = Globals.FormatString(rawNetWeight);
        rawWaterPercentage = Globals.FormatString(rawWaterPercentage);
        rawMealsPerDay = Globals.FormatString(rawMealsPerDay);

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
        else if (rawMealsPerDay.Length > 0 && !int.TryParse(rawMealsPerDay, out mealsPerDay))
        {
            await DisplayAlertAsync("Invalid dry weight", "Please enter the dry (dehydrated) weight (g) or volume (ml) of food per container as either a whole number or decimal.", "OK");
        }
        else
        {
            Globals.GoToList();

            double kcalPerMl = Globals.CalculateKcalPerMl(kcal, netWeight);
            double waterDecimal = waterPercentage * 0.01;

            // TO DO: Move feeding tube calcs here?
             
            //int foodIdPKey = App.SchedulePages.SelectedFood.Id;

            double rER = Globals.CalculateRER(bodyWeight, species);
            double fluidsPerDayTotal = Globals.CalculateFluidsPerDay(bodyWeight);
            double maxTotalVolumePerMeal = Globals.CalculateMaxTotalVolumePerMeal(bodyWeight, MAX_ML_PER_KG);
            double foodPerDay = Globals.CalculateFoodPerDay(rER, kcalPerMl);
            double foodPerMeal = Globals.CalculateFoodPerMeal(foodPerDay, mealsPerDay);
            double waterPerDay = Globals.CalculateWaterPerDay(fluidsPerDayTotal, foodPerDay, waterPercentage);
            double waterPerMeal = Globals.CalculateWaterPerMeal(waterPerDay, mealsPerDay);
            double cansPerDay = Math.Round(netWeight / foodPerDay, 0, MidpointRounding.ToPositiveInfinity);

            await App.Repo.AddNewPatient(
                foodName,
                kcal,
                kcalPerMl,
                netWeight,
                waterPercentage,
                waterContent,
                patientName,
                clientName,
                species,
                bodyWeight,
                rER,
                fluidsPerDayTotal,
                maxTotalVolumePerMeal,
                foodPerDay,
                foodPerMeal,
                waterPerDay,
                waterPerMeal,
                mealsPerDay,
                (int)cansPerDay
                );

            Globals.GoToView();
        }
    }

    public void CalculateTubeFeedingSchedule(double bodyWeight, double kcalPerMl, double waterPercentage, string species)
    {
        double waterDecimal = waterPercentage * 0.01;

        double rER;
        if (species == "Cat")
        {
            rER = bodyWeight * 30 + 70;
        }
        else
        {
            rER = 70 * Math.Pow(bodyWeight, 0.75);
        }

        double totalFluidsPerDay = 2 * bodyWeight * 24;
        double foodPerDay = rER / kcalPerMl;

        double waterPerDay = totalFluidsPerDay - (foodPerDay * waterDecimal);
        if (waterPerDay < 0)
        {
            waterPerDay = 0; // TO DO: CHECK ALTERNATIVE FLUID CALCS, POSS RECOMMEND DIFFERENT FOOD
        }

        double totalFoodAndWaterPerDay = foodPerDay + waterPerDay;
        double maxTotalVolumePerMeal = 10 * bodyWeight;
        double mealsPerDay = totalFoodAndWaterPerDay / maxTotalVolumePerMeal;

        CalculateMeals(foodPerDay, mealsPerDay, waterPerDay, bodyWeight);

        while (totalVolumePerMeal > maxTotalVolumePerMeal)
        {
            mealsPerDay += 1;
            CalculateMeals(foodPerDay, mealsPerDay, waterPerDay, bodyWeight);
        }
    }

    private void CalculateMeals(double foodPerDay, double mealsPerDay, double waterPerDay, double bodyWeight)
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

        waterToAddPerMeal = waterPerMeal - flushPerMeal;
        if (waterToAddPerMeal < 0)
        {
            waterToAddPerMeal = 0;
        }

        totalVolumePerMeal = foodPerMeal + flushPerMeal + waterToAddPerMeal;
    }
}