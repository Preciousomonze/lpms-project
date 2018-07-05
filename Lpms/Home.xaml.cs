using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        Loader l;
        public Home()
        {
            this.InitializeComponent();
            //PUT IN THE NAME OF THE USER
            Query q = new Query();
            q.Get("staffs","WHERE staff_id = '"+(App.Current as App).User_token+"'");
            q.Records();
            Filter f = new Filter();
            fullname.Text += q.RecordExist(q.Results[0, 3], "Unavailable :(");
            //NOW DISPLAY the rows 
                q.Get("patients");
                patient_r.Text = q.CountRows().ToString();

                q.Get("items");
               item_r.Text = q.CountRows().ToString();

               q.Get("tests");
               test_r.Text = q.CountRows().ToString();
               int c = 0;
               q.Get("Reports");
               q.Records();
               for (int i = 0; i < q.CountRows(); i++)
               {
                   Query d = new Query();
                   d.Get("tests", "WHERE test_id = '" + q.Results[i, 3] + "'");
                   if (d.CheckRecords() == true)
                       c += 1;
               }

                   report_r.Text = c.ToString();
               q.Get("Doctors");
               Doctor_r.Text = q.CountRows().ToString();
               q.Get("purchases");
               purchase_r.Text = q.CountRows().ToString();

        //    //START PRINTING TO THE LIST
        //       Query list = new Query();
        //       list.Get("Patients", " Order by date DESC Limit 5");
        //    int count_p_r = list.CountRows();
        //    if (count_p_r > 0)
        //    {
        //        list.Records();
        //        for (int i = 0; i < count_p_r; i++)
        //        {

        //            Border border = new Border();
        //            StackPanel panel = new StackPanel();
        //            //for the text blocks
        //            string[] content = { "Name: ", "DOB: ", "Gender: " };
        //            for (int c = 0; c < content.Length; c++)
        //            {
        //                TextBlock texts = new TextBlock();
        //                //designing
        //                string data = "";
        //                if (c == 0)
        //                {
        //                    data = list.Results[i, 1];

        //                }
        //                else if (c == 1)
        //                {
        //                    data = f.Date(list.Results[i, 5], "dd, M, yyyy");
        //                }
        //                else if (c == 2)
        //                {
        //                    data = f.Gender(list.Results[i, 4]);
        //                }
        //                texts.Text = content[c] + " " + data;

        //                texts.FontSize = 16;
        //                texts.Padding = new Thickness(5, 3, 5, 3);
        //                texts.Foreground = new SolidColorBrush(Color.FromArgb(240, 0, 0, 0));
        //                texts.TextWrapping = TextWrapping.Wrap;

        //                //add
        //                panel.Children.Add(texts);
        //            }

        //            border.Child = panel;


        //            int determine = i % 2;
        //            if (determine == 1)
        //            {
        //                border.Background = new SolidColorBrush(Color.FromArgb(250, 210, 210, 210));
        //            }
        //            else
        //            {
        //                // border.Background = new SolidColorBrush(Color.FromArgb(254, 161, 185, 203));
        //                //    option_border.Background = new SolidColorBrush(Color.FromArgb(250, 215, 215, 215));

        //                border.Background = new SolidColorBrush(Color.FromArgb(250, 250, 250, 250));
        //            }

        //            border.BorderBrush = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
        //            border.BorderThickness = new Thickness(1, 0, 1, 1);
        //            p_list.Children.Add(border);
        //        }
        //    }
        //    else{
        //        Messages msg = new Messages();
        //        msg.Note("No record");
        //    }
        //    //report part
        //    list.Get("reports", "Order by date_time DESC limit 5");
        //    int count_r_r = list.CountRows();
        //    if (count_r_r > 0)
        //    {

        //        list.Records();
        //        for (int i = 0; i < count_r_r; i++)
        //        {

        //            Border border = new Border();
        //            StackPanel panel = new StackPanel();
        //            //for the text blocks
        //            string[] content = { "Title ", "report content ", " date" };
        //            for (int c = 0; c < content.Length; c++)
        //            {
        //                TextBlock texts = new TextBlock();
        //                StackPanel inner_panel = new StackPanel();
        //                Border inner_border = new Border();
        //                //designing
        //                string data = "";
        //                SolidColorBrush bg = new SolidColorBrush();
                     
        //                if (c == 0)
        //                {
        //                    data = list.Results[i, 1];
        //                    bg.Color = Color.FromArgb(254, 243, 243, 243);
        //                    texts.FontSize = 22;
                        
        //                }
        //                else if (c == 1)
        //                {
        //                    data = list.Results[i,2];
        //                    texts.FontSize = 16;
        //                    //bg.Color = Color.FromArgb(254, 243, 243, 243);
                            
        //                }
        //                else if (c == 2)
        //                {
        //                    data = f.Gender(list.Results[i, 4]);
        //                    texts.FontSize = 16;
        //                    bg.Color = Color.FromArgb(254, 243, 243, 243);
                            
        //                }
        //                texts.Text = content[c] + " " + data;

        //                texts.Padding = new Thickness(5, 3, 5, 3);
        //                texts.Foreground = new SolidColorBrush(Color.FromArgb(240, 0, 0, 0));
        //                texts.TextWrapping = TextWrapping.Wrap;

        //                //inner panel
                        
        //                inner_panel.Background = bg;
        //                //inner border
        //                inner_border.BorderThickness = new Thickness(0, 0, 0, 1);
        //                inner_border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        //                //add
        //                inner_panel.Children.Add(texts);
        //                inner_border.Child = inner_panel;
        //                panel.Children.Add(inner_border);
        //            }

        //            border.Child = panel;


        //            int determine = i % 2;
        //            if (determine == 1)
        //            {
        //                border.Background = new SolidColorBrush(Color.FromArgb(250, 210, 210, 210));
        //            }
        //            else
        //            {
        //                // border.Background = new SolidColorBrush(Color.FromArgb(254, 161, 185, 203));
        //                //    option_border.Background = new SolidColorBrush(Color.FromArgb(250, 215, 215, 215));

        //                border.Background = new SolidColorBrush(Color.FromArgb(250, 250, 250, 250));
        //            }

        //            border.BorderBrush = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
        //            border.BorderThickness = new Thickness(1, 0, 1, 1);
        //            r_list.Children.Add(border);
        //        }
        //    }
        //    else
        //    {
        //        Messages msg = new Messages();
        //        msg.Note("No record Available");
        //        r_list.Children.Add(msg._Container);
        //    }
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
            l = new Loader("Purchases");
            this.Frame.Navigate(typeof(Parent), null);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Logout();
            this.Frame.Navigate(typeof(MainPage),null);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            l = new Loader("Profile");
            this.Frame.Navigate(typeof(Parent), null);
        }

    }
}
