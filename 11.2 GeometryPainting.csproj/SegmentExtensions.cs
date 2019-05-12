using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        static Dictionary<Segment, Color> dictionary = new Dictionary<Segment, Color>();

        public static Color GetColor(this Segment segment)
        {
            if (!dictionary.ContainsKey(segment))
                return Color.Black;
            return dictionary[segment];
        }

        public static void SetColor(this Segment segment, Color color)
        {
            if (!dictionary.ContainsKey(segment))
                dictionary.Add(segment, color);
            else dictionary[segment] = color;
        }
    }
}