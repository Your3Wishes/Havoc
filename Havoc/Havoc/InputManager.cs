using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

// Manages all inputs. 
// This is a singleton class (there is only one
// InputManager class that can be accessed anywhere
// in the program

namespace Havoc
{
    public class InputManager
    {
        KeyboardState currentKeyState, prevKeyState;

        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();
                return instance;
            }
        }

        /*
            Updates the currentKeyState and sets the prevKeyState
        */
        public void Update()
        {
            prevKeyState = currentKeyState;
            if (!ScreenManager.Instance.IsTransitioning)
                currentKeyState = Keyboard.GetState();
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




    }
}
