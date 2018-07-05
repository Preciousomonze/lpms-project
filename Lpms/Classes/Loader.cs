using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpms;
namespace Lpms.Classes
{
    /// <summary>
    /// FOR LOADING SPECIFIC PAGES ACCORDING TO THE TOKEN PROVIDED
    /// </summary>
    class Loader
    {
        private string _page = string.Empty;
        public string Page
        {
            get { return _page; }
            set { _page = value; }
        }
        /// <summary>
        /// holds the value of the page to load
        /// </summary>
        private string _page_to_load = string.Empty;
        /// <summary>
        /// holds the value of the page to load
        /// </summary>
        public string Page_to_load
        {
            get { return _page_to_load; }
            set { _page_to_load = value; }
        }
        //CONSTANTS, holds the names of the pages
        private string tests = "Tests";
        public string _tests
        {
            get { return tests; }
           private set { tests = value; }
        }
       private string patients = "Patients";
       public string _patients
       {
           get { return patients; }
           private set { patients = value; }
       }
       private string reports = "Reports";
       public string _reports
       {
           get { return reports; }
           private set { reports = value; }
       }
       private string doctors = "Doctors";
       public string _doctors
       {
           get { return doctors; }
           private set { doctors = value; }
       }
       private string items = "items";
       public string _items
       {
           get { return items; }
           private set { items = value; }
       }
       private string purchases = "purchases";
       public string _purchases
       {
           get { return purchases; }
           private set { purchases = value; }
       }
       private string profile = "profile";
       public string _profile
       {
           get { return profile; }
           private set { profile = value; }
       }
        //TOKENS
       private const int load_tests = 1;
       private const int load_patients = 2;
       private const int load_reports = 3;
       private const int load_doctors = 4;
       private const int load_items = 5;
       private const int load_purchases = 6;
       private const int load_profile = 7;

      // private int app_page_token = (App.Current as App).Page_token;
        
        /// <summary>
        /// For easily loading a page,this is to set the token of what page is to be loaded in the parent page
        /// <param name="page">the name of the page, must be exact name of the page</param>
        /// </summary>
       public Loader(string page)
        {
           page = (page != null ? page : "");
           Filter f = new Filter();
           this.Page = f.Trim(page).ToLower();
           
           //now check for the page requested
          switch(this.Page)
          {
              case "tests" :
                  (App.Current as App).Page_token = load_tests;
                  break;
              case "patients":
                  (App.Current as App).Page_token = load_patients;
                  break;
              case "reports":
                  (App.Current as App).Page_token = load_reports;
                  break;
              case "doctors":
                  (App.Current as App).Page_token = load_doctors;
                  break;
              case "items":
                  (App.Current as App).Page_token = load_items;
                  break;
              case "purchases":
                  (App.Current as App).Page_token = load_purchases;
                  break;
              case "profile":
                  (App.Current as App).Page_token = load_profile;
                  break;
              default:
                  PopUp p = new PopUp("Sorry, the page \""+page+"\" you're trying to access is unavailable at the moment. :(.\n Maybe it hasn't been added to the page list yet or check your spellings.");
                  //(App.Current as App).Page_token = 0;
                  break;
          }
       }
        /// <summary>
        /// For loading a page.
        /// </summary>
       public Loader()
       {
           this.Load();
       }
        /// <summary>
        /// This is what the parent page calls, so that it knows what to load
        /// </summary>
           public void Load(){
               //SO GET THE APP.PAGE_TOKEN VALUE TO KNOW WHAT TO LOAD
               //STORE IN AN ARRAY TO LOOP THROUGH.
               int[] pages_token = {load_tests,load_patients,load_reports,load_doctors,load_purchases,load_items,load_profile};
               //the arrangements of the 2 arrays must be relative;
               string[] pages = { tests, patients, reports, doctors, purchases,items,profile};
       
               for(int i = 0 ; i < pages_token.Length; i++)
               {//now loop through the values
                   if (pages.Length == pages_token.Length)
                   {//THE 2 ARRAYS MUST BE THE SAME LENGTH, AND THEIR VALUES MUST BE RELATIVE, so as to prevent out of index error and mismatch errors.
                       if ((App.Current as App).Page_token == pages_token[i])
                       {
                           this.Page_to_load = pages[i];
                           break;
                       }
                       else { /*continue looping*/}

                   }
                   else
                   {/* do nothing but alert the user*/
                       PopUp p = new PopUp("Internal error! The 2 arrays have different lengths, fix it. in Loader.Load() :-(");
                   }
               }
               
           }
        }

    }

