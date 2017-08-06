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
        public static PlayScreen _play = new PlayScreen();
        public static StartScreen _start = new StartScreen();
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
        public static readonly float Gravity = 9.8f; //TODO: How many pixels is a "meter"?
    }

    public abstract class GameScreen
    {
        internal KeyboardState oldKey;
        internal MouseState oldMouse;
        internal GamePadState oldPad;
        internal static KeyMap keyMap = KeyMap.GetInstance();
        internal static ButtonMap btnMap = ButtonMap.GetInstance();

        internal GameScreen()
        {
            this.oldKey = Keyboard.GetState();
            this.oldMouse = Mouse.GetState();
            this.oldPad = GamePad.GetState(PlayerIndex.One);
        }

        public abstract void Update(GameTime gt);
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

        public PlayScreen() : base() {} //Call GameScreen() to set Old State variables.

        public override void Update(GameTime gt)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gt, SpriteBatch sb, SpriteFont font)
        {
            //throw new NotImplementedException();
        }

        public override void KeyPressed(KeyboardState key)
        {
            //throw new NotImplementedException();
        }

        public override void MousePressed(MouseState mouse)
        {
            //throw new NotImplementedException();
        }

        public override void ButtonPressed(GamePadState pad)
        {
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
            foreach (Buttons b in btnMap.CheckButtonsPressed(oldPad, pad)) //Check Buttons Just Pressed.
            {

            }

            foreach (Buttons b in btnMap.CheckButtonsReleased(oldPad, pad)) //Check Buttons Recently Released.
            {
                if (btnMap.GetButtonPressed(b).Contains("Up")) _sel--;
                else if (btnMap.GetButtonPressed(b).Contains("Down")) _sel++;
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
            foreach(Keys k in keyMap.CheckKeysPressed(oldKey, key)) //Check which keys were just pressed.
            {

            }

            foreach(Keys k in keyMap.CheckKeysReleased(oldKey, key)) //Check which keys were just released.
            {
                if (keyMap.GetKeyPressed(k) == "Up") _sel--;
                else if (keyMap.GetKeyPressed(k) == "Down") _sel++;
            }

            oldKey = key;
            //throw new NotImplementedException();
        }

        public override void MousePressed(MouseState mouse)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gt)
        {
            KeyPressed(Keyboard.GetState());
            MousePressed(Mouse.GetState());
            ButtonPressed(GamePad.GetState(PlayerIndex.One));
            //throw new NotImplementedException();
        }
    }
}
