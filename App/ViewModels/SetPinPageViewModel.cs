using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class RegisterUserViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command OtpCommand { get; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public IReadOnlyList<string> MerchantTypes { get; set; }
        public IReadOnlyList<string> UserTypes { get; set; }

        public string SelectedMerchantType { get; set; }
        public string SelectedUserType { get; set; }

        public RegisterUserViewModel()
        {
            MerchantTypes = new List<string>() { "Shop Services", "Home Services" };
            UserTypes = new List<string>() { "User", "Merchant" };
            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
        }

        private async void OnRegisterClicked()
        {
            var shellVM = Shell.Current.BindingContext as ShellViewModel;
            shellVM.IsLoginTabVisible = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnOtpClicked()
        {

        }
    }
}
