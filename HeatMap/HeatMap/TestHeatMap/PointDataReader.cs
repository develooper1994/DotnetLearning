using HeatMap;
using System.Collections.Generic;
using System.IO;

using System.Runtime.CompilerServices;

namespace TestHeatMap
{
    static class PointDataReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<HeatPoint> Read(string filePath)
        {
            List<HeatPoint> result = new List<HeatPoint>();

            using (var sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    var vals = sr.ReadLine().Split(',');

                    result.Add(new HeatPoint
                    {
                        X = float.Parse(vals[0]),
                        Y = float.Parse(vals[1]),
                        W = float.Parse(vals[2])
                    });
                }
            }

            return result;
        }
    }
}
