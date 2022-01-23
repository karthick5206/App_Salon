﻿using App.Services;
using App.ViewModels;
using App.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{

    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<RegisterUser>();
            DependencyService.RegisterSingleton(new ShellViewModel());
            XF.Material.Forms.Material.Init(this);
                          
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
