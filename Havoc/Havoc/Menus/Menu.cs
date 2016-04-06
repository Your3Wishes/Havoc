using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Represents a game menu. Contains a list of menu items,
// menu effects, an axis of alignment, ect...
// Contains an EventHandler to handle menu changes on input

namespace Havoc
{
    public class Menu
    {
        [XmlElement("Item")]
        public List<MenuItem> Items { get; set; }
        public event EventHandler OnMenuChange;
        public int ItemNumber { get; set; }

        private ScreenManager screenManager;
        private InputManager inputManager;
        private string Axis;
        private string Effects;
        private string id;
        
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        public Menu(ScreenManager screenManagerReference, InputManager inputManagerReference)
        {
            screenManager = screenManagerReference;
            inputManager = inputManagerReference;
            id = String.Empty;
            ItemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void Transition(float alpha)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                    item.Image.getFadeEffect().Increase = true;
                else
                    item.Image.getFadeEffect().Increase = false;
            }
        }

        /*
            Align menu items either horizontally or vertically, centered with screen
        */
        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            foreach(MenuItem item in Items)
            {
                dimensions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);
            }

            dimensions = new Vector2((screenManager.Dimensions.X -
                dimensions.X) / 2, (screenManager.Dimensions.Y - dimensions.Y) / 2);

            foreach(MenuItem item in Items)
            {
                if (Axis == "X")
                    item.Image.setPosition(new Vector2(dimensions.X,
                        (screenManager.Dimensions.Y - item.Image.SourceRect.Height) / 2));
                else if (Axis == "Y")
                    item.Image.setPosition(new Vector2((screenManager.Dimensions.X -
                        item.Image.SourceRect.Width) / 2, dimensions.Y));

                dimensions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);

            }
        }

        /*
            Load menu content and activate all effects
            on menu images. Call AlignMenuItems
        */
        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach(MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach (string s in split)
                    item.Image.ActivateEffect(s);
            }
            AlignMenuItems();
        }

        public void UnLoadContent()
        {
            foreach (MenuItem item in Items)
                item.Image.UnloadContent();
        }

        /*
            Menu logic. Select elements in the menu.
            Selected item is represented by itemNumber.
            menuItem[itemNumber] becomes active (e.g fades in and out etc.)
        */
        public void Update(GameTime gameTime)
        {
            // Horizontal Menu
            if (Axis == "X")
            {
                if (inputManager.KeyPressed(Keys.Right))
                    ItemNumber++;
                else if (inputManager.KeyPressed(Keys.Left))
                    ItemNumber--;
            }
            // Vertical Menu
            else if (Axis == "Y")
            {
                if (inputManager.KeyPressed(Keys.Down))
                    ItemNumber++;
                else if (inputManager.KeyPressed(Keys.Up))
                    ItemNumber--;
            }

            // Make sure itemNumber doesn't exceed items in menu
            if (ItemNumber < 0)
                ItemNumber = 0;
            else if (ItemNumber > Items.Count - 1)
                ItemNumber = Items.Count - 1;
    
            // Activate selected item in menu, deactivate others
            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                    Items[i].Image.IsActive = true;
                else
                    Items[i].Image.IsActive = false;

                Items[i].Image.Update(gameTime);

            }
        }

        /*
            Draw the menu to the scree
        */
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
                item.Image.Draw(spriteBatch);
            
        }



    }
}
