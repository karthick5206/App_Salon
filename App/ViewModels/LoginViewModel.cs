using App.Models;
using App.Services;
using App.Views;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public RegisterUser RegisterUser { get; set; }
        public string LoginMessage { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            //PropertyChanged += (_,__) => LoginCommand.CanExecute(null);
        }

        private bool Valid(object parameter) =>
            !string.IsNullOrWhiteSpace(MobileNumber)
            && !string.IsNullOrWhiteSpace(Password);

        private async void OnLoginClicked(object obj)
        {

            if (string.IsNullOrWhiteSpace(MobileNumber) && string.IsNullOrWhiteSpace(Password))
            {
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
                var isValid = await RegisterUser.Login(shellViewModel.User);
                if (isValid.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Login Success.")
                    {
                        MessageTextColor = System.Drawing.Color.Green,
                        Position = ToastPosition.Bottom
                    });
                    //CrossToastPopUp.Current.ShowToastSuccess("Login Success", Plugin.Toast.Abstractions.ToastLength.Short);
                    await Shell.Current.GoToAsync("//AboutPage");
                }
                else
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Login Failed!")
                    {
                        MessageTextColor = System.Drawing.Color.Red,
                        Position = ToastPosition.Bottom
                    });
                    //CrossToastPopUp.Current.ShowToastError("Login Failed", Plugin.Toast.Abstractions.ToastLength.Short);
                    LoginMessage = "Login Failed";
                }
            }

        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync("//SelectionPage");
        }
    }
}
