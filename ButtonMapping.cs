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

        public ButtonMap GetInstance()
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
                    if (reverseMap == null || reverseMap.Count < 1)
                        reverseMap[b] = new List<string> { btn.Key };
                    else
                        reverseMap[b].Add(btn.Key);
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
        public KeyMap GetInstance()
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

        public string GetKeyPressed(Keys key)
        {
            reverseMap.TryGetValue(key, out string ret);
            return ret;
        }
    }
}
