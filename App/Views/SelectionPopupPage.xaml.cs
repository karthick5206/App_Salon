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
    public partial class SelectionPopupPage : ContentPage    
    {
       
        public SelectionPopupPage()
        {
            InitializeComponent();

            this.BindingContext = new SelectionPopupViewModel();
        }

        private void Grid_Focused(object sender, FocusEventArgs e)
        {
            (sender as Grid).BackgroundColor = Color.Green;
        }

        private void Grid_Unfocused(object sender, FocusEventArgs e)
        {
            (sender as Grid).BackgroundColor = Color.Black;
        }
    }
}