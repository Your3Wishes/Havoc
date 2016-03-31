using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Havoc
{
    public class Test_Level : Level
    {

        public Test_Level(ScreenManager screenManagerReference)
            : base(screenManagerReference){ }

        public override void LoadContent()
        {
            // Add background image
            backGroundImages.Add(new Image(screenManager));
            backGroundImages[0].Path = "Levels/Level1/Sky";
            backGroundImages[0].Effects = "BackgroundPanEffect";

            // Add moving cloud1
            backGroundImages.Add(new Image(screenManager));
            backGroundImages[1].Path = "Levels/Level1/Cloud1";
            backGroundImages[1].Effects = "PanLoopEffect:BackgroundPanEffect";
            backGroundImages[1].Scale = new Vector2(1f, 1f);

            // Add mountains
            backGroundImages.Add(new Image(screenManager));
            backGroundImages[2].Path = "Levels/Level1/Mountains";
            backGroundImages[2].Effects = "BackgroundPanEffect";




            // Add platform
            platforms.Add(new Platform(screenManager));
            platforms[0].Image.Path = "Levels/StaticObjects/MetalPlatform";
           

            // Load Content
            foreach (Platform platform in platforms)
            {
                platform.LoadContent();
            }
            foreach (Image image in backGroundImages)
            {
                image.LoadContent();
            }

            // Set background in correct location
            backGroundImages[0].Position.X = (screenManager.Dimensions.X / 2) - (backGroundImages[0].Texture.Width / 2);
            backGroundImages[0].Position.Y = (screenManager.Dimensions.Y / 2) - (backGroundImages[0].Texture.Height / 2);


            Console.WriteLine("Yo");


            // Set cloud 1 in correct location
            backGroundImages[1].Position.X = (screenManager.Dimensions.X / 2) - (backGroundImages[1].Texture.Width / 2);
            backGroundImages[1].Position.Y = 200;

            // Set mountains in correct location
            backGroundImages[2].Position.X = (screenManager.Dimensions.X / 2) - (backGroundImages[2].Texture.Width / 2);
            backGroundImages[2].Position.Y = 300;

            // Set platform in correct location
            platforms[0].Image.Position.X = screenManager.Dimensions.X / 2 - (platforms[0].Image.SourceRect.Width / 2);
            platforms[0].Image.Position.Y = screenManager.Dimensions.Y - 450;

        }
    }
}
