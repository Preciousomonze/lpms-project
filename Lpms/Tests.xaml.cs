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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tests : Page
    {
        private string t_id = "";
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        public Tests()
        {
            this.InitializeComponent();
            //FOR EDITING
            EditTracker tracker;
            //start displaying the tests records
            Query q = new Query();
            q.GetExtra("SELECT test_name,amount,patient_id,doctor_id,date,_added_by FROM tests");
            q.Records();
            ////
            Query test_id = new Query();
            test_id.GetExtra("SELECT test_id,doctor_id from tests");
            //GET THE RECORDS
            test_id.Records();
            ///////////////////
            int tests_row = q.CountRows();
            //AMOUNT OF COLUMN DEFINITIONS
            int col_num = Convert.ToInt32(tests_r.ColumnDefinitions.Count);
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

            int i = 0;
            //CHECK IF THERE'S ANY RECORD
            

            if (check_records) {
                for (i = 0; i < (tests_row); i++)
                {
                    RowDefinition rd = new RowDefinition();
                   // rd.Height = new GridLength(50);
                    tests_r.RowDefinitions.Add(rd);

                    /*++
                     * COLUMN DEFINITION
                     * CREATE COLUMNS ACCORDING TO THE PARENT COLUMN OF THE FIRST ROW, WHICH IS col_num.
                     */

                    //////////////////////////////////
                    int k = 0;
                    int ke = 1;
                    for (k = 0; k < col_num; k++)
                    {

                        ColumnDefinition cd = new ColumnDefinition();
                        //cd.Width = new GridLength();
                        //cd.MinWidth = 150;

                        tests_r.ColumnDefinitions.Add(cd);

                        //reset column def so that when it starts looping the for again, it creates a new set of column definition
                        //column_def = 0;


                        //calculate odd and even ish
                        int calc = i % 2;
                        //////////////////////////////////////////////////////////
                        Border border = new Border();

                        TextBlock n = new TextBlock();
                        n.FontSize = 17.3;

                        n.Foreground = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0));
                        
                        //TextBlock 


                        ///////////////////////////////////////
                        //stackpanel
                        StackPanel pa = new StackPanel();
                        // pa.Style = new Style();
                        //StackPanel m_pa = new StackPanel();
                        // m_pa.Name = "p";// + i.ToString();
                        StackPanel row_holder = new StackPanel();
                        
                        //add
                        //pa.
                        pa.Children.Add(n);
                        pa.Margin = new Thickness(10);
                        //pa.MaxWidth = 115;
                        pa.HorizontalAlignment = HorizontalAlignment.Center;
                        pa.VerticalAlignment = VerticalAlignment.Center;
                        
                        border.BorderThickness = new Thickness(0, 0, 0, 1);
                        border.BorderBrush = new SolidColorBrush(Color.FromArgb(50,0,0, 0));
                        
                        //border.MaxWidth = 115;
                        //border.MinHeight = 60.5;
                        border.Child = pa;
                        row_holder.Children.Add(border);

                        //CHECK WHAT TO INPUT
                        Query c = new Query();
                        //check if it's the last column
                        int calcu = (col_num - k);


                        if (k == 2)
                        {
                            //GET THE PATIENT'S NAME
                            c.Get("patients", "WHERE patient_id = '" + q.Results[i, k] + "'");
                            //check if the patient still exists, if not, no record available
                            
                            c.Records();

                            n.Text = c.RecordExist(c.Results[0, 1]);
                        }
                        else if (k == 3)
                        {//get the amount of reports
                            c.Get("reports", "WHERE test_id = '" + test_id.Results[i, 0] + "'");
                            n.Text = c.CountRows().ToString();
                        }
                        else if (k == 4)
                        {//get the doctor who performed the test
                            c.Get("doctors", "Where doctor_id = '" + test_id.Results[i, 1] + "'");
                            c.CheckRecords();
                                c.Records();
                                n.Text = c.RecordExist(c.Results[0, 1]);
                            

                        }
                        else if (k == 5)
                        {//DATE
                            n.Text = f.Date(q.Results[i, 4], "dd-MM-yy");
                        }
                        else if (calcu == 2)
                        {//second to the last value, add added by
                            Query inner_q = new Query();
                            inner_q.Get("staffs", "WHERE staff_id ='" + q.Results[i, 5] + "'");
                            if (inner_q.CheckRecords() == true)
                            {
                                inner_q.Record();
                                string person = "";
                                if ((App.Current as App).User_token.ToString() == inner_q.Result[0])
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
                        {

                        }
                        else
                        {
                            n.Text = q.Results[i, k];
                        }
                        //////////////OPTION
                        StackPanel option_panel = new StackPanel();
                        Border option_border = new Border();
                        //////////////////////////////////////////////////////
                       
                        ////FOR CHANGING THE ROW BACKGROUND.
                        if (calc == 1)
                        {
                            border.Background = new SolidColorBrush(Color.FromArgb(250, 225, 225, 225));
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
                            string ed = "Edit";
                            string del = "Delete";
                            string[] stuff_text = { ed, del };
                            string[] stuff_image = { "", "" };
                            HyperlinkButton[,] link = new HyperlinkButton[tests_row,stuff_text.Length];
                                int reduce = i;
                                if (i == tests_row)
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
                                option_txt.Margin = new Thickness(10,0,3,0);
                               // Button button = new Button();
                                link[i,t] = new HyperlinkButton();
                            
                                //HyperlinkButton[] link = new HyperlinkButton[tests_row];
                                link[i,t].Content = option_txt.Text;
                               // link.Click = RoutedEventHandler.Combine(sender, RoutedEventArgs e);
                               // button.ClickMode = ClickMode.Hover;
                                //CLICK EVENT
                                int reduce_t = t;
                                if (t == stuff_text.Length)
                                {
                                    reduce_t = t - 1;
                                }
                                //link[i].Click += new RoutedEventHandler(EditRecord_Click("E",));
                                link[i,t].Click += (s,e) =>{
                                    if ((string)link[reduce, reduce_t].Content == ed)
                                    {//IT's edit
                                        tracker = new EditTracker("Tests", "test_id", test_id.Results[(reduce), 0]);
                                        //track the row.
                                        tracker.Track();
                                              
                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Tests", "test_id", test_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't edit Test " + tracker.TrackResult[1] + "'s record cause you didn't add the record");
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
                                               this.t_id = tracker.TrackResult[0];//set the id to the global var, so that we know what to edit.

                                                t_name_e.Text = tracker.TrackResult[1];
                                                t_price_e.Text = tracker.TrackResult[2];
                                                //get values for the combo boxes.
                                                c.Get("patients", "Where patient_id = '" + q.Results[reduce, 2] + "'");
                                                c.Records();

                                                t_patients_e.SelectedIndex = f.SelectedInverse(c.Results[0, 0], "patients", null, 1);

                                                c.Get("doctors", "Where doctor_id = '" + q.Results[reduce, 3] + "'");
                                                c.Records();

                                                t_doctors_e.SelectedIndex = f.SelectedInverse(c.Results[0, 0], "doctors", null, 1);


                                                Editing.IsOpen = true;

                                            }
                                        }
                                    }
                                    else if ((string)link[reduce, reduce_t].Content == del)
                                    {//it's the delete.
                                        //track first
                                        tracker = new EditTracker("Tests", "test_id", test_id.Results[(reduce), 0]);
                                        //track
                                        tracker.Track();

                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Tests", "test_id", test_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't delete Test " + tracker.TrackResult[1] + "'s record cause you didn't add the record");
                                        }
                                        else
                                        {
                                            this.Frame.Opacity = .4;

                                            //this.Opacity = .4;
                                            parent.IsEnabled = false;

                                            Messages msg = new Messages();
                                            msg.Confirm("Are you sure you want to delete Test " + tracker.TrackResult[1] + "'s record?");
                                            msg.p_container.IsOpen = true;
                                            parent_grid.Children.Add(msg.p_container);

                                            msg.TrueBtn.Click += (z, x) =>
                                            {
                                                //CALL THE DELETE QUERY
                                                Query que = new Query();
                                                que.successMessage = "Test " + tracker.TrackResult[1] + " has been deleted.";
                                                que.Remove("Tests", "where test_id = '" + tracker.TrackResult[0] + "'");
                                                this.Frame.Opacity = 1;
                                                this.Frame.Navigate(typeof(Tests), null);
                                            };

                                            msg.FalseBtn.Click += (z, x) =>
                                            {
                                                this.Frame.Opacity = 1;

                                                parent.IsEnabled = true;

                                            };
                                        
                                        }
                                    }
                                };

                                //private void R_C(object sender, EventArgs e){

                                //}
                                ////ADDING
                                option_inner_panel.Children.Add(link[i,t]);
                                //ADD THE EDIT, DELETE ICON
                                //Image img = new Image();
                                //img.Source = new ;
                            }
                            option_panel.Children.Add(option_inner_panel);
                            option_inner_panel.Orientation = Orientation.Horizontal;
                            option_panel.VerticalAlignment = VerticalAlignment.Center;
                       
                            option_border.Child = option_panel;
                            Grid.SetColumn(option_border, k);
                                    Grid.SetRow(option_border, i+1);
                                    tests_r.Children.Add(option_border);
                                
                        }
                       
         else
                        {
                            Grid.SetRow(row_holder, i + 1);
                            Grid.SetColumn(row_holder, k);
                            tests_r.Children.Add(row_holder);
                         }

                       ///i've even forgotten what this does. :(
                        ke++;
                    }
                   
                }

            }

            else
            {//NO RECORD, SHOW SOMETHING
                Messages msg = new Messages();
                msg.Note("No Tests record available.");
                tests_r.Children.Add(msg._b_Container);
                RowDefinition rd = new RowDefinition();
                // rd.Height = new GridLength(50);
                tests_r.RowDefinitions.Add(rd);
                Grid.SetColumnSpan(msg._b_Container, col_num);
                Grid.SetColumn(msg._b_Container, 0);
                Grid.SetRow(msg._b_Container, 1);
            
            }
           
            ////////////////////////////////////////////////
            //fill some stuff in the form
            Query query = new Query();
            query.Get("Patients");

            int row_count = query.CountRows();
            query.Records();
            //ComboBox pp = t_patients;
           // object[] selected_value = new object[row_count];
            //string[] selected_value_path = new string[row_count];
            //t_patients.SelectedValue = new object();
            //Object[] p = new object[que.CountRows()];
                
              
            for (int r = 0; r < row_count; r++)
            {
                
               object p = query.Results[r, 1];
              //  t_patients.Items.Add(p[r]);
                t_patients.Items.Add(p);
               //  t_patients.SelectedValuePath = new string[row_count];
            // t_patients.SelectedValue = new object[que.CountRows()];
              
               // t_patients.SelectedValue[r] = "s";
                //   t_patients.SelectedValuePath = query.Results[r, 1];
                //   t_patients.SelectedValue = t_patients.SelectedValuePath;
              // selected_value[r] = t_patients.SelectedValuePath;
                               //editing box
                t_patients_e.Items.Add(p);
            }

            query.Get("Doctors");
            query.Records();
            for (int r = 0; r < query.CountRows(); r++)
            {
                Object d = query.Results[r, 1];
                t_doctors.Items.Add(d);
                //editing box
                t_doctors_e.Items.Add(d);
              //  t_doctors.SelectedIndex
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
                //Close

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
            string test_name = f.Trim(t_name.Text);
            string test_price = f.Trim(t_price.Text);
            
                string test_patient = f.Trim(t_patients.SelectedIndex.ToString());
            string test_doctor = f.Trim(t_doctors.SelectedIndex.ToString());

            if (!(test_name == "" || f.Trim(test_price) == "" || (test_patient == "" || test_patient == "0") || (test_doctor == "" || test_doctor == "0")))
            {
                //GOOD TO GO
                string[] t_p = f.Selected(t_patients.SelectedIndex, "Patients", null, 1);//.CopyTo(t, 0);
                string[] t_d = f.Selected(t_patients.SelectedIndex, "Doctors", null, 1);//.CopyTo(t, 0);
            
            
                Query q = new Query();
                test_patient = t_p[0];
                test_doctor = t_d[0];
                string[] val = { test_name, test_price, test_patient, test_doctor, "Now()" };
                q.successMessage = "Test record has been added successfully!";
                q.Add("tests", val, "test_name,amount,patient_id,doctor_id,date");

                this.Frame.Opacity = 1;

                this.Frame.Navigate(typeof(Tests), null);

            }

            else
            {
                if (test_name == "")
                {
                    p = new PopUp("Please enter the test name");
                }
                else if (test_price == "")
                {
                    p = new PopUp("Please enter a Price for the test");
                }
                else if (test_patient == "" || test_patient == "0")
                {
                    p = new PopUp("Please select a patient");
                }
                else if (test_doctor == "" || test_doctor == "0")
                {
                    p = new PopUp("Please select a doctor");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The test couldn't be added. Please try again later");
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
            string test_name = f.Trim(t_name_e.Text);
            string test_price = f.Trim(t_price_e.Text);
//            string[] t = f.Selected("0", "Tests", "test_id");

            string test_patient = f.Trim(t_patients_e.SelectedIndex.ToString());
            string test_doctor = f.Trim(t_doctors_e.SelectedIndex.ToString());

            if (!(test_name == "" || test_price == "" || (test_patient == "" || test_patient == "0") || (test_doctor == "" || test_doctor == "0")))
            {
                //GOOD TO GO
                string[] t_p = f.Selected(t_patients_e.SelectedIndex, "Patients", null, 1);//.CopyTo(t, 0);
                string[] t_d = f.Selected(t_doctors_e.SelectedIndex, "Doctors", null, 1);//.CopyTo(t, 0);


                Query q = new Query();
                q.GetExtra("SELECT test_name from tests WHERE test_id = '"+t_id+"'");
                q.Record();
                test_patient = t_p[0];
                test_doctor = t_d[0];
                string current = DateTime.Now.ToString();
                string editors = f.GetEditors("tests", "test_id", t_id) + user_id + "," + f.Date(current, e_time_format);
                
                string[] val = { test_name, test_price, test_patient, test_doctor,editors, "Now()" };
                q.successMessage = "Test ("+q.Result[0]+")'s record has been updated successfully!";
                q.Change("tests","test_name,amount,patient_id,doctor_id,_editors,date_time_updated",val,"WHERE test_id = '"+t_id+"'");

                this.Frame.Opacity = 1;

                this.Frame.Navigate(typeof(Tests), null);

            }

            else
            {
                if (test_name == "")
                {
                    p = new PopUp("Please enter the test name");
                }
                else if (test_price == "")
                {
                    p = new PopUp("Please enter a Price for the test");
                }
                else if (test_patient == "" || test_patient == "0")
                {
                    p = new PopUp("Please select a patient");
                }
                else if (test_doctor == "" || test_doctor == "0")
                {
                    p = new PopUp("Please select a doctor");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The test couldn't be added. Please try again later");
                }

            }
        
        }
    }
}
