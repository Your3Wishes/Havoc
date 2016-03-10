
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Havoc
{
    public static class Camera2D
    {
        static public Viewport viewport;

        static public Vector2 Position = Vector2.Zero;
        static public Vector2 DestinationPosition = Vector2.Zero;
        static public float Rotation = 0;
        static public float Zoom = 1.55f;
        static public float NewZoom = 1.55f;
        static public float MaxZoomOut = 1.5f;
        static public float MinZoomIn = 2.0f;
        static public float ZoomFactor = 603.75f;
        static public float ZoomSpeed = 0.00005f;

        static public Vector2 Origin;
        static public float PanSpeed = 0.4f;
        static public float AbsXDistance = 0.0f;
        static public float PanSlope = 0.0f;
        static public bool PanLeft = false;
        static public bool PanRight = false;
        static public bool UndefinedSlope = false;
        

        static public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }


        static public void Update(GameTime gameTime)
        {
            PanCamera(gameTime);
            PanZoom(gameTime);
        }

       /*
            Set the destinationPoint to pan the camera to
       */
       static public void PointCameraTo(Vector2 position)
        {
            
            DestinationPosition = position;
        }


        /*
            Pans the camera from Camera's current position
            to DestinationPosition. Both are Camera2D datamembers.
            Calculate the line between the position and destination position
            and move the current position along that line
        */
        static public void PanCamera(GameTime gameTime)
        {


            // Caculate slope of pan direction
            PanSlope = GetSlope(Position, DestinationPosition);

            // Calculate Distance.X to destination
            // Used for choosing the X distance to increment along the 
            // pan line to pan camera
            int xDistance = (int)DestinationPosition.X - (int)Position.X;
            AbsXDistance = Math.Abs((int)DestinationPosition.X - (int)Position.X);


            // Don't pan if destination is close
            if (GetDistance(Position, DestinationPosition) < 40 )
            {
                PanLeft = false;
                PanRight = false;
                return;
            }

            // If slope is not undefined
            if (PanSlope != 0 && !UndefinedSlope)
            {
                float tempX = Position.X;

                // Decide if panning left or right
                if (xDistance > 0) // Pan right
                {
                    Position.X += PanSpeed * AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100.0f;
                    PanRight = true;
                    PanLeft = false;
                }
                else // Pan left
                {
                    Position.X -= PanSpeed * AbsXDistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100.0f;
                    PanLeft = true;
                    PanRight = false;
                }

                // Calculate new Y position as a function of X
                Position.Y = ((PanSlope * (Position.X - tempX)) + Position.Y);
            }
            else  // Slope either undefined or 0
            {
                // If slope 0, pan camera horizontally only
                if (PanSlope == 0)
                {
                    if (xDistance > 0) // Pan right
                    {
                        Position.X += PanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        PanRight = true;
                    }
                    else // Pan left
                    {
                        Position.X -= PanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        PanLeft = true;
                    }
                }
                else // Undefined slope, pan camera vertically
                {
                    PanLeft = false;
                    PanRight = false;
                    int yDistance = (int)DestinationPosition.Y - (int)Position.Y;
                    if (yDistance > 0) // Pan dow
                        Position.Y += PanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else // Pan up
                        Position.Y -= PanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }
   
        }

        /*
            Pan the Zoom from Camera's current zoom to newZoom
        */
        static public void PanZoom(GameTime gameTime)
        {
            // If NewZoom is different enough to PanZoom
            if (Math.Abs(Zoom - NewZoom) > 0.045f)
            {
                // Decide whether to zoom in or out
                if (Zoom < NewZoom) // Zoom in
                {
                    Zoom += ZoomSpeed * (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
                }
                else if (Zoom > NewZoom) // Zoom out
                {
                    Zoom -= ZoomSpeed * (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
                }
            }
           
        }

        /*
            Returns the slope between two points
        */
        static public float GetSlope(Vector2 sourcePosition, Vector2 destPosition)
        {
            // Undefined slope
            if (destPosition.X - sourcePosition.X == 0)
            {
               UndefinedSlope = true;
               return 0;
            }

            UndefinedSlope = false;
           
            float slope = (destPosition.Y - sourcePosition.Y)/ (destPosition.X - sourcePosition.X);
            return slope;
        }


        /*
            Returns the distance between two points
        */
        static public float GetDistance(Vector2 sourcePosition, Vector2 destPosition)
        {
            return (float)Math.Sqrt(Math.Pow(destPosition.X - sourcePosition.X, 2) + Math.Pow(destPosition.Y - sourcePosition.Y, 2));
        }

        /*
            Handles updating the current zoom, based on players distance
            from each other
        */
        static public void UpdateZoomOnPlayers(Player player1, Player player2)
        {
            float distance = GetDistance(player1.Image.Position, player2.Image.Position);
            float newZoom = ZoomFactor / distance;

            if (newZoom > MaxZoomOut && newZoom < MinZoomIn)
            {
                NewZoom = newZoom;
            }
        }



    }
}
