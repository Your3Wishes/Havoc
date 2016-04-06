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
        public Vector2 StartFrame { get; set; } // Starting frame coordinates in spritesheet
        public int NumberOfFrames { get; set; } // Number of frames in animation
        public int NumberOfTotalFrames { get; set; } // Number of total frames in sprite sheet
        public int Speed { get; set; } // Animations speed. Higher number = slower animation
        public float Damage { get; set; } // Damage that this animation does (if any)
        public bool Repeat { get; set; } // Should the animation repeat
        public bool HasHitBoxes { get; set; } // If the animation contains hitboxes (an attack)
        public Rectangle[] HitBoxes { get; set; } // Contains hitbox info for each frame of animation
        public Vector2 KnockBack { get; set; } // KnockBack that this animation does (if any)

        public Animation()
        {
            Speed = 100;
            Damage = 0;
            KnockBack = Vector2.Zero;
            Repeat = true;
            HitBoxes = new Rectangle[0];
        }
    }
}
