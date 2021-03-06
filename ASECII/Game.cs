﻿using SadRogue.Primitives;
using Newtonsoft.Json;
using System.IO;
using Color = SadRogue.Primitives.Color;
using SadConsole;

namespace ASECII {
    internal class Program {
        static int width = 160;
        static int height = 90;

        public static string STATE_FILE = "state.json";
        private static void Main(string[] args) {
            SadConsole.UI.Themes.Library.Default.Colors.ControlHostBack = Color.Black;
            SadConsole.UI.Themes.Library.Default.Colors.ControlBack = Color.Gray;

            //SadConsole.Settings.UnlimitedFPS = true;
            SadConsole.Settings.UseDefaultExtendedFont = true;
            SadConsole.Game.Create(width, height, "Content/IBMCGA.font", g => {
            });
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init() {
            if(LoadState()) {
                return;
            }

            // Create your console
            var firstConsole = new FileMenu(width, height, new LoadMode());

            SadConsole.Game.Instance.Screen = firstConsole;
            firstConsole.FocusOnMouseClick = true;
        }
        public static void SaveState(ProgramState state) {
            if (state != null) {
                File.WriteAllText(STATE_FILE, ASECIILoader.SerializeObject(state));
            } else {
                File.Delete(STATE_FILE);
            }
        }
        public static bool LoadState() {
            if (File.Exists(STATE_FILE)) {
                try {
                    var state = ASECIILoader.DeserializeObject<ProgramState>(File.ReadAllText(STATE_FILE));
                    if(state is EditorState e) {
                    
                            var sprite = ASECIILoader.DeserializeObject<SpriteModel>(File.ReadAllText(e.loaded));


                            sprite.OnLoad();
                            Game.Instance.Screen = new EditorMain(width, height, sprite);
                            return true;
                    
                    }
                } catch {
                    throw;
                }
            }
            return false;
        }
    }

    public interface ProgramState {
        
    }
    public class EditorState : ProgramState {
        public string loaded;
        public EditorState(string loaded) {
            this.loaded = loaded;
        }
    }
}