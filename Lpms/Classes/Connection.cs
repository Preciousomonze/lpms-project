using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Lpms.Classes
{
    /// <summary>
    /// HESTABLISHES A CONNECSION
    /// </summary>
    class Connection
    {
        protected string con_string = string.Empty;
        /// <summary>
        /// the object of MysqlConnection class.
        /// </summary>
        public MySqlConnection con = new MySqlConnection();
        /// <summary>
        /// Set the database name, gets it's value from the global variable that stores the value of the database
        /// this idea was gotten with the help of Ope, cool though, so that way, anytime the ChooseDB() method is called, 
        /// the value entered is what the global variable that this variable holds is changed to :).
        /// </summary>
        private string database = ((App.Current as App).Db_name.Trim() == "") ? "patho" : (App.Current as App).Db_name;
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        
        /// <summary>
        /// Establishes a connection.
        /// </summary>
        /// <param name="connect">Optional.</param>
        public Connection(string connect = "server=localhost;port=3306;userid=root;database=patho;password=;sslmode=None")
        {
            //value[0] = "server=localhost";
            //value[1] = "database=patho";
            ///value_2[0] = "server";
            ///value_2[1] = "localhost";
            ///
            ///value-2[1][0] = "database";
            ///value-2[1][1] = "patho";
            Filter f = new Filter();
            string extras = "";
            //LETS CHECK IF THE USER INPUTED A DATABASE IN THE CONNECT STRING ALREADY, SO WE DON'T NEED TO ADD IT
            if (f.Search(connect, "database=") == true)
            {//the user altered the initial value, so no need to add the database again
                //check for the database value to be able to set the global db variable to what was entered.and also
                //so that the ChooseDB() method can have effect on this too. :)
               
                Split p = new Split();
                Split joiner = new Split();
                string sep = "";
                p.Divide(connect, ';');

                Split c = new Split();
                //now loop through the array, since we've plit the connect string 
                for (int i = 0; i < p.Value_array.Length; i++)
                {
                    //now split again, this time, with equal to"=", so that we get the value
                    if (p.Value_array[i].Trim() == "")
                    {
                        //do nothing
                    }
                    else
                    {
                        c.Divide(p.Value_array[i], '=');

                        //loop again to locate the database value
                        for (int j = 0; j < c.Value_array.Length && c.Value_array.Length > 1; j++)
                        {// the array list must be at least 2

                            if (c.Value_array[j] == "database")
                            {//we've found it, get the value
                                //since the "=" is used , it'll probably be 'database=value',so database is c.Value_array[0],
                                //then the value is c.Value_array[1];
                                //so store the value in the global value
                                (App.Current as App).Db_name = c.Value_array[1];
                                //set the global db var to the database value so that when it's changed, it can work.
                                c.Value_array[1] = this.database;
                                //lets pack up everything, we opened, operated, now we're closing up.
                                // break;
                            }
                            else { /* do nothing */ }
                            //check if its the last value
                            int calc = c.Value_array.Length - j;
                            
                            if (calc == 1)
                            {//it's the last value
                                sep = "";
                            }
                            else
                            {
                                sep = "=";
                            }
                            //extras += c.Value_array[j] + sep;
                            extras = joiner.Join(c.Value_array, '=');
                        }
                        p.Value_array[i] = extras;

                    }

                }
                //join back
                
                con_string = joiner.Join(p.Value_array, ';');
                
            }
            else
            {
                con_string = connect + ";database=" + this.Database;
            }
            //SET THE CONNECTION
            // MySqlConnection con = new MySqlConnection(this.con);
           this.con.ConnectionString = @con_string;
        }
        /// <summary>
        /// Establishes a connection.
        /// </summary>
        /// <param name="server">name of the server</param>
        /// <param name="port">port number</param>
        /// <param name="user_id">the server username</param>
        /// <param name="password">the password of the username</param>
        /// <param name="sslMode">Optional, default is set to None</param>
        public Connection(string server, int port, string user_id, string password, string db = "", string sslMode = "None")
        {
            string db_base;
            if (db.Trim() == "")
            {//use the default database
                db_base = this.Database;
            }
            else{//The user put in his or her own database,
                //change the value of the global variable to what was entered first.
                (App.Current as App).Db_name = db;
                //put it into this variable below.
                db_base = (App.Current as App).Db_name;
            
            }
            
            string connect = @"server=" + server + ";port=" + port.ToString() + ";userid=" + user_id + ";password=" + password + ";database="+db_base+";sslmode=" + sslMode;
            this.con.ConnectionString = connect;
        }
       
        /// <summary>
        /// OPENS THE DB CONNECTION
        /// </summary>
        public void Open(){
           //open the connection
            try
            {
                if (this.con.State.ToString().ToLower() == "closed")//ONLY OPEN THE CONNECTION WHEN IT'S CLOSED, TO AVOID OPENING WHILE IT'S STILL OPENED.
                this.con.Open();
                
            }
            catch (Exception ex)
            {
                PopUp p = new PopUp("Couldn't connect to the server. \n"+ex.ToString());
            }
        }
        /// <summary>
        /// CLOSES THE DB CONNECTION
        /// </summary>
        public void Close()
        {
            //close the connection
            if (this.con != null)
            {
                if (this.con.State.ToString().ToLower() == "open")
                {
                    this.con.Close();
                }
            }
        }


        /// <summary>
        /// method for selecting a database to use.
        /// </summary>
        /// <param name="database">the name of the database</param>
        /// <returns>returns the name of the database set.</returns>
        public string ChooseDb(string database)
        {
            //set the global variable of the database name
            if (database.Trim() != "")
            {
                (App.Current as App).Db_name = database;
                this.database = (App.Current as App).Db_name;
            }
            else
            {
                PopUp p = new PopUp("Please choose a database");
                this.database = (App.Current as App).Db_name;

            }
            return this.database;
        }
    }


    }

