using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Havoc
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector2 virtualScreen = new Vector2(1920, 1080);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            Resolution.SetResolution(1024, 576, false);

            Camera2D.Origin = new Vector2(1920 / 2, 1080 / 2);
        }

        
        protected override void Initialize()
        {
            base.Initialize();
        }

    
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);

        }

        
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyDown(Keys.P))
                Exit();

            ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera2D.GetViewMatrix() * Resolution.getTransformationMatrix());
            ScreenManager.Instance.Draw(spriteBatch);
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
