using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.Graph
{
    using Model;

    public interface IRatioProvider
    {
        float GetRatio(Edge edge);
    }
}
