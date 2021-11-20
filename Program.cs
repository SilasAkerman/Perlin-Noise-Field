using System;
using Raylib_cs;
using SimplexNoise;
using System.Numerics;

namespace Perlin_Noise_Field
{
    class Program
    {
        public const int SPACING = 7;
        public const int WIDTH = (1400 / SPACING) * SPACING;
        public const int HEIGHT = (1000 / SPACING) * SPACING;

        public const int ROWS = HEIGHT / SPACING;
        public const int COLS = WIDTH / SPACING;

        const float noiseScale = 0.005f;
        static float zOffset = 0;
        static float zIncrement = 0.2f;

        static Vector2[] flowfield = new Vector2[ROWS * COLS];
        static Particle[] particles = new Particle[4000];

        static void Main(string[] args)
        {
            Noise.Seed = 420;
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
            }

            Raylib.InitWindow(WIDTH, HEIGHT, "Perlin noise Field");
            Raylib.SetTargetFPS(60);
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            Raylib.EndDrawing();

            while (!Raylib.WindowShouldClose())
            {
                Update();
                Display();
            }

            Raylib.CloseWindow();
        }

        static void Update()
        {
            setFlowfield();
            foreach(Particle particle in particles)
            {
                particle.Update(flowfield);
            }
        }

        static void Display()
        {
            Raylib.BeginDrawing();
            //Raylib.ClearBackground(Color.WHITE);
            //Raylib.DrawFPS(100, 100);
            //for (int row = 0; row < ROWS; row++)
            //{
            //    for (int col = 0; col < COLS; col++)
            //    {
            //        int index = col + row * COLS;
            //        Vector2 dir = Vector2.Normalize(flowfield[index]) * SPACING;
            //        Raylib.DrawLine(col * SPACING + SPACING, row * SPACING + SPACING, col * SPACING + (int)dir.X + SPACING, row * SPACING + (int)dir.Y + SPACING, Color.BLACK);
            //    }
            //}
            foreach (Particle particle in particles)
            {
                particle.Display();
            }
            Raylib.EndDrawing();
        }

        static void setFlowfield()
        {
            zOffset += zIncrement;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    int index = col + row * COLS;
                    double angle = Noise.CalcPixel3D(col, row, (int)zOffset, noiseScale) / 255 * Math.PI * 2 * 4;
                    flowfield[index] = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) / 5;
                }
            }
        }
    }
}
