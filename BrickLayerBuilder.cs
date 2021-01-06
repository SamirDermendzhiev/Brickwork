using System;
using System.Collections.Generic;
using System.Text;

namespace Brickwork
{
    class BrickLayerBuilder
    {
        //Recieved
        Brick[][] FirstBrickLayer;

        //Returned
        Brick[][] SecondBrickLayer;

        //false-> forward || true->backwards
        bool forward = true;

        //Follows numbers on bricks
        int BrickCounter = 0;

        //Area dimensions
        int height;
        int width;

        bool Finish = false;
        public Brick[][] Build(Brick[][] PrevBrickLayer)
        {
            Console.WriteLine("\nBuilding...");

            FirstBrickLayer = PrevBrickLayer;
            height = FirstBrickLayer.Length;
            width = FirstBrickLayer[0].Length;

            if (!CheckBrickLayer(FirstBrickLayer))
            {
                return null;
            } 

            //Set second layer rows
            SecondBrickLayer = new Brick[height][];
            for (int i = 0; i < height; i++)
            {
                //Set second layer row width
                SecondBrickLayer[i] = new Brick[width];
            }

            //Move through the layer
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (forward)
                    {
                        Forward(ref i, ref j, ref forward);
                    }
                    else
                    {
                        Backwards(ref i, ref j, ref forward);
                        if (Finish == true)
                        {
                            return null;
                        }
                    }
                }
            }

            if (!CheckBrickLayer(SecondBrickLayer))
            {
                return null;
            }

            //Print result
            DisplayBrickLayer(SecondBrickLayer);
            //foreach (var row in SecondBrickLayer)
            //{
            //    foreach (var element in row)
            //    {
            //        Console.Write(element.Number + " ");
            //    }
            //    Console.WriteLine();
            //}  

            return SecondBrickLayer;
        }

        private void Forward(ref int row, ref int col, ref bool forward)
        {
            //On reaching empty object, create brick
            if (SecondBrickLayer[row][col] == null)
            {
                BrickCounter++;
                SecondBrickLayer[row][col] = new Brick(BrickCounter);
            }

            //Pick brick rotation if it is not a child
            if (!SecondBrickLayer[row][col].IsChild)
            {
                //Check right
                if (SecondBrickLayer[row][col].Unchecked == false && col + 1 < width && FirstBrickLayer[row][col].Number != FirstBrickLayer[row][col + 1].Number)
                {            
                    SecondBrickLayer[row][col + 1] = new Brick(BrickCounter);
                    SecondBrickLayer[row][col + 1].IsChild = true;
                    SecondBrickLayer[row][col].ChildRow = row;
                    SecondBrickLayer[row][col].ChildColumn = col + 1;
                    SecondBrickLayer[row][col].Unchecked = true;
                }
                //Check under
                else if (row + 1 < height && FirstBrickLayer[row + 1][col].Number != FirstBrickLayer[row][col].Number)
                {
                    SecondBrickLayer[row + 1][col] = new Brick(BrickCounter);
                    SecondBrickLayer[row + 1][col].IsChild = true;
                    SecondBrickLayer[row][col].ChildRow = row + 1;
                    SecondBrickLayer[row][col].ChildColumn = col;
                    SecondBrickLayer[row][col].Checked = true;
                }
                //No valid position -> reverse direction
                else
                {
                    //Reverse direction
                    forward = false;

                    //Remove brick
                    SecondBrickLayer[row][col] = null;
                    BrickCounter--;

                    //Step back
                    if (col > 0)
                    {
                        col -= 2;
                    }
                    else
                    {
                        col = width - 2;
                        row--;
                    }
                }
            }
        }

        private void Backwards(ref int row, ref int col, ref bool forward)
        {
            //Skip child
            if (SecondBrickLayer[row][col].IsChild)
            {
                //Step back
                if (col > 0)
                {
                    col -= 2;
                }
                else
                {
                    col = width - 2;
                    row--;
                }
            }
            //Repeat unchecked
            else if (!SecondBrickLayer[row][col].Checked)
            {
                //Remove child of brick before going back
                int childRow = SecondBrickLayer[row][col].ChildRow;
                int childColumn = SecondBrickLayer[row][col].ChildColumn;
                SecondBrickLayer[childRow][childColumn] = null;

                //Revese direction
                forward = true;
                
                //Repeat element
                if (col > 0)
                {
                    col--;
                }
                else
                {
                    col = width - 1;
                    row--;
                }
            }
            //Remove Checked
            else
            {
                //If we checked the first element and return to it there is no compatible brick pattern
                if (row == 0 && col == 0)
                {
                    Console.WriteLine("-1");
                    Console.WriteLine("The system could not find a compatible pattern!");
                    Finish = true;
                }

                //Remove child and parent brick
                int childRow = SecondBrickLayer[row][col].ChildRow;
                int childColumn = SecondBrickLayer[row][col].ChildColumn;
                SecondBrickLayer[childRow][childColumn] = null;
                SecondBrickLayer[row][col] = null;
                BrickCounter--;

                //Step back
                if (col > 0)
                {
                    col -= 2;
                }
                else
                {
                    col = width - 2;
                    row--;
                }
            }
        }

        public bool CheckBrickLayer(Brick[][] LayerToCheck)
        {
            //Check if every number is encountered twice
            var dict = new Dictionary<int, int>();

            foreach (var row in LayerToCheck)
            {
                foreach (var element in row)
                {
                    if (dict.ContainsKey(element.Number))
                        dict[element.Number]++;
                    else
                        dict[element.Number] = 1;
                }
            }

            foreach (var pair in dict)
            {
                if (pair.Value != 2)
                {
                    Console.WriteLine("Incorrect brick layout!");
                    return false;
                }
            }

            //Check if the tow halfs of the bricks are next to each other
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!LayerToCheck[i][j].Reviewed)
                    {
                        //Check right
                        if (j<width-1 && LayerToCheck[i][j].Number == LayerToCheck[i][j+1].Number)
                        {
                            //Use checked bool to skip child elements
                            LayerToCheck[i][j].Reviewed = true;
                            LayerToCheck[i][j + 1].Reviewed = true;
                        }
                        //Check under
                        else if (i < height && LayerToCheck[i][j].Number == LayerToCheck[i + 1][j].Number)
                        {
                            LayerToCheck[i][j].Reviewed = true;
                            LayerToCheck[i + 1][j].Reviewed = true;
                        }
                        else
                        {
                            Console.WriteLine("Bricks are not placed properly!");
                            return false;
                        }
                    }
                    
                }
            }

            return true;
        }

        public void DisplayBrickLayer(Brick[][] LayerToBuild)
        {                
            //Every row of brick is represented by 5 rows of text
            for (int row = 0; row < height; row++)
            {
                //First row only *
                if (row==0)
                {                   
                    for (int col = 0; col < width; col++)
                    {
                        if (col == 0)
                        {
                            Console.Write("*");
                        }
                        Console.Write("*********");
                    }
                    Console.WriteLine();
                }
                //Rows 2-4
                //Add pad so we can fit numbers up to 5000
                for (int i = 0; i < 3; i++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        //First element is always a *
                        if (col == 0)
                        {
                            Console.Write("*");
                        }
                        //Third row
                        if (i==1)
                        {
                            if ((col + 1 < width && LayerToBuild[row][col].Number != LayerToBuild[row][col + 1].Number)|| col == width - 1)
                            {
                                Console.Write(LayerToBuild[row][col].Number.ToString().PadLeft(6) + "*".PadLeft(3));
                            }
                            else
                            {
                                Console.Write(LayerToBuild[row][col].Number.ToString().PadLeft(6) + " ".PadLeft(3));
                            }
                        }
                        //second and fourth row
                        else
                        {                          
                            if ((col + 1 < width && LayerToBuild[row][col].Number != LayerToBuild[row][col + 1].Number)|| col == width - 1)
                            {
                                Console.Write("*".PadLeft(9));
                            }
                            else
                            {
                                Console.Write(" ".PadLeft(9));
                            }
                        }               
                    }
                    Console.WriteLine();
                }
                
                for (int col = 0; col < width; col++)
                {
                    if (col == 0)
                    {
                        Console.Write("*");
                    }
                    if ((row + 1 < height && LayerToBuild[row][col].Number != LayerToBuild[row + 1][col].Number) || row == height - 1)
                    {
                        Console.Write("*********");
                    }
                    else 
                    {
                        Console.Write("*".PadLeft(9));
                    }
                }
                Console.WriteLine();
                
            }
            Console.WriteLine();

            /*
             ********
             *      *
             *      *
             *      *
             ********
             */

        }
    }
}
