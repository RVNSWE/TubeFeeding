using TubeFeeding.Models;
using TubeFeeding.PageModels;

namespace TubeFeeding.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}