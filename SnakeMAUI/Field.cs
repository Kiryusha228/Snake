using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public class Field
    {
        private char[,] _field;
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public Field(int sizex, int sizey) 
        {
            SizeX = sizex; 
            SizeY = sizey;
            _field = new char[SizeX, SizeY];
        }

        private void Clear()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    _field[i, j] = '.';
                }
            }
        }

        public char[,] DrawField(Snake snake)
        {
            Clear();
            _field[snake.Apple.x, snake.Apple.y] = '$';
            _field[snake._snake[0].x, snake._snake[0].y] = '#';
            for (int i = 1; i < snake._snake.Count; i++)
            {
                _field[snake._snake[i].x, snake._snake[i].y] = '*';
            }
            return _field;
        }
    }
}
