using TubeFeeding.Pages.Controls;

namespace TubeFeeding.Pages;

public partial class AddPatientPage : ContentPage
{
    private const int MAX_ML_PER_KG_DAY_ONE = 10;
    private const int MAX_ML_PER_KG_DAY_TWO = 15;
    private const int MAX_ML_PER_KG = 20;
    private double foodPerMealDayOne;
    private double foodPerMealDayTwo;
    private double foodPerMeal;
    private double waterPerMealDayOne;
    private double waterPerMealDayTwo;
    private double waterPerMeal;
    private double waterToAddPerMealDayOne;
    private double waterToAddPerMealDayTwo;
    private double waterToAddPerMeal;
    private double totalVolumePerMealDayOne;
    private double totalVolumePerMealDayTwo;
    private double totalVolumePerMeal;
    private double waterPerDayOne;
    private double waterPerDayTwo;
    private double waterPerDay;
    private double totalFoodAndWaterPerDayOne;
    private double totalFoodAndWaterPerDayTwo;
    private double totalFoodAndWaterPerDay;
    private string species;
    private Label speciesLabel;

    public AddPatientPage()
    {
        BindingContext = App.PatientPage?.SelectedPatient;

        InitializeComponent();

        speciesLabel = new Label();
        speciesLabel.SetBinding(Label.TextProperty, Binding.Create(static (Picker picker) => picker.SelectedItem, source: picker));
        species = "None";

        btnSave.Clicked += async (s, e) => await SaveNewSchedule();

        btnCancel.Clicked += (s, e) => Globals.GoToList();
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            speciesLabel.Text = (string)picker.ItemsSource[selectedIndex];
        }
    }

    public async Task SaveNewSchedule()
    {
        double bodyWeight = 0;
        double kcal = 0;
        double netWeight = 0;
        double waterPercentage = 0;

        string patientName = Globals.FormatString(newPatientName.Text);
        string clientName = Globals.FormatString(newClientName.Text);
        string rawBodyWeight = Globals.FormatString(newBodyWeight.Text);
        string foodName = Globals.FormatString(newFoodName.Text);
        string rawKcal = Globals.FormatString(newKcal.Text);
        string rawNetWeight = Globals.FormatString(newNetWeight.Text);
        string rawWaterPercentage = Globals.FormatString(newWaterPercentage.Text);
        species = speciesLabel.Text;

        if (Globals.IsStringEmpty(patientName))
        {
            await DisplayAlertAsync("No patient name entered", "Please enter the patient's name.", "OK");
        }
        else if (Globals.IsStringEmpty(clientName))
        {
            await DisplayAlertAsync("No client name entered", "Please enter the client's name.", "OK");
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
        else if (species == "None")
        {
            await DisplayAlertAsync("No species entered", "Please select the patient's species.", "OK");
        }
        else
        {
            Globals.GoToList(); // So "back" from the details page navigates back to the list instead of here

            double kcalPerMl = kcal * 0.001;
            double waterContent = waterPercentage * 0.01;
            double rER = Globals.CalculateRER(bodyWeight, species);
            double foodPerDay = rER / kcalPerMl;
            double foodPerDayOne = foodPerDay * 0.33;
            double foodPerDayTwo = foodPerDay * 0.66;
            double foodWaterContentDayOne = foodPerDayOne * waterContent;
            double foodWaterContentDayTwo = foodPerDayTwo * waterContent;
            double foodWaterContent = foodPerDay * waterContent;
            double maxTotalVolumePerMealDayOne = bodyWeight * MAX_ML_PER_KG_DAY_ONE;
            double maxTotalVolumePerMealDayTwo = bodyWeight * MAX_ML_PER_KG_DAY_TWO;
            double maxTotalVolumePerMeal = bodyWeight * MAX_ML_PER_KG;
            double flushPerMeal = Globals.GetFlushPerMeal(bodyWeight);
            double calcMinTotalFluidsPerDay;
            double calcMaxTotalFluidsPerDay;

            if (species == "Cat")
            {
                calcMinTotalFluidsPerDay = Globals.MinFluidCalculation(40, bodyWeight);
                calcMaxTotalFluidsPerDay = Globals.MaxFluidCalculation(80, bodyWeight);
            }
            else
            {
                calcMinTotalFluidsPerDay = Globals.MinFluidCalculation(60, bodyWeight);
                calcMaxTotalFluidsPerDay = Globals.MaxFluidCalculation(132, bodyWeight);
            }

            double minWaterPerDayOne = calcMinTotalFluidsPerDay - foodWaterContentDayOne;
            double minWaterPerDayTwo = calcMinTotalFluidsPerDay - foodWaterContentDayTwo;
            double minWaterPerDay = calcMinTotalFluidsPerDay - foodWaterContent;
            double maxWaterPerDayOne = calcMaxTotalFluidsPerDay - foodWaterContentDayOne;
            double maxWaterPerDayTwo = calcMaxTotalFluidsPerDay - foodWaterContentDayTwo;
            double maxWaterPerDay = calcMaxTotalFluidsPerDay - foodWaterContent;

            waterPerDayOne = Globals.CalculateInitialWaterPerDay(minWaterPerDayOne, maxWaterPerDayOne);
            waterPerDayTwo = Globals.CalculateInitialWaterPerDay(minWaterPerDayTwo, maxWaterPerDayTwo);
            waterPerDay = Globals.CalculateInitialWaterPerDay(minWaterPerDay, maxWaterPerDay);

            totalFoodAndWaterPerDayOne = foodPerDayOne + waterPerDayOne;
            totalFoodAndWaterPerDayTwo = foodPerDayTwo + waterPerDayTwo;
            totalFoodAndWaterPerDay = foodPerDay + waterPerDay;

            int mealsPerDayOne = (int)Math.Round(totalFoodAndWaterPerDayOne / maxTotalVolumePerMealDayOne, 0, MidpointRounding.AwayFromZero);
            int mealsPerDayTwo = (int)Math.Round(totalFoodAndWaterPerDayTwo / maxTotalVolumePerMealDayTwo, 0, MidpointRounding.AwayFromZero);
            int mealsPerDay = (int)Math.Round(totalFoodAndWaterPerDay / maxTotalVolumePerMeal, 0, MidpointRounding.AwayFromZero);

            CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, mealsPerDayOne, foodWaterContentDayOne, maxWaterPerDayOne);
            CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, mealsPerDayTwo, foodWaterContentDayTwo, maxWaterPerDayTwo);
            CalculateMealsForDay(3, foodPerDay, flushPerMeal, mealsPerDay, foodWaterContent, maxWaterPerDay);

            while (totalVolumePerMealDayOne > maxTotalVolumePerMealDayOne)
            {
                mealsPerDayOne += 1;

                CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, mealsPerDayOne, foodWaterContentDayOne, maxWaterPerDayOne);

                if (mealsPerDayOne > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMealDayTwo > maxTotalVolumePerMealDayTwo)
            {
                mealsPerDayTwo += 1;

                CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, mealsPerDayTwo, foodWaterContentDayTwo, maxWaterPerDayTwo);

                if (mealsPerDayTwo > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMeal > maxTotalVolumePerMeal)
            {
                mealsPerDay += 1;

                CalculateMealsForDay(3, foodPerDay, flushPerMeal, mealsPerDay, foodWaterContent, maxWaterPerDay);

                if (mealsPerDay > 23)
                {
                    break;
                }
            }

            double cansPerDayOne = foodPerDayOne / netWeight;
            double cansPerDayTwo = foodPerDayTwo / netWeight;
            double cansPerDay = foodPerDay / netWeight;

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
                Math.Round(foodPerMealDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(foodPerMealDayTwo, 1, MidpointRounding.AwayFromZero),
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

    private void CalculateMealsForDay(int day, double foodPerDay, double flushPerMeal, double mealsPerDay, double foodWaterContent, double maxWaterPerDay)
    {
        if (day == 1)
        {
            foodPerMealDayOne = foodPerDay / mealsPerDay;
            waterPerMealDayOne = waterPerDayOne / mealsPerDay;
            waterToAddPerMealDayOne = waterPerMealDayOne - flushPerMeal;

            if (waterToAddPerMealDayOne < 0)
            {
                waterPerDayOne = maxWaterPerDay;
                waterPerMealDayOne = waterPerDayOne / mealsPerDay;
                waterToAddPerMealDayOne = waterPerMealDayOne - flushPerMeal;

                if (waterToAddPerMealDayOne < 0)
                {
                    waterToAddPerMealDayOne = 0;
                }
            }

            totalFoodAndWaterPerDayOne = foodPerDay + waterPerDayOne;
            totalVolumePerMealDayOne = foodPerMealDayOne + flushPerMeal + waterToAddPerMealDayOne;
        }
        if (day == 2)
        {
            foodPerMealDayTwo = foodPerDay / mealsPerDay;
            waterPerMealDayTwo = waterPerDayTwo / mealsPerDay;
            waterToAddPerMealDayTwo = waterPerMealDayTwo - flushPerMeal;

            if (waterToAddPerMealDayTwo < 0)
            {
                waterPerDayTwo = maxWaterPerDay;
                waterPerMealDayTwo = waterPerDayTwo / mealsPerDay;
                waterToAddPerMealDayTwo = waterPerMealDayTwo - flushPerMeal;

                if (waterToAddPerMealDayTwo < 0)
                {
                    waterToAddPerMealDayTwo = 0;
                }
            }

            totalFoodAndWaterPerDayTwo = foodPerDay + waterPerDayTwo;
            totalVolumePerMealDayTwo = foodPerMealDayTwo + flushPerMeal + waterToAddPerMealDayTwo;
        }
        if (day == 3)
        {
            foodPerMeal = foodPerDay / mealsPerDay;
            waterPerMeal = waterPerDay / mealsPerDay;
            waterToAddPerMeal = waterPerMeal - flushPerMeal;

            if (waterToAddPerMeal < 0)
            {
                waterPerDay = maxWaterPerDay;
                waterPerMeal = waterPerDay / mealsPerDay;
                waterToAddPerMeal = waterPerMeal - flushPerMeal;

                if (waterToAddPerMeal < 0)
                {
                    waterToAddPerMeal = 0;
                }
            }

            totalFoodAndWaterPerDay = foodPerDay + waterPerDay;
            totalVolumePerMeal = foodPerMeal + flushPerMeal + waterToAddPerMeal;
        }
    }
}