using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Graphics gp;
        Pen pnLine = new Pen(Brushes.Black, (float)0.5);
        Pen pnRec = new Pen(Brushes.Blue, 4);
        List<Coordinate> coordinates = new List<Coordinate>();
        public Form1()
        {
            InitializeComponent();
            txtGridCount.Text = "5";
            txtWidth.Text = "1";
            txtHeight.Text = "1";
            gp = panel1.CreateGraphics();
        }

        private void btnCreateGrid_Click(object sender, EventArgs e)
        {
            var lines = Int32.Parse(txtGridCount.Text);

            if (lines >= 5 && lines <= 25)
            {
                Font fnt = new Font("Arial", 10);
                float x = 0f;
                float y = 0f;
                float xspace = panel1.Width / lines;
                float yspace = panel1.Height / lines;

                for (int i = 0; i < (lines + 1); i++)
                {
                    gp.DrawLine(pnLine, x, 0, x, panel1.Height);
                    x += xspace;
                }
                x = 0f;
                for (int i = 0; i < (lines + 1); i++)
                {
                    gp.DrawLine(pnLine, 0, y, panel1.Width, y);
                    y += yspace;
                }
                x = 0f;
                y = 0f;
                int counter = 1;
                int multiplier = 0;
                int maxWidth = lines;
                int maxHeight = 0;
                bool hasMax = false;
                int sumPerCol = 0;
                int baseCount = 0;
                for (int i = 0; i < lines; i++)
                {
                    for (int j = 0; j < lines; j++)
                    {
                        gp.DrawString(counter.ToString(), new Font("Arial", 12), Brushes.Black, x, y);
                        coordinates.Add(new Coordinate() { ID = (counter - 1), X = x, Y = (multiplier * yspace), MaxWidth = maxWidth, MaxHeight = maxHeight});
                        x += xspace;
                        counter++;                                              
                        
                        if (i == (lines - 1))
                        {                            
                            if (!hasMax)
                            {
                                for (int h = j; h < lines - 1; h++)
                                {
                                    sumPerCol += coordinates[h + lines].ID;
                                    hasMax = true;                                
                                }
                            }
                            else
                            {
                                for (int h = baseCount; h < (baseCount + lines); h++)
                                {
                                    if (coordinates.Count <= h)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        coordinates[h + lines].MaxHeight = sumPerCol;
                                    }
                                }
                                baseCount += lines;
                            }
                            sumPerCol++;
                        }
                    }
                    y += yspace;
                    x = 0f;
                    multiplier++;
                    maxWidth = counter + lines;
                }
            } else
            {
                MessageBox.Show("Grid size must be more than 4 but less than 26");
            }
        }

        private void button2_CreateRectangle(object sender, EventArgs e)
        {
            var lines = Int32.Parse(txtGridCount.Text);
            float xspace = panel1.Width / lines;
            float yspace = panel1.Height / lines;
            var x = 0;
            var width = 0;
            var height = 0;

            Int32.TryParse(txtX.Text, out x);
            Int32.TryParse(txtWidth.Text, out width);
            Int32.TryParse(txtHeight.Text, out height);
            var xcor = coordinates.Find(c => c.ID == (x-1));

            //check for out of bounds
            int baseCount = (xcor.ID + 1);
            if((xcor.ID + width) >= xcor.MaxWidth)
            {
                MessageBox.Show("Rectangle will be out of bounds");
                return;
            }
            for (int i = 1; i < height; i++)
            {
                baseCount += lines;
            }
            if (baseCount > xcor.MaxHeight)
            {
                MessageBox.Show("Rectangle will be out of bounds");
                return;
            }

            //check for overlapping
            bool isTaken = false;
            baseCount = xcor.ID;
            for (int i = xcor.ID; i < (xcor.ID + height); i++)
            {
                for (int j = baseCount; j < (baseCount + width); j++)
                {
                    if (coordinates.Count <= j)
                    {
                        MessageBox.Show("Rectangle will be out of bounds");
                        return;
                    }
                    var cor = coordinates.Find(c => c.ID == j);
                    if(cor != null && cor.IsTaken)
                    {
                        isTaken = true;
                        break;
                    }
                }
                baseCount += lines;
            }

            if (!isTaken)
            {
                Rectangle rec = new Rectangle((int)(xcor.X), (int)(xcor.Y), (width) * (int)xspace, (int)yspace * height);
                gp.DrawRectangle(pnRec, rec);
                baseCount = xcor.ID;
                for (int i = xcor.ID; i < (xcor.ID + height); i++)
                {                    
                    for (int j = baseCount; j < (baseCount + width); j++)
                    {
                        var cor = coordinates.Find(c => c.ID == j);
                        cor.IsTaken = true;
                    }
                    baseCount += lines;
                }
            }
            else
            {
                MessageBox.Show("Rectangle will overlap");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            coordinates = new List<Coordinate>();
        }
    }
}
