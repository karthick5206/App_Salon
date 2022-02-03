using App.Models;
using App.Services;
using App.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Acr.UserDialogs;
using Plugin.DeviceInfo;

namespace App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public RegisterUser RegisterUser { get; set; }
        public string LoginMessage { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
        }

        private bool Valid(object parameter) =>
            !string.IsNullOrWhiteSpace(MobileNumber)
            && !string.IsNullOrWhiteSpace(Password);

        private async void OnLoginClicked(object obj)
        {
            UserDialogs.Instance.ShowLoading();
            if (string.IsNullOrWhiteSpace(MobileNumber) && string.IsNullOrWhiteSpace(Password))
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast(new ToastConfig("Please enter mobile number and pin to proceed login!")
                {
                    MessageTextColor = System.Drawing.Color.Orange,
                    Position = ToastPosition.Bottom
                });
            }
            else
            {
                var shellViewModel = DependencyService.Resolve<ShellViewModel>();
                shellViewModel.User = new User();
                shellViewModel.User.phoneNumber = MobileNumber;
                shellViewModel.User.pinNumber = Password;
                shellViewModel.User.deviceId = CrossDeviceInfo.Current.Id;
                var isValid = await RegisterUser.Login(shellViewModel.User);
                if (isValid.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast(new ToastConfig("Login Success.")
                    {
                        MessageTextColor = System.Drawing.Color.Green,
                        Position = ToastPosition.Bottom
                    });
                    await Shell.Current.GoToAsync("//AboutPage");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast(new ToastConfig("Login Failed!")
                    {
                        MessageTextColor = System.Drawing.Color.Red,
                        Position = ToastPosition.Bottom
                    });
                    LoginMessage = "Login Failed";
                }
            }

        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync("//SelectionPage");
        }

        private async void OnForgotPasswordClicked(object obj)
        {
            await Shell.Current.GoToAsync("//ForgotPasswordPage");
        }
    }
}
