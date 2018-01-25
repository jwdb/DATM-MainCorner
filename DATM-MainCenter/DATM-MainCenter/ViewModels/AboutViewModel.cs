using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace DATM_MainCenter.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://datm.nl/")));
        }

        public ICommand OpenWebCommand { get; }
    }
}