using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Character Alex

namespace Havoc
{
    public class Alex_Character : Player
    {

        public override void LoadContent()
        {
          
            MoveSpeed = 345;
            MoveSpeedInAir = 225;
            Gravity = 45.0f;
            JumpVelocity = 9.7f;
            Image.Effects = "SpriteSheetEffect";
            Image.Path = "GamePlay/Characters/Ryu1";

            LoadAnimations();
            base.LoadContent();
        }

        /*
            Stores data for character's animations
        */
        public void LoadAnimations()
        {
            // Animations
            Animations["idle"].StartFrame = new Vector2(0, 0);
            Animations["idle"].NumberOfFrames = 4;
            Animations["idle"].NumberOfTotalFrames = 10;
            Animations["idle"].Speed = 170;


            Animations["walk"].StartFrame = new Vector2(0, 1);
            Animations["walk"].NumberOfFrames = 5;
            Animations["walk"].NumberOfTotalFrames = 10;
            Animations["walk"].Speed = 250;


            Animations["jump"].StartFrame = new Vector2(0, 2);
            Animations["jump"].NumberOfFrames = 5;
            Animations["jump"].NumberOfTotalFrames = 10;
            Animations["jump"].Speed = 50;
            Animations["jump"].repeat = false;

            Animations["kick"].StartFrame = new Vector2(0, 3);
            Animations["kick"].NumberOfFrames = 5;
            Animations["kick"].NumberOfTotalFrames = 5;
            Animations["kick"].Speed = 80;
            Animations["kick"].repeat = false;


            NumberOfAnimations = 4;
        }

    
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
