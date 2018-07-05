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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUp : Page
    {
        public int user_id;
        public SignUp()
        {
            this.InitializeComponent();
            
        }
        /// <summary>
        /// when the sign up button is clicked
        /// </summary>
        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            Filter filter = new Filter();
            if (filter.Trim(username.Text) == "" || filter.Trim(department.Text) == "" || filter.Trim(name.Text) == "" || filter.Trim(pass.Password) == "" || gender.SelectionBoxItem == null)
            {
                if (filter.Trim(username.Text) == "")
                {
                    PopUp m = new PopUp("Username field is empty");
                }
                else if (filter.Trim(department.Text) == "")
                {
                    PopUp m = new PopUp("department field is empty");
                }
                else if (filter.Trim(name.Text) == "")
                {
                    PopUp m = new PopUp("Your name field is empty");
                }
                else  if (filter.Trim(pass.Password) == "")
                {
                    PopUp m = new PopUp("Password field is empty");
                }
                else   if (gender.SelectionBoxItem == null)
                {
                    PopUp m = new PopUp("Please Select a gender");
                }
            }
            else
            {
                // THE FIELDS AREN'T EMPTY, SO CHECK FOR OTHER STUFF
                Query q = new Query();

                string user = filter.Trim(username.Text);
                string password = filter.Trim(pass.Password);
                string fullname = filter.Trim(name.Text);
                string depart = filter.Trim(department.Text);
                string gen = filter.Trim(gender.SelectionBoxItem.ToString());
                if (!(user == "" || password == "" || fullname == "" || depart == "" || gen == ""))
                {
                    //first check if the username hasn't been used already
                    q.Get("staffs", "Where username = '" + user + "'");
                    if (q.CountRows() > 0)
                    {
                        PopUp p = new PopUp("Sorry, username has already been taken. Try something else");
                    }
                    else
                    {
                        if (gen.ToLower() == "male")
                            gen = "1";
                        else if (gen.ToLower() == "female")
                            gen = "0";
                        //insert
                        //ADD THE VALUES IN AN ARRAY , CAUSE THAT'S HOW THE HOW METHOD CAN ACCEPT VALUES
                        string[] vals = { user, password, fullname, depart, gen, "NOW()" };
                        //q.ChooseDb("nisho");

                        // "INSERT into staffs(username,password,name,department,gender,date_time) VALUES('user','password','fullname','depart','gen',NOW())"
                        q.successMessage = "Thank You " + filter.FirstName(fullname) + ", Your account has been created";
                        q.Add("staffs", vals, "username,password,name,department,gender,date_time");
                        //LOG IN THE USER
                        q.Get("staffs", "Where username = '" + user + "'");
                        q.Record();
                        if (q.CountRows() == 1)
                        {
                            user_id = Convert.ToInt32(q.Result[0]);
                        }
                        (App.Current as App).User_token = user_id;
                        //NAVIGATE
                        this.Frame.Navigate(typeof(Home), null);
                    }
                }
                else
                {
                    PopUp p = new PopUp("sorry, an important field is missing");
                }
            }
        }
        

        private void GoToLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login), null);
        }
    }
}
