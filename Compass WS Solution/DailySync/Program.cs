using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data.SqlClient;
using DailySync.PanEstablishmentsWS;
using System.Data.OleDb;
using ProLatNet;

namespace DailySync
{
    class Program
    {
        private const int LocationGroupEnumCode = 4;

        static int Main(string[] args)
        {
            List<Exception> exceptions = new List<Exception>();

            //try
            //{
            //    ProveedoresCuido();
            //}
            //catch (Exception ex)
            //{
            //    exceptions.Add(ex);
            //    Console.WriteLine("Error:");

            //    Console.WriteLine(ex.Message);
            //}

            //try
            //{
            //    PanEstablishments();
            //}
            //catch (Exception ex)
            //{
            //    exceptions.Add(ex);
            //    Console.WriteLine("Error:");

            //    Console.WriteLine(ex.Message);
            //}

            try
            {
                PoliceData();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
                Console.WriteLine("Error:");

                Console.WriteLine(ex.Message);
            }

            if (exceptions.Count > 0)
            {
                Console.WriteLine(String.Format("Total exception: {0}", exceptions.Count));

                Console.WriteLine();

                for (int i = 0; i < exceptions.Count; i++)
                {
                    Exception ex = exceptions[i];

                    Console.WriteLine(String.Format("Exception #{0}:", 
                                                        (i + 1),
                                                        ex.Message));
                }

                return -1;
            }

            return 0;
        }

        private static void ProveedoresCuido()
        {
            string url = Properties.Settings.Default.ProveedoresCuidoGetProvUrl;

            using (WebClient serviceRequest = new WebClient())
            {
                string proveedores = serviceRequest.DownloadString(new Uri(url));

                JArray providersArray = JsonConvert.DeserializeObject(proveedores) as JArray;

                if (providersArray == null)
                {
                    throw new ApplicationException("The response is not a Json array.");
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
                            "                              [Longitude])" + "\r\n" +
                            "VALUES (1," + "\r\n" +
                            "        4," + "\r\n" +
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
                            "        @Longitude);";

                        SqlParameter sourceIntUniqueKeyParam = cmd.Parameters.Add("@SourceIntUniqueKey", System.Data.SqlDbType.Int);
                        SqlParameter nameParam = cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter descriptionEnParam = cmd.Parameters.Add("@DescriptionEn", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter descriptionEsParam = cmd.Parameters.Add("@DescriptionEs", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter address1Param = cmd.Parameters.Add("@Address1", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter address2Param = cmd.Parameters.Add("@Address2", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter cityParam = cmd.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter stateParam = cmd.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter zipCodeParam = cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.VarChar, 10);
                        SqlParameter latitudeParam = cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float);
                        SqlParameter longitudeParam = cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float);

                        stateParam.Value = "PR";

                        foreach (JObject item in providersArray)
                        {
                            string proveedorId = item.Value<string>("ProveedorID");
                            string nombre = item.Value<string>("Nombre");

                            int provUniqueKey;

                            if (int.TryParse(proveedorId, out provUniqueKey))
                            {
                                string getUrl = string.Format(Properties.Settings.Default.ProveedoresCuidoGetProvByIdTemplateUrl, provUniqueKey);

                                string providerData = serviceRequest.DownloadString(new Uri(getUrl));

                                JObject providerObj = JsonConvert.DeserializeObject(providerData) as JObject;

                                if (providerObj != null)
                                {
                                    string tipoProveedor = providerObj.Value<string>("TipoProveedor").EnsureNotNull(true);
                                    string direccionResidencial1 = providerObj.Value<string>("DireccionResidencial1").EnsureNotNull(true);
                                    string direccionResidencial2 = providerObj.Value<string>("DireccionResidencial2").EnsureNotNull(true);
                                    string zipCodeResidencial = providerObj.Value<string>("ZipCodeResidencial").EnsureNotNull(true);
                                    string ciudadResidencial = providerObj.Value<string>("CiudadResidencial").EnsureNotNull(true);
                                    string latitudLongitud = providerObj.Value<string>("LatitudLongitud").EnsureNotNull(true);

                                    if (latitudLongitud.Contains(","))
                                    {
                                        string[] latLong = latitudLongitud.Split(new char[] {','});

                                        double latitud;
                                        double longitud;

                                        if (latLong.Length > 1 && double.TryParse(latLong[0], out latitud) && double.TryParse(latLong[1], out longitud))
                                        {
                                            sourceIntUniqueKeyParam.Value = provUniqueKey;
                                            nameParam.Value = nombre;
                                            descriptionEnParam.Value = tipoProveedor;
                                            descriptionEsParam.Value = tipoProveedor;
                                            address1Param.Value = direccionResidencial1;
                                            address2Param.Value = direccionResidencial2;
                                            cityParam.Value = ciudadResidencial;
                                            
                                            zipCodeParam.Value = zipCodeResidencial;
                                            latitudeParam.Value = latitud;
                                            longitudeParam.Value = longitud;

                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void PanEstablishments()
        {
            using (PanEstablishmentsWS.wsEstablishmentsSoapClient client = new PanEstablishmentsWS.wsEstablishmentsSoapClient("wsEstablishmentsSoap"))
            {
                OBJ_Establishment[] establishments = client.getEstablishments("activo", "*", "*");

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
                            "                              [Longitude])" + "\r\n" +
                            "VALUES (1," + "\r\n" +
                            "        5," + "\r\n" +
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
                            "        @Longitude);";

                        SqlParameter sourceIntUniqueKeyParam = cmd.Parameters.Add("@SourceIntUniqueKey", System.Data.SqlDbType.Int);
                        SqlParameter nameParam = cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter descriptionEnParam = cmd.Parameters.Add("@DescriptionEn", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter descriptionEsParam = cmd.Parameters.Add("@DescriptionEs", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter address1Param = cmd.Parameters.Add("@Address1", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter address2Param = cmd.Parameters.Add("@Address2", System.Data.SqlDbType.VarChar, 50);
                        SqlParameter cityParam = cmd.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter stateParam = cmd.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 30);
                        SqlParameter zipCodeParam = cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.VarChar, 10);
                        SqlParameter latitudeParam = cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float);
                        SqlParameter longitudeParam = cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float);

                        stateParam.Value = "PR";

                        foreach (OBJ_Establishment item in establishments)
                        {
                            int proveedorId = item.estId;
                            string nombre = item.estName.EnsureNotNull(true);

                            string estAuthNumber = item.estAuthNumber.EnsureNotNull(true);
                            string direccionResidencial1 = item.estAddPhysical1.EnsureNotNull(true);
                            string direccionResidencial2 = item.estAddPhysical2.EnsureNotNull(true);
                            string zipCodeResidencial = item.estAddPhysicalZipCode.EnsureNotNull(true);
                            string ciudadResidencial = item.estAddPhysicalCity.EnsureNotNull(true);
                            string latitudLongitud = item.estLatLong.EnsureNotNull(true);

                            if (latitudLongitud.Contains(","))
                            {
                                string[] latLong = latitudLongitud.Split(new char[] { ',' });

                                double latitud;
                                double longitud;

                                if (latLong.Length > 1 && double.TryParse(latLong[0], out latitud) && double.TryParse(latLong[1], out longitud))
                                {
                                    sourceIntUniqueKeyParam.Value = proveedorId;
                                    nameParam.Value = nombre;
                                    descriptionEnParam.Value = estAuthNumber;
                                    descriptionEsParam.Value = estAuthNumber;
                                    address1Param.Value = direccionResidencial1;
                                    address2Param.Value = direccionResidencial2;
                                    cityParam.Value = ciudadResidencial;

                                    zipCodeParam.Value = zipCodeResidencial;
                                    latitudeParam.Value = latitud;
                                    longitudeParam.Value = longitud;

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }

        }

        private static void PoliceData()
        {
            string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Jose\Dropbox\dev\net\2010\Truenorth\Compass\Solution Files\Data\SampleMDB.mdb;Persist Security Info=False";

            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CompassDbConnString))
            using (OleDbConnection oleConn = new OleDbConnection(connStr))
            {
                conn.Open();
                oleConn.Open();
                

                using (SqlCommand cmd = new SqlCommand())
                using (OleDbCommand oleCmd = new OleDbCommand())
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
                        "                              [IncidentDate])" + "\r\n" +
                        "VALUES (1," + "\r\n" +
                        "        6," + "\r\n" +
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
                        "        @IncidentDate);";

                    SqlParameter sourceIntUniqueKeyParam = cmd.Parameters.Add("@SourceIntUniqueKey", System.Data.SqlDbType.Int);
                    SqlParameter nameParam = cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 30);
                    SqlParameter descriptionEnParam = cmd.Parameters.Add("@DescriptionEn", System.Data.SqlDbType.VarChar, 50);
                    SqlParameter descriptionEsParam = cmd.Parameters.Add("@DescriptionEs", System.Data.SqlDbType.VarChar, 50);
                    SqlParameter address1Param = cmd.Parameters.Add("@Address1", System.Data.SqlDbType.VarChar, 50);
                    SqlParameter address2Param = cmd.Parameters.Add("@Address2", System.Data.SqlDbType.VarChar, 50);
                    SqlParameter cityParam = cmd.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 30);
                    SqlParameter stateParam = cmd.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 30);
                    SqlParameter zipCodeParam = cmd.Parameters.Add("@ZipCode", System.Data.SqlDbType.VarChar, 10);
                    SqlParameter latitudeParam = cmd.Parameters.Add("@Latitude", System.Data.SqlDbType.Float);
                    SqlParameter longitudeParam = cmd.Parameters.Add("@Longitude", System.Data.SqlDbType.Float);
                    SqlParameter incidentDateParam = cmd.Parameters.Add("@IncidentDate", System.Data.SqlDbType.DateTime);

                    stateParam.Value = "PR";
                    address1Param.Value = string.Empty;
                    address2Param.Value = string.Empty;
                    cityParam.Value = string.Empty;
                    zipCodeParam.Value = string.Empty;

                    oleCmd.Connection = oleConn;
                    oleCmd.CommandText =
                        "SELECT incidencia2013sample.[OBJECTID]," + "\r\n" +
                        "       incidencia2013sample.[fecha_ocurrencia]," + "\r\n" +
                        "       incidencia2013sample.[hora_ocurrencia]," + "\r\n" +
                        "       incidencia2013sample.[FK_delito_cometido_Tipo_I]," + "\r\n" +
                        "       incidencia2013sample.[POINT_X]," + "\r\n" +
                        "       incidencia2013sample.[POINT_Y]" + "\r\n" +
                        "  FROM incidencia2013sample;";

                    CoordSys A = CoordSys.GetCS("US_SPC83", "PRVI83", "PUERTO RICO", "METERS");
                    CoordSys B = CoordSys.GetCS("LAT_LONG", "LAT-LONG", "PUERTO RICO", "METERS");

                    double[] LonX = new double[1];
                    double[] LatY = new double[1];
                    double[] Z = new double[1];

                    Z[0] = 0.0;

                    using (OleDbDataReader oleReader = oleCmd.ExecuteReader())
                    {
                        while (oleReader.Read())
                        {
                            if (!oleReader.IsDBNull(0) && !oleReader.IsDBNull(1) && !oleReader.IsDBNull(2) && !oleReader.IsDBNull(3) && !oleReader.IsDBNull(4) && !oleReader.IsDBNull(5))
                            {
                                int objId = oleReader.GetInt32(0);
                                DateTime fecha_ocurrencia = oleReader.GetDateTime(1);
                                string hora_ocurrencia = oleReader.GetString(2).EnsureNotNull(true);
                                int delito_cometido_Tipo_I = oleReader.GetInt32(3);
                                double x = oleReader.GetDouble(4);
                                double y = oleReader.GetDouble(5);

                                DateTime hour;

                                if (DateTime.TryParse(hora_ocurrencia, out hour))
                                {
                                    fecha_ocurrencia = new DateTime(fecha_ocurrencia.Year, fecha_ocurrencia.Month, fecha_ocurrencia.Day, hour.Hour, hour.Minute, hour.Second);
                                }

                                int proveedorId = objId;
                                string nombre = "Delito Tipo I - " + delito_cometido_Tipo_I.ToString();

                                string estAuthNumber = delito_cometido_Tipo_I.ToString();

                                LonX[0] = DMS.GetLon(x.ToString());
                                LatY[0] = DMS.GetLat(y.ToString());

                                CoordSys.Transform(A, B, LonX, LatY, Z, 1);

                                double longitud = Math.Round(LonX[0], 9);
                                double latitud = Math.Round(LatY[0], 9);

                                sourceIntUniqueKeyParam.Value = proveedorId;
                                nameParam.Value = nombre;
                                descriptionEnParam.Value = estAuthNumber;
                                descriptionEsParam.Value = estAuthNumber;

                                latitudeParam.Value = latitud;
                                longitudeParam.Value = longitud;

                                incidentDateParam.Value = fecha_ocurrencia;

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }


        }
    }
}
