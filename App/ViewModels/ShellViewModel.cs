using App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public bool IsLoginTabVisible { get; set; } = true;
        public bool IsRegisterTabVisible { get; set; } = true;

        public User User { get; set; }

        public int GetId() => ++Id;
        public int Id { get; set; } = 0;
    }
}
