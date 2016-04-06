using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


// Represents a playable character
// All characters in game are children
// of this class. 
// This is essentially the most
// important class in the entire project!

namespace Havoc
{
    public class Player
    {
        private bool DEBUG_HIT_BOX = true; // USED FOR DEBUGGING HITBOXES

        ScreenManager screenManager;
        InputManager inputManager;
      
        public int PlayerID { get; set; } // Identifies who is Player1, Player2, etc...
        public Image Image { get; set; } // Holds image of player. Include position, effects, etc...
        public int Outfit { get; set; } // The outfit the player uses, (skin)
        public HitBox HitBox { get; set; } // The hitbox of the player

        protected float AccelerateSpeed;
        protected float MoveSpeed; 
        protected Dictionary<string, Animation> Animations; 
        protected float DeccelerateSpeed; 
        protected float MoveSpeedInAir;  // Speed the player can move while in air
        protected float Gravity;  // Strength of Gravity
        protected float JumpVelocity; // Jump strength
        protected int NumberOfAnimations;  // Number of different animations for player

        private Vector2 Velocity; // Current speed of player
        private int jumpsLeft; 
        private int Health; // The higher the number, the harder hits player takes
        private Vector2 KnockBackVelocity; // KnockBack strength
        private Vector2 AnalogMovement; // The 2d coords of the analog stick
        private float KnockBackAntiVelocity;  // Counteracts horizonal knockback forces
        private float ComboCounter; // Used to recharge CanBeComboed
        private float ComboMaxTimer; // When ComboCounter reaches this number, player can be comboed again
        private bool Accelerating; 
        private bool Deccelerating; 
        private bool HitStun; // The player is in hitstun (can't move)
        private bool CanBeComboed; // False right after being hit
        private bool attacking; 
        private bool inAir;
        private bool jumping;
        private bool facingRight; 
        private bool blockedHorizontalRight;
        private bool blockedHorizontalLeft;
        private bool TakingKnockBack; 
        private bool TakeXKnockBack; // True if taking horizontal knockback

        public Player(ScreenManager screenManagerReference, InputManager inputManagerReference)
        {
            screenManager = screenManagerReference;
            inputManager = inputManagerReference;
            Image = new Image(screenManager);
            Outfit = 1;
            Velocity = Vector2.Zero;
            KnockBackVelocity = Vector2.Zero;
            jumpsLeft = 2;
            Health = 0;
            Gravity = 37.0f;
            KnockBackAntiVelocity = 0.68f;
            facingRight = true;
            NumberOfAnimations = 0;
            HitBox = new HitBox();
            HitStun = false;
            CanBeComboed = true;
            ComboCounter = 0;
            ComboMaxTimer = 600.0f;
            jumping = false;
            inAir = false;
            blockedHorizontalLeft = false;
            blockedHorizontalRight = false;
            TakingKnockBack = false;

            // Animations
            Animations = new Dictionary<string, Animation>();
            Animations.Add("idle", new Animation());
            Animations.Add("walk", new Animation());
            Animations.Add("jump", new Animation());
            Animations.Add("jab", new Animation());
            Animations.Add("fair", new Animation());
            Animations.Add("stun", new Animation());
        }

        public Player()
        {
        }

        public virtual void LoadContent()
        {
            Image.LoadContent();
            Image.setPositionX((screenManager.Dimensions.X / 2) - 100); // Set initial player position
            Image.getSpriteSheetEffect().NumberOfAnimations = NumberOfAnimations; // Tell SpriteSheet how many different animations
            Image.getSpriteSheetEffect().SetAnimation(Animations["idle"]); // Set animation to idle
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (PlayerID == 0)
                MoveSpeed = 8;

            Image.IsActive = true;

            // Resest the hitbox
            HitBox.Rectangle = new Rectangle();

            HandleInput(gameTime);

            HandleLogic(gameTime);

            Image.Update(gameTime);
            Image.setPosition(Image.getPosition() + Velocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            if (DEBUG_HIT_BOX)
                DrawRectangle(HitBox.Rectangle, Color.Aquamarine, spriteBatch);
        }


        public void CollisionCheck(Platform gameObject)
        {
            // Create new player collision box
            Rectangle playerRect = new Rectangle((int)Image.getPosition().X, (int)Image.getPosition().Y, Image.SourceRect.Width, Image.SourceRect.Height);
            // Create new gameObject collision box
            Rectangle gameObjectRect = new Rectangle((int)gameObject.Image.getPosition().X, (int)gameObject.Image.getPosition().Y, gameObject.Image.SourceRect.Width, gameObject.Image.SourceRect.Height);

            // Check if collision occured
            if (playerRect.Intersects(gameObjectRect))
            {
                // Collision Occured
                
                // If player is above the object, set players Y position 
                // so that the player lands exactly on top of platform
                // If we don't do this, player may glitch and land inside 
                // platform sometimes
                if ((Image.getPosition().Y + Image.SourceRect.Height) - 35 < gameObject.Image.getPosition().Y && Velocity.Y > 0)
                {

                    // Landed on top of platform
                    if ((Image.getPosition().X + Image.SourceRect.Width) - 10 > gameObject.Image.getPosition().X && 
                        Image.getPosition().X + 10 < gameObject.Image.getPosition().X + gameObject.Image.SourceRect.Width)
                    {
                        Image.setPositionY(gameObject.Image.getPosition().Y - (Image.SourceRect.Height - 1));
                        Velocity.Y = 0;
                        inAir = false;  // Player isn't in the air
                        // Reset jumps
                        jumping = false;
                        jumpsLeft = 2;
                        TakingKnockBack = false;
                        Velocity.X = 0;
                        TakeXKnockBack = false;

                    }
                    else // Besides the platform
                    {
                        inAir = true;
                    }
                }

                // Collided with the left side of the object. Blocked on right
                if ((Image.getPosition().X + Image.SourceRect.Width) - 10 < gameObject.Image.getPosition().X)
                {
                    Velocity.X = 0;
                    blockedHorizontalRight = true;
                }
                // Collided with the right size of the object. Block on left
                else if  (Image.getPosition().X + 10 > gameObject.Image.getPosition().X + gameObject.Image.SourceRect.Width)
                {
                    Velocity.X = 0;
                    blockedHorizontalLeft = true;
                }

                // Collided with the bottom of the object
                if (Image.getPosition().Y + 20 > gameObject.Image.getPosition().Y + gameObject.Image.SourceRect.Height)
                {
                    // If in the middle of platform
                    if ((Image.getPosition().X + Image.SourceRect.Width) - 10 > gameObject.Image.getPosition().X &&
                        Image.getPosition().X + 10 < gameObject.Image.getPosition().X + gameObject.Image.SourceRect.Width)
                    {
                        Velocity.Y = 0;
                        jumping = false;
                        TakingKnockBack = false;
                        inAir = true;
                    }
                }
            }
            else
            {
                // Did not collide with object
               
                inAir = true;
                blockedHorizontalRight = false;
                blockedHorizontalLeft = false;
            }

        }

        /*
            Check for a collision with a hitbox
            Parameters: hitbox - the hitbox we are checking
                        player - the player who owns the box
            The player is used to decide what direction the force is coming from
        */
        public void CollisionCheck(HitBox hitBox, Player player)
        {
            if (hitBox.Rectangle.Width <= 0) return; // Empty hitbox

            // Create new player collision box
            Rectangle playerRect = new Rectangle((int)Image.getPosition().X, (int)Image.getPosition().Y, Image.SourceRect.Width, Image.SourceRect.Height);

            // Player was hit!
            if (playerRect.Intersects(hitBox.Rectangle))
            {
                if (CanBeComboed)
                {
                    TakeHit(hitBox, player);
                    CanBeComboed = false;
                }
            }
        }

      

        /*
            Handles any state-based logic for Player
            Used for organization
            Called from Update(GameTime gameTime)
        */
        public void HandleLogic(GameTime gameTime)
        {
           

            // Handle Gravity
            // If we are in the air, then fall
            if (inAir)
            {
                Fall(gameTime);
                Accelerating = false;
                Deccelerating = false;
            }

            // Handle moving
            if (Accelerating)
            {
                Accelerate(gameTime);
            }

            if (Deccelerating)
            {
                Deccelerate(gameTime);
            }

            if (TakingKnockBack)
            {
                TakeKnockBack(gameTime);
                Image.getSpriteSheetEffect().SetAnimation(Animations["stun"]);
            }

            // Respawn player if off screen
            if (Image.getPosition().Y >= screenManager.Dimensions.Y)
            {
                Image.setPositionY(0);
                //Image.Position.Y = 0;
                Image.setPositionX(screenManager.Dimensions.X / 2);
                //Image.Position.X = screenManager.Dimensions.X / 2;
                Velocity = Vector2.Zero;
                Health = 0;
            }

            // If player is idle, set animation to idle
            if (Velocity.X == 0 && Velocity.Y == 0 && !attacking)
            {
                Image.getSpriteSheetEffect().SetAnimation(Animations["idle"]);
            }

            if (attacking)
            {
                // Set the player's hitbox to the correct spritesheet frame's hitbox
                try
                {
                    HitBox.Rectangle = Image.getSpriteSheetEffect().CurrentAnimation.HitBoxes[(int)Image.getSpriteSheetEffect().getCurrentFrame().X];
                    HitBox.Damage = Image.getSpriteSheetEffect().CurrentAnimation.Damage;
                    HitBox.KnockBack = Image.getSpriteSheetEffect().CurrentAnimation.KnockBack;
                }
                catch (IndexOutOfRangeException e)
                {
                    HitBox.Rectangle = new Rectangle();
                }

                // Check to see if done attacking
                if (!Image.getSpriteSheetEffect().Animate)
                {
                    attacking = false;
                }
            }

            // Handle player direction
            if (facingRight)
            {
                Image.SpriteEffect = SpriteEffects.None;
                // Position hitboxes relative to player's source rectangle
                HitBox.Rectangle.X += (int)Image.getPosition().X;
                HitBox.Rectangle.Y += (int)Image.getPosition().Y;
            }
            else // Facing left
            {
                Image.SpriteEffect = SpriteEffects.FlipHorizontally;
                // Position hitboxes relative to player's source rectangle
                HitBox.Rectangle.X = ((int)Image.getPosition().X + Image.SourceRect.Width) - HitBox.Rectangle.X - HitBox.Rectangle.Width;
                HitBox.Rectangle.Y += (int)Image.getPosition().Y;
                // Fix wide animations
                OffSetPlayerAnimation();

            }

            // Handle logic for CanBeComboed
            if (!CanBeComboed)
            {
                Image.getSpriteSheetEffect().SetAnimation(Animations["stun"]);
                ComboCounter += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (ComboCounter >= ComboMaxTimer) CanBeComboed = true;
            }

        }

        public void OffSetPlayerAnimation()
        {
            int FrameWidth = Image.getSpriteSheetEffect().FrameWidth;
            int BaseFrameWidth = Image.getSpriteSheetEffect().PlayerBaseFrameWidth;

            if (!facingRight && FrameWidth > BaseFrameWidth && !Image.getSpriteSheetEffect().PlayerOffset)
            {
                Image.setPositionX(Image.getPosition().X - (FrameWidth - BaseFrameWidth));
                Image.getSpriteSheetEffect().PlayerOffset = true;
            }
            else if (Image.getSpriteSheetEffect().PlayerOffset && FrameWidth == BaseFrameWidth)
            {
                Image.setPositionX(Image.getPosition().X + (Image.getSpriteSheetEffect().PrevFrameWidth - BaseFrameWidth));
                Image.getSpriteSheetEffect().PlayerOffset = false;
            }
        }

        /*
            Gravity force applied to Y Velocity
        */
        public void Fall(GameTime gameTime)
        {
            Velocity.Y += Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /*
            Applies an upward Y Velocity
        */
        public void Jump()
        {
            Velocity.Y = -JumpVelocity ;
        }

        /*
            Accelerates the player forward to max speed
            based on the direction they are facing
        */
        public void Accelerate(GameTime gameTime)
        {
            if (facingRight)
                Velocity.X += (AccelerateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            else
                Velocity.X -= (AccelerateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /*
            Deccelerates the player to 0 horizontal speed
            based on the direction they are facing
        */
        public void Deccelerate(GameTime gameTime)
        {
            if (facingRight)
                Velocity.X -= DeccelerateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                Velocity.X += DeccelerateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Decide to stop Decceleration
            if (facingRight)
            {
                // If stopped
                if (Velocity.X <= 0)
                {
                    Velocity.X = 0;
                    Deccelerating = false;
                }
            }
            else // Facing left
            {
                // If stopped
                if (Velocity.X >= 0)
                {
                    Velocity.X = 0;
                    Deccelerating = false;
                }
            }

        }

        /*
          Player takes a hit from hitBox
        */
        public void TakeHit(HitBox hitBox, Player player)
        {
            TakingKnockBack = true;
            // Reset the stun animation everytime player gets hit
            if (Image.getSpriteSheetEffect().CurrentAnimation == Animations["stun"])
                Image.getSpriteSheetEffect().RestartAnimation(Animations["stun"]);
            TakeXKnockBack = true;
            Health += (int)hitBox.Damage;
            KnockBackVelocity.Y = Health * hitBox.KnockBack.Y;

            // Decide if horizontal force is to the left or right
            if (player.facingRight)
                KnockBackVelocity.X = Health * hitBox.KnockBack.X;
            else
                KnockBackVelocity.X = Health * -hitBox.KnockBack.X;

            Velocity.X += KnockBackVelocity.X;
            Velocity.Y -= KnockBackVelocity.Y;

        }

        public void TakeKnockBack(GameTime gameTime)
        {
            if (TakeXKnockBack)
            {
                
                // If player is knocked to the right
                if (KnockBackVelocity.X > 0)
                {
                    // Apply a counter force to horizontal velocitu
                    Velocity.X -= KnockBackAntiVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Velocity.X < 0)
                    {
                        Velocity.X = 0;
                        KnockBackVelocity.X = 0;
                        TakeXKnockBack = false;
                    }
                }
                else if (KnockBackVelocity.X < 0) // If force is to the left
                {
                    // Apply a counter force to horizontal velocity
                    Velocity.X += KnockBackAntiVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Velocity.X > 0)
                    {
                        Velocity.X = 0;
                        KnockBackVelocity.X = 0;
                        TakeXKnockBack = false;
                    }
                }

            }
        }


        /*///////////////////////////////////////*
         * START OF INPUT AND MOVEMENT FUNCTIONS *
        */////////////////////////////////////////                                       
        public void HandleInput(GameTime gameTime)
        {
            // PlayerID 0 is for keyboard
            if (PlayerID == 0)
            {
                if (inputManager.KeyDown(Keys.D))
                {
                    MoveRightInput(gameTime);   
                }
                else if (inputManager.KeyDown(Keys.A))
                {
                    MoveLeftInput(gameTime);
                    
                }
                else
                {
                    NoMovementInput(gameTime);
                }

                if (inputManager.KeyPressed(Keys.Space))
                {
                    JumpInput();
                }

                if (inputManager.KeyPressed(Keys.F))
                {
                    JabInput();
                }

                if ( ((facingRight && inputManager.KeyDown(Keys.D)) || (!facingRight && inputManager.KeyDown(Keys.A))) && inputManager.KeyPressed(Keys.F))
                {
                    FairInput();
                }
            }

            // GamePad Inputs
            else
            { 
                // Move left with stick
                if (inputManager.getLeftAnalog(PlayerID).X < -0.1f)
                {
                    AnalogMovement = inputManager.getLeftAnalog(PlayerID);
                    MoveLeftInput(gameTime);
                }
                // Move right with stick
                else if (inputManager.getLeftAnalog(PlayerID).X > 0.1f)
                {
                    AnalogMovement = inputManager.getLeftAnalog(PlayerID);
                    MoveRightInput(gameTime);
                }
                else
                {
                    AnalogMovement = Vector2.Zero;
                    NoMovementInput(gameTime);
                }

                if (inputManager.ButtonPressed(PlayerID, Buttons.Y))
                {
                    JumpInput();
                }

                if (inputManager.ButtonPressed(PlayerID, Buttons.A))
                {
                    JabInput();
                }

                if (( (facingRight && inputManager.getLeftAnalog(PlayerID).X > 0.1f) ||
                      (!facingRight && inputManager.getLeftAnalog(PlayerID).X < 0.1f)) && 
                       inputManager.ButtonPressed(PlayerID, Buttons.A))
                {
                    FairInput();
                }


            }
        }

        public void NoMovementInput(GameTime gameTime)
        {
            if (!TakingKnockBack)
            {
                // Deccelerate
                if (!inAir)
                {
                    Deccelerating = true;
                }
            }
            Accelerating = false;
        }

        // TODO: Allow player to move in air while doing fair (attacking)
        public void MoveRightInput(GameTime gameTime)
        {
            if (!attacking)
            {
                // Deccelerate if player moving in the opposite direction
                if (!facingRight)
                    Deccelerating = true;

                // Flip character horizontally if not in the air
                if (!inAir)
                    facingRight = true;

                // If not blocked on the right
                if (!blockedHorizontalRight)
                {
                    if (inAir)
                    {
                        Velocity.X = MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds * AnalogMovement.X;
                        // Keyboard
                        if (PlayerID == 0)
                            Velocity.X = MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Accelerating = false;
                    }
                    else
                    {
                        // If done accelerating, set speed to MoveSpeed
                        if ((Velocity.X >= MoveSpeed))
                        {
                            Velocity.X = MoveSpeed;
                            Accelerating = false;
                        }
                        else
                        {
                            Accelerating = true;
                            if (PlayerID != 0)
                                Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * AnalogMovement.X;
                        }
                       
                    }
                }
                else // Blocked on right, inhibit movement
                    Velocity.X = 0;
               
                if (!jumping)
                    Image.getSpriteSheetEffect().SetAnimation(Animations["walk"]);
            }
        }

        public void MoveLeftInput(GameTime gameTime)
        {
            if (!attacking)
            {
                // Deccelerate if player moving in the opposite direction
                if (facingRight)
                    Deccelerating = true;

                // Flip player horizontally if not in the air
                if (!inAir)
                    facingRight = false;

                // Not blocked on the left, so player can move
                if (!blockedHorizontalLeft)
                {
                    if (inAir)
                    {
                        Velocity.X = -MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds * -AnalogMovement.X;
                        // Keyboard
                        if (PlayerID == 0)
                            Velocity.X = -MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Accelerating = false;
                    }
                    else
                    {
                        
                        // If done accelerating, set speed to MoveSpeed
                        if ((Velocity.X <= -MoveSpeed))
                        {
                            Velocity.X = -MoveSpeed;
                            Accelerating = false;
                        }
                        else
                        {
                            Accelerating = true;
                            if (PlayerID != 0)
                                Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * -AnalogMovement.X;
                        }

                    }

                }
                else // Blocked on left, inhibit movement
                {
                    Velocity.X = 0;
                }

                if (!jumping)
                {
                    Image.getSpriteSheetEffect().SetAnimation(Animations["walk"]);
                    
                }
            } 
        }

        public void JumpInput()
        {
            // If player has jumps left and not attacking
            if (jumpsLeft > 0 && !attacking)
            {
                Jump();
                Image.getSpriteSheetEffect().SetAnimation(Animations["jump"]);
                jumping = true;
                // Decrement Jumps
                jumpsLeft--;
            }
        }

        public void JabInput()
        {
            if (!attacking && !inAir)
            {
                attacking = true;
                Accelerating = false;
                Image.getSpriteSheetEffect().SetAnimation(Animations["jab"]);

                // Stop player movment
                if (!inAir)
                    Velocity.X = 0;
            }
            

        }

        public void FairInput()
        {
            if (!attacking && inAir)
            {
                attacking = true;
                Image.getSpriteSheetEffect().SetAnimation(Animations["fair"]);
            }
        }

        /*///////////////////////////////////////*
         * END OF INPUT AND MOVEMENT FUNCTIONS *
        */////////////////////////////////////////

        /*
            Used for debugging purposes
            Draws location of hitbox
        */
        private void DrawRectangle(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            var rect = new Texture2D(screenManager.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

    }

}
