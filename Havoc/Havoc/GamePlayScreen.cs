﻿using System;
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
        Platform platform;

        public override void LoadContent()
        {
            base.LoadContent();

            //XmlManager<Player> playerLoader = new XmlManager<Player>();
            //player = playerLoader.Load("Load/GamePlay/Player.xml");
            player1 = new Alex_Character();
            player1.PlayerID = 1;
            player1.LoadContent();

            player2 = new Alex_Character();
            player2.LoadContent();

            //background = new Image();
            //background.Texture = content.Load<Texture2D>("Levels/BackGrounds/level1");
            //background.Position.X = (ScreenManager.Instance.Dimensions.X / 2) - (background.Texture.Width / 2);
            //background.Position.Y = (ScreenManager.Instance.Dimensions.Y / 2) - (background.Texture.Height / 2);

            //background.LoadContent();
            level = new Test_Level();
            level.LoadContent();

            //platform = new Platform();
            //platform.Image.Texture = content.Load<Texture2D>("Levels/StaticObjects/MetalPlatform");
            //platform.LoadContent();
            

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
            //platform.Update(gameTime);
            level.Update(gameTime);


            // Check for collisions
            foreach(Platform platform in level.platforms)
            {
                player1.CollisionCheck(platform);
                player2.CollisionCheck(platform);

            }

            // Update Camera
            // Point camera to new pan location
            Camera2D.PointCameraTo(GetCenterOfPlayers(player1, player2) - Camera2D.Origin); // For panning camera
            Camera2D.UpdateZoomOnPlayers(player1, player2);
            Camera2D.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //background.Draw(spriteBatch);
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

            return midPoint; 
        }

    }
}