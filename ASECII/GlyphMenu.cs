﻿using ArchConsole;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASECII {
    class GlyphMenu : SadConsole.Console {
        SpriteModel spriteModel;
        MouseWatch mouse;
        Action brushChanged;

        public GlyphMenu(int width, int height, SpriteModel spriteModel, Action brushChanged) : base(width, height) {
            this.spriteModel = spriteModel;
            this.mouse = new MouseWatch();
            this.brushChanged = brushChanged;
        }

        public override bool ProcessMouse(MouseScreenObjectState state) {
            mouse.Update(state, IsMouseOver);
            if (state.IsOnScreenObject && state.Mouse.LeftButtonDown && mouse.leftPressedOnScreen) {
                int index = (state.SurfaceCellPosition.X) + (state.SurfaceCellPosition.Y * Width);
                if (index < 256 && spriteModel.brush.glyph != index) {
                    spriteModel.brush.glyph = (char)index;
                    brushChanged?.Invoke();
                }
            }

            return base.ProcessMouse(state);
        }
        public override void Render(TimeSpan timeElapsed) {
            for (int i = 0; i < 256; i++) {
                string s = ((char)i).ToString();
                this.Print(i % Width, i / Width, s, spriteModel.brush.foreground, spriteModel.brush.background);
            }
            var x = spriteModel.brush.glyph % Width;
            var y = spriteModel.brush.glyph / Width;
            var c = this.GetCellAppearance(x, y);
            this.SetCellAppearance(x, y, new ColoredGlyph(c.Background, c.Foreground, c.Glyph));


            base.Render(timeElapsed);
        }
    }
}
