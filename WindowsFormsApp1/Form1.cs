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
        RectangleManager recManager;
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
                recManager = new RectangleManager(gp, lines, panel1.Width, panel1.Height);

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
                for (int i = 0; i < lines; i++)
                {
                    for (int j = 0; j < lines; j++)
                    {
                        gp.DrawString(counter.ToString(), new Font("Arial", 12), Brushes.Black, x, y);
                        recManager.AddCoordinate(new Coordinate() { ID = (counter - 1), X = x, Y = (multiplier * yspace), MaxWidth = maxWidth});
                        x += xspace;
                        counter++;
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
            var x = 0;
            var width = 0;
            var height = 0;

            Int32.TryParse(txtX.Text, out x);
            Int32.TryParse(txtWidth.Text, out width);
            Int32.TryParse(txtHeight.Text, out height);

            var response = recManager.CreateRectangle(x, width, height);

            if(!response.IsSuccessful)
            {
                MessageBox.Show(response.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            recManager.Coordinates = new List<Coordinate>();
        }
    }
}
