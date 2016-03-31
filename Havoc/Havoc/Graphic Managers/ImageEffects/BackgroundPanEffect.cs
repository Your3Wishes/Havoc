using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

// Background Pan Image effect
// Used to slide the background horizontally
// with respect to the camera horizontal move speed
// Gives extra depth to the background layers

namespace Havoc
{
    public class BackgroundPanEffect : ImageEffect
    {

        public float PanSpeed;

        public BackgroundPanEffect(ScreenManager screenManagerReference)
            : base(screenManagerReference) 
        {
            PanSpeed = -0.35f;
        }


        public BackgroundPanEffect()
        {
            PanSpeed = -0.35f;
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

                // If camera is panning left, move image to the right
                if (Camera2D.PanLeft)
                {
                    image.Position.X += PanSpeed * Camera2D.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100.0f;
                }
                // If camera is panning right, move image to the left
                else if (Camera2D.PanRight) 
                {
                    image.Position.X -= PanSpeed * Camera2D.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100.0f;
                }
            }
        }





    }
}
