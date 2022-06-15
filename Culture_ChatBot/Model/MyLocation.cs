using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Culture_ChatBot.Model
{
    public class MyLocation
    {
        GeoCoordinateWatcher watcher;
        public double Lat;
        public double Lng;

        public void GetLocationProperty()
        {
            watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                Console.WriteLine("Lat: {0}, Long: {1}",
                    coord.Latitude,
                    coord.Longitude);
                Lat = coord.Latitude;
                Lng = coord.Longitude;
            }
            else
            {
                Console.WriteLine("Unknown latitude and longitude.");
            }
        }
    }
}