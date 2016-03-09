using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

// A helper class for the class SpriteSheetEffect
// Represents an animation
// e.g. walk, run, idle
// Contains info needed to choose which
// images are shown from the spritesheet

namespace Havoc
{
    public class Animation
    {
        public Vector2 StartFrame; // Starting frame coordinates in spritesheet
        public int NumberOfFrames; // Number of frames in animation
        public int NumberOfTotalFrames; // Number of total frames in sprite sheet
        public int Speed; // Animations speed. Higher number = slower animation
        public bool repeat; // Should the animation repeat


        public Animation()
        {
            Speed = 100;
            repeat = true;
        }


    }
}
