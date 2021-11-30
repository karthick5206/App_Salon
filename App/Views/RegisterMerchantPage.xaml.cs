using App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterMerchantPage : ContentPage
    {
        public RegisterMerchantPage()
        {
            InitializeComponent();
            BindingContext = new RegisterMerchantViewModel();
        }

        private async void MaterialTextField_Focused(object sender, FocusEventArgs e)
        {
            if (Shell.Current != null)
            {
                App.Current.MainPage = new SelectionPopupPage(Shell.Current);
            }
            //await Shell.Current.GoToAsync("//SelectionPopupPage");
            
        }
    }
}