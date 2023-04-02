using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation_Lab_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graphics graphics;
        bool[,] field;
        int cols, rows;
        int size = 5; //чем меньше число, тем меньше размер клеток и => больше их кол-во
        int density = 5;

        private int Neighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var Check = col == x && row == y;
                    var life = field[col, row];

                    if (life && !Check)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            btStart.Text = "In process...";

            if (timer1.Enabled)
            {
                return;
            }

            rows = pictureBox.Height / size;
            cols = pictureBox.Width / size;

            field = new bool[cols, rows];

            Random random = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(pictureBox.Image);
            timer1.Start();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                return;
            }

            btStart.Text = "Refresh";
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            graphics.Clear(Color.Gray);

            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neigboursCount = Neighbours(x, y);
                    bool life = field[x, y];

                    if (!life && neigboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else
                    {
                        if (life && (neigboursCount < 2 || neigboursCount > 3))
                        {
                            newField[x, y] = false;
                        }
                        else
                        {
                            newField[x, y] = field[x, y];
                        }
                    }

                    if (life)
                    {
                        graphics.FillRectangle(Brushes.Blue, x * size, y * size, size - 1, size - 1);
                    }
                }
            }

            field = newField;
            pictureBox.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
