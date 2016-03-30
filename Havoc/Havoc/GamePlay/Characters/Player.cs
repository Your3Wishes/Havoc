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
        /*///////////////////////////////////////*
         * START OF DATAMEMBERS *
        */////////////////////////////////////////   
        public bool DEBUG_HIT_BOX = false; // USED FOR DEBUGGING HITBOXES
        public int PlayerID; // Identifies who is Player1, Player2, etc...
        public Image Image; // Holds image of player. Include position, effects, etc...
        public Vector2 Velocity; // Current speed of player
        public float MoveSpeed; // Max Speed the player can move
        public bool Accelerating; // If accelerating movement
        public float AccelerateSpeed; // Speed the player accelerates (movement)
        public bool Deccelerating; // If deccelerating movement
        public float DeccelerateSpeed; // Speed the player deccelerates (movement)
        public float MoveSpeedInAir;  // Speed the player can move while in air
        public float Gravity;  // Strength of Gravity
        public float JumpVelocity; // Jump strength
        public Vector2 AnalogMovement; // The 2d strength of the analog stick
        int jumpsLeft; // Number of jumps player has left

        public Dictionary<string, Animation> Animations; // Contains the different player animations
        public Animation CurrentAnimation; // The current player animation
        public int NumberOfAnimations;  // Number of different animations for player
        public int Outfit; // The outfit the player uses, (skin)

        public HitBox HitBox; // The hitbox of the player
        public bool TakingKnockBack; // If taking a knockback force
        public Vector2 KnockBackVelocity; // KnockBack strength
        public float KnockBackAntiVelocity;  // Counteracts horizonal knockback forces
        public int KnockBackCounter;  // For calculating knockback anti horizonal force
        public int Health; // The higher the number, the harder hits player takes

        public bool HitStun; // The player is in hitstun (can't move)
        public bool CanBeComboed; // False right after being hit
        public float ComboCounter; // Used to recharge CanBeComboed
        public float ComboMaxTimer; // When ComboCounter reaches this number, player can be comboed again
        public bool TakeXKnockBack; // True if taking horizontal knockback
  

        bool facingRight; // True if player is facing to the right
        bool attacking; // True if player is attacking
        bool inAir;
       
        bool jumping;
        bool blockedHorizontalRight;
        bool blockedHorizontalLeft;
        /*///////////////////////////////////////*
         * END OF DATAMEMBERS *
        */////////////////////////////////////////   

        public Player()
        {
            Image = new Image();
            Velocity = Vector2.Zero;

            Outfit = 1;
            
            inAir = false;
            Gravity = 37.0f;
            KnockBackAntiVelocity = 0.68f;
            jumping = false;
            blockedHorizontalRight = false;
            blockedHorizontalLeft = false;
            jumpsLeft = 2;
            facingRight = true;
            NumberOfAnimations = 0;

            HitBox = new HitBox();
            HitStun = false;
            CanBeComboed = true;
            ComboCounter = 0;
            ComboMaxTimer = 600.0f;
            Health = 0;
            TakingKnockBack = false;
            KnockBackVelocity = Vector2.Zero;

            // Animations
            Animations = new Dictionary<string, Animation>();

            Animations.Add("idle", new Animation());
            Animations.Add("walk", new Animation());
            Animations.Add("jump", new Animation());
            Animations.Add("kick", new Animation());
            Animations.Add("stun", new Animation());

        }

        public virtual void LoadContent()
        {
            Image.LoadContent();
            Image.Position.X = (ScreenManager.Instance.Dimensions.X / 2) - 100; // Set initial player position
            Image.SpriteSheetEffect.NumberOfAnimations = NumberOfAnimations; // Tell SpriteSheet how many different animations
            Image.SpriteSheetEffect.SetAnimation(Animations["idle"]); // Set animation to idle
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

            // Handle Input
            HandleInput(gameTime);

            HandleLogic(gameTime);

            Image.Update(gameTime);
            Image.Position += Velocity;
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
            Rectangle playerRect = new Rectangle((int)Image.Position.X, (int)Image.Position.Y, Image.SourceRect.Width, Image.SourceRect.Height);
            // Create new gameObject collision box
            Rectangle gameObjectRect = new Rectangle((int)gameObject.Image.Position.X, (int)gameObject.Image.Position.Y, gameObject.Image.SourceRect.Width, gameObject.Image.SourceRect.Height);

            // Check if collision occured
            if (playerRect.Intersects(gameObjectRect))
            {
                // Collision Occured
                
                // If player is above the object, set players Y position 
                // so that the player lands exactly on top of platform
                // If we don't do this, player may glitch and land inside 
                // platform sometimes
                if ((Image.Position.Y + Image.SourceRect.Height) - 35 < gameObject.Image.Position.Y && Velocity.Y > 0)
                {

                    // Landed on top of platform
                    if ((Image.Position.X + Image.SourceRect.Width) - 10 > gameObject.Image.Position.X && 
                        Image.Position.X + 10 < gameObject.Image.Position.X + gameObject.Image.SourceRect.Width)
                    {
                        Image.Position.Y = gameObject.Image.Position.Y - (Image.SourceRect.Height - 1);
                        Velocity.Y = 0;
                        inAir = false;  // Player isn't in the air
                        // Reset jumps
                        jumping = false;
                        jumpsLeft = 2;
                        TakingKnockBack = false;
                        KnockBackCounter = 0;
                        Velocity.X = 0;
                        TakeXKnockBack = false;

                    }
                    else // Besides the platform
                    {
                        inAir = true;
                    }
                }

                // Collided with the left side of the object. Blocked on right
                if ((Image.Position.X + Image.SourceRect.Width) - 10 < gameObject.Image.Position.X)
                {
                    Velocity.X = 0;
                    blockedHorizontalRight = true;
                    blockedHorizontalLeft = false;
                }
                // Collided with the right size of the object. Block on left
                else if  (Image.Position.X + 10 > gameObject.Image.Position.X + gameObject.Image.SourceRect.Width)
                {
                    Velocity.X = 0;
                    blockedHorizontalRight = false;
                    blockedHorizontalLeft = true;
                }

                // Collided with the bottom of the object
                if (Image.Position.Y + 20 > gameObject.Image.Position.Y + gameObject.Image.SourceRect.Height)
                {
                    // If in the middle of platform
                    if ((Image.Position.X + Image.SourceRect.Width) - 10 > gameObject.Image.Position.X &&
                        Image.Position.X + 10 < gameObject.Image.Position.X + gameObject.Image.SourceRect.Width)
                    {
                        Velocity.Y = 0;
                        jumping = false;
                        KnockBackCounter = 0;
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
            Rectangle playerRect = new Rectangle((int)Image.Position.X, (int)Image.Position.Y, Image.SourceRect.Width, Image.SourceRect.Height);

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
                Image.SpriteSheetEffect.SetAnimation(Animations["stun"]);
            }

            // Respawn player if off screen
            if (Image.Position.Y >= ScreenManager.Instance.Dimensions.Y)
            {
                Image.Position.Y = 0;
                Image.Position.X = ScreenManager.Instance.Dimensions.X / 2;
                Velocity = Vector2.Zero;
                KnockBackCounter = 0;
                Health = 0;
            }

            // If player is idle, set animation to idle
            if (Velocity.X == 0 && Velocity.Y == 0 && !attacking)
            {
                Image.SpriteSheetEffect.SetAnimation(Animations["idle"]);
            }

            if (attacking)
            {
                // Set the player's hitbox to the correct spritesheet frame's hitbox
                try
                {
                    HitBox.Rectangle = Image.SpriteSheetEffect.CurrentAnimation.HitBoxes[(int)Image.SpriteSheetEffect.CurrentFrame.X];
                    HitBox.Damage = Image.SpriteSheetEffect.CurrentAnimation.Damage;
                    HitBox.KnockBack = Image.SpriteSheetEffect.CurrentAnimation.KnockBack;
                }
                catch (IndexOutOfRangeException e)
                {
                    HitBox.Rectangle = new Rectangle();
                }

                // Check to see if done attacking
                if (!Image.SpriteSheetEffect.Animate)
                {
                    attacking = false;
                }
            }

            // Handle player direction
            if (facingRight)
            {
                Image.SpriteEffect = SpriteEffects.None;
                // Position hitboxes relative to player's source rectangle
                HitBox.Rectangle.X += (int)Image.Position.X;
                HitBox.Rectangle.Y += (int)Image.Position.Y;
            }
            else // Facing left
            {
                Image.SpriteEffect = SpriteEffects.FlipHorizontally;
                // Position hitboxes relative to player's source rectangle
                HitBox.Rectangle.X = ((int)Image.Position.X + Image.SourceRect.Width) - HitBox.Rectangle.X - HitBox.Rectangle.Width;
                HitBox.Rectangle.Y += (int)Image.Position.Y;
            }

            // Handle logic for CanBeComboed
            if (!CanBeComboed)
            {
                Image.SpriteSheetEffect.SetAnimation(Animations["stun"]);
                ComboCounter += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (ComboCounter >= ComboMaxTimer) CanBeComboed = true;
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
            if (Image.SpriteSheetEffect.CurrentAnimation == Animations["stun"])
                Image.SpriteSheetEffect.CurrentFrame.X = Image.SpriteSheetEffect.CurrentAnimation.StartFrame.X;
            TakeXKnockBack = true;
            Health += (int)hitBox.Damage;
            KnockBackCounter = 0;
            KnockBackVelocity.Y = Health * hitBox.KnockBack.Y;


            // Decide if horizontal force is to the left or right
            if (Image.Position.X < player.Image.Position.X)
                KnockBackVelocity.X = Health * -hitBox.KnockBack.X;
            else
                KnockBackVelocity.X = Health * hitBox.KnockBack.X;

            Velocity.X += KnockBackVelocity.X;
            Velocity.Y -= KnockBackVelocity.Y;

        }

        public void TakeKnockBack(GameTime gameTime)
        {
            //Velocity.Y -= (int)gameTime.ElapsedGameTime.TotalMilliseconds * ((float)KnockBackVelocity.Y * 0.1f);
            
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
                        KnockBackCounter = 0;
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
                        KnockBackCounter = 0;
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
            // PlayerID 1 is for keyboard
            if (PlayerID == 0)
            {
                if (InputManager.Instance.KeyDown(Keys.D))
                {
                    MoveRightInput(gameTime);   
                }
                else if (InputManager.Instance.KeyDown(Keys.A))
                {
                    MoveLeftInput(gameTime);
                    
                }
                
                else
                {
                    NoMovementInput(gameTime);
                }

                if (InputManager.Instance.KeyPressed(Keys.Space))
                {
                    JumpInput();

                }

                if (InputManager.Instance.KeyPressed(Keys.F))
                {
                    JabInput();
                }
            }

            // GamePad Inputs
            else 
            {
                // Move left with stick
                if (InputManager.Instance.getLeftAnalog(PlayerID).X < -0.1f)
                {
                    AnalogMovement = InputManager.Instance.getLeftAnalog(PlayerID);
                    MoveLeftInput(gameTime);
                }
                // Move right with stick
                else if (InputManager.Instance.getLeftAnalog(PlayerID).X > 0.1f)
                {
                    AnalogMovement = InputManager.Instance.getLeftAnalog(PlayerID);
                    MoveRightInput(gameTime);
                }
                else
                {
                    AnalogMovement = Vector2.Zero;
                    NoMovementInput(gameTime);
                }

                if (InputManager.Instance.ButtonPressed(PlayerID, Buttons.Y))
                {
                    JumpInput();
                }

                if (InputManager.Instance.ButtonPressed(PlayerID, Buttons.A))
                {
                    JabInput();
                }


            }
        }

        public void NoMovementInput(GameTime gameTime)
        {
            if (!TakingKnockBack)
            {
                //Velocity.X = 0;
                // Deccelerate
                if (!inAir)
                {
                    Deccelerating = true;
                    
                }
            }
            Accelerating = false;
        }

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
                    Image.SpriteSheetEffect.SetAnimation(Animations["walk"]);
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
                    Image.SpriteSheetEffect.SetAnimation(Animations["walk"]);
                    
                }
            } 
        }

        public void JumpInput()
        {
            // If player has jumps left and not attacking
            if (jumpsLeft > 0 && !attacking)
            {
                Jump();
                Image.SpriteSheetEffect.SetAnimation(Animations["jump"]);
                jumping = true;
                // Decrement Jumps
                jumpsLeft--;
            }
        }

        public void JabInput()
        {
            
            attacking = true;
            Accelerating = false;
            Image.SpriteSheetEffect.SetAnimation(Animations["kick"]);

            // Stop player movment
            if (!inAir)
                Velocity.X = 0;

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
            var rect = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

    }

}
