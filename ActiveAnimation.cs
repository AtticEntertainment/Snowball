using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Snowball
{
    struct ActiveAnimation
    {
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        // Current frame
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        // Time passed on each frame
        private float time;

        // The origin of a texture will be at the bottom-center of each frame
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        // Plays an animation
        public void PlayAnimation(Animation animation)
        {
            // Continues this animation if it is already playing
            if (Animation == animation)
                return;

            // Starts a new animation
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        // Advances the time and draws the current frame
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation");

            // Move to the next frame when time passed is geater than the duration of a frame
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameDuration)
            {
                time -= Animation.FrameDuration;

                // Handle repeating or non repeating
                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame
            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

            // Draw the current frame
            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);

        }
    }

}
