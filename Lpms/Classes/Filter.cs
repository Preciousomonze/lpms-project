using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpms.Classes
{
    /// <summary>
    /// class for filtering inputs
    /// has various methods that filter inputs
    /// </summary>
    class Filter
    {
        /// <summary>
        /// the variable that determines if a condition was returned true or not
        /// </summary>
        //private int token;
       // public int Token
        //{
          //  get { return token;}
           // protected set { token = value; }
        //}
        private string[] selected_result = new string[1];
        /// <summary>
        /// Gets the selected results column values from the selected method.
        /// </summary>
        public string[] SelectedResult {
            get { return selected_result; }
            private set { selected_result = value; }
        }

        /// <summary>
        /// method for trimming inputs.
        /// What is it used for, works exactly like the string.Trim() function
        /// but, i just like customising things. :)
        /// for removing unecessary spaces at the beginning and end of a text
        /// </summary>
        /// <param name="value">the value it's going to trim and return</param>
        /// <returns>returns the trimmed value</returns>
        public string Trim(string value)
        {
            value = (value != null ? value : "");
            return value.Trim();
        }
        /// <summary>
        /// The advanced trim() function for triming a set of characters in a string
        /// </summary>
        /// <param name="text">the string that you want to trim </param>
        /// <param name="value">the set of characters to be trimed in the text string</param>
        /// <returns>returns the trimmed value of the text</returns>
        public string BaddoTrim(string text, char[] values)
        {
            Catcher ca = new Catcher(text.Trim(values));

            return text.Trim(values);
        }
        /// <summary>
        /// checks for email validity
        /// </summary>
        /// <param name="value">the email address to be checked</param>
        /// <param name="msg">the message to display to the frontend when it returns false,optional,if empty, nothing will be displayed</param>
        /// <returns>returns true if valid, and false if not.</returns>
        public bool CheckEmail(string value,string msg="")
        {
            //regular expression for email
            string regexPattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
            //regular expression for phone number
           // string reg = @"^(\+{1}|00)\s{0,1}([0-9]{3}|[0-9]{2})\s{0,1}\-{0,1}\s{0,1}([0-9]{2}|[1-9]{1})\s{0,1}\-{0,1}\s{0,1}([0-9]{8}|[0-9]{7})";

            if (System.Text.RegularExpressions.Regex.IsMatch(value, regexPattern))
            {
          //      this.Token = 1;
                return true;
            }
            else
            {//check if the msg is empty
                if (this.Trim(msg) != "")
                {
                    PopUp p = new PopUp(msg);
                }
                else
                {
                    //do nothing.
                }
                return false;
              //  this.Token = 0;
             
            }
        }
        /// <summary>
        /// checks for phone validity
        /// </summary>
        /// <param name="value">the phone number to be checked</param>
        /// <param name="msg">the message to display to the frontend when it returns false,optional,if empty, nothing will be displayed</param>
        /// <returns>returns true if valid, and false if not.</returns>
        public bool CheckPhone(string value, string msg = "")
        {
            //regular expression for phone number
            string regexPattern = @"^(\+{1}|00)\s{0,1}([0-9]{3}|[0-9]{2})\s{0,1}\-{0,1}\s{0,1}([0-9]{2}|[1-9]{1})\s{0,1}\-{0,1}\s{0,1}([0-9]{8}|[0-9]{7})";

            if (System.Text.RegularExpressions.Regex.IsMatch(value, regexPattern))
            {
                //      this.Token = 1;
                return true;
            }
            else
            {//check if the msg is empty
                if (this.Trim(msg) != "")
                {
                    PopUp p = new PopUp(msg);
                }
                else
                {
                    //do nothing.
                }
                return false;
                //  this.Token = 0;

            }
        }
        /// <summary>
        /// Method for performing the work of displaying either the first or last name
        /// </summary>
        /// <param name="type">this has only 2 values, 0 and 1, 0 means first name, 1 means last name. to determine which one is fetched.</param>
        /// <param name="value">the value where the first or surname is gotten from.</param>
        /// <returns>returns the result</returns>
        private string WorkName(string value,int type)
        {

            string result = string.Empty;
            value = (value != null ? value : "");
            if (value.Trim() == "")
            {
                result = value;
            }
            else
            {//it has something, lets check if there's space,
                if (this.Search(value, " ") == false)
                {//no space
                    result = value;
                }
                else
                {//there's space
                    Split s = new Split();
                    s.Divide(value, ' ');
                    //first make sure, the value_array length is 2, if not, it's not necessary
                    if (s.Value_array.Length < 2)
                    {//ITS INVALID
                       // PopUp m = new PopUp("Error! the value has to have at least 2 words separated with space, so that the first and last name can be determined,but " + value + ". doesn't seem to have that. fix it.");
                        //set it to the default value
                        result = value;
                    }
                    else
                    {//get the values
                        if (type == 0)
                        {
                            //get the value of first name
                            result = s.Value_array[0];
                        }
                        else if(type == 1)
                        {
                            //get the value of surname
                            result = s.Value_array[1];
                        
                        }
                    }
                }
            }
            return result;
        
        }

        /// <summary>
        /// Method for splitting the user's name and displaying only the first name
        /// </summary>
        /// <param name="value">the value that needs to be split, in this case, the value of the column name</param>
        /// <returns>returns the result</returns>
        public string FirstName(string value)
        {
            return this.WorkName(value, 0);
        }
        /// <summary>
        /// Method for splitting the user's name and displaying only the last name
        /// </summary>
        /// <param name="value">the value that needs to be split, in this case, the value of the column name</param>
        /// <returns>returns the result</returns>
        public string SurnName(string value)
        {
            return this.WorkName(value, 1);
        }
        /// <summary>
        /// for determining a gender represented with numbers. 
        /// </summary>
        /// <param name="value">the value , must be 0 or 1, any other returns nothing, 0 is female, 1 is male</param>
        /// <returns></returns>
        public string Gender(int value)
        {
            string gen = "";
            switch (value)
            {

                case 1:
                    gen = "Male";
                    break;
                case 0:
                    gen = "Female";
                    break;
                default:
                    gen = "Other";
                    break;
            }
            return gen;
        }
        /// <summary>
        /// for determining a gender converted from representation with numbers to string. 
        /// </summary>
        /// <param name="value">the value , must be false or true, any other returns nothing, false is female, true is male</param>
        /// <returns></returns>
        public string Gender(string value)
        {
            string gen = "";
            value = (value != null ? value : "");
            string val = this.Trim(value).ToLower();
           if ( val == "true" || val == "1")
           {
                gen = "Male";
           }
           else if( val == "false" || val == "0")
           {
                    gen = "Female";
           }
        else
           {
                gen = "Other";
           }
            return gen;
        }
        /// <summary>
        /// checks the format of the date.
        /// the format must be dd/MM/yyyy
        /// </summary>
        /// <param name="value">the date input value.</param>
        public bool CheckDate(string value)
        {
            return true;
        }

        /// <summary>
        /// For finding a set of characters or words in a string, uses the Contains() method to achieve this.
        /// </summary>
        /// <param name="words">The parent string</param>
        /// <param name="find">the set of characters you want to find in the parent string</param>
        /// <returns>returns true or false</returns>
        public bool Search(string words,string find)
        {
            //reduce them to lower case
            if (this.Trim(words).ToLower().Contains(this.Trim(find).ToLower()))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Converts the date to the format you want
        /// </summary>
        /// <param name="date">the string date you want to change, the default format it recognises is MM/dd/yyyy</param>
        /// <param name="format">the format you want, this recognises the different date formats</param>
        /// <returns>returns the formatted result. :)</returns>
        public string Date(string date,string format)
        {
            string return_date = "";
            //SINCE THE DATE STRING IN CSHARP COMES AS 03/30/2017 12:00:00 AM
            //we have to seperate the year,month and day 
            if ((this.Trim(format) == "" || format == null) || (this.Trim(date) == "" || date == null))
            {
               // PopUp p = new PopUp("");
                return_date = date;
            }
            else
            {
                    Split fs = new Split();
                fs.Divide(date, ' ');
                //second split
                //FIRST HAS A VALUE OF SPLIT /
                //SECOND HAS A VALUE OF :
                //THIRD IS AM  OR PM.
                int year = 0, month=0, day=0, hour = 0, minute = 0, second = 0;
                 
                //check the length of the returned array.

                for (int i = 0; i < fs.Value_array.Length; i++)
                {
                    if (fs.Value_array.Length > 0)
                    {
                        Split ffs = new Split();
                        ffs.Divide(fs.Value_array[0], '/');

                        //some laptops formats could be dd/mm/yyyy or mm/dd/yyyy.
                        //so lets see if the first value is greater than 12 so that we know its days, not month
                        int d_or_m = Convert.ToInt32(ffs.Value_array[0]);

                        if(d_or_m > 12)//the first value is days , not month, so forcefully change
                        {
                            month = Convert.ToInt32(ffs.Value_array[1]);
                            day = Convert.ToInt32(ffs.Value_array[0]);
                        
                        }
                        else//the first value is months, not days
                        {
                            month = Convert.ToInt32(ffs.Value_array[0]);
                            day = Convert.ToInt32(ffs.Value_array[1]);
                        
                        }
                        /////////////////////////////////////////////////
                        year = Convert.ToInt32(ffs.Value_array[2]);
                        ////////////////////////////////////////////////
                        //SECOND OF FIRST SPLIT
                        if (i == 1)
                        {
                            if (fs.Value_array[i] != "")
                            {
                                Split sfs = new Split();
                                sfs.Divide(fs.Value_array[i], ':');

                                hour = Convert.ToInt32(sfs.Value_array[0]);
                                minute = Convert.ToInt32(sfs.Value_array[1]);
                                second = Convert.ToInt32(sfs.Value_array[2]);
                            }
                        }
                        //////////////////////////////////////////////////
                    }
                }
                DateTime dt = new DateTime(year, month, day, hour, minute, second);
                
                return_date = dt.ToString(format);
            }
            return return_date;
        }
        /// <summary>
        /// Gets the value of the selected index and returns the respective primary key value.
        /// mostly used for combobox item stuff.
        /// </summary>
        /// <param name="index">the selected index value from the combo box item. e.g combobox1.SelectedIndex</param>
        /// <param name="table">the table you want to get the record from,if empty or null, it will use the previous</param>
        /// <param name="condition">in case there was a condition in the other query that listed the records in the combobox, e.g"ORDER BY ASC", you have to 
        /// specify it here too, so that it works well.
        /// </param>
        /// <param name="start_at">determines where to start the searching in combobox.</param>
        /// <returns>returns the exact record value</returns>
       
        public string[] Selected(int index,string table,string condition = "",int start_at = 0){
            string[] result = new string[1];
            /*
             * if it starts at zero, run normally
             * else increement by the value it starts with.
             * or do some  maths.
             */

            if(table != null){
            if(!(this.Trim(table) == "")){
            //CALL THE QUERY
               
                int selected_value = index - start_at;//removing the irrelevant values,we're starting from where the main items start from the combobox the record
                   //index must be greater than the start_at value
                if (index >= start_at)
                {//correct


                    Query qu = new Query();
                    
                    qu.Get(table,condition);
                    int columns = qu.CountFields();
                    result = new string[columns];
                    this.SelectedResult = new string[columns];
                    int rows = qu.CountRows();
                    qu.Records();
                    //now loop and get the value, then break out like a prisoner, i think. or like a financial stuff.
                    for (int i = 0; i < rows; i++)
                    {
                        if (i == selected_value)
                        {
                            //it's the one, let's do what we do best and gerrarahea.
                            for (int j = 0; j < columns; j++)
                            {
                                //put in the values of the column field into result
                                result[j] = qu.Results[i, j];
                            }
                            
                            //break out :), we don't need you anymore, yae, just like F boys.
                            break;
                        }
                        else
                        {
                            //continue
                        }
                    }
                }
                else
                {//wrong
                    PopUp p = new PopUp("The index has to be greater than or equal to the start_at value,");
                    //break;

                }

            }
            else
            {
                PopUp p = new PopUp("no argument should be left empty in Selected() Method.");
            }
            }
                else{
                    //it's null.
            }
            result.CopyTo(this.SelectedResult, 0);
                return this.SelectedResult;
        }
        /// <summary>
        /// Works exactly the inverse of Selected().
        /// it's used to try to get which elemented was selected when you're trying to edit a record.
        /// </summary>
        /// <param name="value">the value of the record. this is mostly the primary key value of the record</param>
        /// <param name="index">the selected index value from the combo box item. e.g combobox1.SelectedIndex</param>
        /// <param name="table">the table you want to get the record from,if empty or null, it will use the previous</param>
        /// <param name="condition">in case there was a condition in the other query that listed the records in the combobox, e.g"ORDER BY ASC", you have to 
        /// specify it here too, so that it works well.
        /// </param>
        /// <param name="start_at">determines where to start the searching in combobox.</param>
        /// <returns>returns the selected index of the record</returns>
        public int SelectedInverse(string value,string table,string condition = "",int start_at = 0)
        {
            int selected_index = -1;//initially set it to the default combo box
                    
            table = (table != null ? table : "");
            condition = (condition != null ? condition : "");
            value = (value != null ? value : "");
                if (!(this.Trim(table) == "" || this.Trim(value) == ""))
                {
                        Query qu = new Query();

                        qu.Get(table, condition);
                        qu.Records();
                        for (int i = 0; i < qu.CountRows(); i++)
                        {
                            //check when the ids match
                            if (qu.Results[i, 0] == value)
                            {
                                //its a match, let's do our thing and break out.
                                //get the row number, and at start_at value, so that we get the accurate selected index.
                                selected_index = i + start_at;
                                break;
                            }
                        }
                    }
                else
                {
                  //  PopUp p = new PopUp("no compulsery argument should be left empty in Selected() Method.");
                }
                return selected_index;
        }
        /// <summary>
        /// FOR DETERMINING WHEN TO DISPLAY a text in case there's no record returned.
        /// couldn't think of a better name jare, i know the name makes no sense.
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <param name="note">Optional. what to display if the value is 0., if left empty, default is 'None'</param>
        /// <returns>returns the result. duhh</returns>
        public string Numbers(int value,string note = "")
        {
            string result = "";
            if (value < 1)
            {
                //check if the note is empty
                note = (note != null ? note : "");
                if (this.Trim(note) == "")
                {
                    result = "None";

                }
                else
                {
                    result = note;
                }
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }
        /// <summary>
        /// Checks if the value inputed is not empty, otherwise,returns the note.
        /// </summary>
        /// <param name="value">the value to be checked</param>
        /// <param name="note">the note to be returned if the value is empty</param>
        /// <returns>returns a string, i'm tired jare. :(</returns>
        public string CheckEmpty(string value, string note)
        {
            string result = "";
            //value = (value != null ? value : "");
           // note = (note != null ? note : "");

            if (this.Trim(value) == "")
            {//the value is empty, return the note
                result = note;
            }
            else
            {
                result = value;
            }
            return result;
        }

        /// <summary>
        /// Helps to get editors from the respective table record.
        /// I'm sure you're suprised it's in the Filter class, that's because it gave me issues in the Query class, so i just got tired and brought the imbe here.
        /// </summary>
        /// <param name="table">The table to get the record.</param>
        /// <param name="column_name">the column name of the table that holds unique values for each records, mostly the primary key.</param>
        /// <param name="column_value">the value of the column name.</param>
        /// <returns>returns the editors column value.</returns>
        public string GetEditors(string table, string column_name, string column_value)
        {
            string result = "";
            Filter f = new Filter();
            table = f.Trim(table);
            column_name = f.Trim(column_name);
            column_value = f.Trim(column_value);
           Query que = new Query();
            que.Get(table, "WHERE " + column_name + " = '" + column_value + "'");
            if (que.CheckRecords() == true)
            {
                que.Record();
                //get the exact column of the editors 
                int cols = (que.CountFields() - 1);
                //now loop to get the exact column
                int editors_col = 0;
                int pos = 0;
                for (int i = cols; i >= 0; i--, pos++)
                {
                    if (pos == 1)
                    {//its the _editors column
                        editors_col = i;
                    }

                    if (pos > 1)
                    {
                        break;
                    }
                }
                if (f.Trim(que.Result[editors_col]) != "")
                {
                    result = que.Result[editors_col] + "|";
                }

                else
                {
                    result = que.Result[editors_col];
                }
            }
            return result;
        }
    }
}
