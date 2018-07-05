using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lpms.Classes
{
    /// <summary>
    /// THE CLASS FOR CATCHING ERRORS
    /// SIMPLY CONTAINS THE TRY AND CATCH STUFF
    /// </summary>
    class Catcher 
    {
        /// <summary>
        /// variable for changing exception type, although there's already a default value
        /// </summary>
       // private Exception except = MySqlException;
        public Catcher(object val = null,string va="" )
        {
            try{
                Object err = val;
            }
            catch(Exception ex){
                PopUp p = new PopUp(ex.ToString());
            }
        }
    }
}
