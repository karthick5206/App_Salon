using App.Services;
using App.ViewModels;
using Plugin.Toast;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtpValidationPopupPage : PopupPage
    {
        public Shell Shell { get; set; }
        public RegisterUser RegisterUser { get; set; }
        public ShellViewModel ShellViewModel { get; set; }
        public OtpValidationPopupPage(Shell shell)
        {
            InitializeComponent();
            this.Shell = shell;
            RegisterUser = DependencyService.Resolve<RegisterUser>();
            ShellViewModel = DependencyService.Resolve<ShellViewModel>();
        }

        private void OnClose(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = Shell);
        }

        private void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = Shell);
        }

        private async void MaterialButton_Clicked(object sender, EventArgs e)
        {
            if (txtOtp.Text.Length > 0)
            {
                var status = await RegisterUser.ValidOtpService(ShellViewModel.User.phoneNumber, txtOtp.Text);
                if (status.IsSuccessStatusCode)
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //CrossToastPopUp.Current.ShowToastSuccess("Otp validation success", Plugin.Toast.Abstractions.ToastLength.Short);
                        App.Current.MainPage = Shell;
                        await Shell.Current.GoToAsync("//SetPinPage");
                    });
                else
                    chipMessage.Text = "Invalid Otp";
            }
            //else
            //    CrossToastPopUp.Current.ShowToastWarning("Please enter valid OTP", Plugin.Toast.Abstractions.ToastLength.Short);
        }
    }
}