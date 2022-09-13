using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class RectangleResponse
    {
        private string _message = string.Empty;
        private bool _isSuccessful = true;
        public bool IsSuccessful {
            get { return _isSuccessful; }
            set { _isSuccessful = value; }
        }
        public string Message { 
            get { return IsSuccessful ? "" : _message; }
            set { _message = value;  } 
        }
    }
}
