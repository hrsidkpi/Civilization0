using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{

    public interface ISubConstraint
    {
        bool Check(Unit u);
    }

    public interface ISubTileConstraint
    {
        bool Check(Tile t);
    }

    #region Unit Sub Constraints
    public class UnitTypeConstraint : ISubConstraint
    {
        readonly UnitType type;

        public UnitTypeConstraint(UnitType type)
        {
            this.type = type;
        }

        public bool Check(Unit u)
        {
            return u.type == type;
        }
    }

    public class PlayerConstraint : ISubConstraint
    {
        readonly bool player;

        public PlayerConstraint(bool player)
        {
            this.player = player;
        }

        public bool Check(Unit u)
        {
            return u.player == player;
        }
    }
    public class DistanceConstraint : ISubConstraint
    {
        readonly int x, y;
        readonly int distance;

        public DistanceConstraint(int x, int y, int distance)
        {
            this.x = x;
            this.y = y;
            this.distance = distance;
        }

        public bool Check(Unit u)
        {
            bool res = Math.Abs(u.TileX - x) + Math.Abs(u.TileY - y) <= distance;
            return res;
        }

        public override string ToString()
        {
            return "Distance " + distance + " constraint from x:" + x + ", y:" + y;
        }
    }
    #endregion

    #region 
    public class DistanceTileConstraint : ISubTileConstraint
    {

        public int x, y, dist;

        public DistanceTileConstraint(int x, int y, int dist)
        {
            this.x = x;
            this.y = y;
            this.dist = dist;
        }

        public bool Check(Tile t)
        {
            bool res = Math.Abs(t.TileX - x) + Math.Abs(t.TileY - y) <= dist;
            return res;
        }

        public override string ToString()
        {
            return "Distance " + dist + " constraint from " + x + ", " + y;
        }
    }
    #endregion


    public class LookupConstraint
    {


        public List<ISubConstraint> subConstraints;

        public LookupConstraint(params ISubConstraint[] subConstraints)
        {
            this.subConstraints = subConstraints.ToList();
        }

        public bool Check(Unit u)
        {
            foreach (ISubConstraint s in subConstraints) if (!s.Check(u)) return false;
            return true;
        }

    }

    public class TileLookupConstraint
    {
        public List<ISubTileConstraint> subTileConstraints;

        public TileLookupConstraint(params ISubTileConstraint[] subTileConstraints)
        {
            this.subTileConstraints = subTileConstraints.ToList();
        }

        public bool Check(Tile t)
        {
            foreach (ISubTileConstraint s in subTileConstraints) if (!s.Check(t)) return false;
            return true;
        }

        public override string ToString()
        {
            string s = "Tile lookup constraints: ";
            foreach (ISubTileConstraint c in subTileConstraints) s += c + ", ";
            return s;
        }
    }

}
