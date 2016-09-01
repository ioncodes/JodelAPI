using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Objects;

namespace JodelAPI
{
    public static class Location
    {
        /// <summary>
        /// Unit for calculating distance
        /// </summary>
        public enum Unit
        {
            Kilometers,
            Meters,
            Miles
        }

        /// <summary>
        /// Gets the coordinates.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>Coordinates.</returns>
        public static Coordinates GetCoordinates(string location)
        {
            return Helpers.GetCoords(location);
        }

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void SetLocation(string location)
        {
            var coord = Helpers.GetCoords(location);

            Account.Latitude = coord.Latitude;
            Account.Longitude = coord.Longitude;
        } // from location name via Google API

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="coord">The coord.</param>
        public static void SetLocation(Coordinates coord)
        {
            Account.Latitude = coord.Latitude;
            Account.Longitude = coord.Longitude;
        } // from created object

        /// <summary>
        /// Calculates the distance.
        /// </summary>
        /// <param name="coord1">The coord1.</param>
        /// <param name="coord2">The coord2.</param>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="InternalException">API Error: Calculating Distance</exception>
        public static double CalcDistance(Coordinates coord1, Coordinates coord2, Location.Unit unit)
        {
            double c1Lo = Double.Parse(coord1.Longitude, CultureInfo.InvariantCulture);
            double c2Lo = Double.Parse(coord2.Longitude, CultureInfo.InvariantCulture);
            double c1La = Double.Parse(coord1.Latitude, CultureInfo.InvariantCulture);
            double c2La = Double.Parse(coord2.Latitude, CultureInfo.InvariantCulture);

            switch (unit)
            {
                case Unit.Kilometers:
                    return Distance.KilometresBetweenTwoGeographicCoordinates(c1Lo, c1La, c2Lo, c2La);
                case Unit.Meters:
                    return Distance.MetresBetweenTwoGeographicCoordinates(c1Lo, c1La, c2Lo, c2La);
                case Unit.Miles:
                    return Distance.MilesBetweenTwoGeographicCoordinates(c1Lo, c1La, c2Lo, c2La);
                default:
                    throw new InternalException("API Error: Calculating Distance");
            }
        }
    }
}
