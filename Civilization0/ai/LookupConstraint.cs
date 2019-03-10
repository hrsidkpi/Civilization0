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

    public class UnitTypeConstraint : ISubConstraint
    {
        UnitType type;

        public UnitTypeConstraint(UnitType type)
        {
            this.type = type;
        }

        public bool Check(Unit u)
        {
            return u.type == type;
        }
    }

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
}
