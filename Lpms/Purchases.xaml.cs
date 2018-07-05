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
    /// The purchases page
    /// </summary>
    public sealed partial class Purchases : Page
    {
        string p_id = "";
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        public Purchases()
        {
            this.InitializeComponent();
            //FOR EDITING
            EditTracker tracker;
            //start displaying the purchases records
            Query q = new Query();
            q.GetExtra("SELECT purchase_id,item_id,quantity,patient_id,date_time,_added_by FROM purchases");
            q.Records();
            ////
            Query purchase_id = new Query();
            purchase_id.Get("purchases");
            //GET THE RECORDS
            purchase_id.Records();
            ///////////////////
            int purchases_row = q.CountRows();
            //AMOUNT OF COLUMN DEFINITIONS
            int col_num = Convert.ToInt32(purchases_r.ColumnDefinitions.Count);
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


            if (check_records)
            {
                for (i = 0; i < (purchases_row); i++)
                {
                    RowDefinition rd = new RowDefinition();
                    // rd.Height = new GridLength(50);
                    purchases_r.RowDefinitions.Add(rd);

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

                        purchases_r.ColumnDefinitions.Add(cd);

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
                        border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));

                        //border.MaxWidth = 115;
                        //border.MinHeight = 60.5;
                        border.Child = pa;
                        row_holder.Children.Add(border);

                        //CHECK WHAT TO INPUT
                        Query c = new Query();
                        //check if it's the last column
                        int calcu = (col_num - k);

                          if (k == 0)
                        {
                            n.Text = "#(" + q.Results[i, k] + ")";
                            //n.FontWeight = FontWeight.23;
                        }
                       
                         else if (k == 1)
                        {
                            //GET THE item bought
                            c.Get("items", "WHERE item_id = '" + q.Results[i, k] + "'");
                            if (c.CheckRecords() == true)
                            {
                                c.Records();

                                n.Text = c.Results[0, 1];
                            }
                            else
                            {
                                n.Text = "No record available";
                            }
                        }
                        else if (k == 3)
                        {//get the price of the item
                            //calculate, price * quanity
                            c.Get("items", "WHERE item_id = '" + q.Results[i, 1] + "'");
                            c.Records();
                            if (c.CheckRecords() == true)
                            {
                                int amount = Convert.ToInt32(c.Results[0, 2]) * Convert.ToInt32(q.Results[i, 2]);
                                n.Text = f.Numbers(amount,"No value");
                            }
                            else
                            {
                                n.Text = "Information not available";
                               
                            }
                        }
                        else if (k == 4)
                        {//get the buyer
                            c.Get("patients", "Where patient_id = '" + q.Results[i, 3] + "'");
                            if (c.CheckRecords())
                            {
                                c.Records();
                                n.Text = c.Results[0, 1];
                            }
                            else
                            {
                                n.Text = "Patient's record not available";
                            }

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
                            HyperlinkButton[,] link = new HyperlinkButton[purchases_row, stuff_text.Length];
                            int reduce = i;
                            if (i == purchases_row)
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
                                option_txt.Margin = new Thickness(10, 0, 3, 0);
                                // Button button = new Button();
                                link[i, t] = new HyperlinkButton();

                                //HyperlinkButton[] link = new HyperlinkButton[purchases_row];
                                link[i, t].Content = option_txt.Text;
                                // link.Click = RoutedEventHandler.Combine(sender, RoutedEventArgs e);
                                // button.ClickMode = ClickMode.Hover;
                                //CLICK EVENT
                                int reduce_t = t;
                                if (t == stuff_text.Length)
                                {
                                    reduce_t = t - 1;
                                }
                                //link[i].Click += new RoutedEventHandler(EditRecord_Click("E",));
                                link[i, t].Click += (s, e) =>
                                {
                                    if ((string)link[reduce, reduce_t].Content == ed)
                                    {//IT's edit
                                        tracker = new EditTracker("purchases", "purchase_id", purchase_id.Results[(reduce), 0]);
                                        //track the row.
                                        tracker.Track();
                                               
                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Purchases", "purchase_id", purchase_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't edit purchase #(" + tracker.TrackResult[0] + ")'s record cause you didn't add the record");
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
                                               p_id = tracker.TrackResult[0];
                                                p_quantity_e.Text = tracker.TrackResult[2];
                                                c.Get("patients", "Where patient_id = '" + q.Results[reduce, 3] + "'");
                                                c.Records();

                                                p_patients_e.SelectedIndex = f.SelectedInverse(c.Results[0, 0], "patients", null, 1);

                                                c.Get("items", "Where item_id = '" + q.Results[reduce, 1] + "'");
                                                c.Records();

                                                p_items_e.SelectedIndex = f.SelectedInverse(c.Results[0, 0], "items", null, 1);

                                                //t_price_e.Text = tracker.TrackResult[2];
                                                Editing.IsOpen = true;

                                            }
                                        }
                                    }
                                    else if ((string)link[reduce, reduce_t].Content == del)
                                    {//it's the delete.
                                        //CALL THE DELETE QUERY
                                        //track first
                                        tracker = new EditTracker("Purchases", "purchase_id", purchase_id.Results[(reduce), 0]);
                                        //track
                                        tracker.Track();

                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("purchases", "purchase_id", purchase_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't delete Purchase #(" + tracker.TrackResult[0] + ")'s record cause you didn't add the record");
                                        }
                                        else
                                        {
                                            this.Frame.Opacity = .4;

                                            //this.Opacity = .4;
                                            parent.IsEnabled = false;

                                            Messages msg = new Messages();
                                            msg.Confirm("Are you sure you want to delete Purchase #(" + tracker.TrackResult[0] + ")'s record?");
                                            msg.p_container.IsOpen = true;
                                            parent_grid.Children.Add(msg.p_container);

                                            msg.TrueBtn.Click += (z, x) =>
                                            {


                                                //CALL THE DELETE QUERY
                                                Query que = new Query();
                                                que.successMessage = "Purchase #(" + tracker.TrackResult[0] + ") has been deleted successfully.";
                                                que.Remove("purchases", "where purchase_id = '" + tracker.TrackResult[0] + "'");
                                                this.Frame.Opacity = 1;
                                                this.Frame.Navigate(typeof(Purchases), null);
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
                                option_inner_panel.Children.Add(link[i, t]);
                                //ADD THE EDIT, DELETE ICON
                                //Image img = new Image();
                                //img.Source = new ;
                            }
                            option_panel.Children.Add(option_inner_panel);
                            option_inner_panel.Orientation = Orientation.Horizontal;
                            option_panel.VerticalAlignment = VerticalAlignment.Center;

                            option_border.Child = option_panel;
                            Grid.SetColumn(option_border, k);
                            Grid.SetRow(option_border, i + 1);
                            purchases_r.Children.Add(option_border);

                        }

                        else
                        {
                            Grid.SetRow(row_holder, i + 1);
                            Grid.SetColumn(row_holder, k);
                            purchases_r.Children.Add(row_holder);
                        }

                        ///i've even forgotten what this does. :(
                        ke++;
                    }

                }

            }
            else
            {//NO RECORD, SHOW SOMETHING
                Messages msg = new Messages();
                msg.Note("No Purchases available.");
                purchases_r.Children.Add(msg._b_Container);
                RowDefinition rd = new RowDefinition();
                // rd.Height = new GridLength(50);
                purchases_r.RowDefinitions.Add(rd);
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
            p_patients.SelectedValue = new object();
            for (int r = 0; r < row_count; r++)
            {
                Object p = new object[query.CountRows()];
                p = query.Results[r, 1];
                p_patients.Items.Add(p);
               //  t_patients.SelectedValuePath = new string[row_count];
           //    selected_value_path[r] =
               // t_patients.Selec
               //    p_patients.SelectedValuePath = query.Results[r, 1];
              // selected_value[r] = t_patients.SelectedValuePath;
            //       p_patients.SelectedValue = p_patients.SelectedValuePath = query.Results[r,1];
                //editing box
                p_patients_e.Items.Add(p);
                //p_patients_e.SelectedValue = "a";
                //p_patients_e.SelectedValuePath = query.Results[r, 0];
            }

            query.Get("items");
            query.Records();
            for (int r = 0; r < query.CountRows(); r++)
            {
                Object d = query.Results[r, 1];
                p_items.Items.Add(d);
               
                //editing box
                p_items_e.Items.Add(d);
                
              //  p_items_e.SelectedValue = "a";
              //  p_items_e.SelectedValuePath = query.Results[r, 0];
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
            string purchase_quantity = f.Trim(p_quantity.Text);
            //string[] t = f.Selected("0", "purchases");

            string purchase_patient = f.Trim(p_patients.SelectedIndex.ToString());
            string purchase_item = f.Trim(p_items.SelectedIndex.ToString());

            if (!(purchase_quantity == "" || (purchase_item == "" || purchase_item == "0") || (purchase_patient == "" || purchase_patient == "0")))
            {
                //GOOD TO GO

                string[] p_p = f.Selected(p_patients.SelectedIndex, "patients", null, 1);
                string[] p_i = f.Selected(p_items.SelectedIndex, "Items", null, 1);
                purchase_patient = p_p[0];
                purchase_item = p_i[0];
                Query q = new Query();

                //now edit the item quantity
                // the quantity left and the quantity consumed has to be update
                //using simple maths
                q.Get("items","Where item_id = '"+p_i[0]+"'");
                q.Records();
                //check if the quantity requested is not more than the available quantity
                int quant = Convert.ToInt32(p_quantity.Text);//quantity to be consumed
                int item_quant = Convert.ToInt32(q.Results[0,3]);//the quantity left in the item
                int quant_consumed = Convert.ToInt32(q.Results[0,4]);//the quantity already consumed.


                //get the number of quantity left in the item selected
                string[] s_item = f.Selected(p_items.SelectedIndex, "Items", null, 1);
                int the_item = Convert.ToInt32(s_item[3]);
                //##########################################################

                if (quant <= item_quant && the_item > 0)
                {//within range ,allow
                    //NOW DO SOME CALCULATIONS
                    int quant_left = item_quant - quant;
                    int quant_cons = quant_consumed + quant;
                    string[] q_val = { quant_left.ToString(), quant_cons.ToString(), "now()" };
                    string[] val = { purchase_item, purchase_patient, purchase_quantity,user_id, "Now()" };
                    q.successMessage = null;
                    q.Change("items", "quantity_left,quantity_consumed,date_time_updated", q_val, "Where item_id = '" + p_i[0] + "'");
                    q.successMessage = "purchase record has been added successfully!";
                    q.Add("purchases", val, "item_id,patient_id,quantity,_added_by,date_time");


                    this.Frame.Opacity = 1;

                    this.Frame.Navigate(typeof(Purchases), null);
                }
                else
                {
                    if (the_item > 0)
                    {
                        p = new PopUp("Quantity to be bought is more than the available quantity which is " + item_quant);
                    }
                    else
                    {
                        p = new PopUp("Sorry, the item to be bought has no quantity left to be purchased.");

                    }
                }
            }

            else
            {
                
                 if (purchase_quantity == "")
                {
                    p = new PopUp("Please enter a quantity for the item being purchased");
                }
                else if (purchase_patient == "" || purchase_patient == "0")
                {
                    p = new PopUp("Please select a patient");
                }
                else if (purchase_item == "" || purchase_item == "0")
                {
                    p = new PopUp("Please select an item for purchase");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The purchase couldn't be added. Please try again later");
                }
            }

        }



        //FOR EDITING
        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            //CHECK EVERYTHING
            PopUp p;
            Filter f = new Filter();

            int add_sub = 0;

            string purchase_quantity = f.Trim(p_quantity_e.Text);
            //string[] t = f.Selected("0", "purchases");

            string purchase_patient = f.Trim(p_patients_e.SelectedIndex.ToString());
            string purchase_item = f.Trim(p_items_e.SelectedIndex.ToString());

            if (!(purchase_quantity == "" || (purchase_item == "" || purchase_item == "0") || (purchase_patient == "" || purchase_patient == "0")))
            {
                //GOOD TO GO

                string[] p_p = f.Selected(p_patients_e.SelectedIndex, "patients", null, 1);
                string[] p_i = f.Selected(p_items_e.SelectedIndex, "Items", null, 1);
                purchase_patient = p_p[0];
                purchase_item = p_i[0];
                Query q = new Query();

                //now edit the item quantity
                // the quantity left and the quantity consumed has to be update
                //using simple maths
                q.Get("items","Where item_id = '"+p_i[0]+"'");
                q.Records();
                //check if the quantity requested is not more than the available quantity
                int quant = Convert.ToInt32(p_quantity_e.Text);//quantity to be consumed
                int item_quant = Convert.ToInt32(q.Results[0, 3]);//the quantity left in the item
                int quant_consumed = Convert.ToInt32(q.Results[0, 4]);//the quantity already consumed.
                if (p_id.Trim() != "")
                {
                    //check if it's the same item, same patient and same quantity, so as not to keep changing
                    //the quantity left, in case the user just submitted the edit form without change anything
                    q.Get("purchases", "WHERE purchase_id = '" + p_id + "'");
                    q.Record();
                    //get some stuff from different tables, chai.
                    int selected_patient = f.SelectedInverse(q.Result[3], "patients", null, 1);//get the selected index of the edit.
                    int selected_item = f.SelectedInverse(q.Result[1], "items", null, 1);//get the selected index of the edit.
                    int initial_quant = Convert.ToInt32(q.Result[2]);
                    if (purchase_quantity == q.Result[2] && p_patients_e.SelectedIndex == selected_patient && p_items_e.SelectedIndex == selected_item)
                    {//no value was changed
                        p = new PopUp("Nothing in purchase record #(" + p_id + ") was edited. click cancel or edit a value and submit");
                    }
                    
                    else
                    {
                        //get the number of quantity left in the item selected
                        string[] s_item = f.Selected(p_items_e.SelectedIndex, "Items", null, 1);
                        int the_item = Convert.ToInt32(s_item[3]);

                         //check if it's the initial item selected, so that we know what to show
                         Query d = new Query();

                         d.GetExtra("SELECT item_id from purchases WHERE purchase_id = '" + p_id + "'");
                         d.Record();

                         bool cond = false;
                         bool yes_or_no = false;
                         int greater = 0;
                         if (p_i[0] != d.Result[0])
                         {//it's not the initial selected item.
                             
                             //if (the_item > 0)
                             //{
                             //    cond = true;
                                
                             //}

                         //    else
                         //    {
                         //        yes_or_no = false;
                         //    }
                             if (the_item > 0)
                             {
                                 greater = 1;
                             }

                             cond = true;
                         }
                         else
                         {//its the same item chosen
                             yes_or_no = true;

                             cond = false;
                             
                         }
                        //##########################################################
                        if ((quant <= item_quant && cond == true && greater == 1) ||(yes_or_no == true))
                        {//within range ,allow
                            
                            //NOW DO SOME CALCULATIONS
                            
                            int quant_left = 0;
                            int quant_cons = 0;
                            //check for some conditions
                            
                            //CHECK IF THE ITEM WASN'T CHANGED BUT THE QUANTITY WAS CHANGED
                            //so we minus what was changed, or add what was changed
                            if(purchase_quantity != q.Result[2] && p_items_e.SelectedIndex == selected_item)
                            {//the same item, but the quantity was changed, so carefully calculate
                                //if the text box is greater than the initial quant, add what was added to 
                                //quantity consumed and subtract what was subtracted
                               
                                if (quant > initial_quant)
                                {//what was entered is higher than initial so add to quantity consumed and minus from quantity left.
                                    add_sub = quant - initial_quant;

                                    quant_left = item_quant - add_sub;
                                    quant_cons = quant_consumed + add_sub;
                                
                                }
                                else
                                {//what was entered is lower.
                                    add_sub = initial_quant - quant;

                                    quant_left = item_quant + add_sub;
                                    quant_cons = quant_consumed - add_sub;
                                
                                }
                                
                            }
                            else if(purchase_quantity == q.Result[2] && p_patients_e.SelectedIndex != selected_patient &&(p_items_e.SelectedIndex == selected_item))
                            {//the quantity wasn't changed, but the patient was changed, so leave the quantity as it is
                             //
                                quant_left = item_quant;
                                quant_cons = quant_consumed;
                            }
                            else if ((purchase_quantity == q.Result[2] || purchase_quantity != q.Result[2]) && p_items_e.SelectedIndex != selected_item) 
                            {//it's a different item selected, update the quantity of the initial one

                                Query c = new Query();
                                c.Get("items", "WHERE item_id = '" + q.Result[1] + "'");
                                c.Record();

                                int left = Convert.ToInt32(c.Result[3]) + initial_quant;//add to the quantity left
                                int cons =   Convert.ToInt32(c.Result[4]) - initial_quant;//subtract from the quantity consumed.
                                //
                                quant_left = item_quant - quant;
                                quant_cons = quant_consumed + quant;
                                string[] v = {left.ToString(),cons.ToString(),"NOW()"};
                                q.Change("items", "quantity_left,quantity_consumed,date_time_updated", v,"WHERE item_id = '"+q.Result[1]+"'");

                            }
                            else
                            {//its something else, so act normally.
                                quant_left = item_quant - quant;
                                quant_cons = quant_consumed + quant;
                            
                            }
                            string current = DateTime.Now.ToString();
                            string editors = f.GetEditors("purchases", "purchase_id", p_id) + user_id + "," + f.Date(current, e_time_format);
                
                                string[] q_val = { quant_left.ToString(), quant_cons.ToString(), "now()" };
                                string[] val = { purchase_item, purchase_patient, purchase_quantity,editors };

                                if (yes_or_no == true)
                                {//THE ITEM IS THE SAME
                                    //Query h = new Query();
                                //    h.GetExtra("SELECT quantity_left from items WHERE item_id = '"++"'");
                                  //  h.Record();
                                    //int former_quantity = Convert.ToInt32(h.Result[0]);
                                    if (!(quant > initial_quant && add_sub > item_quant))
                                    {

                                        q.successMessage = null;
                                        q.Change("items", "quantity_left,quantity_consumed,date_time_updated", q_val, "Where item_id = '" + p_i[0] + "'");
                                        q.successMessage = "purchase record #(" + p_id + ") has been edited successfully!";

                                        q.Change("purchases", "item_id,patient_id,quantity,_editors", val, "WHERE purchase_id = '" + p_id + "'");
                                        this.Frame.Opacity = 1;

                                        this.Frame.Navigate(typeof(Purchases), null);
                                    }
                                    else
                                    {
                                        p = new PopUp("Sorry, the quantity you added is exceeding the quantity left of the item selected.");

                                    }
                                }

                                else
                                {//THE ITEM WAS CHANGED
                                    q.successMessage = null;
                                    q.Change("items", "quantity_left,quantity_consumed,date_time_updated", q_val, "Where item_id = '" + p_i[0] + "'");
                                    q.successMessage = "purchase record #(" + p_id + ") has been edited successfully!";

                                    q.Change("purchases", "item_id,patient_id,quantity", val, "WHERE purchase_id = '" + p_id + "'");
                                    this.Frame.Opacity = 1;

                                    this.Frame.Navigate(typeof(Purchases), null);
                          
                                }
                        }
                        else
                        {
                            if (the_item > 0)
                            {
                                p = new PopUp("Quantity to be bought is more than the available quantity which is " + item_quant);
                            }
                            else
                            {
                                p = new PopUp("Sorry, the item to be bought has no quantity left to be purchased.");
                            
                            }
                        }
                    }

                }
                else
                {
                    p = new PopUp("Internal error, the record couldn't be edited. Try again later.");

                }
            }

            else
            {

                if (purchase_quantity == "")
                {
                    p = new PopUp("Please enter a quantity for the item being purchased");
                }
                else if (purchase_patient == "" || purchase_patient == "0")
                {
                    p = new PopUp("Please select a patient");
                }
                else if (purchase_item == "" || purchase_item == "0")
                {
                    p = new PopUp("Please select an item for purchase");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The purchase couldn't be added. Please try again later");
                }
            }

        }

        private void p_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//update the quantity of item when choosing, i don tire jare.
            Filter f = new Filter();

            if (Adding.IsOpen)
            {
                string[] p_i = f.Selected(p_items.SelectedIndex, "Items", null, 1);
                int quant = Convert.ToInt32(p_i[3]);
                if (quant > 0)
                    info.Text = "Max. quantity that can be bought: " + p_i[3];
                else
                    info.Text = "No quantity available";
            }
            if (Editing.IsOpen)
            {//its the editing, 
                //check if it's the initial item selected, so that we know what to show
                Query q = new Query();

                q.GetExtra("SELECT item_id from purchases WHERE purchase_id = '" + p_id + "'");
                q.Record();

                string[] p_i = f.Selected(p_items_e.SelectedIndex, "Items", null, 1);
                int quant = Convert.ToInt32(p_i[3]);
                if (p_i[0] != q.Result[0])
                {//show something, it's not the initial selected item.
                    if (quant > 0)
                        info_e.Text = "Max. quantity that can be bought: " + p_i[3];
                    else
                        info_e.Text = "No quantity available";
                }
                else
                {
                    info_e.Text = "";
                }
            }
        }

    }


}
