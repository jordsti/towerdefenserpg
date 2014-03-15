using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;


using TD.GameLogic;
using TD.Graphics;

namespace TD.Graphics
{
    public class PlayerUnitRenderer
    {
        public const int PUNIT_WIDTH_PX = 30, PUNIT_HEIGHT_PX = 30;

        public static Surface Render(PlayerUnit Unit)
        {

            Surface Buffer = new Surface(PUNIT_WIDTH_PX, PUNIT_WIDTH_PX);
            Color Bg = Color.WhiteSmoke;

            switch (Unit.Class)
            {
                case UnitClasses.Archer: Bg = Color.Orange;
                    break;
                case UnitClasses.Mage: Bg = Color.Blue;
                    break;
                case UnitClasses.Paladin: Bg = Color.DeepPink;
                    break;
                case UnitClasses.Soldier: Bg = Color.Brown;
                    break;
                case UnitClasses.Thieft: Bg = Color.Yellow;
                    break;
 
            }

            Rectangle UnitRect = new Rectangle(new Point(0,0), new Size(Buffer.Width, Buffer.Height));
            Box Border = new Box(new Point(0, 0), new Size(Buffer.Width - 1, Buffer.Height - 1));
            Buffer.Fill(UnitRect, Bg);
            Buffer.Draw(Border, Color.WhiteSmoke, true);
            Buffer.AlphaBlending = true;
            Buffer.Alpha = 230;
            return Buffer;
            
        }
         
    }
}
