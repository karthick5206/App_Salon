using System;
using System.Collections.Generic;
using System.Text;

namespace App.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public bool IsLoginTabVisible { get; set; } = true;
        public bool IsRegisterTabVisible { get; set; } = true;
    }
}
