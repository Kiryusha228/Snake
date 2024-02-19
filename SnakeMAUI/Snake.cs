using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public class Snake
    {

        public List<Position> _snake;

        private List<Position> _emptyCells;

        public Position Apple;

        public int Size
        {
            get 
            { 
                return _snake.Count; 
            }
            private set { }
        }

        public SnakeDirection? _direction;
        public SnakeDirection? _lastDirection;

        public Snake(int x, int y) 
        {
            _emptyCells = new List<Position>();
            _snake = new List<Position>
            {
                new Position(x, y),
                new Position(x, y - 1),
                new Position(x, y - 2)
            };
            Apple = new Position(x, y);
            SpawnApple(x * 2, y * 2);
            

            _direction = SnakeDirection.Right;
            _lastDirection = _direction;
            
        }

        public bool Move(int borderX,int borderY)
        {
            var lastPosition = new Position(_snake[0]);
            switch (_direction)
            {
                case SnakeDirection.Left:
                    if (_lastDirection != SnakeDirection.Right)
                    {
                        _snake[0].MoveLeft(borderY);
                    }
                    else
                    {
                        _snake[0].MoveRignt(borderY);
                        _direction = _lastDirection;
                    }
                    break;
                case SnakeDirection.Right:
                    if (_lastDirection != SnakeDirection.Left)
                    {
                        _snake[0].MoveRignt(borderY);
                    }
                    else
                    {
                        _snake[0].MoveLeft(borderY);
                        _direction = _lastDirection;
                    }
                    break;
                case SnakeDirection.Up:
                    if (_lastDirection != SnakeDirection.Down)
                    {
                        _snake[0].MoveUp(borderX);
                    }
                    else
                    {
                        _snake[0].MoveDown(borderX);
                        _direction = _lastDirection;
                    }
                    break;
                case SnakeDirection.Down:
                    if (_lastDirection != SnakeDirection.Up)
                    {
                        _snake[0].MoveDown(borderX);
                    }
                    else
                    {
                        _snake[0].MoveUp(borderX);
                        _direction = _lastDirection;
                    }
                    break;
                default:
                    break;
            }
            if (_snake[0].CompareTo(Apple) == 0)
            {
                SpawnApple(borderX, borderY);
                Grow();
            }
            _lastDirection = _direction;

            for (int i = 1; i < Size; i++)
            {

                (_snake[i], lastPosition) = (lastPosition, _snake[i]);
            }

            for (int i = 1; i < Size; i++)
            {
                if (_snake[0].CompareTo(_snake[i]) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void Grow()
        {
            _snake.Add(new Position(_snake[Size - 1].x, _snake[Size - 1].y));
        }

        private void SpawnApple(int borderX, int borderY)
        {
            FillEmptyCellsList(borderX, borderY);
            var rnd = new Random();
            Apple = _emptyCells[rnd.Next(0, _emptyCells.Count - 1)];
        }

        private void FillEmptyCellsList(int x, int y)
        {
            _emptyCells.Clear();
            bool flag;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    flag = true;
                    for (int k = 0; k < Size; k++)
                    {
                        if (_snake[k].x == i && _snake[k].y == j)
                        {
                            flag = false; 
                        }
                    }
                    if (flag == true)
                    {
                        _emptyCells.Add(new Position(i,j));
                    }
                }
            }
        }

        public enum SnakeDirection
        {
            Left,
            Right,
            Up,
            Down,
        }
    }
}
