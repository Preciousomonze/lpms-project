using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Lpms.Classes
{    /// <summary>
    /// POPUP CLASS
    /// FOR SHOWING POPUPS
    /// </summary>
    class PopUp
    {
        /// <summary>
        /// holds the instantiated MessageDialog
        /// </summary>
        private MessageDialog box;
        /// <summary>
        /// message to display
        /// @var string
        /// </summary>
        private string msg;

        public PopUp(string msg = "")
        {
            this.msg = msg;
            this.box = new MessageDialog(this.msg);
            this.box.ShowAsync();
        
        }
        /// <summary>
        /// for showing ordinary popups
        /// </summary>
        /// <param name="msg">the message to display</param>
        public void Show(string msg)
        {
            this.msg = msg;
            this.box = new MessageDialog(this.msg);
            this.box.ShowAsync();
        }

    }


}
