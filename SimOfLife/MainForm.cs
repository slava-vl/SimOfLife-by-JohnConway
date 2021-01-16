using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimOfLife
{
    public partial class MainForm : Form
    {

        private Graphics graphics;
        private int resolution;
        private GameLogic gameLogic;

        public MainForm()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer.Enabled) { return; }
            resolution = (int)numericUpDownResolution.Value;
            gameLogic = new GameLogic
            (
                rows: pictureBox.Height / resolution,
                cols: pictureBox.Width / resolution,
                density:(int)numericUpDownDensity.Minimum+ (int)numericUpDownDensity.Maximum- (int)numericUpDownDensity.Value
            );

            Text = $"Generation {gameLogic.CurrenGeneretion}";

            numericUpDownResolution.Enabled = false;
            numericUpDownDensity.Enabled = false;
            resolution = (int)numericUpDownResolution.Value;

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(pictureBox.Image);
            timer.Start();
        }

        private void StopGame()
        {
            if (!timer.Enabled) return;

            timer.Stop();
            numericUpDownResolution.Enabled = true;
            numericUpDownDensity.Enabled = true;

        }
        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Black);

            var world = gameLogic.GetCurrentGeneration();

            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    if(world[x,y])
                            graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }

            pictureBox.Refresh();
            Text = $"Generation {gameLogic.CurrenGeneretion}";
            gameLogic.NextGeneration();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
           StartGame();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer.Enabled) return;

            if (e.Button == MouseButtons.Left) 
            {
                var X = e.Location.X / resolution;
                var Y = e.Location.Y / resolution;
                gameLogic.AddCell(X,Y);
            }
            if (e.Button == MouseButtons.Right)
            {
                var X = e.Location.X / resolution;
                var Y = e.Location.Y / resolution;
                gameLogic.RemoveCell(X, Y);
            }
        }
        

    }
}
