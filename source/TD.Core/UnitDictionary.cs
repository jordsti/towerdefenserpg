using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TD.GameLogic;

namespace TD.Core
{
    public class PlayerUnitDictionary : Dictionary<MapCoord,PlayerUnit>
    {
        public PlayerUnitDictionary()
            : base()
        {

        }

        public int CountClass(UnitClasses UnitClass)
        {
            int count = 0;

            foreach (PlayerUnit pUnit in Values)
            {
                if (pUnit.Class == UnitClass)
                {
                    count++;
                }
            }

            return count;
        }

        public PlayerUnit GetUnitAt(MapCoord Coord)
        {
            PlayerUnit Unit = new PlayerUnit();

            foreach (MapCoord c in Keys)
            {
                if (c.Row == Coord.Row && c.Column == Coord.Column)
                {
                    return this[c];
                }
            }

            return Unit;
        }

        public bool ContainsCoord(MapCoord Coord)
        {
            foreach (MapCoord c in Keys)
            {
                if (c.Row == Coord.Row && c.Column == Coord.Column)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
