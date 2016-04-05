using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Havoc
{
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screenManager;
        private InputManager inputManager;
        private Vector2 virtualScreen = new Vector2(1920, 1080);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            screenManager = new ScreenManager();
            inputManager = new InputManager(screenManager);
            // Pass created inputManager to ScreenManager
            screenManager.InputManager = inputManager;
            screenManager.InitializeScreen();
            Content.RootDirectory = "Content";
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            Resolution.SetResolution(1280, 720, false);

            Camera2D.Instance.Origin = new Vector2(1920 / 2, 1080 / 2);
        }

        
        protected override void Initialize()
        {
            base.Initialize();
        }

    
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenManager.GraphicsDevice = GraphicsDevice;
            screenManager.SpriteBatch = spriteBatch;
            screenManager.LoadContent(Content);
        }

        
        protected override void UnloadContent()
        {
            screenManager.UnloadContent();
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (inputManager.KeyDown(Keys.P))
                Exit();

            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera2D.Instance.GetViewMatrix() * Resolution.getTransformationMatrix());
            screenManager.Draw(spriteBatch);
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
