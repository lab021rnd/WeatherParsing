using System.Data.Entity;
using WeatherParsing;

namespace WeatherParsing
{
    public class WeatherContext : DbContext
    {

      public DbSet<WaveWindModel> WaveWindModels { get; set; }
      public DbSet<OceanModel>  OceanModels { get; set; }

    }
}
