using App.Models;
using App.Services;
using App.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading;

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

        public List<string> MerchantTypes { get; set; }
        public List<string> UserTypes { get; set; }

        public string SelectedMerchantType { get; set; }
        public string SelectedUserType { get; set; }

        public List<string> Genders { get; set; }

        public string SelectedGender { get; set; }

        public RegisterUser RegisterUser { get; set; }

        public RegisterUserViewModel()
        {
            MerchantTypes = new List<string>() { "Shop Services", "Home Services" };
            Genders = new List<string> { "Male", "Female", "Others" };
            UserTypes = new List<string>() { "User", "Merchant" };
            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
        }

        private async void OnRegisterClicked()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnOtpClicked()
        {
            UpdateUserInfo();

            var isValid = await RegisterUser.ValidateMobileNumber(MobileNumber);

            if (isValid.IsSuccessStatusCode)
                App.Current.MainPage = new OtpValidationPopupPage(Shell.Current);
            else
                await Shell.Current.DisplayAlert("Failed", $"Unable to generate OTP {MobileNumber}", "Ok");
        }

        private void UpdateUserInfo()
        {
            var shellViewModel = DependencyService.Resolve<ShellViewModel>();
            shellViewModel.User = new User();
            shellViewModel.User.mailId = Email;
            shellViewModel.User.merchantType = SelectedMerchantType;
            shellViewModel.User.gender = SelectedGender;
            shellViewModel.User.phoneNumber = MobileNumber;
            shellViewModel.User.deviceId = "12345";
        }
    }
}
