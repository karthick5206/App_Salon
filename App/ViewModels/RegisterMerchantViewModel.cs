using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class RegisterMerchantViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command OtpCommand { get; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public List<string> MerchantTypes { get; set; }
        public List<string> UserTypes { get; set; }

        public List<string> Genders { get; set; }

        public string SelectedMerchantType { get; set; }
        public string SelectedUserType { get; set; }
        public string SelectedGender { get; set; }

        public RegisterMerchantViewModel()
        {
            MerchantTypes = new List<string>() { "Shop Services", "Home Services" };
            UserTypes = new List<string>() { "User", "Merchant" };
            Genders = new List<string> { "Male", "Female", "Others" };
            RegisterCommand = new Command(OnRegisterClicked);
            OtpCommand = new Command(OnOtpClicked);
            SelectedMerchantType = MerchantTypes.First();
            OnPropertyChanged(nameof(MerchantTypes));
            OnPropertyChanged(nameof(Genders));
            OnPropertyChanged(nameof(SelectedMerchantType));
        }

        private async void OnRegisterClicked()
        {
            var shellVM = Shell.Current.BindingContext as ShellViewModel;
            shellVM.IsLoginTabVisible = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnOtpClicked()
        {
            var shellVM = Shell.Current.BindingContext as ShellViewModel;
            shellVM.IsLoginTabVisible = false;
            await Shell.Current.GoToAsync("//SetPinPage");
        }
    }
}
