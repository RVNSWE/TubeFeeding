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
            double rER = Globals.CalculateRER(bodyWeight);
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
            double totalFluidsCalcOne;
            double totalFluidsCalcTwo;

            if (species == "Cat")
            {
                totalFluidsCalcOne = Globals.FluidCalculationOne(40, bodyWeight);
                totalFluidsCalcTwo = Globals.FluidCalculationTwo(80, bodyWeight);
            }
            else
            {
                totalFluidsCalcOne = Globals.FluidCalculationOne(60, bodyWeight);
                totalFluidsCalcTwo = Globals.FluidCalculationTwo(132, bodyWeight);
            }

            double minWaterPerDayOne = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayOne);
            double minWaterPerDayTwo = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayTwo);
            double minWaterPerDay = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContent);

            double maxWaterPerDayOne = Globals.GetMaxWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayOne);
            double maxWaterPerDayTwo = Globals.GetMaxWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayTwo);
            double maxWaterPerDay = Globals.GetMaxWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContent);

            waterPerDayOne = Globals.CalculateWaterPerDay(minWaterPerDayOne, maxWaterPerDayOne);
            waterPerDayTwo = Globals.CalculateWaterPerDay(minWaterPerDayTwo, maxWaterPerDayTwo);
            waterPerDay = Globals.CalculateWaterPerDay(minWaterPerDay, maxWaterPerDay);

            totalFoodAndWaterPerDayOne = foodPerDayOne + waterPerDayOne;
            totalFoodAndWaterPerDayTwo = foodPerDayTwo + waterPerDayTwo;
            totalFoodAndWaterPerDay = foodPerDay + waterPerDay;

            int mealsPerDayOne = (int)Math.Round(totalFoodAndWaterPerDayOne / maxTotalVolumePerMealDayOne, 0, MidpointRounding.AwayFromZero);
            int mealsPerDayTwo = (int)Math.Round(totalFoodAndWaterPerDayTwo / maxTotalVolumePerMealDayTwo, 0, MidpointRounding.AwayFromZero);
            int mealsPerDay = (int)Math.Round(totalFoodAndWaterPerDay / maxTotalVolumePerMeal, 0, MidpointRounding.AwayFromZero);

            CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, mealsPerDayOne, minWaterPerDayOne, maxWaterPerDayOne);
            CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, mealsPerDayTwo, minWaterPerDayTwo, maxWaterPerDayTwo);
            CalculateMealsForDay(3, foodPerDay, flushPerMeal, mealsPerDay, minWaterPerDay, maxWaterPerDay);

            while (totalVolumePerMealDayOne > maxTotalVolumePerMealDayOne)
            {
                mealsPerDayOne += 1;

                CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, mealsPerDayOne, minWaterPerDayOne, maxWaterPerDayOne);

                if (mealsPerDayOne > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMealDayTwo > maxTotalVolumePerMealDayTwo)
            {
                mealsPerDayTwo += 1;

                CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, mealsPerDayTwo, minWaterPerDayTwo, maxWaterPerDayTwo);

                if (mealsPerDayTwo > 23)
                {
                    break;
                }
            }
            while (totalVolumePerMeal > maxTotalVolumePerMeal)
            {
                mealsPerDay += 1;

                CalculateMealsForDay(3, foodPerDay, flushPerMeal, mealsPerDay, minWaterPerDay, maxWaterPerDay);

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
                Math.Round(waterPerMeal, 1, MidpointRounding.AwayFromZero),
                Math.Round(waterPerMealDayOne, 1, MidpointRounding.AwayFromZero),
                Math.Round(waterPerMealDayTwo, 1, MidpointRounding.AwayFromZero),
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

    private void CalculateMealsForDay(int day, double foodPerDay, double flushPerMeal, double mealsPerDay, double minWaterPerDay, double maxWaterPerDay)
    {
        if (day == 1)
        {
            double minWaterMinusFlush = minWaterPerDay - (flushPerMeal * mealsPerDay * 2);
            double maxWaterMinusFlush = maxWaterPerDay - (flushPerMeal * mealsPerDay * 2);

            waterPerDayOne = minWaterMinusFlush;
            if (minWaterMinusFlush < 0)
            {
                if (maxWaterMinusFlush < 0)
                {
                    maxWaterMinusFlush = 0;
                }
                waterPerDayOne = maxWaterMinusFlush;
            }

            foodPerMealDayOne = foodPerDay / mealsPerDay;
            waterPerMealDayOne = waterPerDayOne / mealsPerDay;

            totalFoodAndWaterPerDayOne = foodPerDay + waterPerDayOne;
            totalVolumePerMealDayOne = foodPerMealDayOne + flushPerMeal + waterPerMealDayOne;
        }
        if (day == 2)
        {
            double minWaterMinusFlush = minWaterPerDay - (flushPerMeal * mealsPerDay * 2);
            double maxWaterMinusFlush = maxWaterPerDay - (flushPerMeal * mealsPerDay * 2);

            waterPerDayTwo = minWaterMinusFlush;
            if (minWaterMinusFlush < 0)
            {
                if (maxWaterMinusFlush < 0)
                {
                    maxWaterMinusFlush = 0;
                }
                waterPerDayTwo = maxWaterMinusFlush;
            }

            foodPerMealDayTwo = foodPerDay / mealsPerDay;
            waterPerMealDayTwo = waterPerDayTwo / mealsPerDay;

            totalFoodAndWaterPerDayTwo = foodPerDay + waterPerDayTwo;
            totalVolumePerMealDayTwo = foodPerMealDayTwo + flushPerMeal + waterPerMealDayTwo;
        }
        if (day == 3)
        {
            double minWaterMinusFlush = minWaterPerDay - (flushPerMeal * mealsPerDay * 2);
            double maxWaterMinusFlush = maxWaterPerDay - (flushPerMeal * mealsPerDay * 2);

            waterPerDay = minWaterMinusFlush;
            if (minWaterMinusFlush < 0)
            {
                if (maxWaterMinusFlush < 0)
                {
                    maxWaterMinusFlush = 0;
                }
                waterPerDay = maxWaterMinusFlush;
            }

            foodPerMeal = foodPerDay / mealsPerDay;
            waterPerMeal = waterPerDay / mealsPerDay;

            totalFoodAndWaterPerDay = foodPerDay + waterPerDay;
            totalVolumePerMeal = foodPerMeal + flushPerMeal + waterPerMeal;
        }
    }

    /*
     * Override the back button.
     */
    protected override bool OnBackButtonPressed()
    {
        Globals.GoToList();
        return true;
    }
}