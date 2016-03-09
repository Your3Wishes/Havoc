using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Havoc
{
    public class Level
    {
        public List<Platform> platforms;
        public List<Image> backGroundImages;

        public Level()
        {
            platforms = new List<Platform>();
            backGroundImages = new List<Image>();
        }
        

        public virtual void LoadContent()
        {
            
        }

        public void UnLoadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (Image image in backGroundImages)
            {
                image.Update(gameTime);
            }

            foreach (Platform platform in platforms)
            {
                platform.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Image image in backGroundImages)
            {
                image.Draw(spriteBatch);
            }
        }
    }
}
