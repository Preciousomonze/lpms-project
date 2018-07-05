using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Lpms.Classes;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Lpms
{
    /// <summary>
    /// The page that has the frame that loads most contents.
    /// </summary>
    public sealed partial class Parent : Page
    {
        Loader l;
        public Parent()
        {
            this.InitializeComponent();
            Filter f = new Filter();
            Query q = new Query();
            q.GetExtra("SELECT name from staffs WHERE staff_id = '"+(App.Current as App).User_token+"'");
            q.Record();
            if(q.CheckRecords() == true)
            fullname.Text = q.RecordExist(q.Result[0],"Not available :(");
            //PARENT OPACITY
            //this.Opacity = (App.Current as App).ParentOpacity;
            //CHECK FOR THE PAGES TO LOAD INTO THE FRAME
            Loader d = new Loader();
            
            //load into the frame
            string page = d.Page_to_load;
          // ColorConverter c = new ColorConverter();
            //SET THE BORDER PEROPERTY SO THAT THE USER KNOWS WHICH PAGE HE'S ON
            //Color color = (Color)ColorConverter.ConvertFromString("#FFDFD991");
            Thickness bt = new Thickness(0, 0, 0, 1);
            SolidColorBrush vb = new SolidColorBrush(Color.FromArgb(240,20,20,20));
            SolidColorBrush bg = new SolidColorBrush(Color.FromArgb(255, 75, 167, 131));
            //Color vb = new Color();
            //vb.Color = v.B;
            PatientPage.Margin = new Thickness(0, -1, -3, 0);
            if (d.Page_to_load == d._tests)
            {
                container.Navigate(typeof(Tests), this);
                TestPage.Background = bg;
                 TestPage.BorderThickness = bt;
                TestPage.BorderBrush = vb;
            }
            else if (d.Page_to_load == d._patients)
            {
                container.Navigate(typeof(Patients), this);
                PatientPage.Background = bg;    
            
                PatientPage.BorderThickness = bt;
                PatientPage.BorderBrush = vb;
            }
            else if (d.Page_to_load == d._purchases)
            {
                container.Navigate(typeof(Purchases), this);
                PurchasePage.Background = bg;    
            
               PurchasePage.BorderThickness = bt;
               PurchasePage.BorderBrush = vb;
            }
            else if(d.Page_to_load == d._doctors)
            {
                container.Navigate(typeof(Doctors), this);
                DoctorPage.Background = bg;
                DoctorPage.BorderThickness = bt;
                DoctorPage.BorderBrush = vb;
            

            }
            else if (d.Page_to_load == d._items)
            {
                container.Navigate(typeof(Items), this);
                ItemPage.Background = bg;
                ItemPage.BorderThickness = bt;
                ItemPage.BorderBrush = vb;
            
            }

            else if(d.Page_to_load == d._reports)
            {
                container.Navigate(typeof(Reports), this);
                ReportPage.Background = bg;
                ReportPage.BorderThickness = bt;
                ReportPage.BorderBrush = vb;
            
            }
            else if (d.Page_to_load == d._profile)
            {
                container.Navigate(typeof(Profile), this);
            }
            else
            {
                container.Navigate(typeof(Patients), this);
                PatientPage.Background = bg;
                PatientPage.BorderThickness = bt;
                PatientPage.BorderBrush = vb;
            
            }

        }

        /*
        * when the respective buttons are clicked, they each carry a token
        * to the parent page, so that the parent page knows what to load
        */

        private void PatientPage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("patients");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void TestPage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("tests");
            this.Frame.Navigate(typeof(Parent), null);
        }
        private void ItemPage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("items");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void DoctorPage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("Doctors");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void ReportPage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("reports");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void PurchasePage_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("purchases");
            this.Frame.Navigate(typeof(Parent), null);
       }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Logout();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("Profile");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Home), null);
        }

    }
}
