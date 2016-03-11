using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


// SpriteSheet Image Effect
// Used for animating a sprite sheet
// Each row of the sprite sheet represents
// a different animation

namespace Havoc
{
    public class SpriteSheetEffect : ImageEffect
    {
        public int FrameCounter; // Counter for switching frames
        public int SwitchFrame; // When to switch to next frame
        public Vector2 CurrentFrame; // The current animation frame coords
        public int NumberOfAnimations; // Total number of different animations in spritesheet
        public Animation CurrentAnimation; // The current animation to cycle through
        public bool Animate; // Should the animation animate

        public int FrameWidth
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Width / (int)CurrentAnimation.NumberOfTotalFrames;
                return 0;
            }
        }

        public int FrameHeight
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Height / (int)NumberOfAnimations;
                return 0;
            }
        }

        public SpriteSheetEffect()
        {
            SwitchFrame = 150;
            FrameCounter = 0;
            Animate = true;
        }


        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (image.IsActive)
            {
                FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    if (Animate)
                        CurrentFrame.X++;

                    // If the end of the animation is reached
                    if (CurrentFrame.X >= CurrentAnimation.NumberOfFrames)
                    {
                        // Stop non repeating animations
                        if (!CurrentAnimation.repeat && Animate)
                        {
                            Animate = false;
                            CurrentFrame.X--;
                        }
                        else // Start the animation over if it repeats
                        CurrentFrame.X = CurrentAnimation.StartFrame.X;
                    }
                }
            }
            else
                CurrentFrame.X = 1;

            image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                 (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        /*
            Sets the CurrentAnimation
            Update the current animation frame 
            to the new animation's start frame
        */
        public void SetAnimation(Animation animation)
        {
            if (this.CurrentAnimation != animation)
                CurrentFrame.X = animation.StartFrame.X;


            this.CurrentAnimation = animation;
            CurrentFrame.Y = animation.StartFrame.Y;
            SwitchFrame = animation.Speed;
            Animate = true;
        }

    }
}
