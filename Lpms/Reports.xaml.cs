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
using System.Drawing;
using Lpms.Classes;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Lpms
{
    /// <summary>
    /// The reports page
    /// </summary>
    public sealed partial class Reports : Page
    {
        string r_id = "";
        string user_id = (App.Current as App).User_token.ToString();
        string e_time_format = (App.Current as App).Edit_time_format;

        EditTracker tracker;
        /// <summary>
        /// the current test report being shown,holds the id of the test report being shown
        /// </summary>
        string chosen_test = ((App.Current as App).chosen_test_for_report == null ? "" : (App.Current as App).chosen_test_for_report);
        Filter f = new Filter();
        /// <summary>
        /// the theme color
        /// </summary>
        SolidColorBrush theme_color = new SolidColorBrush(Color.FromArgb(255, 75, 167, 131));
        /// <summary>
        /// the selected test color
        /// </summary>
        SolidColorBrush selected_color = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
        public Reports()
        {
            this.InitializeComponent();
            
            //start displaying the items records
            Query q = new Query();
            //show according to what is needed
          
          if (this.chosen_test == "")
          {
              q.Get("reports");
          }
          else
          {
              q.Get("reports", "Where test_id = '" + this.chosen_test + "'");
          }
            q.Records();
            int reports_row = q.CountRows();
            //get the exact location of the _added_by column, so we count from behind, cause we know the position from the back.
            int reports_cols = q.CountFields() - 1;
            int adder_col = 0;
            //int v = 0;

            for (int w = reports_cols, pos = 0; w >= 0; w--,pos++ )
            {
                if (pos == 2)
                {//it's the _added_by col
                    adder_col = w;
                }
                if (pos > 2)
                {
                    break;
                }
            }
            bool check_records = q.CheckRecords();
          
            int i = 0;
            if (check_records == true)
            {
                //create 2 parts, one that shows the test to select

                Grid main_g = new Grid();
                RowDefinition rd = new RowDefinition();
                StackPanel main_p = new StackPanel();
                Border side_p_border = new Border();

                StackPanel side_p = new StackPanel();
                Border side_p_inner_border = new Border();
                side_p_inner_border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                side_p_inner_border.BorderThickness = new Thickness(1);
                side_p_inner_border.HorizontalAlignment = HorizontalAlignment.Stretch;
                side_p_inner_border.Margin = new Thickness(0, 0, 0, 4.0);
                StackPanel side_p_head = new StackPanel();
                side_p_head.Background = new SolidColorBrush(Color.FromArgb(255, 250, 251, 251));
                TextBlock side_p_head_txt = new TextBlock();
                side_p_head_txt.Text = "Tests";
                side_p_head_txt.FontSize = 23;
                side_p_head_txt.Padding = new Thickness(10, 9, 5, 5);

                side_p_head_txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));

                //design
                main_p.Margin = new Thickness(10, 7, 25, 5);

                side_p_border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                //side_p_border.BorderThickness = new Thickness(1);
                // side_p_border.Background = theme_color;
                // side_p_border.CornerRadius = new CornerRadius(3);
                side_p_border.Margin = new Thickness(5, 10, 5, 1);
                side_p_border.HorizontalAlignment = HorizontalAlignment.Right;
                ///////////////////////////////

                for (int j = 0; j < 2; j++)
                {//create 2 columns
                    ColumnDefinition cd = new ColumnDefinition();
                    if (j == 1)
                    {
                        cd.Width = new GridLength(250);
                    }
                    else
                    {
                        cd.Width = new GridLength(600);
                    }
                    main_g.ColumnDefinitions.Add(cd);
                }

                Grid.SetColumn(main_p, 0);
                Grid.SetRow(main_p, 0);

                //for the side menu.
                Grid.SetColumn(side_p_border, 1);
                Grid.SetRow(side_p_border, 0);
                main_g.RowDefinitions.Add(rd);
                //
                side_p_head.Children.Add(side_p_head_txt);
                side_p_inner_border.Child = side_p_head;
                side_p.Children.Add(side_p_inner_border);
                side_p_border.Child = side_p;

                main_g.Children.Add(side_p_border);

                main_g.Children.Add(main_p);
                reports_r.Children.Add(main_g);

                //design the side bar that shows the list of tests that have reports
                Query s_t = new Query();

                s_t.GetExtra("Select test_id,test_name from tests");
                s_t.Records();
                int counter = s_t.CountRows();
                //get the number of tests that have reports
                Query single = new Query();
                single.GetExtra("Select report_name,test_id FROM reports");
                int single_count = single.CountRows();
                single.Records();
                string test_checker = single.Results[0, 1];
                for (int m = 0; m < single_count; m++)
                {
                    //check if the test isn't deleted already
                    Query g = new Query();
                    g.GetExtra("SELECT test_id FROM tests WHERE test_id = '" + single.Results[m, 1] + "'");
                    if (g.CheckRecords() == true)
                    {
                        //check if they belong to more than one report
                        if (test_checker != single.Results[m, 1])
                        {//there's another test, so it's more than one
                            //lets show all
                            Button a_btn = new Button();
                            a_btn.Content = "All";
                            a_btn.BorderBrush = new SolidColorBrush(Color.FromArgb(23, 255, 255, 255));
                            a_btn.BorderThickness = new Thickness(1, 1, 1, 1);
                            a_btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                            a_btn.Background = theme_color;
                            a_btn.Padding = new Thickness(15, 20, 15, 5);
                            a_btn.BorderBrush = new SolidColorBrush(Color.FromArgb(70, 0, 0, 0));
                            a_btn.Margin = new Thickness(-3, -10, -3, 1);
                            if (this.chosen_test == "")
                            {
                                //its all that was chosen, change the color
                                a_btn.Background = selected_color;
                                a_btn.Foreground = new SolidColorBrush(Colors.White);
                            }
                            a_btn.Click += (s, e) =>
                            {
                                //show the selected test reports
                                //set the value of chosen_test to empty;
                                (App.Current as App).chosen_test_for_report = "";
                                this.Frame.Navigate(typeof(Reports), null);
                            };

                            side_p.Children.Add(a_btn);
                            break;
                        }
                    }
                }

                Button[] btn = new Button[counter];

                for (int k = 0; k < counter; k++)
                {//now lets check for tests that have reports ,so that we don't display all/
                    Query t = new Query();
                    t.GetExtra("Select report_name FROM reports where test_id = '" + s_t.Results[k, 0] + "' ");
                    if (t.CheckRecords() == true)
                    {//there are records
                        int o = 0;
                        //start designing
                        int calc = counter - k;
                        btn[k] = new Button();
                        StackPanel panel = new StackPanel();
                        panel.Orientation = Orientation.Horizontal;
                        panel.Margin = new Thickness(15, 20, 15, 5);
                        panel.HorizontalAlignment = HorizontalAlignment.Center;
                        StackPanel count_p = new StackPanel();
                        count_p.Background = new SolidColorBrush(Colors.White);

                        TextBlock count_txt = new TextBlock();
                        count_p.Margin = new Thickness(10, 0, 0, 0);

                        count_txt.Text = t.CountRows().ToString();
                        count_txt.FontSize = 14.5;
                        count_txt.Foreground = theme_color;
                        count_txt.Padding = new Thickness(7, 3, 7, 3);

                        TextBlock txt = new TextBlock();
                        txt.Text = s_t.Results[k, 1];
                        txt.FontSize = 15.5;

                        btn[k].Foreground = new SolidColorBrush(Colors.White);
                        btn[k].HorizontalAlignment = HorizontalAlignment.Stretch;
                        btn[k].BorderBrush = new SolidColorBrush(Color.FromArgb(70, 0, 0, 0));
                        btn[k].Background = theme_color;

                        //check if it's the only result,make sure it's not a deleted test.
                        int amount = 0;
                        for (int h = 0; h < q.CountRows(); h++)
                        {
                            Query g = new Query();
                            g.GetExtra("SELECT test_id from tests WHERE test_id = '" + q.Results[h, 3] + "'");
                            g.Record();
                            if (g.CheckRecords() == true)
                            {//now check if h has looped once
                                if (h > 0)
                                {//now check if the previous value is the same, if it's only one value, they have to be the same :-)
                                    if(q.Results[h,3] != q.Results[h-1,3])
                                    {
                                                amount += 1;
                                    }
                                }
                        
                            }
                        }
                        if (amount > 0 && amount <= 1)
                        {//only 1 value
                            //its the one that's chosen, change the color
                            btn[k].Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                            btn[k].Background = selected_color;
                            count_txt.Foreground = selected_color;

                            sort_txt.Text = "| Showing result for " + s_t.Results[k, 1];
                            sort_txt.FontSize = 13;
                            sort_txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
                            sort_txt.VerticalAlignment = VerticalAlignment.Center;
                            
                        }
                        else
                        {//ts more than one
                            if (this.chosen_test == s_t.Results[k, 0])
                            {
                                //its the one that's chosen, change the color
                                btn[k].Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                                btn[k].Background = selected_color;
                                count_txt.Foreground = selected_color;

                                sort_txt.Text = "| Showing result for " + s_t.Results[k, 1];
                                sort_txt.FontSize = 13;
                                sort_txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
                                sort_txt.VerticalAlignment = VerticalAlignment.Center;
                            }
                        }

                        btn[k].Margin = new Thickness(-3, -6.6, -3, 0);
                        if (calc == 1)
                        {
                            btn[k].BorderThickness = new Thickness(0, 0, 0, 0);
                        }
                        else
                        {
                            btn[k].BorderThickness = new Thickness(1, 0, 1, 1);
                        }
                        count_p.Children.Add(count_txt);
                        panel.Children.Add(txt);
                        panel.Children.Add(count_p);
                        btn[k].Content = panel;

                        //click event
                        //btn[k].Click +=Cancel_Click;
                        //lets solve the annoying issue that's common, if bla is the last value, bla - 1
                        int reduce = k;
                        if (k == counter)
                        {
                            //the last value
                            reduce = k - 1;
                        }
                        btn[k].Click += (s, e) =>
                        {
                            //show the selected test reports
                            //set the value of chosen_test to the selected test id
                            (App.Current as App).chosen_test_for_report = s_t.Results[reduce, 0];
                            this.Frame.Navigate(typeof(Reports), null);
                        };

                        side_p.Children.Add(btn[k]);

                    }
                }

                int ot = 0;//for switching designs.
                for (i = 0; i < (reports_row); i++)
                {

                    //get test info
                    Query c = new Query();
                    c.GetExtra("Select test_id,test_name FROM tests Where test_id = '" + q.Results[i, 3] + "'");
                    c.Record();


                    if (c.CheckRecords())
                    {
                        //start generating designs
                        //get color that will be used
                        SolidColorBrush text_color = new SolidColorBrush(Color.FromArgb(255, 12, 12, 12));
                        SolidColorBrush head_bg = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        SolidColorBrush body_bg = new SolidColorBrush(Color.FromArgb(255, 250, 250, 250));
                        SolidColorBrush footer_bg = head_bg;
                        SolidColorBrush f_text_color = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));
                        SolidColorBrush f_text_color2 = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));

                        double footer_txt_size = 15;
                        //pattern
                        /*
                         * <border>
                         *  <stackpanel>
                         *      <stackpanel head>
                         *          <Textblock/>
                         *          <border/>
                         *      </stackpanel head>
                         *      <stackpanel body>
                         *          <Textblock/>
                         *      </stackpanel body>
                         *      <stackpanel footer>
                         *          <textblock/>
                         *          <textblock/>
                         *      </stackpanel footer>
                         *  </stackpanel>
                         *  </border>
                         */

                        Border parent_border = new Border();
                        parent_border.Margin = new Thickness(5, 15, 5, 5);
                        parent_border.CornerRadius = new CornerRadius(5);
                        StackPanel parent_panel = new StackPanel();
                        //#################### HEAD
                        StackPanel head_panel = new StackPanel();
                        head_panel.MinHeight = 60;

                        Border head_border_t = new Border();
                        head_border_t.BorderThickness = new Thickness(2);
                        head_border_t.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 41, 128, 185));

                        StackPanel inner_head = new StackPanel();
                        inner_head.Margin = new Thickness(10, 6, 5, 5);

                        StackPanel inner_head1 = new StackPanel();
                        StackPanel inner_head2 = new StackPanel();
                        inner_head2.HorizontalAlignment = HorizontalAlignment.Right;
                        inner_head2.Orientation = Orientation.Horizontal;
                        inner_head2.Margin = new Thickness(1, -60, 0, 0);

                        TextBlock head_txt = new TextBlock();
                        head_txt.FontSize = 20;
                        head_txt.Foreground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                        TextBlock head_txt_sub = new TextBlock();
                        head_txt_sub.FontSize = 16;
                        head_txt_sub.Margin = new Thickness(0, 5, 0, 10);
                        head_txt_sub.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 65, 65));

                        ////////////////////////
                        parent_border.BorderThickness = new Thickness(1);
                        parent_border.BorderBrush = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));

                        ////////////////////////

                        //design
                        //inner_head.Orientation = Orientation.Horizontal;
                        head_txt.Text = q.Results[i, 1] + " ";//name of report
                        string test_name = c.RecordExist(c.Result[1], "");
                        string test_id = c.Result[0];
                        ///////////////////////////////////////////
                        Query inner_q = new Query();
                        inner_q.Get("staffs", "WHERE staff_id ='" + q.Results[i, adder_col] + "'");
                        //for showing the adder

                        string person = "| Added by ";
                        if (inner_q.CheckRecords() == true)
                        {
                            inner_q.Record();
                            if ((App.Current as App).User_token.ToString() == inner_q.Result[0])
                                person += "You";
                            else
                                person += inner_q.Result[1];

                        }
                        else
                        {
                            person = "";
                        }
                        head_txt_sub.Text = c.RecordExist("Report for " + test_name + " #(" + test_id + ") "+person+"", "Test Record not available");
                        head_txt_sub.Foreground = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90));
                        head_txt_sub.FontSize = 14;
                        head_txt.FontSize = 18;
                        head_panel.Background = head_bg;
                        head_panel.Height = 50;

                        //Menu flyout
                        //MenuFlyout menu = new MenuFlyout();
                        //Button tn = new Button();
                        //MenuFlyoutItemBase b ;

                        //ADDING THE EDIT AND DELETE STUFF

                        string[] stuff_text = { "Edit", "Delete" };
                        string[] stuff_image = { "", "" };
                        HyperlinkButton[] link = new HyperlinkButton[stuff_text.Length];
                        Border[] brd = new Border[stuff_text.Length];
                        int reduce = i;
                        if (i == counter)
                        {
                            reduce = i - 1;
                        }

                        for (int t = 0; t < stuff_text.Length; t++)
                        {

                            //  option_border.BorderThickness = new Thickness(0, 0, 0, 1);
                            //option_border.BorderBrush = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                            Image img = new Image();
                            ImageBrush source = new ImageBrush();//new Uri("Assets/icons/edit-black.png");
                            //source.ImageSource = source.
                            //img.source = 
                            TextBlock option_txt = new TextBlock();
                            option_txt.Foreground = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
                            option_txt.FontSize = 15;
                            option_txt.Foreground = new SolidColorBrush(Colors.White);
                            option_txt.Text = stuff_text[t];
                            option_txt.Margin = new Thickness(5, 0, 0, 0);
                            option_txt.VerticalAlignment = VerticalAlignment.Center;
                          //  Bitm
                            // Button button = new Button();
                            link[t] = new HyperlinkButton();
                            brd[t] = new Border();
                            brd[t].Background = selected_color;
                            if (option_txt.Text == stuff_text[1])
                                brd[t].Background = new SolidColorBrush(Colors.IndianRed);
                            brd[t].BorderBrush = new SolidColorBrush(Color.FromArgb(53,0,0,0));
                            brd[t].BorderThickness = new Thickness(1);
                            link[t].Foreground = new SolidColorBrush(Colors.White);
                            link[t].Content = option_txt;
                            link[t].Margin = new Thickness(0);
                            brd[t].CornerRadius = new CornerRadius(3);
                           // brd[t].Padding = new Thickness(-5);
                            brd[t].MaxWidth = 85;
                            brd[t].MaxHeight = 38;
                            link[t].Click += (s, e) =>
                            {
                                if (option_txt.Text == stuff_text[0])
                                {//editing time
                                    //call the editing stuff
                                    tracker = new EditTracker("reports", "report_id", q.Results[(reduce), 0]);
                                    //track the row.
                                    tracker.Track();

                                    Auth auth = new Auth();
                                    if (auth.CheckOwner("Reports", "report_id", q.Results[reduce, 0]) != true)
                                    {
                                        PopUp p = new PopUp("Sorry, You can't edit Report \"" + tracker.TrackResult[1] + "\" cause you didn't add the report.");
                                    }
                                    else
                                    {

                                        if (!Editing.IsOpen)
                                        {

                                            //OPEN
                                            this.Frame.Opacity = .4;

                                            parent.IsEnabled = false;
                                            //SET THE EDIT TRACKER
                                            //I DID (i-1) BECAUSE IT WAS INCREASE BY ONE, THEREFORE OUT OF BOUND ISH.

                                            this.r_id = tracker.TrackResult[0];//set the id to the global var, so that we know what to edit.
                                            r_name_e.Text = tracker.TrackResult[1];
                                            r_content_e.Text = tracker.TrackResult[2];
                                            Query a = new Query();
                                            a.Get("tests", "Where test_id = '" + q.Results[reduce, 3] + "'");
                                            a.Record();
                                            r_tests_e.SelectedIndex = f.SelectedInverse(a.Result[0], "tests", null, 1);
                                            Editing.IsOpen = true;
                                        }
                                    }
                                }
                                else if (option_txt.Text == stuff_text[1])
                                {//delete
                                    //track first
                                    tracker = new EditTracker("Reports", "report_id", q.Results[(reduce), 0]);
                                    //track
                                    tracker.Track();

                                    Auth auth = new Auth();
                                    if (auth.CheckOwner("Reports", "report_id", q.Results[reduce, 0]) != true)
                                    {
                                        PopUp p = new PopUp("Sorry, You can't delete Report \"" + tracker.TrackResult[1] + "\" cause you didn't add the report");
                                    }
                                    else
                                    {
                                        this.Frame.Opacity = .4;

                                        //this.Opacity = .4;
                                        parent.IsEnabled = false;

                                        Messages msg = new Messages();
                                        msg.Confirm("Are you sure you want to delete Report \"" + tracker.TrackResult[1] + "\" ?");
                                        msg.p_container.IsOpen = true;
                                        parent_grid.Children.Add(msg.p_container);

                                        msg.TrueBtn.Click += (z, x) =>
                                        {


                                            //CALL THE DELETE QUERY
                                            //CALL THE DELETE QUERY
                                            Query que = new Query();
                                            que.successMessage = "Report has been deleted successfully.";
                                            que.Remove("reports", "where report_id = '" + tracker.TrackResult[0] + "'");
                                            (App.Current as App).chosen_test_for_report = "";
                                            this.Frame.Opacity = 1;

                                            this.Frame.Navigate(typeof(Reports), null);
                                        };

                                        msg.FalseBtn.Click += (z, x) =>
                                        {
                                            this.Frame.Opacity = 1;

                                            parent.IsEnabled = true;

                                        };
                                        
                                    }
                                }
                            };
                            //add
                            brd[t].Child = link[t];
                            inner_head2.Children.Add(brd[t]);

                        }
                        //  inner_head2.Children.Add();

                        //######################## /head


                        //######################## body
                        Border body_border = new Border();
                        body_border.BorderThickness = new Thickness(0, 1, 0, 1);
                        body_border.BorderBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
                        //  body_border.Margin = new Thickness(0, 5, 0, 0);


                        StackPanel body_panel = new StackPanel();
                        body_panel.Background = body_bg;
                        TextBlock body_txt = new TextBlock();
                        body_txt.Text = q.Results[i, 2];
                        body_txt.Foreground = text_color;
                        body_txt.FontSize = 16;
                        body_txt.Padding = new Thickness(10, 7, 8, 9);
                        body_txt.CharacterSpacing = 12;
                        // body_txt.TextTrimming = TextTrimming.CharacterEllipsis;
                        body_txt.TextWrapping = TextWrapping.Wrap;
                        body_txt.IsTextSelectionEnabled = true;
                        //######################## /body

                        //######################## footer
                        StackPanel footer_panel = new StackPanel();
                        footer_panel.Background = footer_bg;

                        StackPanel inner_footer_panel1 = new StackPanel();
                        inner_footer_panel1.Margin = new Thickness(10, 10, 5, 7);
                        inner_footer_panel1.Orientation = Orientation.Horizontal;
                        StackPanel inner_footer_panel2 = new StackPanel();
                        inner_footer_panel2.Margin = new Thickness(5, -25, 10, 7);
                        inner_footer_panel2.Orientation = Orientation.Horizontal;
                        inner_footer_panel2.HorizontalAlignment = HorizontalAlignment.Right;

                        TextBlock footer_txt1 = new TextBlock();

                        TextBlock footer_txt2 = new TextBlock();
                        TextBlock footer_txt3 = new TextBlock();
                        TextBlock footer_txt4 = new TextBlock();
                        footer_txt1.Text = "Created: ";
                        footer_txt1.Foreground = f_text_color;
                        footer_txt1.FontSize = footer_txt_size;
                        footer_txt1.CharacterSpacing = 12;

                        footer_txt2.Text = f.Date(q.Results[i, 4], "");
                        footer_txt2.Foreground = f_text_color2;
                        footer_txt2.FontSize = 13;
                        footer_txt2.CharacterSpacing = 12;
                        footer_txt2.Margin = new Thickness(5, 0, 0, 0);

                        footer_txt3.Text = "Last Updated: ";
                        footer_txt3.Foreground = f_text_color;
                        footer_txt3.FontSize = footer_txt_size;
                        footer_txt3.CharacterSpacing = 12;

                        footer_txt4.Text = f.CheckEmpty(f.Date(q.Results[i, 5], ""), "Never");
                        footer_txt4.FontStyle = Windows.UI.Text.FontStyle.Italic;
                        footer_txt4.Foreground = f_text_color2;
                        footer_txt4.FontSize = 13;
                        footer_txt4.CharacterSpacing = 12;
                        footer_txt4.Margin = new Thickness(5, 0, 0, 0);
                        //######################## /footer
                        int calc = ot % 2;

                        if (calc == 1)
                        {
                            head_border_t.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 120, 28, 45));
                        }

                        ot++;
                        //adding
                        inner_head1.Children.Add(head_txt);
                        inner_head1.Children.Add(head_txt_sub);
                        inner_head.Children.Add(inner_head1);

                        inner_head.Children.Add(inner_head2);
                        head_panel.Children.Add(head_border_t);
                        head_panel.Children.Add(inner_head);
                        //head_panel.Children.Add(head_border_b);

                        body_panel.Children.Add(body_txt);
                        body_border.Child = body_panel;

                        //adding footer stuff
                        inner_footer_panel1.Children.Add(footer_txt1);
                        inner_footer_panel1.Children.Add(footer_txt2);
                        inner_footer_panel2.Children.Add(footer_txt3);
                        inner_footer_panel2.Children.Add(footer_txt4);

                        footer_panel.Children.Add(inner_footer_panel1);
                        footer_panel.Children.Add(inner_footer_panel2);


                        parent_panel.Children.Add(head_panel);
                        parent_panel.Children.Add(body_border);
                        parent_panel.Children.Add(footer_panel);
                        parent_border.Child = parent_panel;

                        main_p.Children.Add(parent_border);

                    }
                }
            }
            else
            {
                //NO RECORD  
                Messages msg = new Messages();
                msg.Note("No Reports record available.");
                reports_r.Children.Add(msg._b_Container);

            }
        

             ////////////////////////////////////////////////
            //fill some stuff in the form
            Query query = new Query();
            query.Get("Tests");

            int row_count = query.CountRows();
            query.Records();


            for (int r = 0; r < row_count; r++)
            {

                object p = query.Results[r, 1]+" #("+query.Results[r,0]+")";
                //  t_patients.Items.Add(p[r]);
                r_tests.Items.Add(p);
                r_tests_e.Items.Add(p);
            }
        }

        
        //Cancel BTN
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {   if (Editing.IsOpen)
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
            string report_content = f.Trim(r_content.Text);
            
            string report_name = f.Trim(r_name.Text);
            string report_test = f.Trim(r_tests.SelectedIndex.ToString());

            if (!(report_content == "" || (report_test == "" || report_test == "0") || report_name == "" ))
            {
                //GOOD TO GO

                string[] r_t = f.Selected(r_tests.SelectedIndex, "Tests", null, 1);
               report_test = r_t[0];
                Query q = new Query();

                    string[] val = { report_test, report_name, report_content,user_id, "Now()" };
                    q.successMessage = "Report "+report_name+" has been added successfully!";
                    q.Add("reports", val, "test_id,report_name,report,_added_by,date_time");


                    this.Frame.Opacity = 1;

                    this.Frame.Navigate(typeof(Reports), null);
              }

            else
            {

                if (report_content == "")
                {
                    p = new PopUp("Please say something about the report");
                }
                else if (report_name == "")
                {
                    p = new PopUp("Please give the report a name");
                }
                else if (report_test == "" || report_test == "0")
                {
                    p = new PopUp("Please select a test the report belongs to");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The report couldn't be added. Please try again later");
                }
            }

        }

    
        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            //CHECK EVERYTHING
            PopUp p;
            Filter f = new Filter();
            string report_content = f.Trim(r_content_e.Text);

            string report_name = f.Trim(r_name_e.Text);
            string report_test = f.Trim(r_tests_e.SelectedIndex.ToString());

            if (!(report_content == "" || (report_test == "" || report_test == "0") || report_name == ""))
            {
                //GOOD TO GO

                string[] r_t = f.Selected(r_tests_e.SelectedIndex, "Tests", null, 1);
                report_test = r_t[0];
                Query q = new Query();
                //##################################CHECKING IF ANYTHING WAS EDITED
                q.GetExtra("SELECT report_name,test_id,report FROM reports WHERE report_id = '" + r_id + "'");
                q.Record();
                string former_name = q.Result[0];
                string former_test = q.Result[1];
                string former_content = q.Result[2];

                    //check if anything was edited
                if(report_name == former_name && report_test == former_test && report_content == former_content)
                {//the same thing,nothing was edited.
                    p = new PopUp("Nothing in this report was edited. click cancel or edit a value and submit");
                    
                }

                else{
                    string recent = DateTime.Now.ToString();
                    string editors = f.GetEditors("Reports", "report_id", r_id) + user_id + ","+ f.Date(recent, e_time_format);
                string[] val = { report_test, report_name, report_content,editors, "Now()" };
                q.successMessage = "Report " + report_name + " has been edited successfully!";
                q.Change("reports", "test_id,report_name,report,_editors,date_time_updated",val,"WHERE report_id = '"+r_id+"'");


                this.Frame.Opacity = 1;

                this.Frame.Navigate(typeof(Reports), null);
                }
            }

            else
            {

                if (report_content == "")
                {
                    p = new PopUp("Please say something about the report");
                }
                else if (report_name == "")
                {
                    p = new PopUp("Please give the report a name");
                }
                else if (report_test == "" || report_test == "0")
                {
                    p = new PopUp("Please select a test the report belongs to");
                }
                else
                {
                    p = new PopUp("Sorry, an error occured. The report couldn't be added. Please try again later");
                }
            }

        }
    }
}


