﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Havoc
{
    public class Player
    {

        public Image Image; // Holds image of player. Include position, effects, etc...
        public Vector2 Velocity; // Current speed of player
        public float MoveSpeed; // Speed the player can move
        public float Gravity; // Strength of Gravity
        public int GravityCounter; // For calculating force of gravity
        public float JumpVelocity; // Jump strength
        public int PlayerID; // Identifies who is Player1, Player2, etc...
        public Dictionary<string, Animation> Animations; // Contains the different player animations
        public Animation CurrentAnimation; // The current player animation
        public Vector2 MaxNumberOfFrames; // Used to determine the maximum amount of frames in row or col


        int jumpsLeft; // Number of jumps player has left


        bool collidedWithGameObject;
        bool inAir;
        bool landing;
        bool jumping;
        bool blockedHorizontalRight;
        bool blockedHorizontalLeft;

        public Player()
        {
            Velocity = Vector2.Zero;
            collidedWithGameObject = false;
            inAir = false;
            landing = false;
            jumping = false;
            blockedHorizontalRight = false;
            blockedHorizontalLeft = false;
            jumpsLeft = 2;
            Animations = new Dictionary<string, Animation>();
            Animations.Add("idle", new Animation());
            Animations.Add("walkRight", new Animation());
            Animations.Add("walkLeft", new Animation());
            CurrentAnimation = Animations["idle"];




        }

        public virtual void LoadContent()
        {
            Image.LoadContent();
            //CollisionRect.LoadContent();
            Image.Position.X = (ScreenManager.Instance.Dimensions.X / 2) - 100;
            Image.SpriteSheetEffect.MaxNumberOfFrames = MaxNumberOfFrames;
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

            // If player is Idle
            if (Velocity.X == 0 && Velocity.Y == 0)
                Image.IsActive = false;

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
                // Assume not falling now
                collidedWithGameObject = true;
   
                

                // If player is above the object, set players Y position 
                // so that the player lands exactly on top of platform
                // If we don't do this, player may glitch and land inside 
                // platform sometimes
                if ((Image.Position.Y + Image.SourceRect.Height) - 35 < gameObject.Image.Position.Y && Velocity.Y > 0)
                {

                    // Landed on top of platform
                    if ((Image.Position.X + Image.SourceRect.Width) > gameObject.Image.Position.X && 
                        Image.Position.X < gameObject.Image.Position.X + gameObject.Image.SourceRect.Width)
                    {
                        Image.Position.Y = gameObject.Image.Position.Y - (Image.SourceRect.Height - 1);
                        Velocity.Y = 0;
                        inAir = false;  // Player isn't jumping
                        jumping = false;
                        jumpsLeft = 2;
                    }

                    // Reset gravity counter
                    GravityCounter = 0;
 
                    
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
                
                
            }
            else
            {
                // Did not collide with object
                collidedWithGameObject = false;
                inAir = true;
                blockedHorizontalRight = false;
                blockedHorizontalLeft = false;
            }

        }


        public void CollideWithGameObject()
        {
           

        }

        public void Fall(GameTime gameTime)
        {
            GravityCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            Velocity.Y = GravityCounter * (float)(Gravity / 1000.0);
        }

        public void Jump(GameTime gameTime)
        {
            Velocity.Y -= (int)gameTime.ElapsedGameTime.TotalMilliseconds * ((float)JumpVelocity/ 10);
        }

       



        public void HandleInput(GameTime gameTime)
        {
            if (PlayerID == 1)
            {
                if (InputManager.Instance.KeyDown(Keys.D))
                {
                    if (!blockedHorizontalRight)
                        Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                }
                else if (InputManager.Instance.KeyDown(Keys.A))
                {
                    if (!blockedHorizontalLeft)
                        Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                }
                else
                    Velocity.X = 0;


                if (InputManager.Instance.KeyPressed(Keys.Space))
                {

                    // If player has jumps left
                    if (jumpsLeft > 0)
                    {
                        jumping = true;
                        // Reset Gravity
                        GravityCounter = 0;
                        // Decrement Jumps
                        jumpsLeft--;
                    }
                }

                if (InputManager.Instance.KeyDown(Keys.Z))
                {
                    Camera2D.Zoom += 0.01f;
                }
                if (InputManager.Instance.KeyDown(Keys.X))
                {
                    Camera2D.Zoom -= 0.01f;
                }
            }
           
            
        }





        
    }
}
