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

        public Image Image; // Holds image of player. Include position, effects, etc...
        public Vector2 Velocity; // Current speed of player
        public float MoveSpeed; // Speed the player can move
        public float MoveSpeedInAir;  // Speed the player can move while in air
        public float Gravity;  // Strength of Gravity
        public int GravityCounter;  // For calculating force of gravity
        public float JumpVelocity; // Jump strength
        public int PlayerID; // Identifies who is Player1, Player2, etc...
        public Dictionary<string, Animation> Animations; // Contains the different player animations
        public Animation CurrentAnimation; // The current player animation
        public int NumberOfAnimations;  // Number of different animations for player


        int jumpsLeft; // Number of jumps player has left


        bool facingRight; // True if player is facing to the right
        bool attacking; // True if player is attacking
        bool inAir;
       
        bool jumping;
        bool blockedHorizontalRight;
        bool blockedHorizontalLeft;

        public Player()
        {
            Image = new Image();
            Velocity = Vector2.Zero;
            
            inAir = false;
            Gravity = 45.0f;
            jumping = false;
            blockedHorizontalRight = false;
            blockedHorizontalLeft = false;
            jumpsLeft = 2;
            facingRight = true;
            NumberOfAnimations = 0;

            // Animations
            Animations = new Dictionary<string, Animation>();

            Animations.Add("idle", new Animation());
            Animations.Add("walk", new Animation());
            Animations.Add("jump", new Animation());
            Animations.Add("kick", new Animation());

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
            Image.IsActive = true;

            // Handle Gravity
            // If we are in the air, then fall
            if (inAir)
            {
                Fall(gameTime);
            }
            else 
            {
                Velocity.Y = 0;
            }

            // Handle Input
            HandleInput(gameTime);

            // Handle Jumping
            if (jumping)
            {
                Jump(gameTime);
            }

        
            // Respawn player if off screen
            if (Image.Position.Y >= ScreenManager.Instance.Dimensions.Y)
            {
                Image.Position.Y = 0;
                Image.Position.X = ScreenManager.Instance.Dimensions.X / 2;
                GravityCounter = 0;
            }

            if (Velocity.X == 0 && Velocity.Y == 0 && !attacking)
            {
                Image.SpriteSheetEffect.SetAnimation(Animations["idle"]);
            }

            if (attacking)
            {
                // Check to see if done attacking
                if (!Image.SpriteSheetEffect.Animate)
                {
                    attacking = false;
                    
                }
            }

            Console.WriteLine(attacking);

            if (facingRight)
                Image.SpriteEffect = SpriteEffects.None;
            else
               Image.SpriteEffect = SpriteEffects.FlipHorizontally;

            Image.Update(gameTime);
            Image.Position += Velocity;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
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
                        // Reset gravity counter
                        GravityCounter = 0;
                        // Reset jumps
                        jumping = false;
                        jumpsLeft = 2;

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
                        GravityCounter = 0;
                        jumping = false;
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

        public void Fall(GameTime gameTime)
        {
            GravityCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            Velocity.Y = GravityCounter * (float)(Gravity * 0.001f);
        }

        public void Jump(GameTime gameTime)
        {
            Velocity.Y -= (int)gameTime.ElapsedGameTime.TotalMilliseconds * ((float)JumpVelocity/ 10);
        }


        /*///////////////////////////////////////*
         * START OF INPUT AND MOVEMENT FUNCTIONS *
        */////////////////////////////////////////                                       
        public void HandleInput(GameTime gameTime)
        {
            if (PlayerID == 1)
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
                    Velocity.X = 0;
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
           
            
        }

        public void MoveRightInput(GameTime gameTime)
        {
            if (!attacking)
            {
                
                facingRight = true;

                // If not blocked on the right
                if (!blockedHorizontalRight)
                {
                    if (inAir)
                        Velocity.X = MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
                if (!jumping)
                    Image.SpriteSheetEffect.SetAnimation(Animations["walk"]);
            }
        }

        public void MoveLeftInput(GameTime gameTime)
        {
            if (!attacking)
            {
                facingRight = false;

                if (!blockedHorizontalLeft)
                {
                    if (inAir)
                        Velocity.X = -MoveSpeedInAir * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
                if (!jumping)
                    Image.SpriteSheetEffect.SetAnimation(Animations["walk"]);
            } 
        }

        public void JumpInput()
        {
            // If player has jumps left
            if (jumpsLeft > 0)
            {
                jumping = true;
                Image.SpriteSheetEffect.SetAnimation(Animations["jump"]);
                // Reset Gravity
                GravityCounter = 0;
                // Decrement Jumps
                jumpsLeft--;
            }
        }

        public void JabInput()
        {
            attacking = true;
            Image.SpriteSheetEffect.SetAnimation(Animations["kick"]);
        }
        /*///////////////////////////////////////*
         * END OF INPUT AND MOVEMENT FUNCTIONS *
        */////////////////////////////////////////

    }
}
