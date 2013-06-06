using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using CompassLib;
using System.Text;
using System.Diagnostics;

namespace CompassWs
{
    /// <summary>
    /// Summary description for CompassWS_V1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CompassWS_V1 : System.Web.Services.WebService
    {

        [WebMethod]
        public List<EnumeratedType> GetEnumeratedTypes()
        {
            List<EnumeratedType> result = new List<EnumeratedType>();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "SELECT [EnumeratedTypeID]," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       [DescriptionEn]," + "\r\n" +
                        "       [DescriptionEs]" + "\r\n" +
                        "  FROM [dbo].[EnumeratedType]";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnumeratedType et = new EnumeratedType(reader);

                            result.Add(et);
                        }
                    }
                }
            }

            return result;
        }

        [WebMethod]
        public List<EnumeratedTypeValue> GetEnumeratedTypeValues(int enumeratedTypeCode)
        {
            List<EnumeratedTypeValue> result = new List<EnumeratedTypeValue>();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "SELECT EnumeratedTypeValueID," + "\r\n" +
                        "       EnumeratedTypeID," + "\r\n" +
                        "       Code," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       DescriptionEn," + "\r\n" +
                        "       DescriptionEs" + "\r\n" +
                        "  FROM dbo.EnumeratedTypeValue" + "\r\n" +
                        " WHERE EnumeratedTypeID = @EnumeratedTypeID";

                    cmd.Parameters.Add("@EnumeratedTypeID", System.Data.SqlDbType.Int).Value = enumeratedTypeCode;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnumeratedTypeValue et = new EnumeratedTypeValue(reader);

                            result.Add(et);
                        }
                    }
                }
            }

            return result;
        }

        [WebMethod]
        public Location GetLocation(int locationID)
        {
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
                        "       M," + "\r\n" +
                        "       CreatedDate," + "\r\n" +
                        "       ModifiedDate," + "\r\n" +
                        "       IncidentDate," + "\r\n" +
                        "       Custom," + "\r\n" +
                        "       SourceIntUniqueKey" + "\r\n" +
                        "  FROM dbo.Location" + "\r\n" +
                        " WHERE LocationID = @LocationID";

                    cmd2.Connection = conn2;
                    cmd2.CommandText =
                        "SELECT ParameterID," + "\r\n" +
                        "       LocationID," + "\r\n" +
                        "       [Name]," + "\r\n" +
                        "       [Value]" + "\r\n" +
                        "  FROM dbo.Parameter" + "\r\n" +
                        " WHERE LocationID = @LocationID";

                    cmd.Parameters.Add("@LocationID", System.Data.SqlDbType.Int).Value = locationID;

                    SqlParameter locationIdParam = cmd2.Parameters.Add("@LocationID", System.Data.SqlDbType.Int);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Location newLoc = new Location(reader);

                            locationIdParam.Value = newLoc.LocationID;

                            using (SqlDataReader reader2 = cmd2.ExecuteReader())
                            {
                                newLoc.Parameters.Load(reader2);
                            }

                            return newLoc;
                        }
                    }
                }
            }

            return null;
        }

        const string TestAuthToken = "tjxMMTgWtZ98nqRf";

        [WebMethod]
        public int AddLocation(Location locationObj, string authToken)
        {
            if (authToken != TestAuthToken)
            {
                throw new UnauthorizedAccessException("Invalid authentication token.");
            }

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "INSERT INTO [dbo].[Location] ([LocationTypeEnumCode]," + "\r\n" +
                        "                              [LocationGroupEnumCode]," + "\r\n" +
                        "                              [SourceIntUniqueKey]," + "\r\n" +
                        "                              [Name]," + "\r\n" +
                        "                              [DescriptionEn]," + "\r\n" +
                        "                              [DescriptionEs]," + "\r\n" +
                        "                              [Address1]," + "\r\n" +
                        "                              [Address2]," + "\r\n" +
                        "                              [City]," + "\r\n" +
                        "                              [State]," + "\r\n" +
                        "                              [ZipCode]," + "\r\n" +
                        "                              [Latitude]," + "\r\n" +
                        "                              [Longitude]," + "\r\n" +
                        "                              [Z]," + "\r\n" +
                        "                              [M]," + "\r\n" +
                        "                              [CreatedDate]," + "\r\n" +
                        "                              [ModifiedDate]," + "\r\n" +
                        "                              [IncidentDate]," + "\r\n" +
                        "                              [Custom])" + "\r\n" +
                        "VALUES (@LocationTypeEnumCode," + "\r\n" +
                        "        @LocationGroupEnumCode," + "\r\n" +
                        "        @SourceIntUniqueKey," + "\r\n" +
                        "        @Name," + "\r\n" +
                        "        @DescriptionEn," + "\r\n" +
                        "        @DescriptionEs," + "\r\n" +
                        "        @Address1," + "\r\n" +
                        "        @Address2," + "\r\n" +
                        "        @City," + "\r\n" +
                        "        @State," + "\r\n" +
                        "        @ZipCode," + "\r\n" +
                        "        @Latitude," + "\r\n" +
                        "        @Longitude," + "\r\n" +
                        "        @Z," + "\r\n" +
                        "        @M," + "\r\n" +
                        "        @CreatedDate," + "\r\n" +
                        "        @ModifiedDate," + "\r\n" +
                        "        @IncidentDate," + "\r\n" +
                        "        @Custom);" + "\r\n" +
                        "" + "\r\n" +
                        "SELECT SCOPE_IDENTITY ();";

                    cmd.Parameters.Add("@LocationTypeEnumCode", System.Data.SqlDbType.Int).Value = locationObj.LocationTypeEnumCode;
                    cmd.Parameters.Add("@LocationGroupEnumCode", System.Data.SqlDbType.Int).Value = locationObj.LocationGroupEnumCode;
                    cmd.Parameters.Add("@SourceIntUniqueKey", System.Data.SqlDbType.Int).Value = locationObj.SourceIntUniqueKey.GetValueOrDbNull();
                    cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 30).Value = locationObj.Name.EmptyIfNull();
                    cmd.Parameters.Add("@DescriptionEn", System.Data.SqlDbType.VarChar, 50).Value = locationObj.DescriptionEn.EmptyIfNull();
                    cmd.Parameters.Add("@DescriptionEs", System.Data.SqlDbType.VarChar, 50).Value = locationObj.DescriptionEs.EmptyIfNull();
                    cmd.Parameters.Add("@Address1", System.Data.SqlDbType.VarChar, 50).Value = locationObj.Address1.EmptyIfNull();
                    cmd.Parameters.Add("@Address2", System.Data.SqlDbType.VarChar, 50).Value = locationObj.Address2.EmptyIfNull();
                    cmd.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 30).Value = locationObj.City.EmptyIfNull();
                    cmd.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 30).Value = locationObj.State.EmptyIfNull();
                    cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.VarChar, 10).Value = locationObj.ZipCode.EmptyIfNull();
                    cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float).Value = locationObj.Latitude.GetValueOrDbNull();
                    cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float).Value = locationObj.Longitude.GetValueOrDbNull();
                    cmd.Parameters.Add("@Z", System.Data.SqlDbType.Float).Value = locationObj.Z.GetValueOrDbNull();
                    cmd.Parameters.Add("@M", System.Data.SqlDbType.Float).Value = locationObj.M.GetValueOrDbNull();
                    cmd.Parameters.Add("@CreatedDate", System.Data.SqlDbType.SmallDateTime).Value = locationObj.CreatedDate;
                    cmd.Parameters.Add("@ModifiedDate", System.Data.SqlDbType.SmallDateTime).Value = locationObj.ModifiedDate.GetValueOrDbNull();
                    cmd.Parameters.Add("@IncidentDate", System.Data.SqlDbType.SmallDateTime).Value = locationObj.IncidentDate.GetValueOrDbNull();
                    cmd.Parameters.Add("@Custom", System.Data.SqlDbType.VarChar, 512).Value = locationObj.Custom.GetValueOrDbNull();

                    object result = cmd.ExecuteScalar();

                    return (int)Convert.ToInt32(result);
                }
            }
        }

        [WebMethod]
        public List<Location> LocationSpatialSearchByRadius(double latitude, double longitude, double distanceInMeters, List<int> optionalLocationGroupFilter)
        {
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
                        "       M," + "\r\n" +
                        "       CreatedDate," + "\r\n" +
                        "       ModifiedDate," + "\r\n" +
                        "       IncidentDate," + "\r\n" +
                        "       Custom," + "\r\n" +
                        "       SourceIntUniqueKey" + "\r\n" +
                        "  FROM dbo.Location" + "\r\n" +
                        " WHERE Coordinate.STDistance (@Coordinate) <= @DistanceInMeters";

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

            return result;
        }

        [WebMethod]
        public List<Location> LocationSpatialSearchByBounds(double southWestLatitude, double southWestLongitude, double northEastLatitude, double northEastLongitude, List<int> optionalLocationGroupFilter)
        {
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
                        "       M," + "\r\n" +
                        "       CreatedDate," + "\r\n" +
                        "       ModifiedDate," + "\r\n" +
                        "       IncidentDate," + "\r\n" +
                        "       Custom," + "\r\n" +
                        "       SourceIntUniqueKey" + "\r\n" +
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

            return result;
        }

        [WebMethod]
        public List<SimpleLocation> SimpleLocationSpatialSearchByRadius(double latitude, double longitude, double distanceInMeters, List<int> optionalLocationGroupFilter)
        {
            List<SimpleLocation> result = new List<SimpleLocation>();

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
                        "       City," + "\r\n" +
                        "       Latitude," + "\r\n" +
                        "       Longitude," + "\r\n" +
                        "       IncidentDate," + "\r\n" +
                        "       Custom" + "\r\n" +
                        "  FROM dbo.Location" + "\r\n" +
                        " WHERE Coordinate.STDistance (@Coordinate) <= @DistanceInMeters";

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

                    cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float).Value = latitude;
                    cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float).Value = longitude;
                    cmd.Parameters.Add("@DistanceInMeters", System.Data.SqlDbType.Float).Value = distanceInMeters;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SimpleLocation newLoc = new SimpleLocation(reader);

                            result.Add(newLoc);
                        }
                    }
                }
            }

            return result;
        }

        [WebMethod]
        public List<SimpleLocation> SimpleLocationSpatialSearchByBounds(double southWestLatitude, double southWestLongitude, double northEastLatitude, double northEastLongitude, List<int> optionalLocationGroupFilter)
        {
            List<SimpleLocation> result = new List<SimpleLocation>();

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
                        "       City," + "\r\n" +
                        "       Latitude," + "\r\n" +
                        "       Longitude," + "\r\n" +
                        "       IncidentDate," + "\r\n" +
                        "       Custom" + "\r\n" +
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

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SimpleLocation newLoc = new SimpleLocation(reader);

                            result.Add(newLoc);
                        }
                    }
                }
            }

            return result;
        }

        [WebMethod]
        public List<SimpleLocationSummary> SimpleLocationSummaryByBounds(double southWestLatitude, double southWestLongitude, double northEastLatitude, double northEastLongitude, int locationGroupFilter)
        {
            List<SimpleLocationSummary> result = new List<SimpleLocationSummary>();

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "DECLARE @minY   VARCHAR (25) = CAST (@SouthWestLatitude AS VARCHAR (25));" + "\r\n" +
                        "DECLARE @maxY   VARCHAR (25) = CAST (@NorthEastLatitude AS VARCHAR (25));" + "\r\n" +
                        "DECLARE @minX   VARCHAR (25) = CAST (@SouthWestLongitude AS VARCHAR (25));" + "\r\n" +
                        "DECLARE @maxX   VARCHAR (25) = CAST (@NorthEastLongitude AS VARCHAR (25));" + "\r\n" +
                        "" + "\r\n" +
                        "DECLARE @BoundingRect   VARCHAR (1024);" + "\r\n" +
                        "" + "\r\n" +
                        "SET @BoundingRect = 'POLYGON((' + @minX + ' '  + @minY + ', ' + " + "\r\n" +
                        "                                  @maxX + ' ' + @minY + ', ' + " + "\r\n" +
                        "                                  @maxX + ' ' + @maxY + ', ' + " + "\r\n" +
                        "                                  @minX + ' ' + @maxY + ', ' + " + "\r\n" +
                        "                                  @minX + ' ' + @minY + '))';" + "\r\n" +
                        "                                  " + "\r\n" +
                        "DECLARE @Bounds   geography;" + "\r\n" +
                        "" + "\r\n" +
                        "SET @Bounds = geography::STPolyFromText (@BoundingRect, 4326);" + "\r\n" +
                        "" + "\r\n" +
                        "SELECT COUNT (*) AS RecordCount, [Custom]" + "\r\n" +
                        "  FROM [dbo].[Location]" + "\r\n" +
                        " WHERE     LocationGroupEnumCode = @LocationGroupEnumCode" + "\r\n" +
                        "       AND Coordinate.STWithin (@Bounds) = 1" + "\r\n" +
                        "GROUP BY [Custom]";

                    cmd.Parameters.Add("@SouthWestLatitude", System.Data.SqlDbType.Float).Value = southWestLatitude;
                    cmd.Parameters.Add("@SouthWestLongitude", System.Data.SqlDbType.Float).Value = southWestLongitude;
                    cmd.Parameters.Add("@NorthEastLatitude", System.Data.SqlDbType.Float).Value = northEastLatitude;
                    cmd.Parameters.Add("@NorthEastLongitude", System.Data.SqlDbType.Float).Value = northEastLongitude;
                    cmd.Parameters.Add("@LocationGroupEnumCode", System.Data.SqlDbType.Int).Value = locationGroupFilter;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SimpleLocationSummary newLoc = new SimpleLocationSummary(reader);

                            result.Add(newLoc);
                        }
                    }
                }
            }

            return result;
        }
    }
}
