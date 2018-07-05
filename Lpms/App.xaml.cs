using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Lpms
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        //DECLARE PUBLIC VARIABLE TO CARRY SOME TOKENS AROUND
        /// <summary>
        /// VARIABLE THAT STORES A USER UNIQUE ID TO KNOW WHO IS LOGGED IN.
        /// </summary>
        private int user_token = 0;
        public int User_token {
            get { return user_token; }
            set { user_token = value;}
            }
        private string edit_time_format = "yyyy-MM-dd hh:mm:ss";
            public string Edit_time_format 
            {
                get {return edit_time_format;}
               private set{ edit_time_format = value;}
            }
        /// <summary>
        /// set the database name, will be changed dynamically with the help of the ChooseDB() method or if the new connection is made
        /// </summary>
        private string db_name = "";
        /// <summary>
        /// set the database name, will be changed dynamically with the help of the ChooseDB() method or if the new connection is made
        /// </summary>
        public string Db_name
        {
            get { return db_name; }
            set { db_name = value; }
        }
        /// <summary>
        /// To identify pages to load in the frame
        /// </summary>
        private int page_token = 0;
        /// <summary>
        /// To identify pages to load in the frame
        /// </summary>
        public int Page_token 
        {
            get { return page_token; }
            set { page_token = value; }
        }

        
        /// <summary>
        /// determines the opacity of the parent page.
        /// </summary>
        public double ParentOpacity = 1;
        public string chosen_test_for_report = "";
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                /*
                 *Check if the user is logged in
                 */
                if (this.user_token == 0)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                else
                {
                    rootFrame.Navigate(typeof(Home), e.Arguments);
                }
                
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
