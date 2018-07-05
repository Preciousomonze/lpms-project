using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// The Login Page
    /// </summary>
    public sealed partial class Login : Page
    {
       public int user_token;
        public Login()
        {
            this.InitializeComponent();
        }

        private void GoToSignUp_Click(object sender, RoutedEventArgs e)
        {
           
            this.Frame.Navigate(typeof(SignUp),null);
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            //CHECK IF THE NORMAL THINGS ARE OKAY
            if (username.Text.Trim() == "")
            {
                PopUp p = new PopUp("Your username field is empty");
            }
            if (pass.Password.Trim() == "")
            {
                PopUp p = new PopUp("Please input a password");
            }
            else
            {
                //CALL IN THE QUERY STUFF
               //Connection c = new Connection();
              // c.ChooseDb("stuff");
              
                Query q = new Query();
              
                q.Get("staffs", "WHERE username = '" + username.Text.Trim() + "' And password = '" + pass.Password.Trim() + "' ");

                //set the count
                int count = 0;
                //while (q.reader.Read())
                //{
                //    user_token = Convert.ToInt32(q.reader[0]);
                //    count += 1;
                //}

                q.Records();
                while (count < q.CountRows())
                {
                    user_token = Convert.ToInt32(q.Results[0, 0]);
                    count += 1;
                }

                if (count > 0)//CHECK IF THE LOGIN DETAILS ARE VALID
                {
                    (App.Current as App).User_token = user_token;
                    this.Frame.Navigate(typeof(Home), null);
                }

                else
                {
                    PopUp p = new PopUp("Incorrect username and password combination.");

                }
            }
        }

        
    }

}
