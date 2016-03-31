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
    public class PanLoopEffect : ImageEffect
    {
        public float PanSpeed;

        public PanLoopEffect(ScreenManager screenManagerReference)
            : base(screenManagerReference)
        {
            PanSpeed = 50.0f;
        }


        public PanLoopEffect()
        {
            PanSpeed = 50f;
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

                // If image goes off-screen, reset it
                if (image.Position.X  > screenManager.Dimensions.X + 300)
                    image.Position.X = 0 - image.SourceRect.Width;

                // Pan image
                image.Position.X += PanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }





    }
}
