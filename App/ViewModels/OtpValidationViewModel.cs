using Acr.UserDialogs;
using App.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class OtpValidationViewModel : BaseViewModel
    {
        public string Otp { get; set; }
        public Command OtpValidateCommand { get; }
        public RegisterUser RegisterUser { get; set; }
        public ShellViewModel ShellViewModel { get; set; }
        public bool IsValidated { get; set; }
        public bool EnableOtpValidation => !string.IsNullOrWhiteSpace(Otp);
        public OtpValidationViewModel()
        {
            OtpValidateCommand = new Command(OnOtpValidateClicked);
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            ShellViewModel = DependencyService.Resolve<ShellViewModel>();
        }

        private async void OnOtpValidateClicked(object obj)
        {
            UserDialogs.Instance.ShowLoading();
            var status = await RegisterUser.ValidOtpService(ShellViewModel.User.phoneNumber, Otp);
            if (status.IsSuccessStatusCode)
                Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Otp Validated")
                    {
                        MessageTextColor = System.Drawing.Color.Green,
                        Position = ToastPosition.Bottom
                    });
                    UserDialogs.Instance.HideLoading();
                    IsValidated = true;
                    await CloseDialog();
                });
            else
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast(new ToastConfig("Invalid Otp!")
                {
                    MessageTextColor = System.Drawing.Color.Red,
                    Position = ToastPosition.Bottom
                });
            }
        }

        internal async Task CloseDialog()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

}