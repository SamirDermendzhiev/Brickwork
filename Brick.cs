using System;
using System.Collections.Generic;
using System.Text;

namespace Brickwork
{
    class Brick
    {
        //Displayed number
        public int Number;
        //Checked to the right but not bottom
        public bool Unchecked;
        //Checked right and bottom
        public bool Checked;
        //Is the main part of the brick
        public bool IsChild;
        //Position of second hald of brick
        public int ChildRow;
        public int ChildColumn;

        //For checking layer
        public bool Reviewed; 

        public Brick(int number)
        {
            Number = number;
            Unchecked = false;
            Checked = false;
            Reviewed = false;
            IsChild = false;
        }
    }
}
