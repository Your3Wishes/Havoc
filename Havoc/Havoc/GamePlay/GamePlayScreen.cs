using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Havoc
{
    public class GamePlayScreen : GameScreen
    {

        private Player player1;
        private Player player2;
        private Level level;

        public GamePlayScreen(ScreenManager screenManagerReference, InputManager inputManagerReference)
        {
            screenManager = screenManagerReference;
            inputManager = inputManagerReference;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            player1 = new Alex_Character(screenManager, inputManager);
            player1.PlayerID = 0;
            player1.LoadContent();

            player2 = new Alex_Character(screenManager, inputManager);
            player2.Outfit = 2;
            player2.PlayerID = 1;
            player2.LoadContent();

            level = new Test_Level(screenManager);
            level.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player1.UnloadContent();
            player2.UnloadContent();
            level.UnLoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player1.Update(gameTime);
            player2.Update(gameTime);
            level.Update(gameTime);


            // Check for collisions with platforms
            foreach(Platform platform in level.Platforms)
            {
                player1.CollisionCheck(platform);
                player2.CollisionCheck(platform);

            }

            // Check for collisions on player attacks
            player1.CollisionCheck(player2.HitBox, player2);
            player2.CollisionCheck(player1.HitBox, player1);
            
            // Update Camera
            // Point camera to new pan location
            Camera2D.Instance.PointCameraTo(GetCenterOfPlayers(player1, player2) - Camera2D.Instance.Origin); // For panning camera
            Camera2D.Instance.UpdateZoomOnPlayers(player1, player2);
            Camera2D.Instance.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            level.Draw(spriteBatch);

            foreach (Platform platform in level.Platforms)
                platform.Draw(spriteBatch);

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
        }

        /*
            Returns the point in between the players
            Ideally used to center camera on this point
        */
        public Vector2 GetCenterOfPlayers(Player player1, Player player2)
        {
            Vector2 midPoint = new Vector2();
            midPoint.X = (player1.Image.getPosition().X + player2.Image.getPosition().X) / 2;
            midPoint.Y = (player1.Image.getPosition().Y + player2.Image.getPosition().Y) / 2;
      
            return midPoint; 
        }

    }
}
