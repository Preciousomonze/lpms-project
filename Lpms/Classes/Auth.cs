using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Lpms.Classes
{
    /// <summary>
    /// Auntheticates the user, Determines if the user is logged in
    /// </summary>
    class Auth
    {
        Filter f = new Filter();
        public Auth()
        {

        }
        /// <summary>
        /// Logs the current user out
        /// </summary>
        public void Logout()
        {
            (App.Current as App).User_token = 0;
        }
        /// <summary>
        /// Checks if the current user is the owner of the information
        /// </summary>
        /// <param name="table">The table name of where to find the information</param>
        /// <param name="column_name">The column that has unique values used to identify each rows, most times the primary key.</param>
        /// <param name="column_value">The vakue of the column specified.</param>
        /// <returns>returns true if it's the owner, otherwise false.</returns>
        public bool CheckOwner(string table,string column_name,string column_value)
        {
            table = (table != null ? f.Trim(table) : "");
            column_name = (column_name != null ? f.Trim(column_name) : "");
            column_value = (column_value != null ? f.Trim(column_value) : "");
            int current_user = (App.Current as App).User_token;
            //
            Query q = new Query();
            q.Get(table, "WHERE " + column_name + "= '"+column_value+"' AND _added_by = '"+current_user+"'");

            if (q.CheckRecords() == true)
            {//theres a record, it's the owner.
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
