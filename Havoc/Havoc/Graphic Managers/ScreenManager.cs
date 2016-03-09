using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


// Manages all game screens. 
// This is a singleton class (there is only one
// ScreenManager class that can be accessed anywhere
// in the program

namespace Havoc
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimensions { private set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<GameScreen> xmlGameScreenManager;

        GameScreen currentScreen, newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        // The single ScreenManager instance
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                 }
               
                return instance;
            }
        }

        /*
            Default constructor
        */
        private ScreenManager()
        {
            Dimensions = new Vector2(1920, 1080);
            
            currentScreen = new GamePlayScreen();
            xmlGameScreenManager = new XmlManager<GameScreen>();
            xmlGameScreenManager.Type = currentScreen.Type;
            currentScreen = xmlGameScreenManager.Load("Load/GamePlayScreen.xml");
        }

        /*
            Sets the new screen to transition into
        */
        public void ChangeScreens(string screenName)
        {
            newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("Havoc." + screenName));
            Image.IsActive = true;
            IsTransitioning = true;

            // Helps with fading transition
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;            
        }

        /*
            Transitions to a new gameScreen
        */
        void Transition(GameTime gameTime)
        {
            
            // Only do anything if screen is transitioning
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlGameScreenManager.Type = currentScreen.Type;
                    if (File.Exists(currentScreen.XmlPath))
                    {
                        currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);
                    }

                    currentScreen.LoadContent();
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }



        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
