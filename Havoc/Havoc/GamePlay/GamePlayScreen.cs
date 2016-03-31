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

        Player player1;
        Player player2;
        Level level;

        public override void LoadContent()
        {
            base.LoadContent();

            player1 = new Alex_Character();
            player1.PlayerID = 0;
            player1.LoadContent();

            player2 = new Alex_Character();
            player2.Outfit = 2;
            player2.PlayerID = 1;
            player2.LoadContent();

            level = new Test_Level();
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

            // Call all update methods
            base.Update(gameTime);
            player1.Update(gameTime);
            player2.Update(gameTime);
            level.Update(gameTime);


            // Check for collisions on platforms
            foreach(Platform platform in level.platforms)
            {
                player1.CollisionCheck(platform);
                player2.CollisionCheck(platform);

            }

           
            // Check for collisions on player attacks
            player1.CollisionCheck(player2.HitBox, player2);
            player2.CollisionCheck(player1.HitBox, player1);
            
            // Update Camera
            // Point camera to new pan location
            Camera2D.PointCameraTo(GetCenterOfPlayers(player1, player2) - Camera2D.Origin); // For panning camera
            Camera2D.UpdateZoomOnPlayers(player1, player2);
            Camera2D.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            level.Draw(spriteBatch);

            foreach (Platform platform in level.platforms)
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
            midPoint.X = (player1.Image.Position.X + player2.Image.Position.X) / 2;
            midPoint.Y = (player1.Image.Position.Y + player2.Image.Position.Y) / 2;
            midPoint.Y += 50;

            return midPoint; 
        }

    }
}
