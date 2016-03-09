using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Represents an object that doesn't move on GameScreen
// Entities can collide with StaticGameObject

namespace Havoc
{
    public class Platform
    {
        public Image Image;

        public Platform()
        {
            Image = new Image();
        }


        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position.X = ScreenManager.Instance.Dimensions.X / 2 - (Image.SourceRect.Width / 2);
            Image.Position.Y = ScreenManager.Instance.Dimensions.Y - 450;
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
