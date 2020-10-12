using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quoteAPPAbaxter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavPage : ContentPage
    {


        public FavPage(string pram)
        {
            InitializeComponent();

            FavBackgroundColour.BackgroundColor = Color.FromHex("EDEDED");

            Q1.Text = pram;
        }

        private async void btn_back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();

        }




    }
}