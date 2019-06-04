using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hundreds
{
    public class Ball
    {
        public int x { get; set; }
        public int y { get; set; }
        public int size { get; set; }
        public int xSpeed { get; set; }
        public int ySpeed { get; set; }

        public int score { get; set; }

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(x, y, size, size);
            }
        }
        public Ball(int x, int y, int size, int xSpeed = 5, int ySpeed = 5, int score = 0)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            this.score = score;
        }

        public void drawBall(Brush brush, Graphics gfx)
        {
            gfx.FillEllipse(brush, new Rectangle(x, y, size, size));
            //need to get center position of ball
            gfx.DrawString($"{score}", new Font("Arial", 16), Brushes.White, new Point(x + (size/2) - 10, y + (size/2) - 10));
            gfx.DrawRectangle(Pens.Red, HitBox);
        }

        public void update(int width, int height)
        {
            x += xSpeed;
            y += ySpeed;

            if(x + size > width)
            {
                xSpeed = -Math.Abs(xSpeed);
            }

            else if(x < 0)
            {
                xSpeed = Math.Abs(xSpeed);
            }

            if(y + size > height)
            {
                ySpeed = -Math.Abs(ySpeed);
            }

            else if(y < 0)
            {
                ySpeed = Math.Abs(ySpeed);
            }
        }
    }
}
