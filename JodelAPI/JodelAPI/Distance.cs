using System;

namespace JodelAPI
{
    internal static class Distance
    {
        public const int RadiusOfEarthInKilometres = 6371;

        /// <summary>
        ///     Earths redious in Metres.
        /// </summary>
        public const int RadiusOfEarthInMetres = 6371000;

        /// <summary>
        ///     Earths redious in Miles.
        /// </summary>
        public const int RadiusOfEarthInMiles = 3959;

        public static double MilesBetweenTwoGeographicCoordinates(double originLongitude, double originLatitude,
            double destinationLongitude, double destinationLatitude)
        {
            return RadiusOfEarthInMiles *
                   AngleBetweenTwoCoordinates(originLongitude, originLatitude, destinationLongitude, destinationLatitude);
        }

        /// <summary>
        ///     Calculates the distance in between to geographic coordiantes.
        /// </summary>
        /// <remarks>Calculated using the haversine formula.</remarks>
        public static double KilometresBetweenTwoGeographicCoordinates(double originLongitude, double originLatitude,
            double destinationLongitude, double destinationLatitude)
        {
            return RadiusOfEarthInKilometres *
                   AngleBetweenTwoCoordinates(originLongitude, originLatitude, destinationLongitude, destinationLatitude);
        }

        /// <summary>
        ///     Calculates the distance in between to geographic coordiantes.
        /// </summary>
        /// <remarks>Calculated using the haversine formula.</remarks>
        public static double MetresBetweenTwoGeographicCoordinates(double originLongitude, double originLatitude,
            double destinationLongitude, double destinationLatitude)
        {
            return RadiusOfEarthInMetres *
                   AngleBetweenTwoCoordinates(originLongitude, originLatitude, destinationLongitude, destinationLatitude);
        }

        /// <summary>
        ///     Calculates the distance in between to geographic coordiantes.
        /// </summary>
        /// <remarks>Calculated using the haversine formula.</remarks>
        private static double AngleBetweenTwoCoordinates(double originLongitude, double originLatitude,
            double destinationLongitude, double destinationLatitude)
        {
            var deltaLat = DegreeToRadian(destinationLatitude - originLatitude);
            var deltaLng = DegreeToRadian(destinationLongitude - originLongitude);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(DegreeToRadian(originLatitude)) * Math.Cos(DegreeToRadian(destinationLatitude)) *
                    (Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2));

            var angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return angle;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        ///     Convert radians to degrees.
        /// </summary>
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}