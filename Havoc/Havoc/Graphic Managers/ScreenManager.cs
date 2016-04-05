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

namespace Havoc
{
    public class ScreenManager
    {
        public InputManager InputManager { get; set; }
        public Vector2 Dimensions { private set; get; }
        public ContentManager Content { private set; get; }
        
        GameScreen currentScreen, newScreen;
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;
        public Image Image;
        public bool IsTransitioning { get; private set; }


        public ScreenManager(InputManager inputManagerReference)
        {
            InputManager = inputManagerReference;
            Dimensions = new Vector2(1920, 1080);
            currentScreen = new GamePlayScreen(this, InputManager);
        }
        


        /*
            Default constructor
        */
        public ScreenManager()
        {
            Dimensions = new Vector2(1920, 1080);
            
        }

        public void InitializeScreen()
        {
            currentScreen = new GamePlayScreen(this, InputManager);
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
            Image.getFadeEffect().Increase = true;
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
                    //xmlGameScreenManager.Type = currentScreen.Type;
                    //if (File.Exists(currentScreen.XmlPath))
                    //{
                    //    currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);
                    //}

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
            Image = new Image(this);
            Image.Path = "ScreenManager/FadeImage";
            Image.Effects = "FadeEffect";
            Image.Scale = new Vector2(640, 480);
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
