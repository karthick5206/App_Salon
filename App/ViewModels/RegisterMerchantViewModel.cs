using App.Models;
using App.Services;
using App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class RegisterMerchantViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command OtpCommand { get; }
        public Command BackCommand { get; }

        public Command OnAppearingCommand { get; set; }

        public Command OnDisappearingCommand { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ValidMobileNumber { get; set; }

        public string ConfirmPassword { get; set; }

        public List<string> MerchantTypes { get; set; }

        internal async Task Validate(string mobileNumber)
        {
            var isValid = await RegisterUser.ValidateMobileNumber(mobileNumber);
            ValidMobileNumber = isValid.IsSuccessStatusCode
                ? isValid.Data
                    ? "Valid Mobile Number"
                    : "InvalidMobile Number or Mobile Number Already Exist"
                : "Server Error";
            if (isValid.IsSuccessStatusCode && isValid.Data)
                ValidMobileNumberTextColor = Color.Green;
            else
                ValidMobileNumberTextColor = Color.Red;
        }

        public List<string> UserTypes { get; set; }

        public List<string> Genders { get; set; }

        public string SelectedMerchantType { get; set; }
        public string SelectedUserType { get; set; }
        public string SelectedGender { get; set; }

        public RegisterUser RegisterUser { get; set; }
        public Color ValidMobileNumberTextColor { get; set; } = Color.Green;

        public RegisterMerchantViewModel()
        {
            MerchantTypes = new List<string>() { "Shop Services", "Home Services" };
            UserTypes = new List<string>() { "User", "Merchant" };
            Genders = new List<string> { "Male", "Female", "Others" };
            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
            BackCommand = new Command(OnBackClicked);
            SelectedMerchantType = MerchantTypes.First();
            OnAppearingCommand = new Command(() => OnAppearing());
            OnDisappearingCommand = new Command(() => OnDisappearing());
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            //PropertyChanged += (_, __) => OtpCommand.CanExecute(null);
        }

        private bool Valid(object parameter) =>
            !string.IsNullOrWhiteSpace(MobileNumber)
            && !string.IsNullOrWhiteSpace(SelectedMerchantType)
            && !string.IsNullOrWhiteSpace(SelectedGender)
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Password);

        private void OnAppearing()
        {
            //Shell.Current.Navigating += Current_Navigating;
        }

        private void OnDisappearing()
        {
            //Shell.Current.Navigating -= Current_Navigating;
        }

        private async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.CanCancel)
            {
                e.Cancel();
                await GoBack();
            }
        }

        private async Task GoBack()
        {
            var result = await Shell.Current.DisplayAlert(
                "Going Back?",
                "Are you sure you want to go back?",
                "Yes, Please!", "Nope!");

            if (result)
            {
                Shell.Current.Navigating -= Current_Navigating;
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        private async void OnRegisterClicked(object parameter)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnOtpClicked(object parameter)
        {
            UpdateUserInfo();

            var isValid = await RegisterUser.ValidateMobileNumber(MobileNumber);

            if (isValid.IsSuccessStatusCode)
                App.Current.MainPage = new OtpValidationPopupPage(Shell.Current);
            else
                await Shell.Current.DisplayAlert("Failed", $"Unable to generate OTP {MobileNumber}", "Ok");
        }

        private async void OnBackClicked()
        {
            await Shell.Current.GoToAsync("//LoginPage");
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
