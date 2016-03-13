using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Havoc
{
   public class HitBox
    {
        public Rectangle Rectangle;
        public float Damage;


        public HitBox()
        {
            Rectangle = new Rectangle();
            Damage = 0;
        }

    }
}
