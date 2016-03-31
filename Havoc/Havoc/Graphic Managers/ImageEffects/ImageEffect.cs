using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Havoc
{
    public class ImageEffect
    {
        protected Image image;
        protected ScreenManager screenManager;
        public bool IsActive;

        public ImageEffect(ScreenManager screenManagerReference)
        {
            screenManager = screenManagerReference;
            IsActive = false;
        }

        public ImageEffect()
        {
        }

        public virtual void LoadContent(ref Image image)
        {
            this.image = image;
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
