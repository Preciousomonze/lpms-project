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
    /// Doctors page.
    /// </summary>
    public sealed partial class Doctors : Page
    {
        string d_id = string.Empty;
        EditTracker tracker;
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        public Doctors()
        {
            this.InitializeComponent();
            //start displaying the doctors records
            Query q = new Query();
            Query inner_q = new Query();
            q.GetExtra("SELECT name,phone,address,gender,specialisation,date,_added_by FROM doctors");
            q.Records();
            int doctors_row = q.CountRows();
            Query doctor_id = new Query();
            doctor_id.Get("doctors");
            doctor_id.Records();
            //AMOUNT OF COLUMN DEFINITIONS
            int col_num = Convert.ToInt32(doctors_r.ColumnDefinitions.Count);
            Filter f = new Filter();
            bool check_records = q.CheckRecords();

            //  RowDefinition[] rd = new RowDefinition[q.CountRows()];
            // RowDefinition rd;


            //STACKPANEL TO HOLD THE ROWS
            // StackPanel parent_row_panel;
            //WE'RE ADDING +1 CAUSE THERE'S ALREADY A ROW DEFINED WHICH IT'S VALUE WOULD BE ZERO
            //SO IF WE DONT INCREMENT IT, IT STARTS WITH 0, WHICH MIGHT BE A BAD IDEA :(.
            //int i = 0;
            //now check the number of rows
            //keep adding rows as long as it doesn't exceed the row limit from the stupid database.
            if (check_records)
            {
                int i = 0;
                for (i = 0; i < (doctors_row); i++)
                {
                    RowDefinition rd = new RowDefinition();
                    //rd.Height = new GridLength(50);
                    doctors_r.RowDefinitions.Add(rd);

                    /*
                     * COLUMN DEFINITION
                     * CREATE COLUMNS ACCORDING TO THE PARENT COLUMN OF THE FIRST ROW, WHICH IS col_num.
                     */

                    //////////////////////////////////
                    int k = 0;
                    for (k = 0; k < col_num; k++)
                    {

                        ColumnDefinition cd = new ColumnDefinition();
                        //cd.Width = new GridLength();
                        //cd.MinWidth = 150;

                        doctors_r.ColumnDefinitions.Add(cd);

                        //reset column def so that when it starts looping the for again, it creates a new set of column definition
                        //column_def = 0;


                        //////////////////////////////////////////////////////////
                        Border border = new Border();

                        TextBlock n = new TextBlock();
                        n.FontSize = 17.3;

                        n.Foreground = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0));


                        ///////////////////////////////////////
                        //stackpanel
                        StackPanel pa = new StackPanel();
                        // pa.Style = new Style();
                        //StackPanel m_pa = new StackPanel();
                        // m_pa.Name = "p";// + i.ToString();
                        StackPanel row_holder = new StackPanel();
                        //row_holder.Orientation = new Orientation();
                        // Orientation orie = row_holder1.Orientation;

                        //add
                        //pa.
                        pa.Children.Add(n);
                        pa.Margin = new Thickness(10);
                        //pa.MaxWidth = 115;
                        pa.HorizontalAlignment = HorizontalAlignment.Center;
                        //border.Style = new Windows.UI.Xaml.Style();
                        //border.Style.TargetType = typeof(Border);
                        border.BorderThickness = new Thickness(0, 0, 0, 1);
                        border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                        //calculate odd and even ish
                        int calc = i % 2;
                        //border.MaxWidth = 115;
                        // border.MinHeight = 61;
                        border.Child = pa;
                        row_holder.Children.Add(border);



                        int calcu = col_num - k;
                        //CHECK WHAT TO INPUT
                        if(k == 3)
                        {//gender
                            n.Text = f.Gender(q.Results[i, k]);
                        }
                         else if (k == 5)
                        {//tests results
                            inner_q.Get("tests", "WHERE doctor_id = '" + doctor_id.Results[i, 0] + "'");
                            n.Text = f.Numbers(inner_q.CountRows());
                        }
                        else if (k == 6)
                        {//DATE REGISTERED
                            n.Text = f.Date(q.Results[i,5],"dd-MM-yyyy");
                        }
                        else if (calcu == 2)
                        {//second to the last value, add added by
                            inner_q.Get("staffs","WHERE staff_id ='"+q.Results[i,6]+"'");
                            if(inner_q.CheckRecords() == true){
                                inner_q.Record();
                                string person = "";
                                if((App.Current as App).User_token.ToString() == inner_q.Result[0])
                                    person = "You";
                                else
                                    person = inner_q.Result[1];

                                n.Text = person;
                            }
                            else
                            {
                                n.Text = "Not Available";
                            }

                        }
                        else if (calcu == 1)
                        {//LAST VALUE

                        }
                        else
                        {
                            n.Text = q.Results[i, k];
                        }
                        //TextBlock 



                        //////////////OPTION
                        StackPanel option_panel = new StackPanel();
                        Border option_border = new Border();
                        //////////////////////////////////////////////////////

                        ////FOR CHANGING THE ROW BACKGROUND.
                        if (calc == 1)
                        {
                            border.Background = new SolidColorBrush(Color.FromArgb(250, 215, 215, 215));
                            option_border.Background = new SolidColorBrush(Color.FromArgb(250, 215, 215, 215));
                        }
                        else
                        {
                            border.Background = new SolidColorBrush(Color.FromArgb(250, 236, 254, 251));
                            option_border.Background = new SolidColorBrush(Color.FromArgb(250, 226, 246, 241));
                        }

                        //DETERMINE WHEN TO ADD THE OPTION STUFF

                        if (calcu == 1)
                        {//CREATE THE OPTIONS STUFF
                            StackPanel option_inner_panel = new StackPanel();
                            //TO MAKE THINGS EASY JARE,LOOP IT THROUGH THE ARRAY
                            string[] stuff_text = { "Edit", "Delete" };
                            string[] stuff_image = { "", "" };
                            HyperlinkButton[,] link = new HyperlinkButton[doctors_row, stuff_text.Length];
                            int reduce = i;
                            if (i == doctors_row)
                            {
                                reduce = i - 1;
                            }

                            for (int t = 0; t < stuff_text.Length; t++)
                            {

                                option_border.BorderThickness = new Thickness(0, 0, 0, 1);
                                option_border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                                TextBlock option_txt = new TextBlock();
                                option_txt.Text = stuff_text[t];
                                option_txt.Foreground = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
                                option_txt.FontSize = 15;
                                option_txt.Margin = new Thickness(10, 0, 0, 0);
                                option_txt.VerticalAlignment = VerticalAlignment.Center;
                                // Button button = new Button();
                                link[i, t] = new HyperlinkButton();

                                //HyperlinkButton[] link = new HyperlinkButton[tests_row];
                                link[i, t].Content = option_txt.Text;
                                // link.Click = RoutedEventHandler.Combine(sender, RoutedEventArgs e);
                                //button.ClickMode = ClickMode.Hover;
                                //CLICK EVENT
                                int reduce_t = t;
                                if (t == stuff_text.Length)
                                {
                                    reduce_t = t - 1;
                                }
                                //link[i].Click += new RoutedEventHandler(EditRecord_Click("E",));
                                link[i, t].Click += (s, e) =>
                                {
                                    if ((string)link[reduce, reduce_t].Content == stuff_text[0])
                                    {//IT's edit
                                        tracker = new EditTracker("doctors", "doctor_id", doctor_id.Results[(reduce), 0]);
                                        //track the row.
                                        tracker.Track();

                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Doctors", "doctor_id", doctor_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't edit Doctor " + f.FirstName(tracker.TrackResult[1]) + "'s record cause you didn't add the record");
                                        }
                                        else
                                        {
                                            if (!Editing.IsOpen)
                                            {

                                                //OPEN
                                                this.Frame.Opacity = .4;

                                                //this.Opacity = .4;
                                                parent.IsEnabled = false;
                                                //SET THE EDIT TRACKER
                                                //I DID (i-1) BECAUSE IT WAS INCREASE BY ONE, THEREFORE OUT OF BOUND ISH.

                                                this.d_id = tracker.TrackResult[0];//set the id to the global var, so that we know what to edit.
                                                d_name_e.Text = tracker.TrackResult[1];
                                                d_address_e.Text = tracker.TrackResult[2];
                                                d_phone_e.Text = tracker.TrackResult[3];
                                                d_spec_e.Text = tracker.TrackResult[5];
                                                //CHECK GENDER
                                                if (tracker.TrackResult[4].ToLower() == "true" || tracker.TrackResult[4].ToLower() == "1")
                                                {//male
                                                    d_gender_e.SelectedIndex = 1;
                                                }
                                                else if (tracker.TrackResult[4].ToLower() == "false" || tracker.TrackResult[4].ToLower() == "0")
                                                {//female
                                                    d_gender_e.SelectedIndex = 2;
                                                }
                                                Editing.IsOpen = true;
                                            }
                                        }
                                    
                                    }

                                    else if ((string)link[reduce, reduce_t].Content == stuff_text[1])
                                    {//it's the delete.
                                         //track first
                                        tracker = new EditTracker("doctors", "doctor_id", doctor_id.Results[(reduce), 0]);
                                        //track
                                        tracker.Track();
                                       
                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Doctors", "doctor_id", doctor_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't delete Doctor " + f.FirstName(tracker.TrackResult[1]) + "'s record cause you didn't add the record");
                                        }
                                        else
                                        {
                                            this.Frame.Opacity = .4;

                                            //this.Opacity = .4;
                                            parent.IsEnabled = false;
                                                
                                            Messages msg = new Messages();
                                            msg.Confirm("Are you sure you want to delete Dr "+f.FirstName(tracker.TrackResult[1]) + "'s record?");
                                            msg.p_container.IsOpen = true;
                                            parent_grid.Children.Add(msg.p_container);

                                            msg.TrueBtn.Click += (z, x) => {

                                                //CALL THE DELETE QUERY

                                                Query que = new Query();
                                                que.successMessage = "Doctor " + f.FirstName(tracker.TrackResult[1]) + " has been deleted successfully.";
                                                que.Remove("doctors", "where doctor_id = '" + tracker.TrackResult[0] + "'");
                                                this.Frame.Opacity = 1;

                                                parent.IsEnabled = true;

                                                this.Frame.Navigate(typeof(Doctors), null);
                                            
                                            };
                                           msg.FalseBtn.Click += (z,x) =>
                                            {
                                                this.Frame.Opacity = 1;

                                                parent.IsEnabled = true;

                                            };
                                            
                                        }
                                    }
                                };
                                ////ADDING
                                option_inner_panel.Children.Add(link[i, t]);

                            }
                            option_panel.Children.Add(option_inner_panel);
                            option_inner_panel.Orientation = Orientation.Horizontal;
                            option_panel.VerticalAlignment = VerticalAlignment.Center;
                            option_border.Child = option_panel;
                            Grid.SetColumn(option_border, k);
                            Grid.SetRow(option_border, i + 1);
                            doctors_r.Children.Add(option_border);

                        }

                        else
                        {
                            Grid.SetRow(row_holder, i + 1);

                            Grid.SetColumn(row_holder, k);
                            doctors_r.Children.Add(row_holder);

                        }

                    }

                }

            }
            else
            {
                //
                Messages msg = new Messages();
                msg.Note("No Doctors Record Available.");
                doctors_r.Children.Add(msg._b_Container);
                RowDefinition rd = new RowDefinition();
                // rd.Height = new GridLength(50);
                doctors_r.RowDefinitions.Add(rd);
                Grid.SetColumnSpan(msg._b_Container, col_num);
                Grid.SetColumn(msg._b_Container, 0);
                Grid.SetRow(msg._b_Container, 1);

            }
                
        }


        //ADD BTN
        private void Add_Click(object sender, RoutedEventArgs e)
        {

            if (!Adding.IsOpen)
            {
                //OPEN
                //  (App.Current as App).ParentOpacity = .4;

                this.Frame.Opacity = .4;

                //this.Opacity = .4;
                parent.IsEnabled = false;

                Adding.IsOpen = true;

            }
        }


        //Cancel BTN
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            if (Adding.IsOpen)
            {
                //Close
                //  (App.Current as App).ParentOpacity = .4;

                this.Frame.Opacity = 1;

                //this.Opacity = .4;
                parent.IsEnabled = true;

                Adding.IsOpen = false;

            }
            if (Editing.IsOpen)
            {

                this.Frame.Opacity = 1;

                //this.Opacity = .4;
                parent.IsEnabled = true;

                Editing.IsOpen = false;

            }
        }
        //add record click btn
        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            //CHECK EVERYTHING
            PopUp p;
            Filter f = new Filter();
            string doctor_name = f.Trim(d_name.Text);
            string doctor_phone = f.Trim(d_phone.Text);
            string doctor_address = f.Trim(d_address.Text);
            string doctor_spec = f.Trim(d_spec.Text);
            string doctor_gender = f.Trim(d_gender.SelectedIndex.ToString());

            if (!(doctor_name == "" || doctor_address == "" || doctor_phone == "" || doctor_spec == "" || (doctor_gender == "" || doctor_gender == "0")))
            {
                //GOOD TO GO
                //check for phone number
                Query q = new Query();
                if (doctor_gender == "1")
                {//male
                    doctor_gender = "1";
                }
                else if (doctor_gender == "2")
                {//female
                    doctor_gender = "0";
                }

                if (f.CheckPhone(doctor_phone, "Invalid phone number, e.g: +2349021610260") == false)
                {

                }
                else
                {
                    //doctor_dob = f.Date(doctor_dob, "yyyy-MM-dd");
                    
                    string[] val = { doctor_name, doctor_address, doctor_phone, doctor_gender, doctor_spec,user_id, "Now()" };
                    q.successMessage = "Dr " + f.FirstName(doctor_name) + "'s record has been added successfully!";
                    q.Add("doctors", val, "name,address,phone,gender,specialisation,_added_by,date");

                    this.Frame.Opacity = 1;

                    this.Frame.Navigate(typeof(Doctors), null);
                }
            }

            else
            {
                if (doctor_name == "")
                {
                    p = new PopUp("Please enter the name of the doctor");
                }
                else if (doctor_address == "")
                {
                    p = new PopUp("Please enter the doctor's address");
                }
                else if (doctor_phone == "")
                {
                    p = new PopUp("Please enter the doctor's phone number");
                }
                else if (doctor_spec == "")
                {
                    p = new PopUp("Please enter the doctor's area of specialty");
                }
                else if (doctor_gender == "" || doctor_gender == "0")
                {
                    p = new PopUp("Please select the doctor's gender");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The doctor couldn't be added. Please try again later");
                }

            }

        }



        //FOR EDITING
        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            // tracker.Track();
            // t_price.Text = tracker.TrackResult.Length.ToString();
            //CHECK EVERYTHING
            PopUp p;
            Filter f = new Filter();
            string doctor_name = f.Trim(d_name_e.Text);
            string doctor_phone = f.Trim(d_phone_e.Text);
            string doctor_address = f.Trim(d_address_e.Text);
            string doctor_spec = f.Trim(d_spec_e.Text);
            string doctor_gender = f.Trim(d_gender_e.SelectedIndex.ToString());

            if (!(doctor_name == "" || doctor_address == "" || doctor_phone == "" || doctor_spec == "" || (doctor_gender == "" || doctor_gender == "0")))
            {
                //GOOD TO GO
                Query q = new Query();
                if (doctor_gender == "1")
                {//male
                    doctor_gender = "1";
                }
                else if (doctor_gender == "2")
                {//female
                    doctor_gender = "0";
                }

                if (f.CheckPhone(doctor_phone, "Invalid phone number, e.g: +2349021610260") == false)
                {

                }
                else
                {
                    string current = DateTime.Now.ToString();
                    string editors = f.GetEditors("Doctors", "doctor_id", d_id)+ user_id+","+f.Date(current,e_time_format);
                    string[] val = { doctor_name, doctor_address, doctor_phone, doctor_gender, doctor_spec,editors };
                   // string[] col = { "name", "address", "phone", "gender", "specialisation" };
                    q.Get("doctors", "Where doctor_id = '" + d_id + "'");
                    q.Records();
                    q.successMessage = "Dr " + f.FirstName(q.Results[0, 1]) + "'s record has been edited successfully!";
                    //MAKE THINGS EASIER JARE, LOOP THROUGH THE VAL, TO INSERT DYNAMICALLY
                    //I CANNOT COME AND BE WOUNDING MYSELF :-|
                    string condition = " Where doctor_id = '" + this.d_id + "'";

                    q.Change("doctors", "name,address,phone,gender,specialisation,_editors", val, condition);

                    this.Frame.Opacity = 1;

                    this.Frame.Navigate(typeof(Doctors), null);

                }

            }

            else
            {
                if (doctor_name == "")
                {
                    p = new PopUp("Please enter the name of the doctor");
                }
                else if (doctor_address == "")
                {
                    p = new PopUp("Please enter the doctor's address");
                }
                else if (doctor_phone == "")
                {
                    p = new PopUp("Please enter the doctor's phone number");
                }

                else if (doctor_spec == "")
                {
                    p = new PopUp("Please enter the doctor's area of specialty");
                }
                else if (doctor_gender == "" || doctor_gender == "0")
                {
                    p = new PopUp("Please select the doctor's gender");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The doctor couldn't be added. Please try again later");
                }

            }

        }

    }
}



