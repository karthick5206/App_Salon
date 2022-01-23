using Acr.UserDialogs;
using App.Services;
using App.ViewModels;
using Plugin.Toast;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtpValidationPopupPage : PopupPage
    {
        OtpValidationViewModel OtpValidationViewModel { get; }
        public OtpValidationPopupPage(OtpValidationViewModel otpValidationViewModel)
        {
            InitializeComponent();
            this.BindingContext = OtpValidationViewModel = otpValidationViewModel;
        }

        private void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async() => await OtpValidationViewModel.CloseDialog());
        }      
    }
}