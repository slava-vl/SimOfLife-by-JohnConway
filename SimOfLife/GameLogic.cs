using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimOfLife
{
    class GameLogic
    {
        public uint CurrenGeneretion { get; private set; }
        private bool[,] world;
        private readonly int rows;
        private readonly int cols;

        public GameLogic(int rows, int cols, int density) 
        {
            this.rows = rows;
            this.cols = cols;
            world = new bool[cols, rows];

            Random rn = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    world[x, y] = rn.Next(density) == 0;
                }
            }
        }
        
        public void NextGeneration() 
        {
            var newWorld = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = world[x, y];

                    if (!hasLife && neighboursCount == 3)
                    {
                        newWorld[x, y] = true;

                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newWorld[x, y] = false;
                    }
                    else 
                    {
                        newWorld[x, y] = world[x, y];
                    }
                }
            }
            world = newWorld;
            CurrenGeneretion++;
        }

        public bool[,] GetCurrentGeneration() 
        {
            var result = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = world[x, y];
                }
            }
            return result;
        }

        private int CountNeighbours(int x, int y) 
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols)%cols;
                    int row = (y + j + rows)% rows;
                    bool isSelfChecking = col == x && row == y;
                    var hasLife = world[col, row];
                    if (!isSelfChecking && hasLife) 
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool ValidateCellPodition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
        private void UpdateCell(int x, int y, bool state) 
        {
            if (ValidateCellPodition(x, y))
                world[x, y] = state;
        }

        public void AddCell(int x, int y) 
        {
            UpdateCell(x, y, state: true);
        }
        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }

    }
}
