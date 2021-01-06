using System;
using System.Collections.Generic;

namespace Brickwork
{
    class Program
    {
        //Area width
        static int width;
        //Area height
        static int height;
        //Brick layout from user input
        static Brick[][] InputBrickLayer;

        static int maxInput = 100;

        static void Main(string[] args)
        {
            bool succes;
            do
            {
                succes = GetInput();
            } while (!succes);

            BrickLayerBuilder builder = new BrickLayerBuilder();

            if (builder.Build(InputBrickLayer) == null)
            {
                return;
            }

            Console.ReadKey();
        }

        private static bool GetInput()
        {
            try
            {
                //Get area dimensions
                Console.WriteLine("Enter the height and width of the area:");

                //Read line, and split it by whitespace into an array of strings
                string[] InputHolder = Console.ReadLine().Split();

                if (InputHolder.Length != 2)
                {
                    Console.WriteLine("Enter two numbers!");
                    return false;
                }

                height = Convert.ToInt32(InputHolder[0]);
                width = Convert.ToInt32(InputHolder[1]);

                //Check for even numbers and length
                if (height > maxInput || width > maxInput || height % 2 != 0 || width % 2 != 0 || height == 0 || width == 0)
                {
                    Console.WriteLine("Size must be under 100 and an even number!");
                    return false;
                }

                Console.WriteLine("Enter the brick layout:");

                //Set layout rows
                InputBrickLayer = new Brick[height][];

                for (int i = 0; i < height; i++)
                {
                    //Set row width
                    InputBrickLayer[i] = new Brick[width];

                    InputHolder = Console.ReadLine().Split();

                    //Check for right number count
                    if (InputHolder.Length != width)
                    {
                        Console.WriteLine("Incorect number count!");
                        return false;
                    }

                    //Fill row with elements
                    for (int j = 0; j < width; j++)
                    {
                        int BrickNumber = Convert.ToInt32(InputHolder[j]);
                        InputBrickLayer[i][j] = new Brick(BrickNumber);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input!");
                return false;
            }

            return true;
        }
    }
}
