using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Snowball
{
    public partial class Engine : Game
    {
        public static PlayScreen _play;// = new PlayScreen();
        public static StartScreen _start;// = new StartScreen();
        private Stack<GameScreen> _screens = new Stack<GameScreen>();//Use a Tree instead? If using a Tree, store the entire Hierarchy indefinitely, and keep track of current branch.
        public GameScreen CurrentScreen
        {
            get
            {
                if (_screens.Count > 0)
                    return _screens.Peek();
                else
                {
                    _screens.Push(_start);
                    return _screens.Peek();
                }
            }
            set
            {
                _screens.Push(value);
            }
        }
        public static readonly float Gravity = 1.225f; //TODO: How many pixels is a "meter"?
    }

    public abstract class GameScreen
    {
        internal KeyboardState oldKey;
        internal MouseState oldMouse;
        internal GamePadState oldPad;
        internal static ControlsMap ctrlMap = ControlsMap.GetInstance();

        internal GameScreen()
        {
            this.oldKey = Keyboard.GetState();
            this.oldMouse = Mouse.GetState();
            this.oldPad = GamePad.GetState(PlayerIndex.One);
        }

        public virtual void Update(GameTime gt)
        {
            KeyPressed(Keyboard.GetState());
            MousePressed(Mouse.GetState());
            ButtonPressed(GamePad.GetState(PlayerIndex.One));
        }
        public abstract void Draw(GameTime gt, SpriteBatch sb, SpriteFont font);
        public abstract void KeyPressed(KeyboardState key);
        public abstract void ButtonPressed(GamePadState pad);
        public abstract void MousePressed(MouseState mouse);

    }

    public class PlayScreen : GameScreen
    {
        private bool fallDamageEnabled = false;
        private float terminal = 50.0f; //TODO: How fast do you need to be going to accrue fall damage?
        private int fallDamage = 2; //TODO: How much damage do you take from Fall Damage? Could be multiplied if you fell faster.
        private Vector2 loc;
        private float velocity;
        private bool jumping;

        public PlayScreen() : base() {
            loc = new Vector2(Engine.Width / 2 - 16, Engine.Height / 2 - 16);
        } //Call GameScreen() to set Old State variables.

        public override void Update(GameTime gt)
        {
            //Console.WriteLine(loc);
            base.Update(gt);

            if (jumping) velocity += Engine.Gravity;
            loc.Y += velocity;

            if (loc.X < 0) loc.X = 0;
            else if (loc.X + 32 > Engine.Width) loc.X = Engine.Width-32;
            if (loc.Y < 0) loc.Y = 0;
            else if (loc.Y + 32 > Engine.Height)
            {
                loc.Y = Engine.Height - 32;
                jumping = false;
                velocity = 0f;
            }
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gt, SpriteBatch sb, SpriteFont font)
        {
            if(loc == null) loc = new Vector2(Engine.Width / 2 - 16, Engine.Height / 2 - 16);
            Texture2D rect = new Texture2D(sb.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.DarkRed });
            sb.Draw(rect, new Rectangle((int)loc.X, (int)loc.Y, 32, 32), Color.DarkRed);
            //throw new NotImplementedException();
        }

        public override void KeyPressed(KeyboardState key)
        {
            foreach(Keys k in ctrlMap.CheckKeysPressed(oldKey, key)) //Check keys just pressed.
            {
                List<String> ky = ctrlMap.GetControlPressed(k);
                if(ky.Contains("Jump"))
                {
                    if (!jumping) velocity -= 22.5f;
                    jumping = true;
                }
            }

            foreach(Keys k in ctrlMap.CheckKeysHeld(oldKey, key)) //Check keys being held.
            {
                List<string> ky = ctrlMap.GetControlPressed(k);
                if (ky.Contains("Up")) loc.Y--;
                else if (ky.Contains("Down")) loc.Y++;
                else if (ky.Contains("Left")) loc.X--;
                else if (ky.Contains("Right")) loc.X++;
            }

            foreach(Keys k in ctrlMap.CheckKeysReleased(oldKey, key)) //Check keys just released.
            {
                List<string> ky = ctrlMap.GetControlPressed(k);
                if (ky.Contains("Up")) loc.Y--;
                else if (ky.Contains("Down")) loc.Y++;
                else if (ky.Contains("Left")) loc.X--;
                else if (ky.Contains("Right")) loc.X++;
            }

            oldKey = key;
            //throw new NotImplementedException();
        }

        public override void MousePressed(MouseState mouse)
        {

            oldMouse = mouse;
            //throw new NotImplementedException();
        }

        public override void ButtonPressed(GamePadState pad)
        {
            foreach(Buttons b in ctrlMap.CheckButtonsPressed(oldPad, pad)) //Check buttons just pressed.
            {
                List<string> ky = ctrlMap.GetControlPressed(b);
                if(ky.Contains("Jump"))
                {
                    if (!jumping) velocity -= 22.5f;
                    jumping = true;
                }
            }

            foreach(Buttons b in ctrlMap.CheckButtonsHeld(oldPad, pad)) //Check buttons being held.
            {
                List<string> ky = ctrlMap.GetControlPressed(b);
                if (ky.Contains("Up")) loc.Y--;
                else if (ky.Contains("Down")) loc.Y++;
                else if (ky.Contains("Left")) loc.X--;
                else if (ky.Contains("Right")) loc.X++;
            }

            foreach(Buttons b in ctrlMap.CheckButtonsReleased(oldPad, pad)) //Check buttons just released.
            {
                List<string> ky = ctrlMap.GetControlPressed(b);
                if (ky.Contains("Up")) loc.Y--;
                else if (ky.Contains("Down")) loc.Y++;
                else if (ky.Contains("Left")) loc.X--;
                else if (ky.Contains("Right")) loc.X++;
            }

            oldPad = pad;
            //throw new NotImplementedException();
        }
    }

    public class StartScreen : GameScreen
    {
        private string[] _options = new string[] { "Start", "Load", "Options", "Quit" };
        private int _sel;
        private string Selected
        {
            get
            {
                if (_sel < 0) _sel = _options.Length - 1;
                if (_sel >= _options.Length) _sel = 0;
                return _options[_sel];
            }
        }
        public StartScreen() : base() { }//Set Old States.

        public override void ButtonPressed(GamePadState pad)
        {
            foreach (Buttons b in ctrlMap.CheckButtonsPressed(oldPad, pad)) //Check Buttons Just Pressed.
            {

            }

            foreach (Buttons b in ctrlMap.CheckButtonsReleased(oldPad, pad)) //Check Buttons Recently Released.
            {
                List<string> pressed = ctrlMap.GetControlPressed(b);
                if (pressed.Contains("Up")) _sel--;
                else if (pressed.Contains("Down")) _sel++;
                else if(pressed.Contains("Confirm"))
                {
                    if(Selected == "Start")
                    {
                        Engine.GetInstance().CurrentScreen = Engine._play;
                        //Switch to PlayScreen;
                    }
                    else if(Selected == "Load")
                    {
                        //Switch to DataScreen;
                    }
                    else if(Selected == "Options")
                    {
                        //Switch to OptionsScreen;
                    }
                    else if(Selected == "Quit")
                    {
                        Engine.GetInstance().Exit();
                    }
                }
            }

            oldPad = pad;
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gt, SpriteBatch sb, SpriteFont font)
        {
            int x = Engine.Width / 2;
            int y = Engine.Height * 2 / 3;
            
            foreach (string s in _options)
            {
                Vector2 length = font.MeasureString(s);
                if (s.Equals(Selected)) sb.DrawString(font, s, new Vector2(x - length.X / 2, y + (length.Y * Array.IndexOf(_options, s))), Color.White);
                else sb.DrawString(font, s, new Vector2(x - length.X / 2, y + (length.Y * Array.IndexOf(_options, s))), Color.Black);
            }
            //throw new NotImplementedException();
        }

        public override void KeyPressed(KeyboardState key)
        {
            foreach(Keys k in ctrlMap.CheckKeysPressed(oldKey, key)) //Check which keys were just pressed.
            {

            }

            foreach(Keys k in ctrlMap.CheckKeysReleased(oldKey, key)) //Check which keys were just released.
            {
                List<string> ky = ctrlMap.GetControlPressed(k);
                if (ky.Contains("Up")) _sel--;
                else if (ky.Contains("Down")) _sel++;
                else if (ky.Contains("Confirm"))
                {
                    if (Selected == "Start")
                    {
                        Engine.GetInstance().CurrentScreen = Engine._play;
                        //Switch to PlayScreen;
                    }
                    else if (Selected == "Load")
                    {
                        //Switch to DataScreen;
                    }
                    else if (Selected == "Options")
                    {
                        //Switch to OptionsScreen;
                    }
                    else if (Selected == "Quit")
                    {
                        Engine.GetInstance().Exit();
                    }
                }
            }

            oldKey = key;
            //throw new NotImplementedException();
        }

        public override void MousePressed(MouseState mouse)
        {

            oldMouse = mouse;
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            //throw new NotImplementedException();
        }
    }
}
