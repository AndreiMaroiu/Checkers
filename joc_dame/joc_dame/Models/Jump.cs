using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joc_dame.Models
{
    class Jump
    {
        public Position from = new();
        public Position over = new();
        public Position where = new();

        public List<Jump> next = new();
        public Jump previous = null;

        public Jump() { }

        public Jump(Position from, Position over, Position where)
            => (this.from, this.over, this.where) = (from, over, where);
    }
}
