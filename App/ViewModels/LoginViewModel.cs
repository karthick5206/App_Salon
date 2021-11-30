using App.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            var shellVM = Shell.Current.BindingContext as ShellViewModel;
            shellVM.IsLoginTabVisible = false;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private async void OnRegisterClicked()
        {
            var shellVM = Shell.Current.BindingContext as ShellViewModel;
            shellVM.IsLoginTabVisible = false;
            await Shell.Current.GoToAsync("//RegisterMerchantPage");
        }
    }
}
