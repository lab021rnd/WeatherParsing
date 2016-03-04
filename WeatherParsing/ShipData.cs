using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherParsing
{
    public class ShipData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string shipName { get; set; }
        public DateTime UTC { get; set; }
        public DateTime LTC { get; set; }
        public DateTime Weather_Time { get; set; }
        public double Speed_VG { get; set; }
        public double Speed_VG_x { get; set; }
        public double Speed_VG_y { get; set; }
        public double Speed_VS { get; set; }
        public double Speed_VS_x { get; set; }
        public double Speed_VS_y { get; set; }
        public double rel_current_speed_Zone1_M { get; set; }
        public double rel_current_dir_Zone1_M { get; set; }
        public double rel_current_speed_Zone2_M { get; set; }
        public double rel_current_dir_Zone2_M { get; set; }
        public double abs_current_speed_W { get; set; }
        public double abs_current_dir_W { get; set; }
        public double propeller_rpm { get; set; }
        public double shaft_torque_KNM { get; set; }
        public double shaft_Power { get; set; }
        public double brake_Power { get; set; }
        public double lat_Position { get; set; }
        public string lat_Position_dir { get; set; }
        public double long_Position { get; set; }
        public string long_Position_dir { get; set; }
        public double Voyage_Distance { get; set; }
        public double heading_Gyro { get; set; }
        public double heading_GPS { get; set; }
        public double rudder_angle { get; set; }
        public double seawater_Temp_M { get; set; }
        public double seawater_Density_M { get; set; }
        public double rel_wind_speed_M { get; set; }
        public double rel_wind_direction_M { get; set; }
        public double abs_wind_speed_W { get; set; }
        public double abs_wind_direction_W { get; set; }
        public double wave_height_eye_M { get; set; }
        public double T_wave_height_M { get; set; }
        public double T_wave_direction_M { get; set; }
        public double T_wave_period_M { get; set; }
        public double L_swell_height_M { get; set; }
        public double L_swell_direction_M { get; set; }
        public double L_swell_period_M { get; set; }
        public double H_sea_height_M { get; set; }
        public double H_sea_direction_M { get; set; }
        public double H_sea_period_M { get; set; }
        public double T_wave_height_W { get; set; }
        public double T_wave_direction_W { get; set; }
        public double T_wave_period_W { get; set; }
        public double L_swell_height_W { get; set; }
        public double L_swell_direction_W { get; set; }
        public double L_swell_period_W { get; set; }
        public double H_sea_height_W { get; set; }
        public double H_sea_direction_W { get; set; }
        public double H_sea_period_W { get; set; }
        public double sea_Depth_M { get; set; }
        public double sea_depth_W { get; set; }
        public double fp_drft { get; set; }
        public double ap_drft { get; set; }
        public double mid_ps_drft { get; set; }
        public double mid_ss_drft { get; set; }
    }
}
