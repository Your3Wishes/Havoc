using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Havoc
{
    public class ImageEffect
    {
        public bool IsActive { get; set; }

        protected Image image;
        protected ScreenManager screenManager;

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
