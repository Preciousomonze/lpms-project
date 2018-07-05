using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
namespace Lpms.Classes
{
    /// <summary>
    /// HELPS TO DISPLAY ANY MESSAGE YOU WANT, IF IT'S ERROR,ETC.
    /// </summary>
    class Messages
    {
        Filter f = new Filter();
        //private int panel_min_width = 40;
        private int panel_min_height = 40;
        /// <summary>
        /// Gets the value of the confirm message, if true or false
        /// </summary>
        public bool confirmed { get; private set; }
        public object Container { get; private set; }
        /// <summary>
        /// gets the button that returns true
        /// </summary>
        public Button TrueBtn { get; private set; }
        /// <summary>
        /// gets the button that returns false
        /// </summary>
        public Button FalseBtn { get; private set; }

        public Popup p_container { get; private set; }
        private Border b_container;
        public Border _b_Container
        {
            get { return b_container;}
          private  set{b_container = value;}
        }
        public Messages()
        {

        }
        /// <summary>
        /// Displays 
        /// </summary>
        /// <param name="msg"></param>
        public void Note(string msg)
        {
            if (msg != null || msg != "")
            {
                //BORDER
                b_container = new Border();
                b_container.BorderThickness = new Thickness(1);
                b_container.BorderBrush = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
                b_container.Background = new SolidColorBrush(Color.FromArgb(255, 217, 237, 247));
                //STACK PANEL
                StackPanel er_panel = new StackPanel();
                er_panel.MinHeight = panel_min_height;
                //TEXTBLOCK
                TextBlock t = new TextBlock();
                t.Text = msg;
                t.FontSize = 17;
                t.TextAlignment = TextAlignment.Center;
                t.Padding = new Thickness(5, 10, 5, 10);
                
                t.Foreground = new SolidColorBrush(Color.FromArgb(255, 49, 112, 143));
                er_panel.Children.Add(t);
                b_container.Margin = new Thickness(5, 7,5,7);
                b_container.Child = er_panel;
                
            }
            else
            {
                //IF THIS CONDITION IS MET
                //THE PERSON IS AN IMBE, HE OR SHE HAS TO OBVIOUSLY PUT A MESSAGE :( SMH.
            }
        }
        /// <summary>
        /// Displays an error message
        /// </summary>
        /// <param name="msg">the message to be display</param>
        public void Error(string msg)
        {
            if (msg != null || msg != "")
            {
                //BORDER
                b_container = new Border();
                b_container.BorderThickness = new Thickness(1);
                b_container.BorderBrush = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
                b_container.Background = new SolidColorBrush(Color.FromArgb(200, 230, 0, 0));
                //STACK PANEL
                StackPanel er_panel = new StackPanel();
                er_panel.MinHeight = panel_min_height;
                //TEXTBLOCK
                TextBlock t = new TextBlock();
                t.Text = msg;
                t.FontSize = 17;
                t.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                er_panel.Children.Add(t);
                b_container.Child = er_panel;

            }
            else
            {
                //IF THIS CONDITION IS MET
                //THE PERSON IS AN IMBE, HE OR SHE HAS TO OBVIOUSLY PUT A MESSAGE :( SMH.
            }
        }
        /// <summary>
        /// For showing a confirmation popup
        /// </summary>
        /// <param name="msg">The message to be displayed in the popup.</param>
        /// <param name="true_btn">The text to be displayed in the true button</param>
        /// <param name="false_btn">The text to be displayed in the false btn</param>
        public void Confirm(string msg,string true_btn = "",string false_btn = "")
        {
            string yes_btn = "Yes";
            string no_btn = "No";
            if(f.Trim(true_btn) != "")
             yes_btn = true_btn;
            if (f.Trim(false_btn) != "")
                no_btn = false_btn;
            
            msg = f.Trim(msg);
            if(msg != "")
            {
                //Popup
                Popup pop = new Popup();
                //BORDER
                b_container = new Border();
                b_container.BorderThickness = new Thickness(1);
                b_container.BorderBrush = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
                b_container.Background = new SolidColorBrush(Color.FromArgb(255, 253, 253, 253));
                //STACK PANEL
                StackPanel pa_panel = new StackPanel();
                StackPanel er_panel = new StackPanel();
                er_panel.MinHeight = panel_min_height;
                StackPanel btn_panel = new StackPanel();
                btn_panel.Background = new SolidColorBrush(Color.FromArgb(255, 247, 247, 247));
                btn_panel.Margin = new Thickness(0, 10, 0, 0);
                btn_panel.Orientation = Orientation.Horizontal;
                //btn
                 TrueBtn = new Button();
                TrueBtn.Content = yes_btn;
                TrueBtn.Foreground = new SolidColorBrush(Colors.White);
                TrueBtn.Background = new SolidColorBrush(Color.FromArgb(255,70,70,70));
                TrueBtn.Padding = new Thickness(10, 5, 7, 5);
                TrueBtn.Margin = new Thickness(1, 1, 25, 5);
                TrueBtn.Click += (s, e) =>
                {
                    confirmed = true;
                    
                    pop.IsOpen = false;

                };

                 FalseBtn = new Button();
                 FalseBtn.Content = no_btn;
                 FalseBtn.Foreground = new SolidColorBrush(Colors.White);
                 FalseBtn.Background = new SolidColorBrush(Color.FromArgb(255, 130, 130, 130));
                 FalseBtn.Padding = new Thickness(10, 5, 7, 5);
                 FalseBtn.Margin = new Thickness(10, 1, 1, 5);
                 FalseBtn.Click += (s, e) =>
                {
                    confirmed = false;
                    pop.IsOpen = false;
                    
                };
                //TEXTBLOCK
                TextBlock t = new TextBlock();
                t.Text = msg;
               
                t.FontSize = 17;
                t.TextAlignment = TextAlignment.Center;
                t.Padding = new Thickness(5, 10, 5, 10);

                t.Foreground = new SolidColorBrush(Color.FromArgb(255, 45, 45, 45));
                er_panel.Children.Add(t);
                btn_panel.Children.Add(TrueBtn);
                btn_panel.Children.Add(FalseBtn);
                //adding
                pa_panel.Children.Add(er_panel);
                pa_panel.Children.Add(btn_panel);
                
                b_container.Child = pa_panel;
                pop.Child = b_container;
                pop.HorizontalAlignment = HorizontalAlignment.Center;
                pop.VerticalAlignment = VerticalAlignment.Center;
                pop.MinHeight = 200;
                pop.MinWidth = 400;
                p_container = pop;
                
            }
            
        }
    }
}
