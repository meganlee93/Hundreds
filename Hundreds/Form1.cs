using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hundreds
{
    public partial class Form1 : Form
    {
        Bitmap map;
        Graphics gfx;
        int totalScore;

        Random random;
        int numOfBall;
        Ball[] balls;
        bool gameOver;
        bool win;
        DialogResult playAgain;
        string display;

        Point mousePos;
        Point centerPoint;
        int radius;

        Point ballCenter;
        int ballRadius;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(map);
            random = new Random();
            //numOfBall = random.Next(4, 7);
            balls = new Ball[6];
            totalScore = 0;
            gameOver = false;
            win = false;
            generateBall();
        }


        public void generateBall()
        {
            
            for (int i = 0; i < balls.Length; i++)
            {
                int x = random.Next(0, ClientSize.Width - 40);
                int y = random.Next(0, ClientSize.Height - 40);
                int size = random.Next(20,41);
                int xSpeed = random.Next(0,2);
                int ySpeed = random.Next(0,2);
                if(xSpeed == 0)
                {
                    xSpeed = -3;
                }

                else
                {
                    xSpeed = 3;
                }

                if(ySpeed == 0)
                {
                    ySpeed = -3;
                }

                else
                {
                    ySpeed = 3;
                }
                balls[i] = new Ball(x,y,size, xSpeed, ySpeed);
            }
        }

        public bool checkCollision()
        {
            double checkX = Math.Pow(mousePos.X - centerPoint.X, 2);
            double checkY = Math.Pow(mousePos.Y - centerPoint.Y, 2);
            if(checkX + checkY <=  Math.Pow(radius,2))
            {
                return true;
            }

            else
            {
                return false;
            }
            //r^2 = (x2-x-1)^2 + (y2-y1)^2

        }

        public bool checkBallCollision(Ball ball1, Ball ball2)
        {
       
            int radius1 = ((ball1.x + ball1.size) - ball1.x)/2;
            int radius2 = ((ball2.x + ball2.size) - ball2.x) / 2;
            Point center1 = new Point(ball1.x + (ball1.size / 2), ball1.y + (ball1.size / 2));
            Point center2 = new Point(ball2.x + (ball2.size / 2), ball2.y + (ball2.size / 2));

            double distance = Math.Sqrt(Math.Pow(center1.X - center2.X, 2) + Math.Pow(center1.Y - center2.Y, 2));
            //squareroot (x2-x1)^2 + (y2-y1)^2
            if(radius1 + radius2 > distance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            gfx.Clear(Color.Transparent);
            if (gameOver == true)
            {
                timer1.Enabled = false;
                if (win == true)
                {
                    
                    display = "You win";                 
                }

                else
                {
                    display = "You lose";
                }

                playAgain = MessageBox.Show(display, "Play again?", MessageBoxButtons.YesNo);
                if (playAgain == DialogResult.Yes)
                {
                    generateBall();
                    totalScore = 0;
                    numOfBall = random.Next(4, 11);
                    timer1.Enabled = true;
                    win = false;
                    gameOver = false;
                }

                else
                {
                    this.Close();
                }
            }


            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].update(ClientSize.Width, ClientSize.Height);

                //checking if mouse position is inside ball hit box
                if (balls[i].HitBox.Contains(mousePos))
                {
                    centerPoint = new Point(balls[i].x + (balls[i].size / 2), balls[i].y + (balls[i].size / 2));
                    radius = ((balls[i].x + balls[i].size) - balls[i].x) / 2;
                    if (checkCollision())
                    {
                        balls[i].size++;
                        balls[i].score++;
                        for (int j = 0; j < balls.Length; j++)
                        {
                            if (i != j)
                            {
                                //generate position
                                //check if balls circular ball interset
                                //ballCenter = new Point(balls[i].x + (balls[i].size / 2), balls[i].y + (balls[i].size / 2));
                                //ballRadius = ((balls[i].x + balls[i].size) - balls[i].x) / 2;
                                if (balls[i].HitBox.IntersectsWith(balls[j].HitBox) && checkBallCollision(balls[i], balls[j]))
                                {
                                    gameOver = true;
                                    break;
                                }
                            }
                        }

                        totalScore++;
                        if (totalScore >= 800)
                        {
                            gameOver = true;
                            win = true;
                        }
                    }
                }

                else
                {
                    for (int j = 0; j < balls.Length; j++)
                    {
                        if (i != j)
                        {
                            //center collision
                            if (balls[i].HitBox.IntersectsWith(balls[j].HitBox) && checkBallCollision(balls[i], balls[j]))
                            {
                                int xTemp = balls[i].xSpeed;
                                int yTemp = balls[i].ySpeed;
                                balls[i].xSpeed = balls[j].xSpeed;
                                balls[j].xSpeed = xTemp;
                                balls[i].ySpeed = balls[j].ySpeed;
                                balls[j].ySpeed = yTemp;
                                break;
                            }
                        }
                    }
                }
            }

            for(int i = 0; i < balls.Length; i++)
            {
                balls[i].drawBall(Brushes.Black, gfx);
            }

            //Need to find center of circle
            //Then find edges
            gfx.DrawString($"{totalScore}/100", new Font("Arial", 10), Brushes.Black, new Point(0, 0));


            pictureBox1.Image = map;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.X, e.Y);
        }
    }
}
