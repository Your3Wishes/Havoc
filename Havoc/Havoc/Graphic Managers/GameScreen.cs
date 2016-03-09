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

        protected ContentManager content;
        [XmlIgnore]
        public Type Type;

        public string XmlPath;


        public GameScreen()
        {
            Type = this.GetType();
            // Loads specific GameScreen xmlPath; e.g. Load/SplashScreen.xml
            XmlPath = "Load/" + Type.ToString().Replace("Havoc.", "") + ".xml";
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
