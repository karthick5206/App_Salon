using Acr.UserDialogs;
using App.Models;
using App.Services;
using App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Plugin.DeviceInfo;

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

        public double Opacity { get; set; } = 1.0;

        public OtpValidationViewModel OtpValidationViewModel { get; set; }

        public bool EnableSignup => OtpValidationViewModel?.IsValidated ?? false;

        public bool EnableValidateOtp => !string.IsNullOrWhiteSpace(MobileNumber);

        public Color VerifyButtonColor => EnableValidateOtp ? Color.FromHex("#42cf60")
            : Color.Gray;

        public bool IsMerchant { get; set; }

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
            PopupNavigation.Instance.Popped += (_, __) =>
            {
                Opacity = 1.0;
                OnPropertyChanged(nameof(EnableSignup));
            };
            //PropertyChanged += (_, __) => OtpCommand.CanExecute(null);
        }

        private bool ValidateMerchant() =>
            !string.IsNullOrWhiteSpace(MobileNumber)
            && !string.IsNullOrWhiteSpace(SelectedMerchantType)
            && !string.IsNullOrWhiteSpace(SelectedGender)
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Password)
            && EnableSignup;

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

        private bool ValidateUser() =>
            !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(MobileNumber)
                && !string.IsNullOrWhiteSpace(Password)
                && EnableSignup;

        private async void OnRegisterClicked(object parameter)
        {
            UpdateUserInfo();

            if (IsMerchant ? ValidateMerchant() : ValidateUser())
            {
                UserDialogs.Instance.ShowLoading();

                var shellViewModel = DependencyService.Resolve<ShellViewModel>();
                shellViewModel.User.pinNumber = Password;
                shellViewModel.User.deviceId = CrossDeviceInfo.Current.Id;

                var response = await RegisterUser.SaveFirstTimeMaster(shellViewModel.User);

                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.HideLoading();

                    UserDialogs.Instance.Toast(new ToastConfig("Registration successful.")
                    {
                        MessageTextColor = System.Drawing.Color.Green,
                        Position = ToastPosition.Bottom
                    });
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();

                    UserDialogs.Instance.Toast(new ToastConfig("Registration Failed.")
                    {
                        MessageTextColor = System.Drawing.Color.Red,
                        Position = ToastPosition.Bottom
                    });
                }
            }
            else
            {
                UserDialogs.Instance.Toast(new ToastConfig(EnableSignup ? "Please fill all the details."
                    : "Verify the mobile number.")
                {
                    MessageTextColor = System.Drawing.Color.Red,
                    Position = ToastPosition.Bottom
                });
            }
        }

        private async void OnOtpClicked(object parameter)
        {
            UpdateUserInfo();

            UserDialogs.Instance.ShowLoading();

            OtpValidationViewModel = new OtpValidationViewModel();

            var isValid = await RegisterUser.ValidateMobileNumber(MobileNumber);

            if (isValid.IsSuccessStatusCode)
            {
                UserDialogs.Instance.HideLoading();
                Opacity = 0.25;
                await PopupNavigation.Instance.PushAsync(new OtpValidationPopupPage(OtpValidationViewModel));
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast(new ToastConfig("Unable to generate OTP,Please try again later.")
                {
                    MessageTextColor = System.Drawing.Color.Red,
                    Position = ToastPosition.Bottom
                });
            }
        }

        private async void OnBackClicked()
        {
            Email = String.Empty;
            SelectedMerchantType = String.Empty;
            SelectedGender = String.Empty;
            MobileNumber = String.Empty;
            Name = String.Empty;
            Password = String.Empty;
            if (OtpValidationViewModel != null)
                OtpValidationViewModel.IsValidated = false;
            await Shell.Current.GoToAsync("//SelectionPage");
        }

        private void UpdateUserInfo()
        {
            var shellViewModel = DependencyService.Resolve<ShellViewModel>();
            shellViewModel.User = new User();
            shellViewModel.User.mailId = Email;
            shellViewModel.User.merchantType = SelectedMerchantType;
            shellViewModel.User.gender = SelectedGender;
            shellViewModel.User.phoneNumber = MobileNumber;
            shellViewModel.User.name = Name;
            shellViewModel.User.pinNumber = Password;
            shellViewModel.User.deviceId = "12345";
        }
    }
}
