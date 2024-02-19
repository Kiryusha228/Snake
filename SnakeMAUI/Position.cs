using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    public class Position : IComparable<Position>
    {
        public int x;
        public int y;

        public Position(int posX, int posY)
        {
            x = posX;
            y = posY;
        }

        public Position(Position position)
        {
            x = position.x;
            y = position.y;
        }

        //int IComparable<Position>.CompareTo(Position? other)
        //{
        //    if (this.x == other!.x && this.y == other.y)
        //    {
        //        return 0;
        //    }
        //    else if (this.x > other.x || (this.x > other.x && this.y > other.y))
        //    {
        //        return 1;
        //    }
        //    else if (this.x < other.x || (this.x < other.x && this.y < other.y))
        //    {
        //        return -1;
        //    }
        //    return -1;
        //}

        public int CompareTo(Position? other)
        {
            //return this.CompareTo(other);

            if (this.x == other!.x && this.y == other.y)
            {
                return 0;
            }
            else if (this.x > other.x || (this.x > other.x && this.y > other.y))
            {
                return 1;
            }
            else if (this.x < other.x || (this.x < other.x && this.y < other.y))
            {
                return -1;
            }
            return -1;
        }

        public void MoveLeft(int border)
        {
            if (y == 0)
            {
                y = border - 1;
                return;
            }
            y--;
        }
        public void MoveRignt(int border)
        {
            if (y == border - 1) 
            {
                y = 0;
                return;
            }
            y++;
        }
        public void MoveUp(int border)
        {
            if (x == 0)
            {
                x = border - 1;
                return;
            }
            x--;
        }
        public void MoveDown(int border)
        {
            if (x == border - 1)
            {
                x = 0;
                return;
            }
            x++;
        }

        
    }
}
