using App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App.ViewModels
{
    internal class SelectionPopupViewModel : BaseViewModel
    {
        public Command BackCommand { get; }
        public Command NextCommand { get; }
        public Command SelectionChangedCommand { get; }

        public Color UserBackgroundColor { get; set; } 
        public Color MerchantBackgroundColor { get; set; } = Color.Gray;

        private Color SelectedColor = Color.FromHex("#42cf60");
        public string SelectedOption { get; set; }
        public SelectionPopupViewModel()
        {
            BackCommand = new Command(OnBackPressed);
            NextCommand = new Command(OnNextPressed);
            SelectionChangedCommand = new Command<string>(OnSelectionChanged);
            UserBackgroundColor = SelectedColor;
            SelectedOption = "//RegisterPage";
        }

        private void OnSelectionChanged(string obj)
        {
            SelectedOption = obj;

            if (obj == "//RegisterMerchantPage")
            {
                MerchantBackgroundColor = SelectedColor;
                UserBackgroundColor = Color.Gray;
            }
            else
            {
                MerchantBackgroundColor = Color.Gray;
                UserBackgroundColor = SelectedColor;
            }
        }

        private async void OnNextPressed(object obj)
        {
            await Shell.Current.GoToAsync(SelectedOption);
        }

        private async void OnBackPressed(object obj)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
