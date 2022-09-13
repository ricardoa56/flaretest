using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class RectangleManager
    {
        private Graphics _gp;
        private int _lines;
        private List<Coordinate> _coordinates = new List<Coordinate>();
        private Pen _pnRec = new Pen(Brushes.Blue, 4);
        private float _width;
        private float _height;
        private float _xCellSpace;
        private float _yCellSpace;
        public RectangleManager(Graphics gp, int lines, float width, float height)
        {
            _gp = gp;
            _lines = lines;
            _height = height;
            _width = width;

            _xCellSpace = _width / lines;
            _yCellSpace = _height / lines;
        }

        public int LineCount { get { return _lines;  } }
        public List<Coordinate> Coordinates { 
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public void AddCoordinate(Coordinate coordinate)
        {
            _coordinates.Add(coordinate);
        }
        private RectangleResponse CheckForOutOfBounds(int location, int rectangleWidth)
        {
            //check for out of bounds
            var xcor = _coordinates.Find(c => c.ID == (location - 1));
            if ((xcor.ID + rectangleWidth) >= xcor.MaxWidth)
            {
                return new RectangleResponse()
                {
                    IsSuccessful = false,
                    Message = "Rectangle will be out of bounds"
                };
            }

            if (_coordinates.Count() < location)
            {
                return new RectangleResponse()
                {
                    IsSuccessful = false,
                    Message = "Rectangle will be out of bounds"
                };
            }

            return new RectangleResponse();
        }

        private RectangleResponse CheckForOverlapping(int location, int rectangleWidth, int rectangleHeight, out int baseCount)
        {
            //check for overlapping
            var xcor = _coordinates.Find(c => c.ID == (location - 1));
            baseCount = xcor.ID;
            for (int i = xcor.ID; i < (xcor.ID + rectangleHeight); i++)
            {
                for (int j = baseCount; j < (baseCount + rectangleWidth); j++)
                {
                    if (_coordinates.Count <= j)
                    {
                        return new RectangleResponse()
                        {
                            IsSuccessful = false,
                            Message = "Rectangle will be out of bounds"
                        };
                    }
                    var cor = _coordinates.Find(c => c.ID == j);
                    if (cor != null && cor.IsTaken)
                    {
                        return new RectangleResponse()
                        {
                            IsSuccessful = false,
                            Message = "Rectangle will overlap"
                        };                        
                    }
                }
                baseCount += _lines;
            }

            return new RectangleResponse();
        }

        public RectangleResponse CreateRectangle(int location, int rectangleWidth, int rectangleHeight)
        {
            var outOfBounds = this.CheckForOutOfBounds(location, rectangleWidth);
            if(!outOfBounds.IsSuccessful)
            {
                return outOfBounds;
            }

            var xcor = _coordinates.Find(c => c.ID == (location - 1));

            //check for overlapping
            int baseCount = 0;
            var overLapping = this.CheckForOverlapping(location, rectangleWidth, rectangleHeight, out baseCount);

            if (overLapping.IsSuccessful)
            {
                if (_gp != null)//for unit test purposes only
                {
                    Rectangle rec = new Rectangle((int)(xcor.X), (int)(xcor.Y), (rectangleWidth) * (int)_xCellSpace, (int)_yCellSpace * rectangleHeight);
                    _gp.DrawRectangle(_pnRec, rec);
                }
                baseCount = xcor.ID;
                for (int i = xcor.ID; i < (xcor.ID + rectangleHeight); i++)
                {
                    for (int j = baseCount; j < (baseCount + rectangleWidth); j++)
                    {
                        var cor = _coordinates.Find(c => c.ID == j);
                        cor.IsTaken = true;
                    }
                    baseCount += _lines;
                }
            }
            else
            {
                return overLapping;
            }

            return new RectangleResponse();
        }

    }
}
