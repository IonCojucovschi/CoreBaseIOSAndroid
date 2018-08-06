using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;

namespace Int.Droid.Helpers
{
    public static class LocationHelper
    {
        public static async Task<double[]> GetCoordonateFromName(Context context, string address)
        {
            double[] result = {0.0, 0.0};
            try
            {
                var geocoder = new Geocoder(context);
                var coordResultList = await geocoder.GetFromLocationNameAsync(address, 1);

                if (coordResultList.Count > 0)
                    result = new[] {coordResultList.First().Latitude, coordResultList.First().Longitude};
            }
            catch
            {
                return new[] {0.0, 0.0};
            }

            return result;
        }
    }
}