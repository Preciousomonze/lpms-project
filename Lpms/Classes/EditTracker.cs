using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpms.Classes
{
    /// <summary>
    /// To determine what row in a table to edit
    /// </summary>
    class EditTracker
    {
        /// <summary>
        /// Holds the table value
        /// </summary>
        private string table = string.Empty;
        /// <summary>
        /// the column name, mostly the primary key is preferable.
        /// </summary>
        private string column = string.Empty;
        /// <summary>
        /// the value of the column selected
        /// </summary>
        private string column_value = string.Empty;

        /*
         * THE VARIABLE THAT STORES THE END RESULT.
         */
        /// <summary>
        /// holds the end result after the track() has been called.
        /// </summary>
        private string[] track_result = new string[1];
        /// <summary>
        /// holds the end result after the track() has been called.
        /// </summary>
        public string[] TrackResult
        {
            get { return track_result; }
            set { track_result = value; }
        }
        Filter f = new Filter();
        /// <summary>
        /// To determine what row in a table to edit.
        /// </summary>
        /// <param name="table">the table in which the row to be edited is stored</param>
        /// <param name="column">the column name used to track the row. Most times the primary key</param>
        /// <param name="column_value">The value of the column.</param>
        public EditTracker(string table,string column,string column_value)
        {
            try
            {
                this.table = f.Trim(table);
                this.column = f.Trim(column);
                this.column_value = f.Trim(column_value);
            }
            catch (Exception ex)
            {
                PopUp p = new PopUp(ex.ToString());
            }
        }
        /// <summary>
        /// To determine what row in a table to edit. MORE DYNAMIC VERSION
        /// THIS DETERMINES THE PRIMARY KEY NAME ON IT'S OWN ON IT'S OWN.
        /// ASSUMING THAT THE KEY NAME IS THE TABLE NAME UNDERSCORE ID
        /// E.G. table "staffs" should have a primary key of "staff_id" 
        /// e.g. table "patients" should have a primary key of "patient_id"
        /// this is the only way it'll work.
        /// STILL UNDER CONSTRUCTION:)
        /// </summary>
        /// <param name="table">the table in which the row to be edited is stored.</param>
        /// <param name="primary_key_value">the primary key value of the row you want to be tracked.</param>
        private EditTracker(string table, string column_value)
        {

        }
        /// <summary>
        ///  to track the row that was recieved when the class was instantiated.
        /// </summary>
        public void Track()
        {
            Query tq = new Query();
            if(!(this.table == "" || this.column == "" || this.column_value == "")){
                tq.Get(this.table, "WHERE " + this.column + " = '" + this.column_value + "'");
                if (tq.CountRows() > 0)
                {
                    
                    tq.Record();
                    //GET THE NUMBER OF COLUMNS RETURNED.
                    track_result = new string[tq.Result.Length];
                    //track_result = new string[calc];
                    //copy the result to track_result
                    tq.Result.CopyTo(this.track_result,0);
                }
                else
                {
                    PopUp p = new PopUp("The record with id \"" + this.column_value + "\" on table \"" + this.table + "\" Doesn't exist.");
                }
            }
            else
            {
                //SOMETHING'S WRONG
               
            }
        }
    }
}
