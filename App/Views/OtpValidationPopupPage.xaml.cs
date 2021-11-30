using App.ViewModels;
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
        public OtpValidationPopupPage(Shell shell)
        {
            InitializeComponent();
            this.Shell = shell;
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
            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = Shell);
        }
    }
}