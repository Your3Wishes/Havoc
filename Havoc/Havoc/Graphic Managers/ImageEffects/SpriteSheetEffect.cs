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
        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 MaxNumberOfFrames;
        public int AnimationLength;
        public Vector2 AnimationStart;
        public Vector2 Animation;

        public int FrameWidth
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Width / (int)MaxNumberOfFrames.X;
                return 0;
            }
        }

        public int FrameHeight
        {
            get
            {
                if (image.Texture != null)
                    return image.Texture.Height / (int)MaxNumberOfFrames.Y;
                return 0;
            }
        }

        public SpriteSheetEffect()
        {
            MaxNumberOfFrames = new Vector2(3, 4);
            CurrentFrame = new Vector2(1, 0);
            SwitchFrame = 100;
            FrameCounter = 0;
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
                    CurrentFrame.X++;

                    if (CurrentFrame.X * FrameWidth >= image.Texture.Width)
                        CurrentFrame.X = AnimationStart.X;

                }
            }
            else
                CurrentFrame.X = 1;

            image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

    }
}
