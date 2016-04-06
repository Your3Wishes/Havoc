﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Manages menus. Each gameScreen class that
// serves as a menu (e.g. TitleScreen) contains 
// a MenuManager class to handle the menu

namespace Havoc
{
    public class MenuManager
    {
        private ScreenManager screenManager;
        private InputManager inputManager;
        private Menu menu;
        private bool isTransitioning;

        public MenuManager(ScreenManager screenManagerReference, InputManager inputManagerReference)
        {
            screenManager = screenManagerReference;
            inputManager = inputManagerReference;
            menu = new Menu(screenManager, inputManagerReference);
            menu.OnMenuChange += menu_OnMenuChange;
        }


        private void menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnLoadContent();
          
            // Load the new menu
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            menu.OnMenuChange += menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach (MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        /*
            Transitions from one menu to another menu
        */
        private void Transition(GameTime gameTime)
        {
            if (isTransitioning)
            {

                for (int i = 0; i < menu.Items.Count; i++)
                {
                    menu.Items[i].Image.Update(gameTime);
                    float first = menu.Items[0].Image.Alpha;
                    float last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if (first == 0.0f && last == 0.0f)
                        menu.ID = menu.Items[menu.ItemNumber].LinkID;
                    else if (first == 1.0f && last == 1.0f)
                    {
                        isTransitioning = false;
                        foreach (MenuItem item in menu.Items)
                            item.Image.RestoreEffects();
                    }
                }
            }
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != String.Empty)
                menu.ID = menuPath;
        }

        public void UnloadContent()
        {
            menu.UnLoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (!isTransitioning)
                menu.Update(gameTime);
            if (inputManager.KeyPressed(Keys.Enter) && !isTransitioning)
            {
                
                if (menu.Items[menu.ItemNumber].LinkType == "Screen")
                    screenManager.ChangeScreens(
                        menu.Items[menu.ItemNumber].LinkID);
                else
                {
                    isTransitioning = true;
                    menu.Transition(1.0f);
                    foreach(MenuItem item in menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                }
            }
            Transition(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }


    }
}
