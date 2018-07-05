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
    /// The profile page.
    /// </summary>
    public sealed partial class Profile : Page
    {
        Filter f = new Filter();
        PopUp p;
        /// <summary>
        /// The id of the current user logged in.
        /// </summary>
        int u_id = (App.Current as App).User_token;
        /// <summary>
        /// variables for storing the user profile info.
        /// </summary>
        string _name,_gender,_department,_qualification,_phone,_dob = "";

        public Profile()
        {

            this.InitializeComponent();

            edit_part.IsEnabled = false;
            edit_part.Opacity = .4;
            Messages m = new Messages();
            m.Note("Leave the 2 fields under empty, if you do not want to change your password.");
            info.Children.Add(m._b_Container);
            //set the values
            Query q = new Query();
            q.GetExtra("SELECT name,username,password,phone,department,qualification,dob,date_time,gender FROM staffs WHERE staff_id = '"+u_id+"'");
            q.Record();
            if (q.CheckRecords())
            {
                d_firstname.Text = f.CheckEmpty(f.FirstName(q.Result[0]),"Add detail");
                d_surname.Text = f.CheckEmpty(f.SurnName(q.Result[0]),"Add detail");
                d_username.Text = f.CheckEmpty(q.Result[1],"Add detail");
                for (int i = 0; i < q.Result[2].Length; i++)
                      d_pass.Text += "*";//we're to add asterisks according to the number of the user's password character.
                d_phone.Text = f.CheckEmpty(q.Result[3],"Add phone number");
                d_department.Text = f.CheckEmpty(q.Result[4],"Add your department");
                d_qualification.Text = f.CheckEmpty(q.Result[5],"Add your qualification");
                d_dob.Text = f.CheckEmpty(f.Date(q.Result[6],"dd/MM/yyyy"),"Add your birth date");
                d_reg_date.Text = f.CheckEmpty(f.Date(q.Result[7],""),"Unavailable");
                d_gender.Text = f.Gender(q.Result[8]);
                p_username.Text = q.Result[1];
            }
        }
        Connection c = new Connection("s", 3306, "root", "", "patho");

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           
            if (CredsEdit.IsOpen) 
            {
                //close
                this.Frame.Opacity = 1;
                parent.IsEnabled = true;
                CredsEdit.IsOpen = false;
            }

            if (DetailsEdit.IsOpen) 
            {
                //close
                this.Frame.Opacity = 1;
                parent.IsEnabled = true;
                DetailsEdit.IsOpen = false;
            }
        }

        private void EditCred_Click(object sender, RoutedEventArgs e)
        {
            //the user is trying to edit the credentials only,
            //so check for some stuff
            Query q = new Query();
            q.GetExtra("SELECT name,username,password FROM staffs WHERE staff_id = '" + u_id + "'");
            q.Record();

            string username = f.Trim(p_username.Text);
            string pass = f.Trim(p_password.Password);
            string new_pass = f.Trim(p_n_password.Password);
            string re_enter_new_pass = f.Trim(p_re_n_password.Password);

            if (new_pass == "" && re_enter_new_pass == "")
            {//the new password is empty,so just update
                if (username == "" || pass == "")
                {
                    if (username == "")
                        p = new PopUp(f.FirstName(q.Result[0]) + ", the username field can't be empty.");

                    else if (pass == "")
                        p = new PopUp(f.FirstName(q.Result[0]) + ", the password box can't be empty.");
                }
                else
                {
                    //check if anything was changed
                    if (username == q.Result[1] && pass == q.Result[2])
                    {
                        p = new PopUp("You didn't edit any of your credentials");
                    }
                    else
                    {//something was changed
                        //make sure the password provided is the same as initial password
                        if (pass != q.Result[2])
                        {
                            p = new PopUp("Access denied " + f.FirstName(q.Result[0]) + ", the password does not match your current password, try again.");
                        }
                        else
                        {//now check if the username already exists
                            Query c = new Query();
                            c.GetExtra("SELECT username from staffs WHERE username='" + username + "' AND staff_id != '"+u_id+"'");
                            if (c.CountRows() > 0)
                            {//username is already 
                                p = new PopUp("Sorry " + f.FirstName(q.Result[0]) + ", that username has already been taken, try another one");
                            }
                            else
                            {
                                //everything seems fine, insert
                                string[] val = { username };
                                q.successMessage = f.FirstName(q.Result[0]) + ", your username has been changed successfully from " + q.Result[1] + " to " + username + ".";
                                q.Change("staffs", "username", val, "WHERE staff_id = '" + u_id + "'");
                                parent.IsEnabled = true;
                                this.Frame.Opacity = 1;
                                this.Frame.Navigate(typeof(Profile), null);
                            }
                        }
                    }
                }
            }
            else
            {//the user wants to change his or her password.
                if (new_pass == "" || re_enter_new_pass == "" || pass == "" || username == "")
                {
                    if(username == "")
                        p = new PopUp(f.FirstName(q.Result[0]) + ", the username field can't be empty.");

                    else
                    p = new PopUp("Please " + f.FirstName(q.Result[0]) + ", none of the password boxes should be empty. Try again!");
                }

                else
                {//nothing is empty,now check for other stuff. :-)
                    if (pass != q.Result[2])
                    { //the current password doesn't match;
                        p = new PopUp("Access denied " + f.FirstName(q.Result[0]) + ", the password does not match your current password, try again.");
                    }
                    else if (new_pass != re_enter_new_pass)
                    {//passwords don't match
                        p = new PopUp(f.FirstName(q.Result[0]) + ", the new password you entered didn't match when you re-entered it, try again.");
                    }
                    else
                    {//seems good, insert
                        //now check if the username already exists
                            Query c = new Query();
                            c.GetExtra("SELECT username from staffs WHERE username='" + username + "' AND staff_id != '"+u_id+"'");
                            if (c.CountRows() > 0)
                            {//username is already 
                                p = new PopUp("Sorry " + f.FirstName(q.Result[0]) + ", that username has already been taken, try another one");
                            }
                            else
                            {//good to go.
                                string[] val = { username, new_pass };
                                q.successMessage = "Your credentials have been updated successfully!";
                                q.Change("staffs", "username,password", val, "WHERE staff_id = '"+u_id+"'");
                                parent.IsEnabled = true;
                                this.Frame.Opacity = 1;
                                this.Frame.Navigate(typeof(Profile), null);
                            }
                    }
                }

            }

        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            //check for stuff
            string __name = f.Trim(p_name.Text);
            string __gender = f.Trim(p_gender.SelectedIndex.ToString());
            string __department = f.Trim(p_department.Text);
            string __qualifi = f.Trim(p_qualification.Text);
            string __dob = f.Trim(p_dob.Text);
            string __phone = f.Trim(p_phone.Text);
            if (__name == "" || __gender == "" || __department == "" || __dob == "")//|| __qualifi == ""  || __phone == ""z)
            {
                if (__name == "")
                    p = new PopUp("Please fill in your name");

              //  else if (__phone == "")
                //    p = new PopUp("Please you must provide your phone number");
                else if (__department == "")
                    p = new PopUp("Please put in your department");
                //else if (__qualifi == "")
                  //  p = new PopUp("Please put in your qualification");
                else if (__dob == "")
                    p = new PopUp("Provide your date of birth");
                else if (__gender == "")
                    p = new PopUp("Please select your gender or sex");
                else
                    p = new PopUp("Sorry, an unkown error occured, please try again later.");
            }
            else
            {
                try{
                    __dob = f.Date(__dob, "yyyy-MM-dd");
                
                }
                catch (Exception ex)
                {
                    p = new PopUp("Please provide a valid date of birth in the format MM/dd/YYYY");
                    
                }
                //###############################################

                Query q = new Query();
                q.GetExtra("SELECT name,phone,department,qualification,dob,gender FROM staffs WHERE staff_id = '"+u_id+"'");
                q.Record();
               string former_name = q.Result[0],former_phone = q.Result[1],former_dept = q.Result[2],former_qualifi = q.Result[3],former_dob = q.Result[4],former_gender = q.Result[5];
                   //nothing is empty
                //check if anything was changed
                //transform gender
               if (__gender == "0")//male
                   __gender = "1";
               else if (__gender == "1")//female
                   __gender = "0";

               former_gender = f.Gender(former_gender);
               if (former_gender.ToLower() == "male")
               {
                   former_gender = "1";
               }
               else if (former_gender.ToLower() == "female") { former_gender = "0"; }
               former_dob = f.Date(former_dob, "yyyy-MM-dd");
                ////////////////////////////////////////

               if (__name == former_name && __department == former_dept && __phone == former_phone && __gender == former_gender && __qualifi == former_qualifi && __dob == former_dob)
               {
                   p = new PopUp("Sorry " + f.FirstName(former_name) + ", You didn't edit anything in your profile. Edit something before submitting.");
                }

               else
               {//something was changed
                   //check if phone number is correct
                   if (__phone != "")
                   {
                       if (f.CheckPhone(__phone, "Invalid phone number, e.g: +2349021610260") == false)
                       {
                       }
                       else
                       {
                           //store in the field so that the other method can process it
                           this._department = __department;
                           this._name = __name;
                           this._phone = __phone;
                           this._qualification = __qualifi;
                           this._gender = __gender;
                           this._dob = __dob;


                           //open the popup for password for clarification
                           if (!DetailsEdit.IsOpen)
                           {
                               this.Frame.Opacity = .4;

                               parent.IsEnabled = false;

                               DetailsEdit.IsOpen = true;
                           }
                       }

                   }
                   else
                   {
                       //store in the field so that the other method can process it
                       this._department = __department;
                       this._name = __name;
                       this._phone = __phone;
                       this._qualification = __qualifi;
                       this._gender = __gender;
                       this._dob = __dob;


                       //open the popup for password for clarification
                       if (!DetailsEdit.IsOpen)
                       {
                           this.Frame.Opacity = .4;

                           parent.IsEnabled = false;

                           DetailsEdit.IsOpen = true;
                       }
                       
                   }
               }
            }

        }
        private void EditDetails_Click(object sender, RoutedEventArgs e)
        {
            //the user passed the first test, now make sure it's the user, he or she must enter the correct password
            
            Query c = new Query();
            c.GetExtra("Select name,username FROM staffs sWHERE staff_id = '" + u_id + "' ");
            c.Record();

            if (f.Trim(password.Password) == "") 
            {
                p = new PopUp("Please " + f.FirstName(c.Result[0]) + ", the password field cannot be empty.");
            }
            else {
                Query q = new Query();
                q.GetExtra("Select password from staffs WHERE staff_id = '" + u_id + "' ");
                q.Record();
                //check
                if (password.Password == q.Result[0]) 
                {
                    //correct, now edit
                    string[] val = {this._name,this._phone,this._department,this._gender,this._qualification,this._dob};
                    q.successMessage = f.FirstName(c.Result[0])+", your details have been updated successfully!";
                    q.Change("staffs", "name,phone,department,gender,qualification,dob", val, "WHERE staff_id = '" + u_id + "' ");
                    
                    //
                    parent.IsEnabled = true;//i almost forgot this :).
                    this.Frame.Opacity = 1;
                    this.Frame.Navigate(typeof(Profile), null);
                }
                else
                {
                    p = new PopUp("Sorry " + f.FirstName(c.Result[0]) + ", the password you entered is incorrect. Try again");
                }

            }
            


        }

        private void EcredEdit_Click(object sender, RoutedEventArgs e)
        {
            Query q = new Query();
            q.GetExtra("SELECT name,gender,phone,department,qualification,dob FROM staffs WHERE staff_id = '" + u_id + "'");
            q.Record();
            if (q.CheckRecords() == true)
            {
                if (!CredsEdit.IsOpen)
                {
                    this.Frame.Opacity = .4;
                    parent.IsEnabled = false;
                    CredsEdit.IsOpen = true;
                }
            }
            else
            {
                p = new PopUp("Sorry, an error occured, could not fetch details, try again later.");
            }
        }

        private void EditProfile_Checked(object sender, RoutedEventArgs e)
        {
            Query q = new Query();
            q.GetExtra("SELECT name,gender,phone,department,qualification,dob FROM staffs WHERE staff_id = '" + u_id + "'");
            q.Record();
            if (q.CheckRecords() == true)
            {
                p_name.Text = q.Result[0];
                //gender stuff
                string sex = f.Gender(q.Result[1]);
                if (sex.ToLower() == "male")//male
                    p_gender.SelectedIndex = 0;
                else if (sex.ToLower() == "female")//female
                    p_gender.SelectedIndex = 1;
                
                    p_phone.Text = q.Result[2];
                    p_department.Text = q.Result[3];
                    p_qualification.Text = q.Result[4];
                    p_dob.Text = f.Date(q.Result[5], "MM/dd/yyyy");
                edit_part.Opacity = 1;
                edit_part.IsEnabled = true;
            }
            else
            {
                p = new PopUp("Sorry, an error occured, could not fetch details, try again later.");
            }
        }

        private void EditProfile_Unchecked(object sender, RoutedEventArgs e)
        {
            p_name.Text = "";
                p_gender.SelectedIndex = -1;
            
            p_phone.Text = "";
            p_department.Text = "";
            p_qualification.Text ="";
            p_dob.Text = "";
            
            edit_part.IsEnabled = false;
            edit_part.Opacity = .4;
        }
    }
}


