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
            Image.Path = "GamePlay/Characters/Alex";

            // Animations
            Animations["idle"].StartFrame = new Vector2(0, 0);
            Animations["idle"].NumberOfFrames = 3;

            Animations["walkRight"].StartFrame = new Vector2(0, 2);
            Animations["walkRight"].NumberOfFrames = 3;

           
            Animations["walkLeft"].StartFrame = new Vector2(0, 1);
            Animations["walkLeft"].NumberOfFrames = 3;

 
            MaxNumberOfFrames = new Vector2(3, 4);

            base.LoadContent();
        }

    
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
