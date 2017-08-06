using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Snowball
{
    /// <summary>
    /// Button Mapping for Controllers.
    /// </summary>
    public class ButtonMap
    {
        //TODO: Reconcile ButtonMapping and KeyMapping into one?
        private static ButtonMap _btnMap;
        private static Dictionary<string, List<Buttons>> mapping;
        private static Dictionary<Buttons, List<string>> reverseMap;

        private ButtonMap()
        {
            if (mapping == null)
                mapping = new Dictionary<string, List<Buttons>>();
            mapping["Up"] = new List<Buttons> { Buttons.DPadUp, Buttons.LeftThumbstickUp };
            mapping["Down"] = new List<Buttons> { Buttons.DPadDown, Buttons.LeftThumbstickDown };
            mapping["Left"] = new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft };
            mapping["Right"] = new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight };
            mapping["Confirm"] = new List<Buttons> { Buttons.A };
            mapping["Back"] = new List<Buttons> { Buttons.B };
            CreateReverseMap();
        }

        public static ButtonMap GetInstance()
        {
            if (_btnMap == null)
                _btnMap = new ButtonMap();
            return _btnMap;
        }

        /// <summary>
        /// This should be called any time the mapping is changed.
        /// </summary>
        private void CreateReverseMap()
        {
            reverseMap = new Dictionary<Buttons, List<string>>();
            foreach (KeyValuePair<string, List<Buttons>> btn in mapping)
            {
                foreach (Buttons b in btn.Value)
                {
                    if(reverseMap.ContainsKey(b))
                    {
                        reverseMap[b].Add(btn.Key);
                    }
                    else
                    {
                        reverseMap[b] = new List<string> { btn.Key };
                    }
                }
            }
        }

        /// <summary>
        /// Used for creating the Mapping from the Reverse Mapping.
        /// </summary>
        private void CreateMap()
        {
            if (reverseMap == null || reverseMap.Count < 1)
                return;
            mapping = new Dictionary<string, List<Buttons>>();
            foreach (KeyValuePair<Buttons, List<string>> btn in reverseMap)
            {
                foreach (string b in btn.Value)
                {
                    if (mapping.ContainsKey(b))
                        mapping[b].Add(btn.Key);
                    else
                        mapping[b] = new List<Buttons> { btn.Key };
                }
            }
        }

        /// <summary>
        /// Check which buttons were just pressed.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsPressed(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach(Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonDown(btn) && oldState.IsButtonUp(btn))
                    ret.Add(btn);
            }

            return ret;
        }

        /// <summary>
        /// Check which buttons were just released.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsReleased(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach (Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonUp(btn) && oldState.IsButtonDown(btn))
                    ret.Add(btn);
            }

            return ret;
        }

        /// <summary>
        /// Check which buttons are being held.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsHeld(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach(Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonDown(btn) && oldState.IsButtonDown(btn)) ret.Add(btn);
            }
            return ret;
        }


        /// <summary>
        /// Called after you check if a button has been pressed. Gets a list of actions associated with the button that was pressed.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public List<string> GetButtonPressed(Buttons btn)
        {
            reverseMap.TryGetValue(btn, out List<string> ret);
            return ret;
        }
    }

    public class KeyMap
    {
        private static KeyMap _keyMap;
        private static Dictionary<string, List<Keys>> mapping; //Map multiple keys to an Action.
        private static Dictionary<Keys, string> reverseMap; //Lookup what "action" each key is Mapped to.
        private KeyMap()
        {
            if (mapping == null)
                mapping = new Dictionary<string, List<Keys>>();
            mapping["Up"] = new List<Keys> { Keys.W, Keys.Up };
            mapping["Down"] = new List<Keys> { Keys.S, Keys.Down };
            mapping["Left"] = new List<Keys> { Keys.A, Keys.Left };
            mapping["Right"] = new List<Keys> { Keys.D, Keys.Right };
            mapping["Confirm"] = new List<Keys> { Keys.Enter };
            mapping["Back"] = new List<Keys> { Keys.Escape };
            CreateReverseMap();
        }
        public static KeyMap GetInstance()
        {
            if (_keyMap == null)
                _keyMap = new KeyMap();
            return _keyMap;
        }

        public void SetMapping(Dictionary<string, Keys[]> newMap)
        {
            //this.mapping = newMap;
            //createReverseMap();
        }

        /// <summary>
        /// This should be called any time the mapping is changed.
        /// </summary>
        private void CreateReverseMap()
        {
            reverseMap = new Dictionary<Keys, string>();
            foreach (KeyValuePair<string, List<Keys>> key in mapping)
            {
                foreach (Keys k in key.Value)
                {
                    reverseMap[k] = key.Key;
                }
            }
        }

        /// <summary>
        /// Used for creating the Mapping from the Reverse Mapping.
        /// </summary>
        private void CreateMap()
        {
            if (reverseMap == null || reverseMap.Count < 1)
                return;
            mapping = new Dictionary<string, List<Keys>>();
            foreach (KeyValuePair<Keys, string> key in reverseMap)
            {
                if (mapping.ContainsKey(key.Value))
                    mapping[key.Value].Add(key.Key);
                else
                    mapping[key.Value] = new List<Keys> { key.Key };
            }
        }

        /// <summary>
        /// Check which keys were just pressed.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysPressed(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach (Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyDown(key) && oldState.IsKeyUp(key))
                    ret.Add(key);
            }

            return ret;
        }

        /// <summary>
        /// Check which keys were just released.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysReleased(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach (Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyUp(key) && oldState.IsKeyDown(key))
                    ret.Add(key);
            }

            return ret;
        }

        /// <summary>
        /// Check which keys are being held.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysHeld(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach(Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyDown(key) && oldState.IsKeyDown(key)) ret.Add(key);
            }
            return ret;
        }

        /// <summary>
        /// Called after you check if a key has been pressed. Gets related action for further logic.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKeyPressed(Keys key)
        {
            reverseMap.TryGetValue(key, out string ret);
            return ret;
        }
    }

    public class ControlsMap
    {
        private static ControlsMap _ctrl;
        private Dictionary<string, List<Enum>> mapping;
        private Dictionary<Enum, List<string>> reverseMap;

        private ControlsMap()
        {
            mapping = new Dictionary<string, List<Enum>>();
            mapping["Up"] = new List<Enum> { Keys.Up, Keys.W, Buttons.DPadUp, Buttons.LeftThumbstickUp };
            mapping["Down"] = new List<Enum> { Keys.Down, Keys.S, Buttons.DPadDown, Buttons.LeftThumbstickDown };
            mapping["Left"] = new List<Enum> { Keys.Left, Keys.A, Buttons.DPadLeft, Buttons.LeftThumbstickLeft };
            mapping["Right"] = new List<Enum> { Keys.Right, Keys.D, Buttons.DPadRight, Buttons.LeftThumbstickRight };
            mapping["Confirm"] = new List<Enum> { Keys.Enter, Buttons.A };
            mapping["Cancel"] = new List<Enum> { Keys.Escape, Buttons.B };
            CreateReverseMap();
        }

        public static ControlsMap GetInstance()
        {
            if (_ctrl == null) _ctrl = new ControlsMap();
            return _ctrl;
        }

        /// <summary>
        /// Used for mapping actions to controls.
        /// </summary>
        private void CreateReverseMap()
        {
            reverseMap = new Dictionary<Enum, List<string>>();
            foreach (KeyValuePair<string, List<Enum>> ctrl in mapping)
            {
                foreach (Enum c in ctrl.Value)
                {
                    if (reverseMap.ContainsKey(c))
                    {
                        reverseMap[c].Add(ctrl.Key);
                    }
                    else
                    {
                        reverseMap[c] = new List<string> { ctrl.Key };
                    }
                }
            }
        }

        /// <summary>
        /// Used for creating the Mapping from the Reverse Mapping.
        /// </summary>
        private void CreateMap()
        {
            if (reverseMap == null || reverseMap.Count < 1)
                return;
            mapping = new Dictionary<string, List<Enum>>();
            foreach (KeyValuePair<Enum, List<string>> ctrl in reverseMap)
            {
                foreach (string c in ctrl.Value)
                {
                    if (mapping.ContainsKey(c))
                        mapping[c].Add(ctrl.Key);
                    else
                        mapping[c] = new List<Enum> { ctrl.Key };
                }
            }
        }

        /// <summary>
        /// Check which keys were just pressed.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysPressed(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach (Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyDown(key) && oldState.IsKeyUp(key))
                    ret.Add(key);
            }

            return ret;
        }

        /// <summary>
        /// Check which keys were just released.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysReleased(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach (Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyUp(key) && oldState.IsKeyDown(key))
                    ret.Add(key);
            }

            return ret;
        }

        /// <summary>
        /// Check which keys are being held.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Keys> CheckKeysHeld(KeyboardState oldState, KeyboardState newState)
        {
            List<Keys> ret = new List<Keys>();
            foreach (Keys key in reverseMap.Keys)
            {
                if (newState.IsKeyDown(key) && oldState.IsKeyDown(key)) ret.Add(key);
            }
            return ret;
        }

        /// <summary>
        /// Check which buttons were just pressed.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsPressed(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach (Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonDown(btn) && oldState.IsButtonUp(btn))
                    ret.Add(btn);
            }

            return ret;
        }

        /// <summary>
        /// Check which buttons were just released.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsReleased(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach (Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonUp(btn) && oldState.IsButtonDown(btn))
                    ret.Add(btn);
            }

            return ret;
        }

        /// <summary>
        /// Check which buttons are being held.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public List<Buttons> CheckButtonsHeld(GamePadState oldState, GamePadState newState)
        {
            List<Buttons> ret = new List<Buttons>();
            foreach (Buttons btn in reverseMap.Keys)
            {
                if (newState.IsButtonDown(btn) && oldState.IsButtonDown(btn)) ret.Add(btn);
            }
            return ret;
        }

        /// <summary>
        /// Called after you check if a control has been pressed. Gets related action for further logic.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public List<string> GetControlPressed(Enum ctrl)
        {
            reverseMap.TryGetValue(ctrl, out List<string> ret);
            return ret;
        }
    }
}
