using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazingView.Db.Entities
{

    public class Strategy
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PairName { get; set; }
        public int InputCount { get; set; }
        public int ApplyTimeout { get; set; }

        public List<StrategyInput> Inputs { get; set; }

    }
}

