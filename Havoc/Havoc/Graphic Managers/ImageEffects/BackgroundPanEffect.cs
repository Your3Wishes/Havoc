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

        public float PanSpeed { get; set; }

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
                if (Camera2D.Instance.PanLeft)
                {
                    image.setPositionX(image.getPosition().X + PanSpeed * Camera2D.Instance.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.01f);
                    //image.Position.X += PanSpeed * Camera2D.Instance.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.01f;
                }
                // If camera is panning right, move image to the left
                else if (Camera2D.Instance.PanRight) 
                {
                    image.setPositionX(image.getPosition().X - PanSpeed * Camera2D.Instance.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.01f);
                    //image.Position.X -= PanSpeed * Camera2D.Instance.AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.01f;
                }
            }
        }





    }
}
