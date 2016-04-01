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

        public Alex_Character(ScreenManager screenManagerReference, InputManager inputManagerReference)
            : base(screenManagerReference, inputManagerReference)
        {
        }
        

        public override void LoadContent()
        {
          
            MoveSpeed = 450;
            AccelerateSpeed = 34.5f;
            DeccelerateSpeed = 125.5f;
            MoveSpeedInAir = 270;

            JumpVelocity = 12.7f;
            Image.Effects = "SpriteSheetEffect";
            if (Outfit == 1)
                Image.Path = "GamePlay/Characters/Ryu";
            else if (Outfit == 2)
                Image.Path = "GamePlay/Characters/RyuEvil";

            LoadAnimations();
            base.LoadContent();
        }

        /*
            Stores data for character's animations
        */
        public void LoadAnimations()
        {
            // Set Animation Data
            Animations["idle"].StartFrame = new Vector2(0, 0);
            Animations["idle"].NumberOfFrames = 4;
            Animations["idle"].NumberOfTotalFrames = 10;
            Animations["idle"].Speed = 170;

            Animations["walk"].StartFrame = new Vector2(0, 1);
            Animations["walk"].NumberOfFrames = 5;
            Animations["walk"].NumberOfTotalFrames = 10;
            Animations["walk"].Speed = 100;

            Animations["jump"].StartFrame = new Vector2(0, 2);
            Animations["jump"].NumberOfFrames = 5;
            Animations["jump"].NumberOfTotalFrames = 10;
            Animations["jump"].Speed = 250;
            Animations["jump"].Repeat = false;

            // KICK ANIMATION DATA
            Animations["kick"].StartFrame = new Vector2(0, 3);
            Animations["kick"].NumberOfFrames = 5;
            Animations["kick"].NumberOfTotalFrames = 5;
            Animations["kick"].Speed = 60;
            Animations["kick"].Damage = 3.0f;
            Animations["kick"].KnockBack = new Vector2( 0.0022f, 0.006f);
            //Animations["kick"].KnockBack.Y = 0.006f;
            //Animations["kick"].KnockBack.X = 0.0022f;

            Animations["kick"].Repeat = false;
            Animations["kick"].HitBoxes = new Rectangle[Animations["kick"].NumberOfFrames];
            Animations["kick"].HitBoxes[0] = new Rectangle(0, 0, 0, 0); // No hitbox on this frame
            Animations["kick"].HitBoxes[1] = new Rectangle(55, 20, 40, 51);
            Animations["kick"].HitBoxes[2] = new Rectangle(60, 20, 50, 60);
            Animations["kick"].HitBoxes[3] = new Rectangle(60, 65, 33, 33);
            Animations["kick"].HitBoxes[4] = new Rectangle(0, 0, 0, 0); // No hitbox on this frame
            // END KICK ANIMATION DATA

            Animations["stun"].StartFrame = new Vector2(0, 4);
            Animations["stun"].NumberOfFrames = 4;
            Animations["stun"].NumberOfTotalFrames = 5;
            Animations["stun"].Speed = 250;
            Animations["stun"].Repeat = false;

            NumberOfAnimations = 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
