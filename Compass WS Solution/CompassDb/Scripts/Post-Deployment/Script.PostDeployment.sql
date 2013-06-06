/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedType
        WHERE EnumeratedTypeID = 1)
   BEGIN
      INSERT INTO dbo.EnumeratedType (EnumeratedTypeID,
                                      [Name],
                                      DescriptionEn,
                                      DescriptionEs)
      VALUES (1,
              'LocationTypeEnumCode',
              'Location Type',
              'Tipo de Ubicación');
   END;

IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedType
        WHERE EnumeratedTypeID = 2)
   BEGIN
      INSERT INTO dbo.EnumeratedType (EnumeratedTypeID,
                                      [Name],
                                      DescriptionEn,
                                      DescriptionEs)
      VALUES (2,
              'LocationGroupEnumCode',
              'Location Group',
              'Grupo de Ubicación');
   END;


IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 1 AND Code = 1)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (1,
              1,
              'Coordinate',
              'Coordinate',
              'Coordenada');
   END;

IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 2 AND Code = 1)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (2,
              1,
              'TestData',
              'Test Data',
              'Data de Prueba');
   END;

IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 2 AND Code = 2)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (2,
              2,
              'PanMerchant',
              'PAN Merchant',
              'Comercio PAN');
   END;

--IF NOT EXISTS
--      (SELECT NULL AS [Empty]
--         FROM dbo.EnumeratedTypeValue
--        WHERE EnumeratedTypeID = 2 AND Code = 3)
--   BEGIN
--      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
--                                           Code,
--                                           [Name],
--                                           DescriptionEn,
--                                           DescriptionEs)
--      VALUES (2,
--              3,
--              'AreaWithoutPower',
--              'Area Without Power',
--              'Sectores Sin Servicio Eléctrico');
--   END;

IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 2 AND Code = 4)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (2,
              4,
              'CimaCareProviders',
              'CIMA  - Care Providers',
              'CIMA - Proveedores de cuido');
   END;


IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 2 AND Code = 6)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (2,
              6,
              'TypeOneCrimes',
              'Type I Crimes',
              'Crímenes Tipo I');
   END;



IF NOT EXISTS
      (SELECT NULL AS [Empty]
         FROM dbo.EnumeratedTypeValue
        WHERE EnumeratedTypeID = 2 AND Code = 7)
   BEGIN
      INSERT INTO dbo.EnumeratedTypeValue (EnumeratedTypeID,
                                           Code,
                                           [Name],
                                           DescriptionEn,
                                           DescriptionEs)
      VALUES (2,
              7,
              'BrokenHydrant',
              'Broken Hydrant',
              'Hidrante Roto');
   END;