using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Graph.Model
{
    [DebuggerDisplay("[{Source.ID} => {Destination.ID}] - {Nodes.Count} ({Weight})")]
    [Serializable]
    public class Path : IEnumerable<Edge>
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

        /// <summary>
        /// Очередность узлов пути
        /// </summary>
        public List<Node> Nodes
        {
            get; private set;
        }
        /// <summary>
        /// Проверяет возможность построения маршрута по узлам пути
        /// </summary>
        public bool IsSolid
        {
            get
            {
                if (Nodes.Count > 1)
                {
                    for (int i = 0; i < Nodes.Count - 1; i++)
                    {
                        if (!Nodes[i].IsRelateTo(Nodes[i + 1]))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// Если путь можно построить - возвращает список граней такого пути
        /// </summary>
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
        public float Weight
        {
            get
            {
                ThrowIfNotSolid();

                float weight = 0;
                for (int i = 0; i < Nodes.Count - 1; i++)
                {
                    weight += Nodes[i].WeightTo(Nodes[i + 1]);
                }
                return weight;
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

        public IEnumerator<Edge> GetEnumerator()
        {
            ThrowIfNotSolid();
            return new PathEnum(Nodes);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
        public static implicit operator Path(List<Node> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException();

            Path res = new Path(new Direction(nodes[0], nodes[nodes.Count - 1]));
            res.Nodes = new List<Node>(nodes);
            return res;
        }
    }

    public class PathEnum : IEnumerator<Edge>
    {
        private List<Node> _nodes;
        private int position = -1;

        public PathEnum(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _nodes[position].RelatedEdges[_nodes[position + 1]];
                }
                catch (Exception ex)
                {
                    throw new MissingMemberException("Ошибка при получении элемента. Возможно путь был изменен", ex);
                }
            }
        }

        Edge IEnumerator<Edge>.Current
        {
            get
            {
                return (Edge)Current;
            }
        }

        public bool MoveNext()
        {
            position++;
            return position < _nodes.Count - 1;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
            _nodes = null;
        }
    }
}
