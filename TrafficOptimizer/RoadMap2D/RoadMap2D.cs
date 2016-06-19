using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using SharpGL;

namespace TrafficOptimizer.RoadMap2D
{
    using Model;
    using RoadMap;
    using RoadMap.Model;
    using Tools;
    public class RoadMap2D : IDraw
    {
        /// <summary>
        /// OpenGL window
        /// </summary>
        public OpenGL OGL
        {
            get;
            private set;
        }
        public RoadMapParameters RoadMapParametrs;

        public RoadMap RoadMap
        {
            get;
            private set;
        }

        protected Dictionary<Section, PointF> _sections = new Dictionary<Section, PointF>();
        protected Dictionary<Road, Road2D> _roadsPosition = new Dictionary<Road, Road2D>();
        public Road GetRoad(Section s1, Section s2)
        {
            if (_sections.ContainsKey(s1) && _sections.ContainsKey(s2))
            {
                return RoadMap.GetRoad(new Segment(s1, s2));
            }
            return null;
        }
        public Section GetIntersectedSection(PointF point)
        {
            foreach (var e in _sections)
            {
                if (Tools.IsPointInside(point, e.Value, RoadMapParametrs.StreakSize))
                    return e.Key;
            }
            return null;
        }

        public RoadMap2D(OpenGL ogl) : base()
        {
            RoadMap = new RoadMap();
            OGL = ogl;
            RoadMapParametrs = new RoadMapParameters();
        }
        public RoadMap2D(OpenGL ogl, RoadMapParameters parameters) : base()
        {
            RoadMap = new RoadMap();
            OGL = ogl;
            RoadMapParametrs = parameters;
        }

        /// <summary>
        /// Добавление новой дороги или полосы на карту 
        /// </summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        public void AddRoad(PointF start, PointF end)
        {
            if (!Tools.IsRoundsIntersect(start, RoadMapParametrs.StreakSize, end, RoadMapParametrs.StreakSize))
            {
                Section src = GetIntersectedSection(start) ?? RoadMap.MakeSection();
                Section dst = GetIntersectedSection(end) ?? RoadMap.MakeSection();
                Road r = RoadMap.SetRoad(src, dst, Tools.Distance(start, end));

                if (!_sections.ContainsKey(src))
                    _sections.Add(src, start);
                if (!_sections.ContainsKey(dst))
                    _sections.Add(dst, end);

                Road2D r2d = Road2D.GetRoad(r, start, end, RoadMapParametrs.StreakSize);
                if (_roadsPosition.ContainsKey(r))
                    _roadsPosition[r] = r2d;
                else
                    _roadsPosition.Add(r, r2d);
            }
            else
                throw new ApplicationException("Секции начала и конца пересекаются");
        }
        public void RemoveStreak(Section src, Section dst)
        {
            RoadMap.RemoveStreak(src, dst);
            Road r = GetRoad(src, dst);
            if (r != null)
            {
                if (r.Streaks > 0)
                {
                    var last = _roadsPosition[r];
                    _roadsPosition[r] = Road2D.GetRoad(r, last.StartPoint, last.EndPoint, RoadMapParametrs.StreakSize);
                }
                else
                {
                    RemoveRoad(r);
                }
            }
        }
        public void RemoveRoad(Road r)
        {
            RoadMap.RemoveRoad(new Segment(r.Source, r.Destination));

            if (r.Source.RelatedRoads.Count() == 0)
                _sections.Remove(r.Source);
            if (r.Destination.RelatedRoads.Count() == 0)
                _sections.Remove(r.Destination);

            _roadsPosition.Remove(r);
        }
        public void MoveSection(PointF oldPos, PointF newPos)
        {
            Section s = GetIntersectedSection(oldPos);
            if (s != null)
            {
                if (GetIntersectedSection(newPos) == null)
                {
                    _sections[s] = newPos;  // Обновляем положение секции
                    // Переписываем расположения всех точек для дорог, прилежащих к этой секции
                    foreach (Road r in s.OutRoads)
                    {
                        _roadsPosition[r] = Road2D.GetRoad(r, newPos, _sections[r.Destination], RoadMapParametrs.StreakSize);
                    }
                    foreach (Road r in s.InRoads)
                    {

                        _roadsPosition[r] = Road2D.GetRoad(r, _sections[r.Source], newPos, RoadMapParametrs.StreakSize);
                    }
                }
                else
                    throw new ApplicationException("Новая позиция пересекается с уже существующей секцией");
            }
        }

        private void setColor(Color c)
        {
            OGL.Color(c.R, c.G, c.B, c.A);
        }
        private void DrawSection(PointF point, Color c, int accuracy = 360)
        {
            OGL.Begin(OpenGL.GL_LINE_LOOP);
            setColor(c);

            double h = 0;
            for (int a = 0; a < accuracy; a++)
            {
                h = a * Math.PI / 180;
                OGL.Vertex(point.X + Math.Cos(h) * RoadMapParametrs.StreakSize, point.Y + Math.Sin(h) * RoadMapParametrs.StreakSize);
            }

            OGL.End();
        }
        private void DrawLine(PointF p1, PointF p2, Color c)
        {
            setColor(c);
            OGL.Vertex(p1.X, p1.Y);
            OGL.Vertex(p2.X, p2.Y);
        }
        public void Draw()
        {
            OGL.Begin(OpenGL.GL_LINES);

            foreach (Road2D r in _roadsPosition.Values)
            {
                DrawLine(r.StartPoint, r.EndPoint, Color.Black);
                foreach (Streak2D s in r.PrimaryLine.Streaks)
                {
                    DrawLine(s.BorderRightStart, s.BorderRightEnd, Color.Blue);
                }
                foreach (Streak2D s in r.SlaveLine.Streaks)
                {
                    DrawLine(s.BorderRightStart, s.BorderRightEnd, Color.Red);
                }
            }

            OGL.End();

            foreach (PointF p in _sections.Values)
            {
                DrawSection(p, Color.DarkOrange);
            }
        }
    }
}
