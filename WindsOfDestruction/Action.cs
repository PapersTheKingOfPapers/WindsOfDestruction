using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class Action
    {
        public List<Content> Contents { get; set; }
    }

    public class Content
    {
        public AttackAction AttackAction { get; set; }
        public DeathAction DeathAction { get; set; }
    }

    public class AttackAction
    {
        public string attackedUnitID { get; set; }
    }
    public class DeathAction
    {

    }
}
