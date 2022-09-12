using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public partial class Coordinate
    {
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int MaxWidth{ get; set; }
        public int MaxHeight { get; set; }

        public bool IsTaken { get; set; }
}
}
