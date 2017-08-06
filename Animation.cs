using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Snowball
{
    class Animation
    {
        //Duration of each frame
        public float FrameDuration
        {
            get { return frameDuration; }
        }
        float frameDuration;

        //Texture containing all frames for the animation
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        // Determines whether or not the animation repeats
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        // Gets the number of frames in the animation.
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        // Gets the width of a frame in the animation.
        public int FrameWidth
        {
            get { return Texture.Height; }
        }
        //For now, all animations will be arranged horizontally in perfect square frames.


        // Gets the height of a frame in the animation.
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        //Constructor
        public Animation(Texture2D texture, float frameDuration, bool isLooping)
        {
            this.texture = texture;
            this.frameDuration = frameDuration;
            this.isLooping = isLooping;
        }

    }
}
