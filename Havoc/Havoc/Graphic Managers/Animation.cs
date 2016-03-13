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
        public float Damage; // Damage that this animation does (if any)
        public Vector2 KnockBack; // KnockBack that this animation does (if any)
        public bool Repeat; // Should the animation repeat
        public bool HasHitBoxes; // If the animation contains hitboxes (an attack)
        public Rectangle[] HitBoxes; // Contains hitbox info for each frame of animation


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
