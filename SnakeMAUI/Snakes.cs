//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SnakeConsole
//{
//    internal class Snakes
//    {
//        public List<Position> _snake1;
//        public List<Position> _snake2;

//        private List<Position> _emptyCells;

//        public Position Apple;

//        public int Size1
//        {
//            get
//            {
//                return _snake1.Count;
//            }
//            private set { }
//        }

//        public int Size2
//        {
//            get
//            {
//                return _snake1.Count;
//            }
//            private set { }
//        }

//        public SnakeDirection? _direction1;
//        private SnakeDirection? _lastDirection1;
//        public SnakeDirection? _direction2;
//        private SnakeDirection? _lastDirection2;

//        public Snakes(int x, int y, int x2, int y2)
//        {
//            _emptyCells = new List<Position>();
//            _snake1 = new List<Position>
//            {
//                new Position(x, y),
//                new Position(x, y - 1),
//                new Position(x, y - 2)
//            };
//            _snake2 = new List<Position>
//            {
//                new Position(x, y),
//                new Position(x, y + 1),
//                new Position(x, y + 2)
//            };
//            Apple = new Position(x, y);
//            SpawnApple(x * 2, y * 2);


//            _direction1 = SnakeDirection.Right;
//            _lastDirection1 = _direction1;
//            _direction2 = SnakeDirection.Left;
//            _lastDirection2 = _direction2;
//        }

//        public bool Move1(int borderX, int borderY)
//        {
//            var lastPosition = new Position(_snake[0]);
//            switch (_direction)
//            {
//                case SnakeDirection.Left:
//                    if (_lastDirection1 != SnakeDirection.Right)
//                    {
//                        _snake[0].MoveLeft(borderY);
//                    }
//                    else
//                    {
//                        _snake[0].MoveRignt(borderY);
//                        _direction = _lastDirection1;
//                    }
//                    break;
//                case SnakeDirection.Right:
//                    if (_lastDirection1 != SnakeDirection.Left)
//                    {
//                        _snake[0].MoveRignt(borderY);
//                    }
//                    else
//                    {
//                        _snake[0].MoveLeft(borderY);
//                        _direction = _lastDirection1;
//                    }
//                    break;
//                case SnakeDirection.Up:
//                    if (_lastDirection1 != SnakeDirection.Down)
//                    {
//                        _snake[0].MoveUp(borderX);
//                    }
//                    else
//                    {
//                        _snake[0].MoveDown(borderX);
//                        _direction = _lastDirection1;
//                    }
//                    break;
//                case SnakeDirection.Down:
//                    if (_lastDirection1 != SnakeDirection.Up)
//                    {
//                        _snake[0].MoveDown(borderX);
//                    }
//                    else
//                    {
//                        _snake[0].MoveUp(borderX);
//                        _direction = _lastDirection1;
//                    }
//                    break;
//                default:
//                    break;
//            }
//            if (_snake[0].CompareTo(Apple) == 0)
//            {
//                SpawnApple(borderX, borderY);
//                Grow();
//            }
//            _lastDirection1 = _direction1;

//            for (int i = 1; i < Size; i++)
//            {

//                (_snake[i], lastPosition) = (lastPosition, _snake[i]);
//            }

//            for (int i = 1; i < Size; i++)
//            {
//                if (_snake[0].CompareTo(_snake[i]) == 0)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }
//    }
//}
