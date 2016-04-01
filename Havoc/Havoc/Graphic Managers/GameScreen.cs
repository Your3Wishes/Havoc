using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


// Represents a renderable screen.Base class to all 
// types of screens to be rendered

namespace Havoc
{
    public class GameScreen
    {
        protected ScreenManager screenManager;
        protected InputManager inputManager;
        protected ContentManager content;
        
        public GameScreen(ScreenManager screenManagerReference, InputManager inputManagerReference)
        {
            screenManager = screenManagerReference;
            inputManager = inputManagerReference;
        }

        public GameScreen()
        {
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(
                screenManager.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            inputManager.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
