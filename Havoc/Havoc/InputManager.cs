using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

// Manages all inputs. 
// This is a singleton class (there is only one
// InputManager class that can be accessed anywhere
// in the program

namespace Havoc
{
    public class InputManager
    {
        private ScreenManager screenManager;
        private KeyboardState currentKeyState, prevKeyState;
        private GamePadState currentGamePad1State, prevGamePad1State, currentGamePad2State, prevGamePad2State;
        private GamePadCapabilities capabilities1, capabilities2;

        public InputManager(ScreenManager screenManagerReference)
        {
            screenManager = screenManagerReference;
        }

        /*
            Updates the currentKeyState and sets the prevKeyState
        */
        public void Update()
        {

            // Check for controllers
            capabilities1 = GamePad.GetCapabilities(PlayerIndex.One);
            capabilities2 = GamePad.GetCapabilities(PlayerIndex.Two);


            prevKeyState = currentKeyState;
            prevGamePad1State = currentGamePad1State;
            prevGamePad2State = currentGamePad2State;

            if (!screenManager.IsTransitioning)
            {
                currentKeyState = Keyboard.GetState();
                if (capabilities1.IsConnected)
                    currentGamePad1State = GamePad.GetState(PlayerIndex.One);
                if (capabilities2.IsConnected)
                    currentGamePad2State = GamePad.GetState(PlayerIndex.Two);
            }
        }

        /*
            Returns true if specific keys were pressed and not held down
        */
        public bool KeyPressed(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;

            }
            return false;
        }

        /*
            Returns true if specific keys were released
        */
        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;

            }
            return false;
        }

        /*
            Returns true if specific keys are held down
        */
        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;

            }
            return false;
        }

        /*
            Returns true if specific buttons were pressed and not held down
            id is the player ID. Refers to a specific GamePad
        */
        public bool ButtonPressed(int id, params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (id == 1)
                    if (currentGamePad1State.IsButtonDown(button) && prevGamePad1State.IsButtonUp(button))
                        return true;
                if (id == 2)
                        if (currentGamePad2State.IsButtonDown(button) && prevGamePad2State.IsButtonUp(button))
                            return true;

            }
            return false;
        }

        /*
            Returns the vector that represents analog input
            id is the player ID. Refers to a specific GamePad
        */
        public Vector2 getLeftAnalog(int id)
        {
            if (id == 1)
                return currentGamePad1State.ThumbSticks.Left;
            if (id == 2)
                return currentGamePad2State.ThumbSticks.Left;
            else return Vector2.Zero;
        }



    }
}
