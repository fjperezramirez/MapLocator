using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CompassLib;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDelitos();
        }

        private static void Test1()
        {
            double latitude = 18.4041767;
            double longitude = -66.0796127;
            double distanceInMeters = 1000;

            List<Location> result = new List<Location>();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            using (SqlConnection conn2 = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();
                conn2.Open();

                using (SqlCommand cmd = new SqlCommand())
                using (SqlCommand cmd2 = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "DECLARE @Coordinate   geography;" + "\r\n" +
                        "" + "\r\n" +
                        "SET @Coordinate = geography::Point (@Latitude, @Longitude, (4326));" + "\r\n" +
                        "" + "\r\n" +
                        "SELECT LocationID," + "\r\n" +
                        "       LocationTypeEnumCode," + "\r\n" +
                        "       LocationGroupEnumCode," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       DescriptionEn," + "\r\n" +
                        "       DescriptionEs," + "\r\n" +
                        "       Address1," + "\r\n" +
                        "       Address2," + "\r\n" +
                        "       City," + "\r\n" +
                        "       State," + "\r\n" +
                        "       ZipCode," + "\r\n" +
                        "       Latitude," + "\r\n" +
                        "       Longitude," + "\r\n" +
                        "       Z," + "\r\n" +
                        "       M" + "\r\n" +
                        "  FROM dbo.Location" + "\r\n" +
                        " WHERE Coordinate.STDistance (@Coordinate) <= @DistanceInMeters";

                    cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float).Value = latitude;
                    cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float).Value = longitude;
                    cmd.Parameters.Add("@DistanceInMeters", System.Data.SqlDbType.Float).Value = distanceInMeters;

                    cmd2.Connection = conn2;
                    cmd2.CommandText =
                        "SELECT ParameterID," + "\r\n" +
                        "       LocationID," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       [Value]" + "\r\n" +
                        "  FROM dbo.Parameter" + "\r\n" +
                        " WHERE LocationID = @LocationID";

                    SqlParameter locationIdParam = cmd2.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Location newLoc = new Location(reader);

                            locationIdParam.Value = newLoc.LocationID;

                            using (SqlDataReader reader2 = cmd2.ExecuteReader())
                            {
                                newLoc.Parameters.Load(reader2);
                            }

                            result.Add(newLoc);
                        }
                    }
                }
            }

            Console.WriteLine(result);
        }

        private static void Test2()
        {
            double southWestLatitude = 18.40;
            double southWestLongitude = -66.08;
            double northEastLatitude = 18.41;
            double northEastLongitude = -66.07;
            List<int> optionalLocationGroupFilter = null;

            List<Location> result = new List<Location>();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            using (SqlConnection conn2 = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();
                conn2.Open();

                using (SqlCommand cmd = new SqlCommand())
                using (SqlCommand cmd2 = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "DECLARE @minY varchar(25) = CAST(@SouthWestLatitude AS varchar(25));" + "\r\n" +
                        "DECLARE @maxY varchar(25) = CAST(@NorthEastLatitude AS varchar(25));" + "\r\n" +
                        "DECLARE @minX varchar(25) = CAST(@SouthWestLongitude AS varchar(25));" + "\r\n" +
                        "DECLARE @maxX varchar(25) = CAST(@NorthEastLongitude AS varchar(25));" + "\r\n" +
                        "" + "\r\n" +
                        "DECLARE @BoundingRect varchar(1024);" + "\r\n" +
                        "" + "\r\n" +
                        "SET @BoundingRect = 'POLYGON((' + @minX + ' '  + @minY + ', ' + " + "\r\n" +
                        "                                  @maxX + ' ' + @minY + ', ' + " + "\r\n" +
                        "                                  @maxX + ' ' + @maxY + ', ' + " + "\r\n" +
                        "                                  @minX + ' ' + @maxY + ', ' + " + "\r\n" +
                        "                                  @minX + ' ' + @minY + '))';" + "\r\n" +
                        "DECLARE @Bounds geography;" + "\r\n" +
                        "SET @Bounds = geography::STPolyFromText(@BoundingRect, 4326);" + "\r\n" +
                        "" + "\r\n" +
                        "SELECT LocationID," + "\r\n" +
                        "       LocationTypeEnumCode," + "\r\n" +
                        "       LocationGroupEnumCode," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       DescriptionEn," + "\r\n" +
                        "       DescriptionEs," + "\r\n" +
                        "       Address1," + "\r\n" +
                        "       Address2," + "\r\n" +
                        "       City," + "\r\n" +
                        "       State," + "\r\n" +
                        "       ZipCode," + "\r\n" +
                        "       Latitude," + "\r\n" +
                        "       Longitude," + "\r\n" +
                        "       Z," + "\r\n" +
                        "       M" + "\r\n" +
                        "  FROM dbo.Location" + "\r\n" +
                        " WHERE     Coordinate.STWithin(@Bounds) = 1";

                    cmd.Parameters.Add("@SouthWestLatitude", System.Data.SqlDbType.Float).Value = southWestLatitude;
                    cmd.Parameters.Add("@SouthWestLongitude", System.Data.SqlDbType.Float).Value = southWestLongitude;
                    cmd.Parameters.Add("@NorthEastLatitude", System.Data.SqlDbType.Float).Value = northEastLatitude;
                    cmd.Parameters.Add("@NorthEastLongitude", System.Data.SqlDbType.Float).Value = northEastLongitude;

                    if (optionalLocationGroupFilter != null && optionalLocationGroupFilter.Count > 0)
                    {
                        int lastIndex = optionalLocationGroupFilter.Count - 1;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine();

                        for (int i = 0; i < optionalLocationGroupFilter.Count; i++)
                        {
                            int item = optionalLocationGroupFilter[i];

                            if (i == 0)
                            {
                                sb.Append("       AND (   ");
                            }
                            else
                            {
                                sb.Append("            OR ");
                            }

                            sb.Append("LocationGroupEnumCode = @LocationGroupEnumCode" + i.ToString());

                            cmd.Parameters.Add("@LocationGroupEnumCode" + i.ToString(), System.Data.SqlDbType.Int).Value = item;

                            if (i == lastIndex)
                            {
                                sb.Append(")");
                            }
                            else
                            {
                                sb.AppendLine();
                            }
                        }

                        cmd.CommandText += sb.ToString();
                    }

                    cmd2.Connection = conn2;
                    cmd2.CommandText =
                        "SELECT ParameterID," + "\r\n" +
                        "       LocationID," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       [Value]" + "\r\n" +
                        "  FROM dbo.Parameter" + "\r\n" +
                        " WHERE LocationID = @LocationID";

                    SqlParameter locationIdParam = cmd2.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Location newLoc = new Location(reader);

                            locationIdParam.Value = newLoc.LocationID;

                            using (SqlDataReader reader2 = cmd2.ExecuteReader())
                            {
                                newLoc.Parameters.Load(reader2);
                            }

                            result.Add(newLoc);
                        }
                    }
                }
            }

            Console.WriteLine(result);
        }

        private static void GetDelitos()
        {
            string data = System.IO.File.ReadAllText(@"C:\Shared\Hackton\delitosjson.txt");

            
            JArray providersArray = JsonConvert.DeserializeObject(data) as JArray;

            if (providersArray == null)
            {
                throw new ApplicationException("The response is not a Json array.");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (JObject item in providersArray)
            {
                string delitoDesc = item.Value<string>("delito_desc");
                string delitoId = item.Value<string>("delito_id");

                dic[delitoId] = delitoDesc;
            }

            foreach (var item in dic)
            {
                Debug.WriteLine(item.Key + "|" + item.Value);
            }
        }
    }
}
