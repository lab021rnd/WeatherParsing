using System.Data.Entity;
using WeatherParsing;

namespace WeatherParsing
{
    public class WeatherContext : DbContext
    {

      public DbSet<oceanWeather> oceanWeathers { get; set; }

    }
}
