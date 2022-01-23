using Acr.UserDialogs;
using App.Models;
using App.Services;
using App.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public string MobileNumber { get; set; }
        public string Pin { get; set; }
        public Command RegisterCommand { get; }
        public Command OtpCommand { get; }
        public Command BackCommand { get; }
        public RegisterUser RegisterUser { get; set; }
        public double Opacity { get; set; } = 1.0;
        public OtpValidationViewModel OtpValidationViewModel { get; set; }
        public bool EnableSignup => OtpValidationViewModel?.IsValidated ?? false;
        public bool NotEnableSignup => !EnableSignup;
        public ForgotPasswordViewModel()
        {

            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
            BackCommand = new Command(OnBackClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            PopupNavigation.Instance.Popped += (_, __) =>
            {
                Opacity = 1.0;
                OnPropertyChanged(nameof(EnableSignup));
            };
            //PropertyChanged += (_, __) => OtpCommand.CanExecute(null);
        }

        private async void OnOtpClicked(object parameter)
        {
            UpdateUserInfo();

            UserDialogs.Instance.ShowLoading();

            OtpValidationViewModel = new OtpValidationViewModel();

            if(!string.IsNullOrWhiteSpace(MobileNumber))
            {
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
            else
            {
                UserDialogs.Instance.Toast(new ToastConfig("Please enter mobile number.")
                {
                    MessageTextColor = System.Drawing.Color.Red,
                    Position = ToastPosition.Bottom
                });
            }
           
        }

        private async void OnRegisterClicked(object parameter)
        {
            UpdateUserInfo();
            UserDialogs.Instance.Toast(new ToastConfig("Password reset succeddful.")
            {
                MessageTextColor = System.Drawing.Color.Green,
                Position = ToastPosition.Bottom
            });

            MobileNumber = String.Empty;
            Pin = String.Empty;
        }

        private async void OnBackClicked()
        {
            MobileNumber = String.Empty;
            Pin = String.Empty;
            if (OtpValidationViewModel != null)
                OtpValidationViewModel.IsValidated = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private void UpdateUserInfo()
        {
            
        }

    }
}
