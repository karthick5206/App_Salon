using App.ViewModels;
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
    public partial class RegisterMerchantPage : ContentPage
    {
        private RegisterMerchantViewModel RegisterMerchantViewModel { get; }
        public RegisterMerchantPage()
        {
            InitializeComponent();
            BindingContext = RegisterMerchantViewModel = new RegisterMerchantViewModel();
            RegisterMerchantViewModel.IsMerchant = true;
        }

        private async void MaterialTextField_Focused(object sender, FocusEventArgs e)
        {
            RegisterMerchantViewModel.IsBusy = true;
            await RegisterMerchantViewModel.Validate(RegisterMerchantViewModel.MobileNumber);
            RegisterMerchantViewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.BindingContext != null)
            {
                if (((RegisterMerchantViewModel)this.BindingContext).OnAppearingCommand.CanExecute(null))
                {
                    ((RegisterMerchantViewModel)this.BindingContext).OnAppearingCommand.Execute(null);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (this.BindingContext != null)
            {
                if (((RegisterMerchantViewModel)this.BindingContext).OnDisappearingCommand.CanExecute(null))
                {
                    ((RegisterMerchantViewModel)this.BindingContext).OnDisappearingCommand.Execute(null);
                }
            }
        }
    }
}