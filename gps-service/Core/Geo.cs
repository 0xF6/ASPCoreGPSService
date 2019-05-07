namespace gps_service.Core
{
    using System;
    using Models;

    public static class Geo
    {
        public static float Distance(GeoPosition pos1, GeoPosition pos2)
        {
            var rad = 6372795;

            
            var llat1 = pos1.Latitude;
            var llong1 = pos1.Longitude;
            var llat2 = pos2.Latitude;
            var llong2 = pos2.Longitude;

            
            var lat1 = llat1 * MathF.PI / 180f;
            var lat2 = llat2 * MathF.PI / 180f;
            var long1 = llong1 * MathF.PI / 180f;
            var long2 = llong2 * MathF.PI / 180f;

            
            var cl1 = MathF.Cos(lat1);
            var cl2 = MathF.Cos(lat2);
            var sl1 = MathF.Sin(lat1);
            var sl2 = MathF.Sin(lat2);
            var delta = long2 - long1;
            var cdelta = MathF.Cos(delta);
            var sdelta = MathF.Sin(delta);

            
            var y = MathF.Sqrt(MathF.Pow(cl2 * sdelta, y: 2) + MathF.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, y: 2));
            var x = sl1 * sl2 + cl1 * cl2 * cdelta;
            var ad = MathF.Atan2(y, x);
            var dist = ad * rad;
            return dist;
        }
    }
}