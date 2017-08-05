using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snowball
{
    class Player
    {


        //Determine wether or not the player is alive
        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;


        //Determine whether or not the player is on the ground
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        //Keep a count of enemies defeated by the player
        public int KillCounter
        {
            get { return killCounter; }
            set { killCounter = value; }

        }
        int killCounter;

        //Multiplier for player stats as enemies are defeated
        public float StatMulti {
            get { return statMulti; }

        }
        float statMulti;



        private Rectangle localBounds;

        //Create a box which refers to the player
        //The height and width of the box are determined by the player's sprite
        //The hitbox
        public Rectangle BoundBox
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X; //no sprites currently
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;  //no sprites currently

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }



        //Position Vector
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;


        //Velocity vector
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        //Float for user move commands
        private float moveCommand;


        // Initial values for controlling horizontal movement
        private const float initMoveAcceleration = 10000.0f;
        private const float initMaxMoveSpeed = 2000.0f;

        // Initial values for controlling vertical movement
        private const float initMaxJumpTime = 0.5f;
        private const float initJumpVelocity = -3000.0f;
        private const float gravityAcceleration = 3500.0f;
        private const float maxFallSpeed = 500.0f;
        private float airTime; //How long can the player stay airborne after jumping
        private bool isJumping;
        private bool canJump;


        private const Buttons JumpButton = Buttons.A;


        //Get user inputs for movement horizontal movement
        private void GetInput(
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                moveCommand = -1.0f;
            }

            else if (keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                moveCommand = 1.0f;
            }

            //User input for jump attempts
            isJumping =
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);



        }

        public void Update(
    GameTime gameTime,
    KeyboardState keyboardState,
    GamePadState gamePadState)
        {
            GetInput(keyboardState, gamePadState);

            ApplyPhysics(gameTime);

            // Clear input commands.
            moveCommand = 0.0f;
            isJumping = false;
        }


        //Have the player jump
        //Air time refer's to the time a player can maintain a positive vertical velocity from a jump, not the amount of time in the air.
        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((canJump && IsOnGround) || airTime > 0.0f)
                {
                    airTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    sprite.PlayAnimation(/*whatever the name of the jump sprite will be*/);
                }

                //If the player has remaining air time
                if (0.0f < airTime && airTime <= initMaxJumpTime)
                {
                    //Set a curve for vertical velocity
                    velocityY = initJumpVelocity * (1.0f - (float)Math.Pow(airTime / initMaxJumpTime, 0.1f));
                }
                else
                {
                    //Remove remaining air time
                    airTime = 0.0f;
                }
            }
            else
            {
                //Remove remaining air time
                airTime = 0.0f;
            }
            canJump = !isJumping;

            return velocityY;
        }


        /// Apply updates to player position and velocity as neccessary
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            //TODO: allow for variable movement speed
            velocity.X += moveCommand * initMoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + gravityAcceleration * elapsed, -maxFallSpeed, maxFallSpeed);


            velocity.Y = DoJump(velocity.Y, gameTime);


            //Limit player horizontal movespeed
            //TODO: allow for variable movement speed
            velocity.X = MathHelper.Clamp(velocity.X, -initMaxMoveSpeed, initMaxMoveSpeed);

            // Apply velocity.
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));


        }

        //Reset killcount and stat multiplier on player death
        public void OnDeath()
        {
            killCounter = 0;
            statMulti = 1.0f;
            isAlive = false;
        }





    }
}
