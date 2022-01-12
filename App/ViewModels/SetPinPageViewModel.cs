using App.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.DeviceInfo;
using Plugin.Toast;

namespace App.ViewModels
{
    public class SetPinPageViewModel : BaseViewModel
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

        public string Pin { get; set; }
        public string RetypePin { get; set; }

        public RegisterUser RegisterUser { get; set; }

        public string RegisterMessage { get; set; }

        public Command BackCommand { get; }

        public SetPinPageViewModel()
        {
            MerchantTypes = new List<string>() { "Shop Services", "Home Services" };
            UserTypes = new List<string>() { "User", "Merchant" };
            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
            BackCommand = new Command(OnBackClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            //PropertyChanged += (_, __) => RegisterCommand.CanExecute(null);
        }      

        private bool Valid(object parameter) =>
           !string.IsNullOrWhiteSpace(Pin)
           && !string.IsNullOrWhiteSpace(RetypePin)
           && Pin.Equals(RetypePin);

        private async void OnRegisterClicked(object parameter)
        {
            var shellViewModel = DependencyService.Resolve<ShellViewModel>();
            shellViewModel.User.pinNumber = Pin;
            shellViewModel.User.deviceId = CrossDeviceInfo.Current.Id;

            var response = await RegisterUser.SaveFirstTimeMaster(shellViewModel.User);

            if (response.IsSuccessStatusCode)
            {
                //CrossToastPopUp.Current.ShowToastSuccess("Registration Success", Plugin.Toast.Abstractions.ToastLength.Short);
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                //CrossToastPopUp.Current.ShowToastError("Registration Failed", Plugin.Toast.Abstractions.ToastLength.Short);
                RegisterMessage = "Registration Failed";
            }

        }

        private async void OnBackClicked()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnOtpClicked()
        {

        }
    }
}
