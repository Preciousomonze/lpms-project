using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpms.Classes
{
    /// <summary>
    /// THE CLASS THAT HANDLES SPLITNG OF VALUES INTO ARRAYS AND JOINING BACK
    /// </summary>
    class Split
    {
        private string text;
        private char delimeter;
        /// <summary>
        /// Gets the length of the array that has values.
        /// </summary>
        public int Length { get; private set; }
        private string[] value_array;
        /// <summary>
        /// the split values returned as an array.
        /// </summary>
        public string[] Value_array { get { return value_array; } set { value_array = value; } }
        private string value_joined;
        /// <summary>
        /// the joined value returns the concatenated value
        /// </summary>
        public string Value_joined { get { return value_joined; } set { value_joined = value; } }
        /// <summary>
        /// holds the value of value_joined in array format so that it can be u
        /// </summary>
       // public string Value_joined_array = string.Empty;
        public Split()
        {

        }

        /*
         * FOR DIVIDING
         */
        /// <summary>
        /// For splitting a text value
        /// </summary>
        /// <param name="text">the string value</param>
        /// <param name="delimeter">the delimeter value, must be char value.e.g ',','|','.'</param>
        /// <returns>returns the array value</returns>
        public string[] Divide(string text, char delimeter)
        {
            this.text = text;//HOLD THE TEXT VALUE
            this.delimeter = delimeter;
            // string[] tokens = dividing.Split(new[] { "is Marco and" }, StringSplitOptions.None);

            //DIVIDE THE string
            try
            {
                string[] Value_array = this.text.Split(this.delimeter);
                this.Value_array = Value_array;

                //make sure the arrays returned all have values
                int i = 0;
                if(this.Value_array[i] != null){
                    for (i = 0; i < this.Value_array.Length; i++)
                    {
                        if (this.Value_array[i].Trim() != "")
                        {
                            Length += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PopUp p = new PopUp(ex.ToString());
            }
            
            return this.Value_array;
        }

        /*
         * FOR JOINING BACK
         */
        /// <summary>
        /// For joining an array
        /// </summary>
        /// <param name="val">the string array value to return</param>
        /// <param name="delimeter">the delimeter value, must be char value.e.g ',','|','.'</param>
        /// <returns>returns the joined value</returns>
        public string Join(string[] val, char delimeter)
        {
            this.value_array = val;

            string Value_joined = string.Join(delimeter.ToString(), this.value_array);
            this.Value_joined = Value_joined;
            return this.Value_joined;
        }
    }

}
