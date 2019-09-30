using System;

namespace Bll.Services {
  public static class Gis {
    public const double Radius = 6371;
    public const double Radian = Math.PI / 180;

    /*
     * Distance between latitudes and longitudes
     */
    static double Haversine(double lat1, double lon1,
                            double lat2, double lon2) {      
      double dLat = Radian * (lat2 - lat1);
      double dLon = Radian * (lon2 - lon1);

      // Convert to radians 
      lat1 *= Radian; // lat1 = (Math.PI / 180) * lat1;
      lat2 *= Radian; // lat2 = (Math.PI / 180) * lat2;

      // Apply formula 
      double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(lat1) * Math.Cos(lat2);
      // double c = 2 * Math.Asin(Math.Sqrt(a));

      return Radius * (2 * Math.Asin(Math.Sqrt(a)));
    }
  }
}
