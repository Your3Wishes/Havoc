using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Xml.Serialization;

// Represents an Image to be rendered. Contains
// attributes and effects that the image can possess

namespace Havoc
{
    public class Image
    {
        protected ScreenManager screenManager;
        public float Alpha { get; set; }
        private string Text, FontName;
        public string Path { get; set; }
        public Vector2 Scale { get; set; }
        public Rectangle SourceRect { get; set; }
        public bool IsActive { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public string Effects { get; set; }
        public Texture2D Texture { get; set; }
        
        private Vector2 Position;
        private Vector2 Origin;
        private ContentManager content;
        private RenderTarget2D renderTarget;
        private SpriteFont font;
        private Dictionary<string, ImageEffect> effectList;
        private FadeEffect FadeEffect;
        private SpriteSheetEffect SpriteSheetEffect;
        private BackgroundPanEffect BackgroundPanEffect;
        private PanLoopEffect PanLoopEffect;

        /*
            Adds an effect to image. 'T effect' assumed to be 
            any derived class of ImageEffect type
        */
        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T), screenManager);
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            // Add the effect into image's effectList
            effectList.Add(effect.GetType().ToString().Replace("Havoc.", ""), (effect as ImageEffect));
        }

        /*
            Activates a particular effect on an image
            e.g. activate a fade effect
        */
        public void ActivateEffect(string effect)
        {
            if(effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        /*
            Deactivates a particular effect on an image
        */
        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach( var effect in effectList)
            {
                if (effect.Value.IsActive)
                    Effects += effect.Key + ":";
            }

            if (Effects != String.Empty)
                Effects.Remove(Effects.Length - 1);
        }

        public void RestoreEffects()
        {
            foreach ( var effect in effectList)
                DeactivateEffect(effect.Key);

            string[] split = Effects.Split(':');
            foreach (string s in split)
                ActivateEffect(s);

        }

        public Image(ScreenManager screenManagerReference)
        {
            screenManager = screenManagerReference;
            Path = Effects = Text = String.Empty;
            FontName = "Fonts/Orbitron";
            Position = Vector2.Zero;
            Scale  = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
            IsActive = true;
            SpriteEffect = SpriteEffects.None;
        }

        /*
            Load all content needed for image
        */
        public void LoadContent()
        {
            content = new ContentManager(
                screenManager.Content.ServiceProvider, "Content");

            // Load texture
            if (Path != String.Empty)
                Texture = content.Load<Texture2D>(Path);

            // Load font
            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            // Set X dimension
            if (Texture != null)
                dimensions.X += Texture.Width;
            dimensions.X += font.MeasureString(Text).X;

            // Set Y dimension
            if (Texture != null)
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            else
                dimensions.Y = font.MeasureString(Text).Y;

            // Set SourceRectangle with dimension X/Y width/height
            if (SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);


            renderTarget = new RenderTarget2D(screenManager.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            screenManager.GraphicsDevice.SetRenderTarget(renderTarget);
            screenManager.GraphicsDevice.Clear(Color.Transparent);
            screenManager.SpriteBatch.Begin();
            if (Texture != null)
                screenManager.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            screenManager.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            screenManager.SpriteBatch.End();

            Texture = renderTarget;

            screenManager.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);
            SetEffect<BackgroundPanEffect>(ref BackgroundPanEffect);
            SetEffect<PanLoopEffect>(ref PanLoopEffect);

            // Activate effects in eff
            if (Effects != String.Empty)
            {
                string[] split = Effects.Split(':');
                foreach(string item in split)
                {
                    ActivateEffect(item);
                }
            }



        }

        /*
            Unload and deactivate all effects on Image
        */
        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
                
        }

        /*
            Update Image
        */
        public void Update(GameTime gameTime)
        {

            // Apply each effect on image
            foreach (var effect in effectList)
            {
                if (effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
               
        }

        /*
            Draw the image to the screen
        */
        public void Draw(SpriteBatch spriteBatch)
        {
            Origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            spriteBatch.Draw(Texture, Position + Origin, SourceRect, Color.White * Alpha, 0.0f, Origin, Scale, SpriteEffect, 0.0f);
        }

        // Public Properties
        public Vector2 getPosition()
        {
            return Position;
        }

        public void setPositionY(float y)
        {
            Position.Y = y;
        }

        public void setPositionX(float x)
        {
            Position.X = x;
        }

        public void setPosition(Vector2 Position)
        {
            this.Position = Position;
        }

        public FadeEffect getFadeEffect()
        {
            return FadeEffect;
        }

        public SpriteSheetEffect getSpriteSheetEffect()
        {
            return SpriteSheetEffect;
        }

        public BackgroundPanEffect getBackgroundPanEffect()
        {
            return BackgroundPanEffect;
        }

        public PanLoopEffect getPanLoopEffect()
        {
            return PanLoopEffect;
        }


    }
}
