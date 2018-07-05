using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Lpms.Classes
{
    /// <summary>
    /// A CLASS FOR QUERYING THE DATABASE
    /// has the Get() method for selecting.
    /// 
    /// the Add() for adding inserting.
    /// the Change() for updating.
    /// the Remove() for deleting.
    /// </summary>

    class Query : Connection
    {
        //set conditions
        const int insert = 1;
        const int select = 2;
        const int update = 3;
        const int delete = 4;
        /// <summary>
        /// holds the functions of mysql
        /// </summary>
        string[] func_val = { "NOW()" };
        //popups
        PopUp box;
        //RECONSTRUCT THE CLASS
        // protected Query itself = new Query();
        /*
         * @var MySqlCommand Holder
         */
        public MySqlCommand cmd = new MySqlCommand();//FOR GETTING COMMANDS DONE
        /*
        public MySqlCommand Command {
            get { 
                return command; }  
            set {
               MySqlCommand command = value;
                
               }  
             }
         * */
        /// <summary>
        /// READER VARIABLE
        /// </summary>
        public MySqlDataReader reader = null;
        /*
        public MySqlDataReader Reader
        {
            get { return reader; }
            set { MySqlDataReader reader = value; }
        }
         * */
        /// <summary>
        /// THE COMMANDER COMMANDING THE THIRD MARINE COMMANDO
        /// </summary>
        //public object commando;

        /*
         * @var_name table
         * @var string
         * for holding the names of tables
         */
        private string table = string.Empty;

        /*
         * @var_name condition
         * for storing conditional values.
         * @var string
         */
        private string condition = string.Empty;
        /// <summary>
        /// storing the success message that should be displayed
        /// </summary>
        private string success_msg = string.Empty;
        /// <summary>
        ///Gets or sets the message to be displayed to the user if the query was successful,ignore or set to null if you don't want anything to show.
        ///Note: it must be called before the query method you want the message to be display for.
      /// </summary>
        public string successMessage
        {
            get { return success_msg; }
            set { success_msg = value; }
        }
        /// <summary>
        /// Gets the  the results of the current get query
        /// </summary>
        private string[,] results = new string[3,5];
        /// <summary>
        /// gets the result the results of the current get query
        /// </summary>
        public string[,] Results
        {
            get { return results; }
            set { results = value; }
        }
        /// <summary>
        /// Gets the single result of the current get query
        /// </summary>
        private string[] result = new string[1];
        /// <summary>
        /// Gets the single result of the current get query
        /// </summary>
        public string[] Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// Gets the number of fields in a selected query.
        /// </summary>
        private int field_count = 0;
        /// <summary>
        /// Gets the number of fields in a selected query.
        /// </summary>
        public int FieldCount
        {
            get { return 0; }
           private set { field_count = value; }
        }
        /*
        public Query()
        {
            //ONCE IT'S INSTANTIATED, CONNECT TO THE DATABASE
            //Connection connection = new Connection();
           // MySqlDataReader clone_reader = null;
            //this.reader = clone_reader;
        }
        */

        /// <summary>
        ///  the main method that performs the query. Takes in two argument
        /// </summary>
        /// <param name="token">the value that determines if it's an insert, update, or select query.</param>
        /// <param name="query">the sql query statement.</param>
        protected void Quering(int token, string query)
        {
            //OPEN THE CONNECTION
            this.Open();

            try
            {
                //DO THE COMMAND
                this.cmd = new MySqlCommand(query, this.con);

                //CHECK FOR CONDITIONS TO KNOW WHAT TO DO
                if (token == insert || token == update || token == delete)
                {
                    try
                    {
                        this.cmd.ExecuteNonQuery();
                        //close connection
                        this.Close();
                        if(this.successMessage != null)
                        if (this.success_msg.Trim() != "")  
                        box = new PopUp(this.success_msg);
                    }

                    catch (MySqlException ex)//IF ANYTHING HAPPENS, BE THE GOAL KEEPER :0, CASH THE HERROR!
                    {
                        PopUp box = new PopUp(ex.ToString());
                    }

                }
                else if (token == select)
                {
                    try
                    {
                        this.reader = this.cmd.ExecuteReader();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        box = new PopUp("ppptyttt "+ex.ToString());
                    }
                 }
                else
                {
                    //INVAALID
                }
            }
            catch (MySqlException ex)//IF ANYTHING HAPPENS, BE THE GOAL KEEPER :0, CASH THE HERROR!
            {
                PopUp box = new PopUp(ex.ToString());

            }
            finally
            {
                //  this.con.Close();
            }
        }


        /*
         * the method for binding prepared staments to its value
         * @var string
         * binders collects the binding stuff
         * values collects the respective binding values
         */
        /// <summary>
        /// the method for binding prepared staments to its value
        /// </summary>
        /// <param name="binders">binders collects the binding stuff</param>
        /// <param name="values">values collects the respective binding values</param>
        protected void Bind(string binders, string values)
        {
            if (binders == string.Empty || values == string.Empty)
            {//EMPTY DO NOTHING
            }
            else
            {
               // string binder_array = "";
            }
            cmd.Parameters.AddWithValue("", "");
        }


        /// <summary>
        /// FOR RETRIEVING INFORMATION FROM A DATABASE TO DISPLAY TO THE FRONTEND
        ///
        /// </summary>
        /// <param name="table">STORES THE VALUE OF THE TABLE NAME</param>
        /// <param name="condition">THE CONDITIONAL STATEMENT,, E.G WHERE BBLA = 'BLA'
        /// its optional, if empty, it still works,BUT IF NOT EMPTY, THE CONDITIONAL STATEMENT MUST BE VALID
        /// </param>
        public void Get(string table, string condition = "")
        {
            this.table = table;
            
            this.condition = (condition != null ? condition : "");
            this.CheckTable("the current get method has an empty table");
            //CHECK IF CONDITION IS FALSE SO THAT WE'LL KNOW IF WE'RE TO ADD AND FOR THE DELETED CONDITION
            string add_necessary = "";
          
            string extension = string.Empty;

            if (this.condition.Trim() == "")
            {
                //EMPTY CONDITION //STRUCTURE THE CODE WELL
                add_necessary = "WHERE deleted = '0'";
                extension = "" + add_necessary + "";
            }
            else
            {//ITS NOT NULL, BUT CHECK IF IT'S A WHERE CLAUSE, to determine some stuff
                Filter f = new Filter();
                if(f.Search(this.condition.ToLower(),"where") == true)
                {//ther's a where clause
                    //filter
                    //remove the where  clause,this way, no error will be made.
                    this.condition = this.condition.ToLower().Replace("where", " ");

                    add_necessary = "WHERE (deleted ='0'  OR deleted = NULL) AND";
                        extension = " " + add_necessary + " " + this.condition + "";
                }
                else
                {//no where clause
                    add_necessary = "WHERE (deleted ='0' OR deleted = NULL)";
                        extension = ""+add_necessary+ ""+this.condition + "";
                }
            }
                string q = "SELECT * FROM " + this.table + " "+ extension;
                //CALL IN THE QUERYing method
                this.Quering(select, q);
        }
        
        /// <summary>
        ///  GetExtra
        /// INCASE THE USER DOESN'T USE THE REGULAR SELECT STATEMENT, Probably he or she wants to specify some columns.
        /// Still under construction. :).
        /// </summary>
        /// <param name="statement">the sql statement to type</param>
        
        public void GetExtra(string statement)
        {
            //we need to add some stuff, the delete clause.
                Filter f = new Filter();
                if (f.Search(statement.ToLower(), "where") == true)
                {//ther's a where clause
                    //filter
                    //remove the where  clause,this way, no error will be made.
                    statement = statement.ToLower().Replace("where", " WHERE (deleted = '0' OR deleted = NULL) AND ");


                }

                else if (f.Search(statement.ToLower(), "order") == true && f.Search(statement.ToLower(), "where") == false)
                {
                    statement = statement.ToLower().Replace("order", " WHERE (deleted = '0' OR deleted = NULL) ORDER");
                }
                else
                {
                    statement += " WHERE deleted = '0'  OR deleted = NULL ";
                }
                string q = statement;//+ extra_stat;
            //call in the queriing method
            this.Quering(select, q);
       //     cmd.CommandText = statement;
            //prepare
         //   cmd.Prepare();

        }

        /// <summary>
        /// for adding details into the database.
        /// </summary>
        /// <param name="table">The name of the table</param>
        /// <param name="values">the values to be entered using comma as the separator must be exactly the number
        /// of columns entered. this is of type array, so values should be inputed like an array
        /// e.g {"value1","value2","value3"}   
        /// </param>
        /// <param name="columns">the columns needed, should be seperated with comma. e.g : name,id...
        /// or you can leave it empty .
        /// </param>
        public void Add(string table, string[] values, string columns = "")
        {
            //filter values
            Filter filter = new Filter();
            this.table = table;
            #region CHECK IF THE column is empty
            //CHECK IF THE column is empty
            string col = string.Empty;

            if (filter.Trim(columns) == "")
            {
                col = "";
            }
            else
            {//COLUMN HAS SOME VALUES,make sure the number of columns are equal to the number of values
                #region handling the columns stuff
                Split split = new Split();//divide
                split.Divide(columns, ',');
                //set the start first, we'll accomplish this at the end, under the looping ish.
                //col = (bla,bla,bla,bla,bla);
                col = "(";
                //now store
                for (int i = 0; i < split.Value_array.Length; i++)//keep adding to the col variable with comma
                {//actually this is unecessary work, but it's just to make sure
                    if (filter.Trim(split.Value_array[i]) != "")
                    {//NOW THIS IS WHAT MAKES THE CODE USEFUL

                        /*
                         * SOMEONE MIGHT PUT COMMA AT THE END OR IN BETWEEN BY MISTAKE,
                         * AND THAT COULD DESTROY EVERYTHING, THEN THAT
                         * STUPID ERROR STARTS SHOWING AGAIN.
                         * THIS PREVENTS THAT :)
                         */
                        //ADD THE VALUE

                        //CHECK IF THE VALUE ISN'T THE LAST ARRAY SO THAT WE DON'T PUT COMMA IN THE LAST VALUE
                        int calc = split.Value_array.Length - i;
                        if (calc == 1)
                        {//its the last value, don't put the comma
                            col += split.Value_array[i];
                        }
                        else
                        {
                            col += split.Value_array[i] + " , ";
                        }
                    }
                    else
                    {//do nothing

                    }
                }

                col += ") ";
            }


                #endregion


            #endregion

                #region FILTER THE VALUES ISH PROPERLY TO INSERT INTO THE QUERY
                //FILTER THE VALUES ISH PROPERLY TO INSERT INTO THE QUERY
                /*
                 * THERE COULD BE SOME CONDITIONS.
                 * the string[] values variable holds values in string, so even if you input a sql functional value like "NOW()"
                 * it will be passed as a string, which would not turn out good, if we don't handle it properly.
                 * so we're going to set up an array that holds functional values to determine when we would remove string nature
                 * what i mean is we'll create an array that holds values of functions in sql command, so when we are looking through
                 * the string[] value values, we can check if any functional sql value is part of the values, by comparing the values 
                 * with the values inside the other array that stores functional values
                 */
                //THE ARRAY THAT STORES THE FUNCTIONAL VALUES, any sql functional values used in queries should be added here. :)
               // string[] func_val = { "NOW()" };
                //THE VARIABLE THAT WILL STORE UP string[] value values
                string val = "";
                //for storing
                string _func_or_val = "";
                string comma = ",";
                int j = 0;
                //ORDINARY VALUES
              //  string ord_val = "";
                for (int i = 0; i < values.Length; i++)
                {//start looping through the values to store properly
                    //NOW WHILE CHECKING THROUGH THE VALUES, ALSO CHECK FOR FUNCTIONAL VALUES TO REMOVE THE QUOTES
                    for ( j = 0; j < func_val.Length; j++)
                    {//STORE THE VALUE
                        //check if it's the last value
                        int calc = values.Length - i;
                        if (calc == 1)//its the last value, remove the comma
                        {
                            comma = "";
                        }
                        else
                        {
                            comma = ",";
                        }

                        if (filter.Trim(values[i]).ToUpper() == filter.Trim(func_val[j].ToUpper())/*making sure they're all uppercase to perfect comparising*/)
                        {//it's a functional value, remove the quotes
                            _func_or_val += filter.Trim(values[i]) + comma;
                        }
                        else
                        {//ITS NOT A FUNCTIONAL value, leave the quotes
                            _func_or_val += "'"+ filter.Trim(values[i]) +"'" + comma;
                        }
                    }
                    //reset j to keep checking if the values match the functional values.
                    j = 0;
                }
                #endregion

            
            //FOR THE VALUES,
            //ADD THE WHOLE RESULTS TOGETHER.
                val = _func_or_val; 
            string q = "INSERT INTO " + this.table + col + " VALUES("+ val +")";
            //CALL IN THE QUERYing method
            this.Quering(insert, q);

        }
        /// <summary>
        /// method for updating a record in a table
        /// </summary>
        /// <param name="table">the name of the table</param>
        /// <param name="columns">the column(s) to be edited.</param>
        /// <param name="values">the value of the column(s) listed</param>
        /// <param name="condition">the conditon, e.g set bla = 'bla' where bla_id = 'bla'</param>
        public void Change(string table, string columns,string[] values,string condition)
        {
            //filter values
            Filter filter = new Filter();
            this.table = table;
            this.condition = condition;
           
            //CHECK IF THE column is empty
            string col = string.Empty;

            if (filter.Trim(columns) == "" || filter.Trim(table) ==  "" || filter.Trim(condition) == "")
            {
                //IT CAN'T BE EMPTY
                if(columns == ""){
                box = new PopUp("Columns can't be empty in the current Change() method.");
                }
                else if(table == ""){
                    box = new PopUp("table can't be empty in the current Change() method.");
                }
                else if (condition == "")
                {
                    box = new PopUp("condition can't be empty  in the current Change() method.");
                }
                else
                {
                    box = new PopUp("A value is empty, can't work. I don't even know what is wrong sef, oh, find the problem yourself, i cannot come and be killing myself.");
                }
            }
            else
            {//COLUMN HAS SOME VALUES,make sure the number of columns are equal to the number of values
                #region handling the columns stuff
                Split split = new Split();//divide
                split.Divide(columns, ',');
                //set the start first, we'll accomplish this at the end, under the looping ish.
                //col = bla = 'bla',bla = 'bla';
                //make sure before counting the array, they must not be empty.
               string comma = "";
                //CHECK IF THE COLUMNS LENGTH AND VALUE LENGTH ARE THE SAME.
                if (split.Length == values.Length)
                {
                    //now store
                    for (int i = 0; i < split.Value_array.Length; i++)//keep adding to the col variable with comma
                    {//actually this is unecessary work, but it's just to make sure
                        int j = 0;
                        if (filter.Trim(split.Value_array[i]) != "")
                        {//NOW THIS IS WHAT MAKES THE CODE USEFUL

                            /*
                             * SOMEONE MIGHT PUT COMMA AT THE END OR IN BETWEEN BY MISTAKE,
                             * AND THAT COULD DESTROY EVERYTHING, THEN THAT
                             * STUPID ERROR STARTS SHOWING AGAIN.
                             * THIS PREVENTS THAT :)
                             */
                            //ADD THE VALUE

                            //FILTER THE VALUES ISH PROPERLY TO INSERT INTO THE QUERY
                            /*
                             * THERE COULD BE SOME CONDITIONS.
                             * the string[] values variable holds values in string, so even if you input a sql functional value like "NOW()"
                             * it will be passed as a string, which would not turn out good, if we don't handle it properly.
                             * so we're going to set up an array that holds functional values to determine when we would remove string nature
                             * what i mean is we'll create an array that holds values of functions in sql command, so when we are looking through
                             * the string[] value values, we can check if any functional sql value is part of the values, by comparing the values 
                             * with the values inside the other array that stores functional values
                             */
                            //THE ARRAY THAT STORES THE FUNCTIONAL VALUES, any sql functional values used in queries should be added here. :)
                           // string[] func_val = { "NOW()" };
                            //THE VARIABLE THAT WILL STORE UP string[] value values

                            //NOW WHILE CHECKING THROUGH THE VALUES, ALSO CHECK FOR FUNCTIONAL VALUES TO REMOVE THE QUOTES
                    for ( j = 0; j < func_val.Length; j++)
                    {//STORE THE VALUE
                            //CHECK IF THE VALUE ISN'T THE LAST ARRAY SO THAT WE DON'T PUT COMMA IN THE LAST VALUE
                        int calc = values.Length - i;
                        if (calc == 1)//its the last value, remove the comma
                        {
                            comma = "";
                        }
                        else
                        {
                            comma = ",";
                        }

                        if (filter.Trim(values[i]).ToUpper() == filter.Trim(func_val[j].ToUpper())/*making sure they're all uppercase to perfect comparising*/)
                        {//it's a functional value, remove the quotes
                            
                                col += filter.Trim(split.Value_array[i]) +" = "+filter.Trim(values[i])+""+comma;
                            
                        }
                        else
                        {//ITS NOT A FUNCTIONAL value, leave the quotes
                           
                            col += filter.Trim(split.Value_array[i]) + " = '" + filter.Trim(values[i]) + "'" + comma;
                        }
                    }
                    //reset j to keep checking if the values match the functional values.
                   
                         }
                        else
                        {//do nothing

                        }
                    }

                #endregion

                    //update
                    string q = "UPDATE " + this.table + " SET " + col + " " + this.condition;
                    //
                    this.Quering(update, q);
                }
                else
                {//THE COLUMN AND VALUE ARE NOT THE SAME, THAT WILL BE AN ISSUE
                    box = new PopUp("Internal Error: the number of columns and the number of values aren't equivalent in the recent Change() Method.");

                }
            }

            
        }
        /// <summary>
        /// method for deleting a record in a table
        /// </summary>
        /// <param name="table">the name of the table</param>
        /// <param name="condition">the condition,e.g, where bla =' bla'</param>
        public void Remove(string table, string condition)
        {
            this.table = table;
            this.condition = (condition != null ? condition : "") ;
            //this.CheckTable("");
           // string q = "DELETE FROM " + this.table + " " + this.condition;
            if (this.condition.Trim() != "")
            {
                string q = "UPDATE " + this.table + " SET deleted = '1' " + this.condition + "";
                this.Quering(delete, q);
            }
            else
            {
                box = new PopUp("Couldn't delete the record.");
            }
        }
        /// <summary>
        /// check if the table is null or not
        /// <param name="msg">The message popup to display when the method returns false</param>
        ///
        /// <returns>returns true or false.</returns>
        /// </summary>
        private bool CheckTable(string msg)
        {
           
            if (this.table.Trim() != null && this.table.Trim() != "")
            {
                return true;
            }
            else
            {
                box = new PopUp(msg);
                return false;

            }
        }

        /// <summary>
        /// For reading through the reader.
        /// the same as using reader.Read().
        /// Advantage of using this is that it handles any exception affecting the reader.Read(). :-)
        /// so if you want to ease of the stress, use this, otherwise, gerrout and be using your old school method. :-|
        /// </summary>
        /// <returns>returns MySqlDataReader.Read()</returns>
          private bool Reading()
        {
            bool result = false;
            try 
            {
                result =this.reader.Read();
            }
            catch(Exception reading_ex) 
            {
                box = new PopUp("pppppppppppppp "+reading_ex.ToString());
               
            }
            return result;
        }

          /// <summary>
          /// Gets the number of fields in the most recent select query.
          /// </summary>
          /// <returns>returns the amount of columns in a selected query.</returns>
          public int CountFields()
          {
              //first open reader so that reader doesn't become null.this gave me a tough time. :-(
              this.OpenReader();
              this.field_count = this.reader.FieldCount;
              return this.field_count;
          }

        /// <summary>
        /// The method for counting the number of rows read
        /// </summary>
        /// <returns>returns the number of rows counted</returns>
        public int CountRows()
        {
            //OPEN THE READER INCASE ITS CLOSED, CAUSE THE PROBLEM I ENCOUNTERED, WAS NOT A FUNNY SOMETHING.
            this.OpenReader();
           int count = 0;
           
               while (this.Reading())
               {
                   count += 1;
               }
           
            this.CLoseReader();
            return count;
        }
       
        /// <summary>
        /// for getting the single result of the query
        /// stores the values in string[column] 
        /// with variable name this.Result
        /// e.g. this.Result[column].
        /// </summary>
        public void Record()
        {
            if (this.CountRows() > 0)
            {//if there are any rows returned, do something.
                this.OpenReader();
                if (!(this.reader == null))
                {//there's a valid query, the .get() method has been called, so carry on
                    int rows = 0;
                    int field_count = 0;
                    //get the number of rows, although it's just a single row.
                    this.result = new string[this.reader.FieldCount];
                   
                    while (this.Reading())
                    {

                        // Get the number of columns returned since it's only one record
                        while (field_count < this.reader.FieldCount)
                        {//count the number of the fields
                            if (this.reader[field_count] != null)//TO AVOID OBJECT REFERENCE SET TO NULL ERROR, WE MAKE SURE 
                            {//THE reader doesn't have a null value, so that we can convert it to string
                                this.result[field_count] = this.reader[field_count].ToString();
                            }
                            else
                            {//SINCE IT'S NULL, JUST PUT IN AN EMPTY QUOTE. THE SMARTEST WAY I'VE FOUND SO FAR, TO SOLVE THIS PROBLEM.
                                this.result[field_count] = "";
                            }
                            field_count++;
                        }
                        rows++;
                    }


                }
                else
                {//it means the get method hasnt executed any query
                    PopUp p = new PopUp("No query was done, a query has to be done with the aid of the Get() method before the Records() method can be functional. :(");
                }
                this.CLoseReader();
            }
            else
            {
                //empty result.
            }
        }



        /// <summary>
        /// for getting the records of the query
        /// stores the values in string[row,column] 
        /// with variable name this.Results
        /// e.g. this.Results[row,column].
        /// </summary>
        public void Records()
        {
            if (this.CountRows() > 0)
            {//if there are any rows returned, do something.
                this.OpenReader();
                if (!(this.reader == null))
                {//theres a valid query, the .get() method has been called, so carry on
                    int rows = 0;
                    int field_count = 0;
                    //get the number of rows
                    while (this.Reading())
                    {
                        //WE'RE GOING TO MAKE SOMETHING LIKE A MULTI DIMENSIONAL ARRAY
                        // Get the number of columns returned
                        while (field_count < this.reader.FieldCount)
                        {//count the number of the fields
                            field_count++;
                        }
                        rows++;
                    }

                    //BASED ON HOW MANY ROWS WERE READ, set the values of the dimensional array
                    this.Results = new string[rows, field_count];//so the number of rows returned is the maximum value the [rows,] part can take
                    //and the number of fields is the maximum value the [,field_count] part can take
                    int i = 0;
                    int j = 0;
                    //OPEN THE READER again, so that the reader.Read() will be refreshed and have something to read even if
                    //it has already been read above.
                    this.OpenReader();
                    /*
                     * THIS TIME, WE'RE READING THE VALUE TO STORE IT IN THE MULTI DIMENSIONAL ARRAY
                     * THIS WAY, WE CAN GET VALUES OF MORE THAN ONE ROW. ;-)
                    */
                    while (this.reader.Read())
                    {
                        //FOR THE COLUMNS
                        while (j < this.reader.FieldCount)
                        {
                            if (this.reader[j] != null)//TO AVOID OBJECT REFERENCE SET TO NULL ERROR, WE MAKE SURE 
                            {//THE reader doesn't have a null value, so that we can convert it to string
                                this.Results[i, j] = this.reader[j].ToString();
                            }
                            else
                            {//SINCE IT'S NULL, JUST PUT IN AN EMPTY QUOTE. THE SMARTEST WAY I'VE FOUND SO FAR, TO SOLVE THIS PROBLEM.
                                this.Results[i, j] = "";
                            }
                            j++;
                        }
                        //reset j, to get column values again for the next row level, to make the loop perfect.
                        j = 0;
                        i++;
                    }

                }
                else
                {//it means the get method hasnt executed any query
                    PopUp p = new PopUp("No query was done, a query has to be done with the aid of the Get() method before the Records() method can be functional. :(");
                }
                this.CLoseReader();
            }
            else
            {
                //empty result.
            }
        }
        /// <summary>
        /// counts the number of rows affected
        /// </summary>
        /// <returns>returns the amount ofrecords </returns>
        public int CountRecords()
        {
            return CountRows();
        }
        /// <summary>
        /// Helps to open the MysqlDataReader, IT'S USEFUL BECAUSE ONCE THE READER HAS USED THE .READ(), IT GETS EMPTY
        /// SO TO REFILL IT, THIS METHOD HELPS TO DO THAT :).
        /// LIFE'S GOOD :-).
        /// </summary>
        private void OpenReader()
        {
            try
            {
                if (this.reader != null)
                {
                    this.reader.Close();
                    //reopen the connection so that the reader will be good as new.
                    this.Close();
                    this.Open();
                }
                   this.reader = this.cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                PopUp p = new PopUp("pp pp "+ex.ToString());
            }
        }
        /// <summary>
        /// Helps to close the MysqlDataReader.
        /// Always call this method if you're using a method that is using the reader, which always has to use the OpenReader()
        /// at the beginning of the method.
        /// </summary>
        private void CLoseReader()
        {
            try
            {
                if(this.reader != null)
                this.reader.Close();
            }
            catch (Exception ex)
            {
                PopUp p = new PopUp(ex.ToString());
            }
        }
        /// <summary>
        /// checks if there is any row or record available
        /// </summary>
        /// <returns>returns true or false</returns>
        public bool CheckRecords()
        {
            int counting = this.CountRows();
            if (counting > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if the most recent query with a condition returns any record
        /// </summary>
        /// <param name="value">What to be displayed if a record is found.</param>
        /// <param name="msg">Message to be shown if no record is found.</param>
        /// <returns>returns either the record, or a message to be displayed if the record doesn't exist.</returns>
        public string RecordExist(string value,string msg = "")
        {
            string result = "";
            //open reader first
            this.OpenReader();
            //now check if there's any record
            if (this.CountRows() < 1)
            {
                //no record, display the message
                msg = (msg != null ? msg : "");
                if (msg.Trim() == "")
                {
                    //show the default message.
                    //UHM, LET ME THINK OF WHAT TO PUT :)
                    result = "No record available";
                }
                else
                {
                    result = msg;
                }
            }
            else
            {//there's a record, show it.
                //now i'm stuck, how do i know what the user wants to display? :(
                //oh, let me go up and add a param that passes what to be displayed, eases the stress. :-)
                result = value;

            }
            return result;
        }

        
    }
}
