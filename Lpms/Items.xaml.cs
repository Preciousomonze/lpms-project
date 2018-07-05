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
    /// The items page
    /// </summary>
    public sealed partial class Items : Page
    {
        //EDITING DATA
        EditTracker tracker;
        private string i_id = "";
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        public Items()
        {
            this.InitializeComponent();

            //start displaying the items records
            Query q = new Query();
            q.GetExtra("SELECT item_name,item_cost,quantity_left,quantity_consumed,date_time,_added_by FROM items");
            q.Records();
            int items_row = q.CountRows();
            Query item_id = new Query();
            item_id.Get("items");
            item_id.Records();
            //AMOUNT OF COLUMN DEFINITIONS
            int col_num = Convert.ToInt32(items_r.ColumnDefinitions.Count);
            Filter f = new Filter();

            //  RowDefinition[] rd = new RowDefinition[q.CountRows()];
            // RowDefinition rd;

            bool check_records = q.CheckRecords();
          
            //STACKPANEL TO HOLD THE ROWS
            // StackPanel parent_row_panel;
            //WE'RE ADDING +1 CAUSE THERE'S ALREADY A ROW DEFINED WHICH IT'S VALUE WOULD BE ZERO
            //SO IF WE DONT INCREMENT IT, IT STARTS WITH 0, WHICH MIGHT BE A BAD IDEA :(.
            //int i = 0;
            //now check the number of rows
            //keep adding rows as long as it doesn't exceed the row limit from the stupid database.

            int i = 0;
            if (check_records)
            {
                for (i = 0; i < (items_row); i++)
                {
                    RowDefinition rd = new RowDefinition();
                    //rd.Height = new GridLength(50);
                    items_r.RowDefinitions.Add(rd);

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

                        items_r.ColumnDefinitions.Add(cd);

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

                        if (calcu == 1)
                        {//LAST VALUE

                        }
                        else if (k == 2 || k == 3)
                        {
                            if (q.Results[i, k] == "0")
                            {
                                n.Text = "None";
                            }
                            else
                            {
                                n.Text = f.Numbers(Convert.ToInt32(q.Results[i, k]));
                            }
                        }
                        else if (k == 4)
                        {
                            n.Text = f.Date(q.Results[i, k], "dd/MM/yyyy");
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
                            HyperlinkButton[,] link = new HyperlinkButton[items_row, stuff_text.Length];
                            int reduce = i;
                            if (i == items_row)
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

                                        tracker = new EditTracker("items", "item_id", item_id.Results[(reduce), 0]);
                                        //track the row.
                                        tracker.Track();

                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Items", "item_id", item_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't edit Item " + tracker.TrackResult[1] + "'s record cause you didn't add the record");
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
                                                this.i_id = tracker.TrackResult[0];//set the id to the global var, so that we know what to edit.
                                                i_name_e.Text = tracker.TrackResult[1];
                                                i_cost_e.Text = tracker.TrackResult[2];
                                                i_quantity_e.Text = tracker.TrackResult[3];

                                                Editing.IsOpen = true;

                                            }
                                        }
                                    }
                                    else if ((string)link[reduce, reduce_t].Content == stuff_text[1])
                                    {//it's the delete.
                                        tracker = new EditTracker("items", "item_id", item_id.Results[(reduce), 0]);
                                        //track the row
                                        tracker.Track();

                                        Auth auth = new Auth();
                                        if (auth.CheckOwner("Items", "item_id", item_id.Results[reduce, 0]) != true)
                                        {
                                            PopUp p = new PopUp("Sorry, You can't delete this item's record cause you didn't add the record");
                                        }
                                        else
                                        {
                                            this.Frame.Opacity = .4;

                                            //this.Opacity = .4;
                                            parent.IsEnabled = false;

                                            Messages msg = new Messages();
                                            msg.Confirm("Are you sure you want to delete this item's record?");
                                            msg.p_container.IsOpen = true;
                                            parent_grid.Children.Add(msg.p_container);

                                            msg.TrueBtn.Click += (z, x) =>
                                            {


                                                //CALL THE DELETE QUERY
                                                Query que = new Query();
                                                que.successMessage = "Item " + tracker.TrackResult[1] + " with id(" + tracker.TrackResult[0] + ") has been deleted.";

                                                que.Remove("Items", "Where item_id = '" + item_id.Results[(reduce), 0] + "'");
                                                this.Frame.Opacity = 1;

                                                this.Frame.Navigate(typeof(Items), null);
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
                            items_r.Children.Add(option_border);

                        }

                        else
                        {
                            Grid.SetRow(row_holder, i + 1);

                            Grid.SetColumn(row_holder, k);
                            items_r.Children.Add(row_holder);

                        }

                    }

                }
            }
            else
            {
                //NO RECORD
                Messages msg = new Messages();
                msg.Note("No items record available.");
                items_r.Children.Add(msg._b_Container);
                RowDefinition rd = new RowDefinition();
                // rd.Height = new GridLength(50);
                items_r.RowDefinitions.Add(rd);
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
            string item_name = f.Trim(i_name.Text);
            string item_quantity = f.Trim(i_quantity.Text);
            string item_cost = f.Trim(i_cost.Text);

            if (!(item_name == "" || item_quantity == "" || item_cost == "" ))
            {
                //GOOD TO GO
                Query q = new Query();
               

                string[] val = { item_name,item_cost,item_quantity,user_id, "Now()" };
                q.successMessage = "Item " + item_name + " has been added successfully!";
                q.Add("items", val, "item_name,item_cost,quantity_left,_added_by,date_time");

                this.Frame.Opacity = 1;

                this.Frame.Navigate(typeof(Items), null);

            }

            else
            {
                if (item_name == "")
                {
                    p = new PopUp("Please enter the name of the item");
                }
                else if (item_cost == "")
                {
                    p = new PopUp("Please enter the price of the item");
                }
                else if (item_quantity == "")
                {
                    p = new PopUp("Please enter the quantity of the item.");
                }
               
                else
                {
                    p = new PopUp("Sorry, an error occured. The item couldn't be added. Please try again later");
                }

            }

        }



        //FOR EDITING
        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            PopUp p;
            Filter f = new Filter();
            // tracker.Track();
            // t_price.Text = tracker.TrackResult.Length.ToString();
            //CHECK EVERYTHING
            string item_name = f.Trim(i_name_e.Text);
            string item_quantity = f.Trim(i_quantity_e.Text);
            string item_cost = f.Trim(i_cost_e.Text);

            if (!(item_name == "" || item_quantity == "" || item_cost == ""))
            {
                //GOOD TO GO
                Query q = new Query();
                q.Get("items","Where item_id = '"+i_id+"'");
                q.Record();
                string current = DateTime.Now.ToString();
                    string editors = f.GetEditors("items", "item_id", q.Result[0]) +user_id + ","+f.Date(current,e_time_format);
                string[] val = { item_name, item_cost, item_quantity,editors, "Now()" };
                q.successMessage = "Item " + q.Result[1] + " with id #"+i_id+" has been updated successfully!";
                q.Change("items","item_name,item_cost,quantity_left,_editors,date_time_updated", val,"Where item_id = '"+i_id+"'");

                this.Frame.Opacity = 1;

                this.Frame.Navigate(typeof(Items), null);

            }

            else
            {
                if (item_name == "")
                {
                    p = new PopUp("Please enter the name of the item");
                }
                else if (item_cost == "")
                {
                    p = new PopUp("Please enter the price of the item");
                }
                else if (item_quantity == "")
                {
                    p = new PopUp("Please enter the quantity of the item.");
                }

                else
                {
                    p = new PopUp("Sorry, an error occured. The item couldn't be added. Please try again later");
                }

            }


        }

    }
}
