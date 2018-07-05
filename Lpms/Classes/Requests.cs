using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpms.Classes
{
    /// <summary>
    /// Handles most stuff involving requests.
    /// still under construction, will be used in later version.
    /// :)
    /// </summary>
    class Requests
    {
        private int request_id { get; set; }
        private int request_type { get; set; }
        //constants for determining which type is the request type
        private const int tests = 0;
        private const int items = 1;
        private const int patients = 2;
        private const int purchases = 3;
        private const int doctors = 4;
        private const int reports = 5;

        Filter f = new Filter();
        public Requests()
        {

        }
        /// <summary>
        /// Checks which type of request , the request is meant for. E.G. checks if the request is for tests,purchases,etc.
        /// </summary>
        /// <param name="token">This value determines which type of request to return</param>
        /// <returns>returns the result.</returns>
        private string[] ProcessingRequests(int token,string condition)
        {
            Query q = new Query();

            string[] result = new string[0];
            switch (token)
            {
                case tests:
                    q.Get("Tests", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;
                case items:
                    q.Get("Items", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;
                case patients: 
                    q.Get("Patients", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;
                case purchases:
                    q.Get("Purchases", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;
                case doctors:
                    q.Get("Doctors", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;

                case reports:
                    q.Get("Reports", condition);
                    result = new string[q.CountFields()];
                    q.Record();
                    result = q.Result;
                    break;
            }
            return result;
        }
        /// <summary>
        /// Checks if the request is allowable,returns true if it's allowable, else, returns false
        /// </summary>
        /// <param name="request_id">The id of the request in the request table.</param>
        /// <param name="request_type">The type of request,if it's for test,purchase,etc.</param>
        /// <param name="current_user">the current user logged in</param>
        /// <returns>returns true if it's allowable, else, returns false</returns>
        public bool CheckRequest(int request_id,int request_type,int current_user)
        {
            this.request_id = request_id;
            this.request_type = request_type;

            Query r_que = new Query();
            //query the request table and make sure it's a request that is accepted
            r_que.Get("SELECT * FROM requests WHERE request_id = '" + this.request_id + "' AND accepted = '1'");
            //check if  the query returns true
            if(r_que.CountRows() < 1){

                return false;
            }
            else
            {//THERE'S A RECORD
                r_que.Record();

                //GET THE PROCESSING REQUEST METHOD.
                Query t_que = new Query();
                //
                
                string[] r_stuff = this.ProcessingRequests(request_type,"WHERE _editors LIKE '"+request_id+"'");
                //column numbers are different, so to get the exact column for _added_by and _editors,
                int cols = (r_stuff.Length - 1);
                //now loop to get the exact column
                int editors_col = 0;
                int adder_col = 0;
                int pos = 0;
                for (int i = cols; i >= 0; i--,pos++)
                {
                    if (pos == 1)
                    {//its the _editors column
                        editors_col = i;
                    }
                    else if (pos == 2)
                    {//its the _added_by column
                        adder_col = i;
                    }
                    if (pos > 2)
                    {
                        break;
                    }
                }

                //check if the the request has exceeded it's allowance.
                int r_amount = Convert.ToInt32(r_que.Result[4]);
                int e_amount = 0;
                //get the value from the editors column

                //search for the id, and check if the id appears more or less time than the amount value in the request table
               Split s = new Split();
                s.Divide(r_que.Result[4],'|');
                
                for(int i = 0; i < s.Value_array.Length; i++)
                {
                    if(f.Search(s.Value_array[i],r_que.Result[0]) == true)
                    {//The id was found,
                        e_amount += 1;
                    }

                }


                if(e_amount < r_amount )
                {//its still possible, the amount is less than what is allowed
                    return true;
                }
                else
                {
                    return false;
                }
            }
             
        }

        /// <summary>
        /// This helps to send the request to the request table
        /// </summary>
        /// <param name="request_type">the type of request,eg,purchases,tests,etc.</param>
        /// <param name="owner">the adder of the record</param>
        public void SendRequest(int request_type,int owner)
        {

        }
        
    }
}
