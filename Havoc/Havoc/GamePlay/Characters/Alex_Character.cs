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
            NumberOfAnimations = 5;

            JumpVelocity = 12.7f;
            Image.Effects = "SpriteSheetEffect";
            if (Outfit == 1)
                Image.Path = "GamePlay/Characters/Ryu";
            else if (Outfit == 2)
                Image.Path = "GamePlay/Characters/RyuEvil";

            base.LoadContent();

            LoadAnimations();
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
            Image.getSpriteSheetEffect().PlayerBaseFrameWidth = Image.Texture.Width / (int)Animations["idle"].NumberOfTotalFrames;


            Animations["walk"].StartFrame = new Vector2(0, 1);
            Animations["walk"].NumberOfFrames = 5;
            Animations["walk"].NumberOfTotalFrames = 10;
            Animations["walk"].Speed = 100;

            Animations["jump"].StartFrame = new Vector2(0, 2);
            Animations["jump"].NumberOfFrames = 5;
            Animations["jump"].NumberOfTotalFrames = 10;
            Animations["jump"].Speed = 250;
            Animations["jump"].Repeat = false;

            // jab ANIMATION DATA
            Animations["jab"].StartFrame = new Vector2(0, 3);
            Animations["jab"].NumberOfFrames = 5;
            Animations["jab"].NumberOfTotalFrames = 5;
            Animations["jab"].Speed = 60;
            Animations["jab"].Damage = 3.0f;
            Animations["jab"].KnockBack = new Vector2( 0.0022f, 0.006f);

            Animations["jab"].Repeat = false;
            Animations["jab"].HitBoxes = new Rectangle[Animations["jab"].NumberOfFrames];
            Animations["jab"].HitBoxes[0] = new Rectangle(0, 0, 0, 0); // No hitbox on this frame
            Animations["jab"].HitBoxes[1] = new Rectangle(55, 20, 40, 51);
            Animations["jab"].HitBoxes[2] = new Rectangle(60, 20, 50, 60);
            Animations["jab"].HitBoxes[3] = new Rectangle(60, 65, 33, 33);
            Animations["jab"].HitBoxes[4] = new Rectangle(0, 0, 0, 0); // No hitbox on this frame
            // END jab ANIMATION DATA

            Animations["stun"].StartFrame = new Vector2(0, 4);
            Animations["stun"].NumberOfFrames = 4;
            Animations["stun"].NumberOfTotalFrames = 5;
            Animations["stun"].Speed = 650;
            Animations["stun"].Repeat = false;

            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
