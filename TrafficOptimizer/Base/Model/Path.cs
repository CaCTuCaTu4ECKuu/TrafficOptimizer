using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Base.Model
{
    [DebuggerDisplay("[{Source.ID} => {Destination.ID}] - {Nodes.Count} ({Weight})")]
    [Serializable]
    public class Path
    {
        public Direction Direction
        {
            get;
            private set;
        }
        public Node Source
        {
            get { return Direction.Source; }
        }
        public Node Destination
        {
            get { return Direction.Destination; }
        }

        public List<Node> Nodes
        {
            get; private set;
        }
        public bool IsSolid
        {
            get
            {
                for (int i = 0; i < Nodes.Count - 1; i++)
                {
                    if (!Nodes[i].IsRelateTo(Nodes[i+1]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public List<Edge> Edges
        {
            get
            {
                ThrowIfNotSolid();

                List<Edge> res = new List<Edge>();
                for (int i = 0; i < Nodes.Count - 1; i++)
                {
                    res.Add(Nodes[i].RelatedEdges[Nodes[i+1]]);
                }
                return res;
            }
        }
        public decimal Weight
        {
            get
            {
                if (Nodes.Count > 1)
                {
                    ThrowIfNotSolid();

                    decimal weight = 0;
                    for (int i = 0; i < Nodes.Count - 1; i++)
                    {
                        weight += Nodes[i].WeightTo(Nodes[i + 1]);
                    }
                    return weight;
                }
                else
                    return decimal.MaxValue;
            }
        }

        public Path(Direction direction)
        {
            Direction = direction;
            Nodes = new List<Node>();
        }
        public Path(Path path)
        {
            Direction = path.Direction;
            Nodes = new List<Node>(path.Nodes);
        }

        private void ThrowIfNotSolid()
        {
            if (!IsSolid)
            {
                throw new Exception("Путь не связан");
            }
        }

        public Edge this[int index]
        {
            get
            {
                if (index >= 0 && index < Nodes.Count - 1)
                {
                    ThrowIfNotSolid();
                    return Nodes[index].RelatedEdges[Nodes[index + 1]];
                }
                else
                    throw new IndexOutOfRangeException("Значение должно быть в пределах количества граней");
            }
        }
        public static implicit operator Path(Edge edge)
        {
            Path p = new Path(edge.Direction);
            p.Nodes.Add(p.Source);
            p.Nodes.Add(p.Destination);
            return p;
        }
    }
}
