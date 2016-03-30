﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// SplashScreen is the first gameScreen to be displayed in our game

namespace Havoc
{
    public class SplashScreen : GameScreen
    {
        public Image Image;

      

        public override void LoadContent()
        {
            base.LoadContent();
            Image = new Image();
            Image.Path = "SplashScreen/image";
            Image.Effects = "FadeEffect";
            Image.IsActive = false;
            Image.Alpha = 0.5f;
            Image.LoadContent();
            // Set Image to center of screen
            Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X -
                       Image.SourceRect.Width) / 2, (ScreenManager.Instance.Dimensions.Y -
                       Image.SourceRect.Height) / 2);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Z))
                ScreenManager.Instance.ChangeScreens("TitleScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
