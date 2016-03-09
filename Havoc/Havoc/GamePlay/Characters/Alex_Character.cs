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
            Image = new Image();
            MoveSpeed = 345;
            Gravity = 45.0f;
            JumpVelocity = 9.7f;
            Image.Effects = "SpriteSheetEffect";
            Image.Path = "GamePlay/Characters/Ryu";

            // Animations
            Animations["idle"].StartFrame = new Vector2(0, 0);
            Animations["idle"].NumberOfFrames = 4;
            Animations["idle"].NumberOfTotalFrames = 5;
            Animations["idle"].Speed = 170;


            Animations["walk"].StartFrame = new Vector2(0, 1);
            Animations["walk"].NumberOfFrames = 5;
            Animations["walk"].NumberOfTotalFrames = 5;
            Animations["walk"].Speed = 250;


            Animations["jump"].StartFrame = new Vector2(0, 2);
            Animations["jump"].NumberOfFrames = 5;
            Animations["jump"].NumberOfTotalFrames = 5;
            Animations["jump"].Speed = 50;
            Animations["jump"].repeat = false;


            NumberOfAnimations = 3;

            base.LoadContent();
        }

    
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
