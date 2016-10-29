using System.Globalization;
using JodelAPI.Objects;

namespace JodelAPI
{
    public class Location
    {
        private static User _user;

        internal Location(User user)
        {
            _user = user;
        }

        /// <summary>
        ///     Unit for calculating distance
        /// </summary>
        public enum Unit
        {
            Kilometers,
            Meters,
            Miles
        }

        /// <summary>
        ///     Gets the coordinates.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>Coordinates.</returns>
        public Coordinates GetCoordinates(string location)
        {
            return Helpers.GetCoords(location);
        }

        /// <summary>
        ///     Sets the location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void SetLocation(string location)
        {
            var coord = Helpers.GetCoords(location);

            _user.Latitude = coord.Latitude;
            _user.Longitude = coord.Longitude;
        } // from location name via Google API

        /// <summary>
        ///     Sets the location.
        /// </summary>
        /// <param name="coord">The coord.</param>
        public void SetLocation(Coordinates coord)
        {
            _user.Latitude = coord.Latitude;
            _user.Longitude = coord.Longitude;
        } // from created object

        /// <summary>
        ///     Calculates the distance.
        /// </summary>
        /// <param name="coord1">The coord1.</param>
        /// <param name="coord2">The coord2.</param>
        /// <param name="unit">The unit.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="InternalException">API Error: Calculating Distance</exception>
        public double CalcDistance(Coordinates coord1, Coordinates coord2, Unit unit)
        {
            double c1Lo = double.Parse(coord1.Longitude, CultureInfo.InvariantCulture);
            double c2Lo = double.Parse(coord2.Longitude, CultureInfo.InvariantCulture);
            double c1La = double.Parse(coord1.Latitude, CultureInfo.InvariantCulture);
            double c2La = double.Parse(coord2.Latitude, CultureInfo.InvariantCulture);

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