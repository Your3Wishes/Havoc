using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace Havoc
{
    public class Platform
    {
        public Image Image { get; set; }

        private ScreenManager screenManager;

        public Platform(ScreenManager screenManagerReference)
        {
            screenManager = screenManagerReference;
            Image = new Image(screenManager);
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.setPositionX(screenManager.Dimensions.X / 2 - (Image.SourceRect.Width / 2));
            Image.setPositionY(screenManager.Dimensions.Y - 450);
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }


    }
}
