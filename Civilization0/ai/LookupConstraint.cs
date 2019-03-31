using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{

    /// <summary>
    /// Represents a constraint units can satisfy. Contains a method to check if a specific unit satisfies the constraint.
    /// </summary>
    public interface ISubConstraint
    {
        /// <summary>
        /// Checks if a unit satisfies the constraint.
        /// </summary>
        /// <param name="u">The unit to check</param>
        /// <returns>True if u satisfies the constraint and false otherwise.</returns>
        bool Check(Unit u);
    }

    /// <summary>
    /// Represents a constraint tiles can satisfy. Contains a method to check if a specific tile satisfies the constraint.
    /// </summary>
    public interface ISubTileConstraint
    {
        /// <summary>
        /// Checks if a tile satisfies the constraint.
        /// </summary>
        /// <param name="t">The tile to check</param>
        /// <returns>True if t satisfies the constraint and false otherwise.</returns>
        bool Check(Tile t);
    }

    #region Unit Sub Constraints
    /// <summary>
    /// A constraint to check if a unit is of a specific type.
    /// </summary>
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

    /// <summary>
    /// A constraint to check if a unit belongs to a specific player.
    /// </summary>
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

    /// <summary>
    /// A constraaint to check if a unit is within a certain distance from specific coordinates.
    /// </summary>
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

    #region Tile Sub Constraints 

    /// <summary>
    /// A constraint to check if a tile is of a specific type.
    /// </summary>
    public class TileTypeConstraint : ISubTileConstraint
    {
        public TileType type;

        public TileTypeConstraint(TileType type)
        {
            this.type = type;
        }

        public bool Check(Tile t)
        {
            return t.type == type;
        }
    }

    /// <summary>
    /// A constraint to check if a tile is within a certain distance from specific coordinates.
    /// </summary>
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


    /// <summary>
    /// Represents a series of unit sub constraints. Contains a method to check if a unit satisfies all constraints.
    /// </summary>
    public class LookupConstraint
    {
        
        //List of sub constraints.
        public List<ISubConstraint> subConstraints;

        /// <summary>
        /// Create a LookupConstraint
        /// </summary>
        /// <param name="subConstraints">Array of unit sub constraints</param>
        public LookupConstraint(params ISubConstraint[] subConstraints)
        {
            this.subConstraints = subConstraints.ToList();
        }

        /// <summary>
        /// Checks if a unit satisfies all the constraints
        /// </summary>
        /// <param name="u">The unit to check</param>
        /// <returns>True if all constraints are satisfied by u, or false if at least one constraint is unsatisfied.</returns>
        public bool Check(Unit u)
        {
            foreach (ISubConstraint s in subConstraints) if (!s.Check(u)) return false;
            return true;
        }

    }

    /// <summary>
    /// Represents a series of tile sub constraints. Contrains a method to check if a tile satisfies all sub constraints.
    /// </summary>
    public class TileLookupConstraint
    {
        //The list of sub constraints.
        public List<ISubTileConstraint> subTileConstraints;

        /// <summary>
        /// Create a TileLookupConstraint
        /// </summary>
        /// <param name="subTileConstraints">Array of tile sub constraints</param>
        public TileLookupConstraint(params ISubTileConstraint[] subTileConstraints)
        {
            this.subTileConstraints = subTileConstraints.ToList();
        }

        /// <summary>
        /// Check if a tile satisfies all sub constraints.
        /// </summary>
        /// <param name="t">The tile to check</param>
        /// <returns>True if t satisfies all sub constraints, or false if at least one constraint is not satisfied.</returns>
        public bool Check(Tile t)
        {
            foreach (ISubTileConstraint s in subTileConstraints) if (!s.Check(t)) return false;
            return true;
        }

        /// <summary>
        /// Get a string representation of the constraint.
        /// </summary>
        /// <returns>A string with all the sub constraints in string format.</returns>
        public override string ToString()
        {
            string s = "Tile lookup constraints: ";
            foreach (ISubTileConstraint c in subTileConstraints) s += c + ", ";
            return s;
        }
    }

}
