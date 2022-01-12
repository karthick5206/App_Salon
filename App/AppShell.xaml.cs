using App.ViewModels;
using App.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ShellViewModel ShellViewModel { get; private set; }
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterMerchantPage), typeof(RegisterMerchantPage));
            Routing.RegisterRoute(nameof(RegisterUserPage), typeof(RegisterUserPage));
            SetShellViewModel();
        }

        private void SetShellViewModel()
        {
            if (ShellViewModel == null)
                BindingContext = ShellViewModel = DependencyService.Resolve<ShellViewModel>();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                SetShellViewModel();

                if (args.Target.Location.OriginalString == "//AboutPage")
                {
                    ShellViewModel.IsLoginTabVisible = false;
                }
                else if (args.Target.Location.OriginalString == string.Empty)
                {
                    args.Cancel();
                }
                else
                    ShellViewModel.IsLoginTabVisible = true;

            });

            base.OnNavigating(args);

            // Cancel any back navigation
            //if (e.Source == ShellNavigationSource.Pop)
            //{
            //    e.Cancel();
            //}
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            // Perform required logic
        }
    }
}
