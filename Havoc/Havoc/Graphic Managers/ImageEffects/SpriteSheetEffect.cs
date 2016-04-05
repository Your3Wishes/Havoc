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
        // TODO: Fix player position when facing left on wide animations

        private Vector2 CurrentFrame;
        public int NumberOfAnimations { get; set; } // Total number of different animations in spritesheet
        public Animation CurrentAnimation { get; set; } // The current animation to cycle through
        public bool Animate { get; set; } // Should the animation animate
        public bool PlayerOffset; // If the player has been offset to fix left facing animations
        public int PlayerBaseFrameWidth { get; set; } // The frame width of "idle" usually
        public int PrevFrameWidth { get; set; } // Width of the previous animation's frames

        private int FrameCounter; // Counter for switching frames
        private int SwitchFrame; // When to switch to next frame

        public SpriteSheetEffect(ScreenManager screenManagerReference)
            : base(screenManagerReference) { }
        
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
            PlayerOffset = false;
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
                        if (!CurrentAnimation.Repeat && Animate)
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

            // Move the image's source rectangle to the current frame on the sprite sheet
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

            if (this.CurrentAnimation != null)
                PrevFrameWidth = FrameWidth;

            this.CurrentAnimation = animation;
            CurrentFrame.Y = animation.StartFrame.Y;
            SwitchFrame = animation.Speed;
            Animate = true;



        }

        public void RestartAnimation(Animation animation)
        {
            CurrentFrame = new Vector2(CurrentAnimation.StartFrame.X, CurrentAnimation.StartFrame.Y);
        }

        public Vector2 getCurrentFrame()
        {
            return CurrentFrame;
        }

        



    }
}
