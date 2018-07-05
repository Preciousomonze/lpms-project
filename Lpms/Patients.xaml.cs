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
    /// The page which displays the patient's records
    /// </summary>
    public sealed partial class Patients : Page
    {
        //FOR EDITING
        EditTracker tracker;
        private string p_id = "";
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        public Patients()
        {
            this.InitializeComponent();
            //start displaying the patients records
            Query q = new Query();
            Query inner_q = new Query();
            q.GetExtra("SELECT name,dob,address,Gender,_added_by FROM patients");
            q.Records();
            int patients_row = q.CountRows();
            Query patient_id = new Query();
            patient_id.Get("Patients");
            patient_id.Records();
            //AMOUNT OF COLUMN DEFINITIONS
            int col_num = Convert.ToInt32(patients_r.ColumnDefinitions.Count);
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
            if(check_records){
            int i = 0;
            for (i = 0; i < (patients_row); i++)
            {
                RowDefinition rd = new RowDefinition();
                //rd.Height = new GridLength(50);
                patients_r.RowDefinitions.Add(rd);
           
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
                    
                    patients_r.ColumnDefinitions.Add(cd);

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
                    pa.Orientation = Orientation.Horizontal;
                    // pa.Style = new Style();
                    //StackPanel m_pa = new StackPanel();
                    // m_pa.Name = "p";// + i.ToString();
                    StackPanel row_holder = new StackPanel();
                    //row_holder.Orientation = Orientation.Horizontal;
                   // row_holder.HorizontalAlignment = HorizontalAlignment.Stretch;
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
                    if (k == 1)
                    {
                        n.Text = f.Date(q.Results[i, k], "dd/MM/yyyy");
                    }
                    else if ( k == 2) 
                    {
                        n.Text = q.Results[i, k];
                        
                        pa.Loaded += (y, u) =>
                        {
                            n.MaxWidth = 80;
                            // n.TextWrapping = TextWrapping.WrapWholeWords;
                            n.TextTrimming = TextTrimming.CharacterEllipsis;
                      
                        };

                        pa.PointerExited += (y, u) =>
                        {
                            n.MaxWidth = 80;
                            // n.TextWrapping = TextWrapping.WrapWholeWords;
                            n.TextTrimming = TextTrimming.CharacterEllipsis;

                        };
                        pa.PointerEntered += (y, u) => {
                          //  n.Width = 0;
                            n.MaxWidth = 250;
                            n.MinWidth = 80;
                            n.TextTrimming = TextTrimming.None;
                      
                        };
                        
                    }
                    else if (k == 3)
                    {
                        n.Text = f.Gender(q.Results[i, k]);
                    }
                    else if (k == 4)
                    {//tests results
                        inner_q.Get("tests", "WHERE patient_id = '" + patient_id.Results[i, 0] + "'");
                        n.Text = f.Numbers(inner_q.CountRows());
                    }
                    else if (k == 5)
                    {//purchases
                        inner_q.Get("purchases", "Where patient_id = '" + patient_id.Results[i, 0] + "'");
                        n.Text = f.Numbers(inner_q.CountRows());
                    }
                    else if (k == 6)
                    {//NO OF REPORTS
                        //report is related to test_id, so we have to get the test id results first, before, we know the patient
                        inner_q.Get("tests", "Where patient_id = '" + patient_id.Results[i, 0] + "'");
                        int tests_no = inner_q.CountRows();
                        //hold the value of reports gotten
                        int report_count = 0;
                        int report_only = 0;
                        int l = 0;
                        //LOOP THROUGH THE QUERY
                        inner_q.Records();
                        for (l = 0; l < tests_no; l++)
                        {
                            
                            //get the number of reports for the tests returned
                            inner_q.Get("reports", "Where test_id = '" + inner_q.Results[l, 0] + "'");
                            //n.Text = q.CountRows().ToString();
                            if(inner_q.CheckRecords() == true)
                            report_only += inner_q.CountRows();

                        }
                        //HOW MANY TIMES THE LOOP EXCECUTED SHOWS THE NUMBER OF TESTS VALID FOR THAT PATIENT

                        report_count = report_only;
                        n.Text = f.Numbers(report_count);
                    }
                    else if (calcu == 2)
                    {//second to the last value, add added by
                        inner_q.Get("staffs", "WHERE staff_id ='" + q.Results[i, 4] + "'");
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
                        HyperlinkButton[,] link = new HyperlinkButton[patients_row, stuff_text.Length];
                        int reduce = i;
                        if (i == patients_row)
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
                            option_txt.Margin = new Thickness(10, 0 ,0 ,0);
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
                                    tracker = new EditTracker("Patients", "patient_id", patient_id.Results[(reduce), 0]);
                                    //track the row.
                                    tracker.Track();

                                    Auth auth = new Auth();
                                    if (auth.CheckOwner("Patients", "patient_id", patient_id.Results[reduce, 0]) != true)
                                    {
                                        PopUp p = new PopUp("Sorry, You can't edit " + f.FirstName(tracker.TrackResult[1]) + "'s record cause you didn't add the record");
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
                                            this.p_id = tracker.TrackResult[0];//set the id to the global var, so that we know what to edit.
                                            p_name_e.Text = tracker.TrackResult[1];
                                            p_address_e.Text = tracker.TrackResult[2];
                                            p_phone_e.Text = tracker.TrackResult[3];
                                            p_dob_e.Text = f.Date(tracker.TrackResult[5], "MM/dd/yyyy");
                                            //CHECK GENDER
                                            if (tracker.TrackResult[4].ToLower() == "true")
                                            {//male
                                                p_gender_e.SelectedIndex = 1;
                                                //p_gender_e.SelectedItem = "Male";
                                            }
                                            else if (tracker.TrackResult[4].ToLower() == "false")
                                            {//female
                                                p_gender_e.SelectedIndex = 2;
                                            }
                                            Editing.IsOpen = true;

                                        }
                                    }
                                }
                                else if ((string)link[reduce, reduce_t].Content == stuff_text[1])
                                {//it's the delete.
                                    //CALL THE DELETE QUERY
                                    //track first
                                    tracker = new EditTracker("Patients", "patient_id", patient_id.Results[(reduce), 0]);
                                    //track
                                    tracker.Track();

                                    Auth auth = new Auth();
                                    if (auth.CheckOwner("patients", "patient_id", patient_id.Results[reduce, 0]) != true)
                                    {
                                        PopUp p = new PopUp("Sorry, You can't delete " + f.FirstName(tracker.TrackResult[1]) + "'s record cause you didn't add the record");
                                    }
                                    else
                                    {
                                        this.Frame.Opacity = .4;

                                        //this.Opacity = .4;
                                        parent.IsEnabled = false;

                                        Messages msg = new Messages();
                                        msg.Confirm("Are you sure you want to delete " + f.FirstName(tracker.TrackResult[1]) + "'s record?");
                                        msg.p_container.IsOpen = true;
                                        parent_grid.Children.Add(msg.p_container);

                                        msg.TrueBtn.Click += (z, x) =>
                                        {
                                            //CALL THE DELETE QUERY
                                            Query que = new Query();
                                            que.successMessage = "Patient " + f.FirstName(tracker.TrackResult[1]) + " has been deleted successfully.";
                                            que.Remove("patients", "where patient_id = '" + tracker.TrackResult[0] + "'");
                                            this.Frame.Opacity = 1;
                                            this.Frame.Navigate(typeof(Patients), null);
                                        };


                                        msg.FalseBtn.Click += (z, x) =>
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
                        patients_r.Children.Add(option_border);

                    }

                    else
                    {
                        Grid.SetRow(row_holder, i + 1);

                        Grid.SetColumn(row_holder, k);
                        patients_r.Children.Add(row_holder);

                    }

                }
               
            }

        }
        else{
                //
        Messages msg = new Messages();
       msg.Note("No Patients Record Available.");
       patients_r.Children.Add(msg._b_Container);
       RowDefinition rd = new RowDefinition();
       // rd.Height = new GridLength(50);
       patients_r.RowDefinitions.Add(rd);
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
            if(Editing.IsOpen){

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
            string patient_name = f.Trim(p_name.Text);
            string patient_phone = f.Trim(p_phone.Text);
            string patient_address = f.Trim(p_address.Text);
            string patient_dob = f.Trim(p_dob.Text);
            string patient_gender = f.Trim(p_gender.SelectedIndex.ToString());

            if (!(patient_name == "" || patient_address == "" || patient_phone == "" || patient_dob == "" || (patient_gender == "" || patient_gender == "0")))
            {
                //GOOD TO GO
                //check for phone number
                Query q = new Query();
                if (patient_gender == "1")
                {//male
                    patient_gender = "1";
                }
                else if (patient_gender == "2")
                {//female
                    patient_gender = "0";
                }
                if (f.CheckPhone(patient_phone, "Invalid phone number, e.g: +2349021610260") == false)
                {

                }
                else
                {
                    try
                    {
                        patient_dob = f.Date(patient_dob, "yyyy-MM-dd");
                        string[] val = { patient_name, patient_address, patient_phone, patient_gender, patient_dob,user_id, "Now()" };
                        q.successMessage = patient_name + "'s record has been added successfully!";
                        q.Add("patients", val, "name,address,phone,gender,dob,_added_by,date");

                        this.Frame.Opacity = 1;

                        this.Frame.Navigate(typeof(Patients), null);
                    }
                    catch (Exception ex)
                    {
                        p = new PopUp("Please the date of birth format must be in MM/DD/YYYY. Try again!");
                    }

                }
            }
            else
            {
                if (patient_name == "")
                {
                    p = new PopUp("Please enter the name of the patient");
                }
                else if (patient_address == "")
                {
                    p = new PopUp("Please enter the patient's address");
                }
                else if (patient_phone == "")
                {
                    p = new PopUp("Please enter the patient's phone number");
                }
                else if (patient_dob == "")
                {
                    p = new PopUp("Please enter the patient's date of birth");
                }
                else if (patient_gender == "" || patient_gender == "0")
                {
                    p = new PopUp("Please select the patient's gender");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The patient couldn't be added. Please try again later");
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
            string patient_name = f.Trim(p_name_e.Text);
            string patient_phone = f.Trim(p_phone_e.Text);
            string patient_address = f.Trim(p_address_e.Text);
            string patient_dob = f.Trim(p_dob_e.Text);
            string patient_gender = f.Trim(p_gender_e.SelectedIndex.ToString());

            if (!(patient_name == "" || patient_address == "" || patient_phone == "" || patient_dob == "" || (patient_gender == "" || patient_gender == "0")))
            {
                //GOOD TO GO
                Query q = new Query();
                if (patient_gender == "1")
                {//male
                    patient_gender = "1";
                }
                else if (patient_gender == "2")
                {//female
                    patient_gender = "0";
                }

                if (f.CheckPhone(patient_phone, "Invalid phone number, e.g: +2349021610260") == false)
                {

                }
                else
                {
                    try
                    {
                        patient_dob = f.Date(patient_dob, "yyyy-MM-dd");
                        string current = DateTime.Now.ToString();
                        string editors = f.GetEditors("patients", "patient_id", p_id) + user_id + "," + f.Date(current, e_time_format);
                   
                        string[] val = { patient_name, patient_address, patient_phone, patient_gender, patient_dob,editors };
                      //  string[] col = { "name", "address", "phone", "gender", "dob" };
                        q.Get("Patients", "Where patient_id = '" + p_id + "'");
                        q.Records();
                        q.successMessage = f.FirstName(q.Results[0, 1]) + "'s record has been edited successfully!";
                        //MAKE THINGS EASIER JARE, LOOP THROUGH THE VAL, TO INSERT DYNAMICALLY
                        //I CANNOT COME AND BE WOUNDING MYSELF :-|
                        string condition = " Where patient_id = '" + this.p_id + "'";

                        q.Change("Patients", "name,address,phone,gender,dob,_editors", val, condition);

                        this.Frame.Opacity = 1;

                        this.Frame.Navigate(typeof(Patients), null);
                    }
                    catch (Exception ex)
                    {

                        p = new PopUp("Please the date of birth format must be in MM/DD/YYYY. Try again!");
                    }


                }
            }
            else
            {
                if (patient_name == "")
                {
                    p = new PopUp("Please enter the name of the patient");
                }
                else if (patient_address == "")
                {
                    p = new PopUp("Please enter the patient's address");
                }
                else if (patient_phone == "")
                {
                    p = new PopUp("Please enter the patient's phone number");
                }
                else if (patient_dob == "")
                {
                    p = new PopUp("Please enter the patient's date of birth");
                }
                else if (patient_gender == "" || patient_gender == "0")
                {
                    p = new PopUp("Please select the patient's gender");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The patient couldn't be added. Please try again later");
                }

            }

        }
    }
}
