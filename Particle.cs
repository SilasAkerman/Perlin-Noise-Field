using System;
using System.Numerics;
using Raylib_cs;

namespace Perlin_Noise_Field
{
    class Particle
    {
        Vector2 pos;
        Vector2 vel;
        Vector2 acc;

        Color color = new Color(255, 255, 255, 5);
        Vector2 prevPos;
        double maxSpeed = 4;
        int size = 1;
        Random random = new Random();
        int hue = 0;
        
        public Particle()
        {
            pos = new Vector2(random.Next(Program.WIDTH), random.Next(Program.HEIGHT));
            vel = new Vector2((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1);//Vector2.Zero;
            acc = Vector2.Zero;
            prevPos = new Vector2(pos.X, pos.Y);
        }

        public void Update(Vector2[] flowfield)
        {
            edges();
            follow(flowfield);

            vel += acc;
            if (vel.Length() > maxSpeed) vel /= vel.Length() / 4;
            updatePrevPos();
            pos += vel;
            acc *= 0;
        }

        private void applyForce(Vector2 force)
        {
            acc += force;
        }

        public void Display()
        {
            //Raylib.DrawCircleV(pos, size, color);
            //hue++;
            //if (hue >= 255) hue = 0;
            //color = Raylib.ColorFromHSV(hue, 255, 255);
            //color.a = 15;
            Raylib.DrawLineEx(pos, prevPos, size, color);
        }

        private void follow(Vector2[] flowfield)
        {
            int row = (int)Math.Floor(pos.Y / Program.SPACING);
            int col = (int)Math.Floor(pos.X / Program.SPACING);
            int index = col + row * Program.COLS;
            applyForce(flowfield[index]);
        }

        private void edges()
        {
            if (pos.X < 0)
            {
                pos.X = Program.WIDTH-1;
                updatePrevPos();
            }
            if (pos.X >= Program.WIDTH)
            {
                pos.X = 0;
                updatePrevPos();
            }
            if (pos.Y < 0)
            {
                pos.Y = Program.HEIGHT-1;
                updatePrevPos();
            }
            if (pos.Y >= Program.HEIGHT)
            {
                pos.Y = 0;
                updatePrevPos();
            }
        }

        private void updatePrevPos()
        {
            prevPos.X = pos.X;
            prevPos.Y = pos.Y;
        }
    }
}
