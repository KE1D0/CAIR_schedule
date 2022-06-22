using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CAIR_Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            enter.IsEnabled = false;
            Database db = Database.getDatabase();
            try
            {
                Console.WriteLine(login.Text.ToString() + " " + pass.Text.ToString());
                if (db.Login(login.Text.ToString(), pass.Text.ToString()))
                {
                    await Navigation.PopModalAsync(true);
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            enter.IsEnabled = true;
            await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
        }
    }
}