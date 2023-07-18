{
  "Version": "8.0.9.1582",
  "UId": "447399cf-9eed-43de-96ff-f1db31473cfd",
  "ManagerName": "SourceCodeSchemaManager",
  "Name": "GenNeuralNetworkAdapter",
  "Caption": "GenNeuralNetworkAdapter",
  "ExtendParent": false,
  "DenyExtending": false,
  "Description": "",
  "SourceCode": "namespace Terrasoft.Configuration.NeuralNetwork
\n{
\n\tusing System;
\n\tusing System.Collections.Generic;
\n\tusing System.Linq;
\n\tusing Newtonsoft.Json.Linq;
\n\tusing Newtonsoft.Json;
\n\tusing Terrasoft.Core;
\n\tusing Terrasoft.Core.DB;
\n\tusing Terrasoft.Core.Entities;
\n\tusing Terrasoft.Core.Configuration;
\n\tusing Terrasoft.Common;
\n\tusing Terrasoft.Web.Common;
\n\tusing Terrasoft.Web.Http.Abstractions;
\n\t
\n\tpublic class GenNeuralNetworkAdapter
\n\t{
\n\t\tpublic Guid GenRequestId { get; set; }//запит
\n\t\tpublic List<NeuroNode> Searches { get; set; }
\n\t\t
\n\t\tprivate UserConnection _userConnection;
\n\t\tpublic UserConnection UserConnection {
\n\t\t\tget {
\n\t\t\t\treturn _userConnection ??
\n\t\t\t\t\t(_userConnection = HttpContext.Current.Session[\"UserConnection\"] as UserConnection);
\n\t\t\t}
\n\t\t\tset {
\n\t\t\t\t_userConnection = value;
\n\t\t\t}
\n\t\t}
\n\t\t
\n\t\tpublic bool SearchVehicle(Guid requestId, Guid GenSearchTransportId)
\n\t\t{
\n\t\t\tint dateMutual = 1;
\n\t\t\tRequest req = new Request();
\n\t\t\tVehicle veh = new Vehicle();
\n\t\t\treq.Id = requestId;
\n\t\t\t
\n\t\t\t
\n\t\t\t//DateTime GenDownloadDateFrom = DateTime.MinValue;
\n\t\t\t//DateTime GenDownloadDateTo = DateTime.MinValue;
\n\t\t\t//Guid GenOwnerId = Guid.Empty;
\n\t\t\t//Guid GenAuthorId = Guid.Empty;
\n\t\t\t//Guid GenDownloadTypeId = Guid.Empty;
\n\t\t\t//bool GenIsTemperatureRegime = false;
\n\t\t\t//decimal GenVolume = 0m;
\n\t\t\t//decimal GenTonnage = 0m;
\n\t\t\t//Guid GenVehicleTypeId = Guid.Empty;
\n\t\t\t////decimal GenWidth = 0m;
\n\t\t\t//Guid GenManagerTeamId = Guid.Empty;
\n\t\t\t//Guid GenDepartmentId = Guid.Empty;
\n\t\t\t//Guid DownloadGenCountryId = Guid.Empty;
\n\t\t\t//Guid DownloadGenRegionId = Guid.Empty;
\n\t\t\t//Guid DownloadGenCityId = Guid.Empty;
\n\t\t\t//Guid UnloadingGenCountryId = Guid.Empty;
\n\t\t\t//Guid UnloadingGenRegionId = Guid.Empty;
\n\t\t\t//Guid UnloadingGenCityId = Guid.Empty;
\n\t\t\t//
\n\t\t\t////дані, які зчитали з транспорту
\n\t\t\t//Guid VehicleGenAuthorId = Guid.Empty;
\n\t\t\t//Guid VehicleGenCityLocationId = Guid.Empty;
\n\t\t\t//Guid VehicleGenCityTargetId = Guid.Empty;
\n\t\t\t//Guid VehicleGenCountryLocationId = Guid.Empty;
\n\t\t\t//Guid VehicleGenCountryTargetId = Guid.Empty;
\n\t\t\t//
\n\t\t\t//Guid VehicleGenDownloadTypeId = Guid.Empty;
\n\t\t\t//
\n\t\t\t//DateTime VehicleGenDueDate = DateTime.MinValue;
\n\t\t\t//int VehicleGenFree = 0;
\n\t\t\t//
\n\t\t\t//Guid VehicleGenRegionLocationId = Guid.Empty;
\n\t\t\t//Guid VehicleGenRegionTargetId = Guid.Empty;
\n\t\t\t//DateTime VehicleGenStartDate = DateTime.MinValue;
\n\t\t\t//Guid VehicleGenStateId = Guid.Empty;
\n\t\t\t//decimal VehicleGenVolume = 0m;
\n\t\t\t//Guid VehicleGenVehicleTypeId = Guid.Empty;
\n\t\t\t//decimal VehicleGenTonage = 0m;
\n\t\t\t//Guid VehicleGenManagerTeamId = Guid.Empty;
\n\t\t\t//Guid VehicleGenDepartmentId = Guid.Empty;
\n\t\t\t
\n\t\t\tif (requestId == Guid.Empty)
\n\t\t\t\treturn false;
\n\t\t\tif (GenSearchTransportId == Guid.Empty)
\n\t\t\t\treturn false;
\n\t\t\t
\n\t\t\t
\n\t\t\tSearches = new List<NeuroNode>();
\n\t\t\tvar topology = new Topology(19, 1, 0.1, 25, 10); // 14 - hidden layer з 14ма нейронами 
\n\t\t\tvar neuralNetwork = new NeuralNetwork(topology);
\n
\n\t\t\t
\n\t\t\tGenRequestId = requestId;
\n\t\t\t//прочитати запит з бази\\, зберегти в локальні параметри поля для підбору
\n\t\t\tSelect selReq = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenRequest\",\"Id\")\t\t\t\t\t\t
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadDateFrom\")\t\t//1
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadDateTo\")\t\t//2
\n\t\t\t\t.Column(\"GenRequest\",\"GenOwnerId\")\t\t\t\t//3
\n\t\t\t\t.Column(\"GenRequest\",\"GenAuthorId\")\t\t\t\t//4
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadTypeId\")\t\t//5 DELETE
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsTemperatureRegime\")\t//6
\n\t\t\t\t.Column(\"GenRequest\",\"GenVolume\")\t\t\t\t//7
\n\t\t\t\t.Column(\"GenRequest\",\"GenTonnage\")\t\t\t\t//8
\n\t\t\t\t.Column(\"GenRequest\",\"GenVehicleTypeId\")\t\t//9
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsTopDownload\")//10
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsBackDownload\")//11
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsSideDownload\")//12
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsEmbankment\")//13
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsPouring\")//14
\n\t\t\t\t//GenIsEmbankment   GenIsPouring
\n\t\t\t\t//GenOwner? , GenState?
\n\t\t\t\t.From(\"GenRequest\")
\n\t\t\t\t.Where(\"Id\").IsEqual(Column.Parameter(GenRequestId))
\n\t\t\tas Select;
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selReq.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\tif (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\treq.GenDownloadDateFrom = (reader.GetValue(1) != System.DBNull.Value) ? (DateTime)reader.GetValue(1) : DateTime.MinValue;
\n\t\t\t\t\t\treq.GenDownloadDateTo = (reader.GetValue(2) != System.DBNull.Value) ? (DateTime)reader.GetValue(2) : DateTime.MinValue;
\n\t\t\t\t\t\treq.GenOwnerId = (reader.GetValue(3) != System.DBNull.Value) ? (Guid)reader.GetValue(3) : Guid.Empty;
\n\t\t\t\t\t\treq.GenAuthorId = (reader.GetValue(4) != System.DBNull.Value) ? (Guid)reader.GetValue(4) : Guid.Empty;
\n\t\t\t\t\t\treq.GenDownloadTypeId = (reader.GetValue(5) != System.DBNull.Value) ? (Guid)reader.GetValue(5) : Guid.Empty;
\n\t\t\t\t\t\treq.GenIsTemperatureRegime = (reader.GetValue(6) != System.DBNull.Value) ? (bool)reader.GetValue(6) : false;
\n\t\t\t\t\t\treq.GenVolume = (reader.GetValue(7) != System.DBNull.Value) ? (decimal)reader.GetValue(7) : 0m;
\n\t\t\t\t\t\treq.GenTonnage = (reader.GetValue(8) != System.DBNull.Value) ? (decimal)reader.GetValue(8) : 0m;
\n\t\t\t\t\t\treq.GenVehicleTypeId = (reader.GetValue(9) != System.DBNull.Value) ? (Guid)reader.GetValue(9) : Guid.Empty;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\treq.GenIsTopDownload = (reader.GetValue(10) != System.DBNull.Value) ? (bool)reader.GetValue(10) : false;
\n\t\t\t\t\t\treq.GenIsBackDownload = (reader.GetValue(11) != System.DBNull.Value) ? (bool)reader.GetValue(11) : false;
\n\t\t\t\t\t\treq.GenIsSideDownload = (reader.GetValue(12) != System.DBNull.Value) ? (bool)reader.GetValue(12) : false;
\n\t\t\t\t\t\treq.GenIsEmbankment = (reader.GetValue(13) != System.DBNull.Value) ? (bool)reader.GetValue(13) : false;
\n\t\t\t\t\t\treq.GenIsPouring = (reader.GetValue(14) != System.DBNull.Value) ? (bool)reader.GetValue(14) : false;
\n\t\t\t\t\t\t
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\tif (req.GenDownloadDateFrom == DateTime.MinValue)
\n\t\t\t\treturn false;
\n\t\t\tif (req.GenDownloadDateTo == DateTime.MinValue)
\n\t\t\t\treq.GenDownloadDateTo = req.GenDownloadDateFrom;
\n\t\t\t
\n\t\t\tif (req.GenAuthorId != Guid.Empty)
\n\t\t\t{
\n\t\t\t\tSelect selByAuthor = new Select(UserConnection)
\n\t\t\t\t\t.Column(\"GenTeam\", \"GenManagerTeamId\")\t\t//0
\n\t\t\t\t\t.Column(\"GenManagerTeam\", \"GenDepartmentId\")//1
\n\t\t\t\t\t
\n\t\t\t\t\t.From(\"GenTeam\").As(\"GenTeam\")
\n\t\t\t\t\t.LeftOuterJoin(\"GenManagerTeam\").As(\"GenManagerTeam\")
\n\t\t\t\t\t.On(\"GenManagerTeam\", \"Id\").IsEqual(\"GenTeam\", \"GenManagerTeamId\")
\n\t\t\t\t\t.Where(\"GenTeam\", \"GenEmployeeId\").IsEqual(Column.Parameter(req.GenAuthorId))
\n\t\t\t\tas Select;
\n\t\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t\t{
\n\t\t\t\t\tusing (var reader = selByAuthor.ExecuteReader(dbExecutor))
\n\t\t\t\t\t{
\n\t\t\t\t\t\tif (reader.Read())
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\treq.GenManagerTeamId = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\treq.GenDepartmentId = (reader.GetValue(1) != System.DBNull.Value) ? (Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t}
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\tSelect selDownload = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenCountryId\")
\n\t\t\t\t.Column(\"GenRegionId\")
\n\t\t\t\t.Column(\"GenCityId\")
\n\t\t\t\t
\n\t\t\t\t.From(\"GenDownloadPoint\")
\n\t\t\t\t.Where(\"GenRequestId\").IsEqual(Column.Parameter(req.Id))
\n\t\t\tas Select;
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selDownload.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\tif (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\treq.DownloadGenCountryId = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\treq.DownloadGenRegionId = (reader.GetValue(1) != System.DBNull.Value) ? (Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\treq.DownloadGenCityId = (reader.GetValue(2) != System.DBNull.Value) ? (Guid)reader.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\tSelect selUnloading = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenCountryId\")
\n\t\t\t\t.Column(\"GenRegionId\")
\n\t\t\t\t.Column(\"GenCityId\")
\n\t\t\t\t
\n\t\t\t\t.From(\"GenUnloadingPoint\")
\n\t\t\t\t.Where(\"GenRequestId\").IsEqual(Column.Parameter(req.Id))
\n\t\t\tas Select;
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selUnloading.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\tif (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\treq.UnloadingGenCountryId = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\treq.UnloadingGenRegionId = (reader.GetValue(1) != System.DBNull.Value) ? (Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\treq.UnloadingGenCityId = (reader.GetValue(2) != System.DBNull.Value) ? (Guid)reader.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t//new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\")  Україна
\n\t\t\tif (req.DownloadGenCountryId != new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\") || req.UnloadingGenCountryId != new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\"))
\n\t\t\t\tdateMutual = 7;
\n\t\t\t
\n\t\t\t//прочитати з бази весь вільний актуальний транспорт, 
\n\t\t\t//select...
\n\t\t\tSelect selectVehicle = new Select(UserConnection)
\n\t\t\t.Column(\"GenVehicle\",\"Id\")
\n\t\t\t.Column(\"GenVehicle\",\"GenAuthorId\")//1
\n\t\t\t.Column(\"GenVehicle\",\"GenCityLocationId\")//2
\n\t\t\t.Column(\"GenVehicle\",\"GenCityTargetId\")//3
\n\t\t\t.Column(\"GenVehicle\",\"GenCountryLocationId\")//4
\n\t\t\t.Column(\"GenVehicle\",\"GenCountryTargetId\")//5
\n\t\t\t.Column(\"GenVehicle\",\"GenDownloadTypeId\")//6
\n\t\t\t.Column(\"GenVehicle\",\"GenDueDate\")//7
\n\t\t\t.Column(\"GenVehicle\",\"GenFree\")//8
\n\t\t\t.Column(\"GenVehicle\",\"GenRegionLocationId\")//9
\n\t\t\t.Column(\"GenVehicle\",\"GenRegionTargetId\")//10
\n\t\t\t.Column(\"GenVehicle\",\"GenStartDate\")//11
\n\t\t\t.Column(\"GenVehicle\",\"GenStateId\")//12
\n\t\t\t.Column(\"GenVehicle\",\"GenVolume\")//13
\n\t\t\t.Column(\"GenVehicle\",\"GenVehicleTypeId\")//14
\n\t\t\t.Column(\"GenVehicle\",\"GenTonage\")//15
\n\t\t\t.Column(\"GenVehicle\",\"GenIsTopDownload\")//16
\n\t\t\t.Column(\"GenVehicle\",\"GenIsBackDownload\")//17
\n\t\t\t.Column(\"GenVehicle\", \"GenIsSideDownload\")//18
\n\t\t\t.Column(\"GenVehicle\", \"GenIsEmbankment\")//19
\n\t\t\t.Column(\"GenVehicle\", \"GenIsPouring\")//20
\n\t\t\t\t
\n\t\t\t.From(\"GenVehicle\")
\n\t\t\t.Where()
\n\t\t\t\t.OpenBlock(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"3ef75b2a-19da-4653-9966-8b2320e22d79\")))//Активно
\n\t\t\t\t.Or(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"618691db-1545-4086-a463-4341cf664268\")))//Реверс
\n\t\t\t\t.CloseBlock()
\n\t\t\t\t.And(\"GenStartDate\").IsLessOrEqual(Column.Parameter(req.GenDownloadDateTo.AddDays(dateMutual)))//дата по з запиту+7днів/1-україна
\n\t\t\t\t.And(\"GenDueDate\").IsGreaterOrEqual(Column.Parameter(req.GenDownloadDateFrom))//дата з з запиту
\n\t\t\tas Select;
\n\t\t\t//while(reader.Read()){...
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selectVehicle.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\twhile (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\tveh = new Vehicle();
\n\t\t\t\t\t\tNeuroNode node = new NeuroNode();
\n\t\t\t\t\t\tnode.InputVal = new List<double>();
\n\t\t\t\t\t\tnode.GenVehicleId = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\tveh.VehicleGenAuthorId = (reader.GetValue(1) != System.DBNull.Value) ? (Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenCityLocationId = (reader.GetValue(2) != System.DBNull.Value) ? (Guid)reader.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenCityTargetId = (reader.GetValue(3) != System.DBNull.Value) ? (Guid)reader.GetValue(3) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenCountryLocationId = (reader.GetValue(4) != System.DBNull.Value) ? (Guid)reader.GetValue(4) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenCountryTargetId = (reader.GetValue(5) != System.DBNull.Value) ? (Guid)reader.GetValue(5) : Guid.Empty;
\n
\n\t\t\t\t\t\tveh.VehicleGenDownloadTypeId = (reader.GetValue(6) != System.DBNull.Value) ? (Guid)reader.GetValue(6) : Guid.Empty;
\n
\n\t\t\t\t\t\tveh.VehicleGenDueDate =(reader.GetValue(7) != System.DBNull.Value) ? (DateTime)reader.GetValue(7) : DateTime.MinValue;
\n\t\t\t\t\t\t//VehicleGenFree = 
\n
\n\t\t\t\t\t\tveh.VehicleGenRegionLocationId = (reader.GetValue(9) != System.DBNull.Value) ? (Guid)reader.GetValue(9) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenRegionTargetId = (reader.GetValue(10) != System.DBNull.Value) ? (Guid)reader.GetValue(10) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenStartDate = (reader.GetValue(11) != System.DBNull.Value) ? (DateTime)reader.GetValue(11) : DateTime.MinValue;
\n\t\t\t\t\t\t//VehicleGenStateId = (reader.GetValue(12) != System.DBNull.Value) ? (Guid)reader.GetValue(12) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenVolume = (reader.GetValue(13) != System.DBNull.Value) ? (decimal)reader.GetValue(13) : 0m;
\n\t\t\t\t\t\tveh.VehicleGenVehicleTypeId = (reader.GetValue(14) != System.DBNull.Value) ? (Guid)reader.GetValue(14) : Guid.Empty;
\n\t\t\t\t\t\tveh.VehicleGenTonage = (reader.GetValue(15) != System.DBNull.Value) ? (decimal)reader.GetValue(15) : 0m;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\tveh.VehicleGenIsTopDownload = (reader.GetValue(16) != System.DBNull.Value) ? (bool)reader.GetValue(16) : false;
\n\t\t\t\t\t\tveh.VehicleGenIsBackDownload = (reader.GetValue(17) != System.DBNull.Value) ? (bool)reader.GetValue(17) : false;
\n\t\t\t\t\t\tveh.VehicleGenIsSideDownload = (reader.GetValue(18) != System.DBNull.Value) ? (bool)reader.GetValue(18) : false;
\n\t\t\t\t\t\tveh.VehicleGenIsEmbankment = (reader.GetValue(19) != System.DBNull.Value) ? (bool)reader.GetValue(19) : false;
\n\t\t\t\t\t\tveh.VehicleGenIsPouring = (reader.GetValue(20) != System.DBNull.Value) ? (bool)reader.GetValue(20) : false;
\n
\n\t\t\t\t\t\tif (veh.VehicleGenAuthorId != Guid.Empty)
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tSelect selByVehicleAuthor = new Select(UserConnection)
\n\t\t\t\t\t\t\t\t.Column(\"GenTeam\", \"GenManagerTeamId\")\t\t//0
\n\t\t\t\t\t\t\t\t.Column(\"GenManagerTeam\", \"GenDepartmentId\")//1
\n
\n\t\t\t\t\t\t\t\t.From(\"GenTeam\").As(\"GenTeam\")
\n\t\t\t\t\t\t\t\t.LeftOuterJoin(\"GenManagerTeam\").As(\"GenManagerTeam\")
\n\t\t\t\t\t\t\t\t.On(\"GenManagerTeam\", \"Id\").IsEqual(\"GenTeam\", \"GenManagerTeamId\")
\n\t\t\t\t\t\t\t\t.Where(\"GenTeam\", \"GenEmployeeId\").IsEqual(Column.Parameter(veh.VehicleGenAuthorId))
\n\t\t\t\t\t\t\tas Select;
\n\t\t\t\t\t\t\tusing (var dbExecutorInner1 = UserConnection.EnsureDBConnection())
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tusing (var readerInner1 = selByVehicleAuthor.ExecuteReader(dbExecutorInner1))
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tif (readerInner1.Read())
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\tveh.VehicleGenManagerTeamId = (readerInner1.GetValue(0) != System.DBNull.Value) ? (Guid)readerInner1.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\t\tveh.VehicleGenDepartmentId = (readerInner1.GetValue(1) != System.DBNull.Value) ? (Guid)readerInner1.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t}
\n\t\t\t\t\t\t
\n\t\t\t\t\t\t//read into loop llocal params vals
\n\t\t\t\t\t\t//треба отримати запит та транспорт
\n\t\t\t\t\t\tnode.InputVal = CalcInputVal(req,veh);
\n\t\t\t\t\t\t//}
\n\t\t\t\t\t\t//calc input vals for neuronNetwork
\n\t\t\t\t\t\t//node.InputVal.Add(); // 0/1 
\n\t\t\t\t\t\t//................
\n\t\t\t\t\t\tnode.result = neuralNetwork.WorkHard(node.InputVal, 100000);
\n\t\t\t\t\t\t
\n\t\t\t\t\t\tSearches.Add(node);
\n\t\t\t\t\t\t
\n\t\t\t\t\t\t
\n\t\t\t\t\t\t
\n\t\t\t\t\t\t//GenSearchTransportId
\n\t\t\t\t\t\t//call neuro
\n\t\t\t\t\t\t//insert into db
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\tif (Searches.Count > 0)
\n\t\t\t{
\n\t\t\t\tInsert _insertSearches = new Insert(UserConnection)
\n\t\t\t\t\t.Into(\"GenSearches\");
\n\t\t\t\tforeach (var item in Searches)
\n\t\t\t\t{
\n\t\t\t\t\tstring inputs = JsonConvert.SerializeObject(item.InputVal);
\n
\n\t\t\t\t\t_insertSearches = _insertSearches.Values()
\n\t\t\t\t\t\t.Set(\"GenInputValues\", Column.Parameter(inputs))
\n\t\t\t\t\t\t.Set(\"GenResult\", Column.Parameter(item.result))
\n\t\t\t\t\t\t.Set(\"GenRate\", Column.Parameter((decimal)(item.result*100d)))
\n\t\t\t\t\t\t.Set(\"GenSearchTransportId\", Column.Parameter(GenSearchTransportId))
\n\t\t\t\t\t\t.Set(\"GenVehicleId\", Column.Parameter(item.GenVehicleId));
\n\t\t\t\t}
\n\t\t\t\t_insertSearches.Execute();
\n\t\t\t}
\n\t\t\t
\n\t\t\t
\n\t\t\t//NeuroNode node = new NeuroNode();
\n\t\t\t//node.GenVehicleId = //id транспорту, що прочитали
\n\t\t\t//node.InputVal = new List<double>();
\n\t\t\t//проходимо по параметрах запиту і трнспорту, на їх основі формуємо вхідний масив
\n\t\t\t//після всього
\n\t\t\t//ініціалізуємо мережу
\n\t\t\t//проходимо по колекції Searches передаєм мережі InputVal та зберігаємо результат обчислення в result
\n\t\t\treturn true;
\n\t\t}
\n\t\t
\n\t\tpublic bool LearnOnRealData(DateTime dateFrom)
\n\t\t{
\n\t\t\tSearches = new List<NeuroNode>();
\n\t\t\t
\n\t\t\tbool res = false;
\n\t\t\t
\n\t\t\tList<Request> requests = new List<Request>();
\n\t\t\tList<Vehicle> vehicles = new List<Vehicle>();
\n\t\t\t//List<Transit> transits = new List<Transit>();
\n\t\t\t//Dictionary<Guid,Guid> transits = new Dictionary<Guid,Guid>();
\n\t\t\t
\n\t\t\tDictionary<string,Guid> transits = new Dictionary<string,Guid>();
\n\t\t\t//багато коду
\n\t\t\t
\n\t\t\t
\n\t\t\tvar topology = new Topology(19, 1, 0.1, 25, 10); // 14 - hidden layer з 14ма нейронами 
\n\t\t\tvar neuralNetwork = new NeuralNetwork(topology);
\n\t\t\t
\n\t\t\t// у нас вже є ноди з сформованими в них вхідними даними адаптованими під мережу 
\n\t\t\t// нам треба на основі існуючих рейсів, навчити нашу нейронну мережу, тобто 
\n\t\t\t// у нас в базі є сформований рейс, який включає в себе транспорт та запит, 
\n\t\t\t// у випадку якщо менеджер відтворює аналогічний запит (крім дата з, дата по), то ми маємо в ноду в result передати значення 1(так як такий рейс вже був, а значить транспорт відповідає запитк) 
\n\t\t\t
\n\t\t\t// треба створити два класи, в одному з яких поля по транспорту, в другому по запиту
\n\t\t\tSelect selReq = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenRequest\",\"Id\")\t\t\t\t\t\t
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadDateFrom\")\t\t//1
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadDateTo\")\t\t//2
\n\t\t\t\t.Column(\"GenRequest\",\"GenOwnerId\")\t\t\t\t//3
\n\t\t\t\t.Column(\"GenRequest\",\"GenAuthorId\")\t\t\t\t//4
\n\t\t\t\t.Column(\"GenRequest\",\"GenDownloadTypeId\")\t\t//5
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsTemperatureRegime\")\t//6
\n\t\t\t\t.Column(\"GenRequest\",\"GenVolume\")\t\t\t\t//7
\n\t\t\t\t.Column(\"GenRequest\",\"GenTonnage\")\t\t\t\t//8
\n\t\t\t\t.Column(\"GenRequest\",\"GenVehicleTypeId\")\t\t//9
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsTopDownload\")//10
\n\t\t\t\t.Column(\"GenRequest\",\"GenIsBackDownload\")//11
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsSideDownload\")//12
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsEmbankment\")//13
\n\t\t\t\t.Column(\"GenRequest\", \"GenIsPouring\")//14
\n\t\t\t\t.From(\"GenRequest\")
\n\t\t\t\t.Where(\"GenDownloadDateFrom\").IsGreaterOrEqual(Column.Parameter(dateFrom))
\n\t\t\tas Select;
\n\t\t\t
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selReq.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\twhile (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\t//створюємо сутність нашого запиту
\n\t\t\t\t\t\tRequest request = new Request();
\n
\n\t\t\t\t\t\trequest.Id = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\trequest.GenDownloadDateFrom = (reader.GetValue(1) != System.DBNull.Value) ? (DateTime)reader.GetValue(1) : DateTime.MinValue;
\n\t\t\t\t\t\trequest.GenDownloadDateTo = (reader.GetValue(2) != System.DBNull.Value) ? (DateTime)reader.GetValue(2) : DateTime.MinValue;
\n\t\t\t\t\t\trequest.GenOwnerId = (reader.GetValue(3) != System.DBNull.Value) ? (Guid)reader.GetValue(3) : Guid.Empty;
\n\t\t\t\t\t\trequest.GenAuthorId = (reader.GetValue(4) != System.DBNull.Value) ? (Guid)reader.GetValue(4) : Guid.Empty;
\n\t\t\t\t\t\trequest.GenDownloadTypeId = (reader.GetValue(5) != System.DBNull.Value) ? (Guid)reader.GetValue(5) : Guid.Empty;
\n\t\t\t\t\t\trequest.GenIsTemperatureRegime = (reader.GetValue(6) != System.DBNull.Value) ? (bool)reader.GetValue(6) : false;
\n\t\t\t\t\t\trequest.GenVolume = (reader.GetValue(7) != System.DBNull.Value) ? (decimal)reader.GetValue(7) : 0m;
\n\t\t\t\t\t\trequest.GenTonnage = (reader.GetValue(8) != System.DBNull.Value) ? (decimal)reader.GetValue(8) : 0m;
\n\t\t\t\t\t\trequest.GenVehicleTypeId = (reader.GetValue(9) != System.DBNull.Value) ? (Guid)reader.GetValue(9) : Guid.Empty;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\trequest.GenIsTopDownload = (reader.GetValue(10) != System.DBNull.Value) ? (bool)reader.GetValue(10) : false;
\n\t\t\t\t\t\trequest.GenIsBackDownload = (reader.GetValue(11) != System.DBNull.Value) ? (bool)reader.GetValue(11) : false;
\n\t\t\t\t\t\trequest.GenIsSideDownload = (reader.GetValue(12) != System.DBNull.Value) ? (bool)reader.GetValue(12) : false;
\n\t\t\t\t\t\trequest.GenIsEmbankment = (reader.GetValue(13) != System.DBNull.Value) ? (bool)reader.GetValue(13) : false;
\n\t\t\t\t\t\trequest.GenIsPouring = (reader.GetValue(14) != System.DBNull.Value) ? (bool)reader.GetValue(14) : false;
\n
\n\t\t\t\t\t\tif (request.GenAuthorId != Guid.Empty)
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tSelect selByAuthorReq = new Select(UserConnection)
\n\t\t\t\t\t\t\t\t.Column(\"GenTeam\", \"GenManagerTeamId\")\t\t//0
\n\t\t\t\t\t\t\t\t.Column(\"GenManagerTeam\", \"GenDepartmentId\")//1
\n
\n\t\t\t\t\t\t\t\t.From(\"GenTeam\").As(\"GenTeam\")
\n\t\t\t\t\t\t\t\t.LeftOuterJoin(\"GenManagerTeam\").As(\"GenManagerTeam\")
\n\t\t\t\t\t\t\t\t.On(\"GenManagerTeam\", \"Id\").IsEqual(\"GenTeam\", \"GenManagerTeamId\")
\n\t\t\t\t\t\t\t\t.Where(\"GenTeam\", \"GenEmployeeId\").IsEqual(Column.Parameter(request.GenAuthorId))
\n\t\t\t\t\t\t\t\tas Select;
\n\t\t\t\t\t\t\tusing (var dbExecutor1 = UserConnection.EnsureDBConnection())
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tusing (var reader1 = selByAuthorReq.ExecuteReader(dbExecutor1))
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tif (reader1.Read())
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\trequest.GenManagerTeamId = (reader1.GetValue(0) != System.DBNull.Value) ? (Guid)reader1.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\t\trequest.GenDepartmentId = (reader1.GetValue(1) != System.DBNull.Value) ? (Guid)reader1.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t}
\n
\n\t\t\t\t\t\tSelect selDownloadReq = new Select(UserConnection)
\n\t\t\t\t\t\t\t.Column(\"GenCountryId\")
\n\t\t\t\t\t\t\t.Column(\"GenRegionId\")
\n\t\t\t\t\t\t\t.Column(\"GenCityId\")
\n
\n\t\t\t\t\t\t\t.From(\"GenDownloadPoint\")
\n\t\t\t\t\t\t\t.Where(\"GenRequestId\").IsEqual(Column.Parameter(request.Id))
\n\t\t\t\t\t\t\tas Select;
\n\t\t\t\t\t\tusing (var dbExecutor2 = UserConnection.EnsureDBConnection())
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tusing (var reader2 = selDownloadReq.ExecuteReader(dbExecutor2))
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tif (reader2.Read())
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\trequest.DownloadGenCountryId = (reader2.GetValue(0) != System.DBNull.Value) ? (Guid)reader2.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\trequest.DownloadGenRegionId = (reader2.GetValue(1) != System.DBNull.Value) ? (Guid)reader2.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\trequest.DownloadGenCityId = (reader2.GetValue(2) != System.DBNull.Value) ? (Guid)reader2.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t}
\n
\n\t\t\t\t\t\tSelect selUnloadingReq = new Select(UserConnection)
\n\t\t\t\t\t\t\t.Column(\"GenCountryId\")
\n\t\t\t\t\t\t\t.Column(\"GenRegionId\")
\n\t\t\t\t\t\t\t.Column(\"GenCityId\")
\n
\n\t\t\t\t\t\t\t.From(\"GenUnloadingPoint\")
\n\t\t\t\t\t\t\t.Where(\"GenRequestId\").IsEqual(Column.Parameter(request.Id))
\n\t\t\t\t\t\t\tas Select;
\n\t\t\t\t\t\tusing (var dbExecutor3 = UserConnection.EnsureDBConnection())
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tusing (var reader3 = selUnloadingReq.ExecuteReader(dbExecutor3))
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tif (reader3.Read())
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\trequest.UnloadingGenCountryId = (reader3.GetValue(0) != System.DBNull.Value) ? (Guid)reader3.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\trequest.UnloadingGenRegionId = (reader3.GetValue(1) != System.DBNull.Value) ? (Guid)reader3.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\trequest.UnloadingGenCityId = (reader3.GetValue(2) != System.DBNull.Value) ? (Guid)reader3.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t}
\n\t\t\t\t\t\t//new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\")  Україна
\n\t\t\t\t\t\t//if (request.DownloadGenCountryId != new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\") || request.UnloadingGenCountryId != new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\"))
\n\t\t\t\t\t\t\t//dateMutual = 7;
\n
\n\t\t\t\t\t\t//після цього добавляємо ці дані до колекції нашого запиту, попередньо створивши колекцію запитів
\n\t\t\t\t\t\trequests.Add(request);
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\t
\n\t\t\t
\n\t\t\t//Searches = new List<NeuroNode>();
\n\t\t\t
\n\t\t\t
\n\t\t\tSelect selectVehicle = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenVehicle\",\"Id\")
\n\t\t\t\t.Column(\"GenVehicle\",\"GenAuthorId\")//1
\n\t\t\t\t.Column(\"GenVehicle\",\"GenCityLocationId\")//2
\n\t\t\t\t.Column(\"GenVehicle\",\"GenCityTargetId\")//3
\n\t\t\t\t.Column(\"GenVehicle\",\"GenCountryLocationId\")//4
\n\t\t\t\t.Column(\"GenVehicle\",\"GenCountryTargetId\")//5
\n\t\t\t\t.Column(\"GenVehicle\",\"GenDownloadTypeId\")//6
\n\t\t\t\t.Column(\"GenVehicle\",\"GenDueDate\")//7
\n\t\t\t\t.Column(\"GenVehicle\",\"GenFree\")//8
\n\t\t\t\t.Column(\"GenVehicle\",\"GenRegionLocationId\")//9
\n\t\t\t\t.Column(\"GenVehicle\",\"GenRegionTargetId\")//10
\n\t\t\t\t.Column(\"GenVehicle\",\"GenStartDate\")//11
\n\t\t\t\t.Column(\"GenVehicle\",\"GenStateId\")//12
\n\t\t\t\t.Column(\"GenVehicle\",\"GenVolume\")//13
\n\t\t\t\t.Column(\"GenVehicle\",\"GenVehicleTypeId\")//14
\n\t\t\t\t.Column(\"GenVehicle\",\"GenTonage\")//15
\n\t\t\t\t.Column(\"GenVehicle\",\"GenIsTopDownload\")//16
\n\t\t\t\t.Column(\"GenVehicle\",\"GenIsBackDownload\")//17
\n\t\t\t\t.Column(\"GenVehicle\", \"GenIsSideDownload\")//18
\n\t\t\t\t.Column(\"GenVehicle\", \"GenIsEmbankment\")//19
\n\t\t\t\t.Column(\"GenVehicle\", \"GenIsPouring\")//20
\n
\n\t\t\t\t.From(\"GenVehicle\")
\n\t\t\t\t.Where(\"GenStartDate\").IsGreaterOrEqual(Column.Parameter(dateFrom))
\n\t\t\t//.Where()
\n\t\t\t//\t.OpenBlock(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"3ef75b2a-19da-4653-9966-8b2320e22d79\")))//Активно
\n\t\t\t//\t.Or(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"618691db-1545-4086-a463-4341cf664268\")))//Реверс
\n\t\t\t//\t.CloseBlock()
\n\t\t\t//\t.And(\"GenStartDate\").IsLessOrEqual(Column.Parameter(GenDownloadDateTo.AddDays(dateMutual)))//дата по з запиту+7днів/1-україна
\n\t\t\t//\t.And(\"GenDueDate\").IsGreaterOrEqual(Column.Parameter(GenDownloadDateFrom))//дата з з запиту
\n\t\t\tas Select;
\n\t\t\t
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selectVehicle.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\twhile (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\tVehicle vehicle = new Vehicle();
\n
\n\t\t\t\t\t\t//NeuroNode node = new NeuroNode();
\n\t\t\t\t\t\t//node.InputVal = new List<double>();
\n\t\t\t\t\t\t//node.GenVehicleId = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.Id = (reader.GetValue(0) != System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenAuthorId = (reader.GetValue(1) != System.DBNull.Value) ? (Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenCityLocationId = (reader.GetValue(2) != System.DBNull.Value) ? (Guid)reader.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenCityTargetId = (reader.GetValue(3) != System.DBNull.Value) ? (Guid)reader.GetValue(3) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenCountryLocationId = (reader.GetValue(4) != System.DBNull.Value) ? (Guid)reader.GetValue(4) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenCountryTargetId = (reader.GetValue(5) != System.DBNull.Value) ? (Guid)reader.GetValue(5) : Guid.Empty;
\n
\n\t\t\t\t\t\tvehicle.VehicleGenDownloadTypeId = (reader.GetValue(6) != System.DBNull.Value) ? (Guid)reader.GetValue(6) : Guid.Empty;
\n
\n\t\t\t\t\t\tvehicle.VehicleGenDueDate =(reader.GetValue(7) != System.DBNull.Value) ? (DateTime)reader.GetValue(7) : DateTime.MinValue;
\n\t\t\t\t\t\t//VehicleGenFree = 
\n
\n\t\t\t\t\t\tvehicle.VehicleGenRegionLocationId = (reader.GetValue(9) != System.DBNull.Value) ? (Guid)reader.GetValue(9) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenRegionTargetId = (reader.GetValue(10) != System.DBNull.Value) ? (Guid)reader.GetValue(10) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenStartDate = (reader.GetValue(11) != System.DBNull.Value) ? (DateTime)reader.GetValue(11) : DateTime.MinValue;
\n\t\t\t\t\t\t//VehicleGenStateId = (reader.GetValue(12) != System.DBNull.Value) ? (Guid)reader.GetValue(12) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenVolume = (reader.GetValue(13) != System.DBNull.Value) ? (decimal)reader.GetValue(13) : 0m;
\n\t\t\t\t\t\tvehicle.VehicleGenVehicleTypeId = (reader.GetValue(14) != System.DBNull.Value) ? (Guid)reader.GetValue(14) : Guid.Empty;
\n\t\t\t\t\t\tvehicle.VehicleGenTonage = (reader.GetValue(15) != System.DBNull.Value) ? (decimal)reader.GetValue(15) : 0m;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\tvehicle.VehicleGenIsTopDownload = (reader.GetValue(16) != System.DBNull.Value) ? (bool)reader.GetValue(16) : false;
\n\t\t\t\t\t\tvehicle.VehicleGenIsBackDownload = (reader.GetValue(17) != System.DBNull.Value) ? (bool)reader.GetValue(17) : false;
\n\t\t\t\t\t\tvehicle.VehicleGenIsSideDownload = (reader.GetValue(18) != System.DBNull.Value) ? (bool)reader.GetValue(18) : false;
\n\t\t\t\t\t\tvehicle.VehicleGenIsEmbankment = (reader.GetValue(19) != System.DBNull.Value) ? (bool)reader.GetValue(19) : false;
\n\t\t\t\t\t\tvehicle.VehicleGenIsPouring = (reader.GetValue(20) != System.DBNull.Value) ? (bool)reader.GetValue(20) : false;
\n
\n\t\t\t\t\t\tSelect selByVehicleAuthor = new Select(UserConnection)
\n\t\t\t\t\t\t\t.Column(\"GenTeam\", \"GenManagerTeamId\")\t\t//0
\n\t\t\t\t\t\t\t.Column(\"GenManagerTeam\", \"GenDepartmentId\")//1
\n
\n\t\t\t\t\t\t\t.From(\"GenTeam\").As(\"GenTeam\")
\n\t\t\t\t\t\t\t.LeftOuterJoin(\"GenManagerTeam\").As(\"GenManagerTeam\")
\n\t\t\t\t\t\t\t.On(\"GenManagerTeam\", \"Id\").IsEqual(\"GenTeam\", \"GenManagerTeamId\")
\n\t\t\t\t\t\t\t.Where(\"GenTeam\", \"GenEmployeeId\").IsEqual(Column.Parameter(vehicle.VehicleGenAuthorId))
\n\t\t\t\t\t\t\tas Select;
\n\t\t\t\t\t\tusing (var dbExecutorInner1 = UserConnection.EnsureDBConnection())
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tusing (var readerInner1 = selByVehicleAuthor.ExecuteReader(dbExecutorInner1))
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tif (readerInner1.Read())
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tvehicle.VehicleGenManagerTeamId = (readerInner1.GetValue(0) != System.DBNull.Value) ? (Guid)readerInner1.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\t\t\t\tvehicle.VehicleGenDepartmentId = (readerInner1.GetValue(1) != System.DBNull.Value) ? (Guid)readerInner1.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t}
\n\t\t\t\t\t\t//додаємо транспорт до колекції
\n\t\t\t\t\t\tvehicles.Add(vehicle);
\n\t\t\t\t\t\t
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\tSelect selectTransit = new Select(UserConnection)
\n\t\t\t.Column(\"GenTransit\",\"Id\")
\n\t\t\t.Column(\"GenTransit\",\"GenTransportId\")//1
\n\t\t\t.Column(\"GenTransit\",\"GenRequestId\")//2
\n\t\t\t\t\t\t
\n\t\t\t.From(\"GenTransit\")
\n\t\t\t.Where(\"CreatedOn\").IsGreaterOrEqual(Column.Parameter(dateFrom))
\n\t\t\t//.Where()
\n\t\t\t//\t.OpenBlock(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"3ef75b2a-19da-4653-9966-8b2320e22d79\")))//Активно
\n\t\t\t//\t.Or(\"GenStateId\").IsEqual(Column.Parameter(new Guid(\"618691db-1545-4086-a463-4341cf664268\")))//Реверс
\n\t\t\t//\t.CloseBlock()
\n\t\t\t//\t.And(\"GenStartDate\").IsLessOrEqual(Column.Parameter(GenDownloadDateTo.AddDays(dateMutual)))//дата по з запиту+7днів/1-україна
\n\t\t\t//\t.And(\"GenDueDate\").IsGreaterOrEqual(Column.Parameter(GenDownloadDateFrom))//дата з з запиту
\n\t\t\tas Select;
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selectTransit.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\twhile (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\tTransit transit = new Transit();
\n\t\t\t\t\t\t
\n\t\t\t\t\t\ttransit.Id = (reader.GetValue(0)!=System.DBNull.Value) ? (Guid)reader.GetValue(0) : Guid.Empty;
\n\t\t\t\t\t\ttransit.GenTransportId = (reader.GetValue(1)!=System.DBNull.Value)?(Guid)reader.GetValue(1) : Guid.Empty;
\n\t\t\t\t\t\ttransit.GenRequestId = (reader.GetValue(2)!=System.DBNull.Value)?(Guid)reader.GetValue(2) : Guid.Empty;
\n\t\t\t\t\t\t
\n\t\t\t\t\t\t//transits.Add(transit.GenRequestId,transit.GenTransportId); //RequestId - key and TransportId is value
\n\t\t\t\t\t\tif (!transits.ContainsKey(transit.GenRequestId.ToString()+transit.GenTransportId.ToString()))
\n\t\t\t\t\t\t\ttransits.Add(transit.GenRequestId.ToString()+transit.GenTransportId.ToString(), transit.Id); 
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\t//після того як отримали сформований список транспортів та запитів, треба зчитати рейси по id
\n\t\t\t//для кожного запиту перебираємо колекцію транспорту, формуємо ноди
\n\t\t\t//в результаті від того, чи є створений рейс для транспорт-запит ми вже і формуємо node.InputVal()
\n\t\t\tforeach(var req in requests)
\n\t\t\t{
\n\t\t\t\tforeach(var veh in vehicles)
\n\t\t\t\t{
\n
\n\t\t\t\t\tNeuroNode node = new NeuroNode();
\n\t\t\t\t\tnode.InputVal = new List<double>();
\n\t\t\t\t\t
\n\t\t\t\t\tif (transits.ContainsKey(req.Id.ToString()+veh.Id.ToString()))
\n\t\t\t\t\t\tnode.refVal = 1d;
\n\t\t\t\t\telse
\n\t\t\t\t\t{
\n\t\t\t\t\t\tif (Searches.Count < transits.Count*0.7m)
\n\t\t\t\t\t\t\tnode.refVal = 0d;
\n\t\t\t\t\t\telse
\n\t\t\t\t\t\t\tcontinue;
\n\t\t\t\t\t}
\n\t\t\t\t\t
\n\t\t\t\t\tnode.InputVal =  CalcInputVal(req,veh);
\n
\n\t\t\t\t\t//
\n\t\t\t\t\t//node.result = neuralNetwork.Learn(dataset, 100000);
\n\t\t\t\t
\n\t\t\t\t\t//var dataset = new List<Tuple<double, double[]>>
\n\t\t\t\t\t// dataset includes node.refVal and node.InputVal
\n\t\t\t\t\t
\n\t\t\t\t\tSearches.Add(node);
\n\t\t\t\t}
\n\t\t\t\t
\n\t\t\t}
\n\t\t\tvar dataset = new List<Tuple<double, double[]>>();
\n\t\t\t
\n\t\t\t//формуємо датасет
\n\t\t\tforeach(var s in Searches)
\n\t\t\t{
\n\t\t\t\tdataset.Add(new Tuple<double,double[]>(s.refVal, s.InputVal.ToArray()));\t\t
\n\t\t\t}
\n\t\t\tvar result = neuralNetwork.Learn(dataset,100000,3);
\n\t\t\t//підраховуємо 
\n\t\t\t//foreach(var s in Searches)
\n\t\t\t//{
\n\t\t\t//\tvar result = neuralNetwork.Learn(dataset,100000);
\n\t\t\t//}
\n\t\t\t
\n\t\t\t
\n\t\t\t\t\t
\n\t\t\t//var topology = new Topology(27, 1, 0.1, 14); // 14 - hidden layer з 14ма нейронами 
\n\t\t\t//var neuralNetwork = new NeuralNetwork(topology);
\n\t\t\t//var difference = neuralNetwork.Learn(dataset, 100000);
\n
\n\t\t\t//в якості датасету ми передаємо 
\n\t\t\t//var results = new List<double>();
\n\t\t\t//foreach (var data in dataset)
\n\t\t\t//{
\n\t\t\t//\tvar res = neuralNetwork.FeedForward(data.Item2).Output;
\n\t\t\t//\tresults.Add(res);
\n\t\t\t//}
\n\t\t\t
\n\t\t\t//за умови що в нас є вже створений рейс за id транспорту та запиту, ми передаємо в res 1
\n\t\t\t
\n\t\t\treturn res;
\n\t\t}
\n\t\t
\n\t\tpublic List<double> CalcInputVal (Request req, Vehicle veh)
\n\t\t{
\n\t\t\tNeuroNode node = new NeuroNode();
\n\t\t\tnode.InputVal = new List<double>();
\n\t\t\t//країна завантаження
\n\t\t\tif(req.DownloadGenCountryId == veh.VehicleGenCountryLocationId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t//регіон завантаження
\n\t\t\tif(req.DownloadGenRegionId == veh.VehicleGenRegionLocationId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t//місто завантаження
\n\t\t\tif(req.DownloadGenCityId == veh.VehicleGenCityLocationId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n
\n\t\t\t//країна розвантаження
\n
\n\t\t\tif(req.UnloadingGenCountryId == veh.VehicleGenCountryTargetId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t//регіон розвантаження
\n\t\t\tif(req.UnloadingGenRegionId == veh.VehicleGenRegionTargetId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t//місто розвантаження
\n\t\t\tif(req.UnloadingGenCityId == veh.VehicleGenCityTargetId)
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n
\n\t\t\t/*
\n\t\t\t//дата завантаження та розвантаження (TODO добавити +7/+1)
\n\t\t\tif(req.DownloadGenCountryId == new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\"))// Україна
\n\t\t\t{
\n\t\t\t\tif(veh.VehicleGenDueDate == req.GenDownloadDateTo && veh.VehicleGenStartDate == req.GenDownloadDateFrom)
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t}
\n
\n\t\t\t\tif(veh.VehicleGenDueDate > req.GenDownloadDateFrom)
\n\t\t\t\t{
\n\t\t\t\t\tif(veh.VehicleGenStartDate < req.GenDownloadDateTo.AddDays(1))
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t}
\n\t\t\t\t\telse
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t\telse
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tif(veh.VehicleGenDueDate == req.GenDownloadDateTo && veh.VehicleGenStartDate == req.GenDownloadDateFrom)
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t}
\n
\n\t\t\t\tif(veh.VehicleGenDueDate > req.GenDownloadDateFrom)
\n\t\t\t\t{
\n\t\t\t\t\tif(veh.VehicleGenStartDate < req.GenDownloadDateTo.AddDays(7))
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t}
\n\t\t\t\t\telse
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t\telse
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t*/
\n\t\t\tint dayOffset = 7;
\n\t\t\tif(req.DownloadGenCountryId == new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\"))// Україна
\n\t\t\t\tdayOffset = 1;
\n\t\t\t//req.GenDownloadDateFrom //req.GenDownloadDateTo
\n\t\t\t
\n\t\t\t//veh.VehicleGenStartDate //veh.VehicleGenDueDate
\n\t\t\t
\n\t\t\tif (Math.Abs((req.GenDownloadDateFrom-veh.VehicleGenStartDate).Days)<1&&
\n\t\t\t   Math.Abs((req.GenDownloadDateTo-veh.VehicleGenDueDate).Days)<1)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n\t\t\t
\n\t\t\tdouble val = 0.01;
\n\t\t\t
\n\t\t\tif ((req.GenDownloadDateFrom-veh.VehicleGenStartDate).Days >= dayOffset &&
\n\t\t\t\t(req.GenDownloadDateTo-veh.VehicleGenDueDate).Days > dayOffset)
\n\t\t\t{
\n\t\t\t\tval = 0.99;
\n\t\t\t}
\n\t\t\tif ((req.GenDownloadDateFrom-veh.VehicleGenStartDate).Days <= -dayOffset &&
\n\t\t\t\t(req.GenDownloadDateTo-veh.VehicleGenDueDate).Days >= dayOffset)
\n\t\t\t{
\n\t\t\t\tval = 0.99;
\n\t\t\t}
\n\t\t\tif ((req.GenDownloadDateFrom-veh.VehicleGenStartDate).Days < -dayOffset &&
\n\t\t\t\t(req.GenDownloadDateTo-veh.VehicleGenDueDate).Days <= -dayOffset)
\n\t\t\t{
\n\t\t\t\tval = 0.99;
\n\t\t\t}
\n\t\t\tif ((req.GenDownloadDateFrom-veh.VehicleGenStartDate).Days >= dayOffset &&
\n\t\t\t\t(req.GenDownloadDateTo-veh.VehicleGenDueDate).Days <= -dayOffset)
\n\t\t\t{
\n\t\t\t\tval = 0.99;
\n\t\t\t}
\n\t\t\tnode.InputVal.Add(val);
\n\t\t\t
\n\t\t\t
\n\t\t\t//тип транспорту реф з температурою або без температури
\n\t\t\tif(!req.GenIsTemperatureRegime)//тут може бути як і рефрежиратор так і ізотерм
\n\t\t\t\tnode.InputVal.Add(0.99);
\n
\n\t\t\tif(req.GenIsTemperatureRegime)//тут може бути тільки рефрежиратор
\n\t\t\t\tif(veh.VehicleGenVehicleTypeId == new Guid(\"b1e43255-35ed-406f-9033-351ac9689f71\"))
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(0.01);
\n
\n\t\t\t//тип завантаження 
\n\t\t\t// всього є 5 типів завантаження: бокове, верхнє,заднє, налив, насип. 
\n\t\t\t// також у нас можуть бути такі комбінації: заднє-верхнє, заднє-бокове, верхнє-бокове
\n\t\t\t
\n\t\t\t/*if(veh.VehicleGenDownloadTypeId == new Guid(\"c04fbf41-3ad0-4a3e-92fd-56daa12655ff\"))//бокове та верхнє\t
\n\t\t\t\tif(veh.VehicleGenVehicleTypeId!= new Guid(\"8cd435d7-16cd-4d3a-a493-071dfa96cd1f\"))//тип транспорту тент
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(1);
\n\t\t\telse
\n\t\t\t\tnode.InputVal.Add(1);*/
\n\t\t\t/*
\n\t\t\tif(req.GenIsTopDownload || req.GenIsSideDownload || req.GenIsBackDownload )
\n\t\t\t{\t
\n\t\t\t\t//бокове
\n\t\t\t\t//if(req.GenIsSideDownload)
\n\t\t\t\t//{
\n\t\t\t\t\t//верхнє-бокове
\n\t\t\t\t\tif(veh.VehicleGenIsTopDownload == req.GenIsTopDownload && veh.VehicleGenIsSideDownload == req.GenIsSideDownload && req.GenIsTopDownload && req.GenIsSideDownload )
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t}
\n\t\t\t\t\t//заднє-бокове
\n\t\t\t\t\tif(veh.VehicleGenIsBackDownload == req.GenIsBackDownload && veh.VehicleGenIsSideDownload == req.GenIsSideDownload && req.GenIsBackDownload && req.GenIsSideDownload )
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t}
\n\t\t\t\t\t//заднє-верхнє
\n\t\t\t\t\tif(veh.VehicleGenIsBackDownload == req.GenIsBackDownload && veh.VehicleGenIsTopDownload == req.GenIsTopDownload && req.GenIsTopDownload && req.GenIsBackDownload)
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t}
\n\t\t\t\t\telse
\n\t\t\t\t\t{
\n\t\t\t\t\t\t//верхнє
\n\t\t\t\t\t\tif(veh.VehicleGenIsTopDownload == req.GenIsTopDownload && req.GenIsTopDownload)
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t}
\n\t\t\t\t\t\t//заднє
\n\t\t\t\t\t\tif(veh.VehicleGenIsBackDownload == req.GenIsBackDownload && req.GenIsBackDownload)
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t}
\n\t\t\t\t\t\t//бокове
\n\t\t\t\t\t\tif(veh.VehicleGenIsSideDownload == req.GenIsSideDownload && req.GenIsSideDownload)
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\t}
\n\t\t\t\t\t}
\n\t\t\t\t\t\t\t
\n\t\t\t\t
\n\t\t\t\t\t
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tif(veh.VehicleGenIsPouring == req.GenIsPouring && req.GenIsPouring)
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n
\n\t\t\t\t}
\n\t\t\t\tif(veh.VehicleGenIsEmbankment == req.GenIsEmbankment && req.GenIsEmbankment )
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(1);
\n
\n\t\t\t\t}
\n\t\t\t\telse
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t*/
\n\t\t\tif (req.GenIsTopDownload == veh.VehicleGenIsTopDownload == true)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n\t\t\tif (req.GenIsBackDownload == veh.VehicleGenIsBackDownload == true)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n\t\t\tif (req.GenIsSideDownload == veh.VehicleGenIsSideDownload == true)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n\t\t\tif (req.GenIsPouring == veh.VehicleGenIsPouring == true)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n\t\t\tif (req.GenIsEmbankment == veh.VehicleGenIsEmbankment == true)
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n
\n\t\t\t//VehicleGenIsTopDownload
\n\t\t\t//VehicleGenIsBackDownload
\n\t\t\t//VehicleGenIsSideDownload
\n\t\t\t//VehicleGenIsPouring
\n\t\t\t//VehicleGenIsEmbankment
\n
\n
\n\t\t\t//if(GenVehicleTypeId == new Guid(\"b1e43255-35ed-406f-9033-351ac9689f71\")) //рефрежиратор
\n\t\t\t//\tnode.InputVal.Add(1);
\n\t\t\t//else
\n\t\t\t//\tnode.InputVal.Add(0);
\n
\n\t\t\t//температурний режим
\n\t\t\t//if(GenVehicleTypeId == new Guid()//VehicleGenVehicleTypeId)
\n\t\t\t//\tnode.InputVal.Add(1);
\n\t\t\t//else
\n\t\t\t//node.InputVal.Add(0);
\n
\n\t\t\t//для України маса та об'єм 
\n\t\t\tif(req.DownloadGenCountryId == new Guid(\"a470b005-e8bb-df11-b00f-001d60e938c6\"))// Україна
\n\t\t\t{
\n\t\t\t\t//об'єм
\n\t\t\t\tif((veh.VehicleGenVolume >= req.GenVolume)||(veh.VehicleGenVolume <= req.GenVolume && veh.VehicleGenVolume*1.1m >= req.GenVolume))
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\telse
\n\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t//тонаж
\n\t\t\t\tif((veh.VehicleGenTonage >= req.GenTonnage)|| (veh.VehicleGenTonage <= req.GenTonnage && veh.VehicleGenTonage*1.2m >= req.GenTonnage )) 
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\telse
\n\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}//для Європи маса та об'єм 
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\t//об'єм
\n\t\t\t\tif(veh.VehicleGenVolume >= req.GenVolume)
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\telse
\n\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t//тонаж
\n\t\t\t\tif(veh.VehicleGenTonage >= req.GenTonnage)
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\telse
\n\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t}
\n
\n
\n\t\t\t//тип завантаження
\n\t\t\t//if(VehicleGenDownloadTypeId == GenDownloadTypeId)
\n\t\t\t//node.InputVal.Add(1);
\n\t\t\t//else//
\n\t\t\t//node.InputVal.Add(0);
\n
\n\t\t\t//автор
\n\t\t\tif(veh.VehicleGenAuthorId == req.GenAuthorId )
\n\t\t\t{
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t}
\n\t\t\t/*if(req.GenManagerTeamId == veh.VehicleGenManagerTeamId)
\n\t\t\t\t\t\tnode.InputVal.Add(0);
\n\t\t\t\t\t\tnode.InputVal.Add(1);
\n\t\t\t\t\t\tnode.InputVal.Add(0);*/
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tif(req.GenManagerTeamId == veh.VehicleGenManagerTeamId)
\n\t\t\t\t{
\n\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\t}
\n\t\t\t\telse
\n\t\t\t\t{
\n\t\t\t\t\tif(req.GenDepartmentId == veh.VehicleGenDepartmentId)
\n\t\t\t\t\t{
\n\t\t\t\t\t\t//if(veh.VehicleGenAuthorId == req.GenAuthorId )
\n\t\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\t\tnode.InputVal.Add(0.99);
\n\t\t\t\t\t}
\n\t\t\t\t\telse 
\n\t\t\t\t\t{
\n\t\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\t\tnode.InputVal.Add(0.01);
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\t
\n\t\t\t
\n\t\t\treturn node.InputVal;
\n\t\t}
\n\t\t
\n\t\tpublic bool LearnOnFeedback()
\n\t\t{
\n\t\t\tbool res = false;
\n\t\t\tvar dataset = new List<Tuple<double, double[]>>();
\n\t\t\t
\n\t\t\tSelect selSearches = new Select(UserConnection)
\n\t\t\t\t.Column(\"GenSearches\", \"GenInputValues\")\t//0
\n\t\t\t\t.Column(\"GenSearches\", \"GenResult\")\t\t\t//1
\n\t\t\t\t.Column(\"GenSearches\", \"GenIsLiked\")\t\t//2
\n\t\t\t\t.Column(\"GenSearches\", \"GenIsDisliked\")\t\t//3
\n\t\t\t\t
\n\t\t\t\t.From(\"GenSearches\").As(\"GenSearches\")
\n\t\t\t\t.Where(\"GenSearches\", \"ModifiedOn\").IsGreaterOrEqual(Column.Parameter(DateTime.UtcNow.AddDays(-2)))
\n\t\t\t\t.And()
\n\t\t\t\t\t.OpenBlock(\"GenSearches\", \"GenIsLiked\").IsEqual(Column.Parameter(true))
\n\t\t\t\t\t.Or(\"GenSearches\", \"GenIsDisliked\").IsEqual(Column.Parameter(true))
\n\t\t\t\t.CloseBlock()
\n\t\t\tas Select;
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selSearches.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\twhile (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\tstring GenInputValues = (reader.GetValue(0)!=System.DBNull.Value)?(string)reader.GetValue(0) : \"\";
\n\t\t\t\t\t\tdecimal GenResult = (reader.GetValue(1)!=System.DBNull.Value)?(decimal)reader.GetValue(1) : 0m;
\n\t\t\t\t\t\tbool GenIsLiked = (reader.GetValue(2)!=System.DBNull.Value)?(bool)reader.GetValue(2) : false;
\n\t\t\t\t\t\tbool GenIsDisliked = (reader.GetValue(3)!=System.DBNull.Value)?(bool)reader.GetValue(3) : false;
\n\t\t\t\t\t\tif (!string.IsNullOrEmpty(GenInputValues))
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tList<double>  inputs = JsonConvert.DeserializeObject<List<double>>(GenInputValues);
\n\t\t\t\t\t\t\tdouble refVal = 0d;
\n\t\t\t\t\t\t\tif(GenIsLiked == true && GenIsDisliked != true && GenResult >= 0.5m)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\trefVal = 1d;
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\telse if(GenIsLiked == true && GenIsDisliked != true && GenResult < 0.5m)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\trefVal = 0d;
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\telse if(GenIsLiked != true && GenIsDisliked == true && GenResult >= 0.5m)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\trefVal = 0d;
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\telse if(GenIsLiked != true && GenIsDisliked == true && GenResult < 0.5m)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\trefVal = 1d;
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\telse
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tcontinue;
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\tdataset.Add(new Tuple<double,double[]>(refVal, inputs.ToArray()));
\n\t\t\t\t\t\t}
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t\tif (dataset.Count > 0)
\n\t\t\t{
\n\t\t\t\tvar topology = new Topology(19, 1, 0.1, 25, 10); // 14 - hidden layer з 14ма нейронами 
\n\t\t\t\tvar neuralNetwork = new NeuralNetwork(topology);
\n\t\t\t\tvar result = neuralNetwork.Learn(dataset,10,3);
\n\t\t\t}
\n\t\t\t
\n\t\t\tres = true;
\n\t\t\treturn res;
\n\t\t}
\n\t\t
\n\t}
\n\t
\n\tpublic class NeuroNode
\n\t{
\n\t\tpublic Guid GenVehicleId { get; set; } //транспорт
\n\t\tpublic List<double> InputVal { get; set; }//вхідні данні адаптовані під мережу
\n\t\tpublic double result { get; set; }//результат
\n\t\tpublic double refVal { get; set; }//для навчання
\n\t}
\n\t
\n\tpublic class Transit
\n\t{
\n\t\t//тут у нас буде id транспорту та id запиту
\n\t\tpublic Guid Id {get;set;} = Guid.Empty;
\n\t\tpublic Guid GenTransportId {get; set; } = Guid.Empty;
\n\t\tpublic Guid GenRequestId {get; set; } = Guid.Empty;
\n\t\t
\n\t}
\n\tpublic class Vehicle 
\n\t{
\n\t\tpublic Guid Id {get;set;} = Guid.Empty;
\n
\n\t\tpublic Guid VehicleGenAuthorId {get; set; } = Guid.Empty;
\n\t\tpublic Guid VehicleGenCityLocationId {get; set; } = Guid.Empty;
\n\t\tpublic Guid VehicleGenCityTargetId {get; set; } = Guid.Empty;
\n\t\tpublic Guid VehicleGenCountryLocationId {get; set;} = Guid.Empty;
\n\t\tpublic Guid VehicleGenCountryTargetId {get;set;} = Guid.Empty;
\n
\n\t\tpublic Guid VehicleGenDownloadTypeId {get;set;} = Guid.Empty;
\n
\n\t\tpublic DateTime VehicleGenDueDate {get; set; } = DateTime.MinValue;
\n\t\tpublic int VehicleGenFree {get; set; } = 0;
\n
\n\t\tpublic Guid VehicleGenRegionLocationId {get; set; } = Guid.Empty;
\n\t\tpublic Guid VehicleGenRegionTargetId {get; set; } = Guid.Empty;
\n\t\tpublic DateTime VehicleGenStartDate {get; set; } = DateTime.MinValue;
\n\t\tpublic Guid VehicleGenStateI {get; set; } = Guid.Empty;
\n\t\tpublic decimal VehicleGenVolume {get; set; } = 0m;
\n\t\tpublic Guid VehicleGenVehicleTypeId {get; set; } = Guid.Empty;
\n\t\tpublic decimal VehicleGenTonage {get; set; } = 0m;
\n\t\tpublic Guid VehicleGenManagerTeamId {get; set; } = Guid.Empty;
\n\t\tpublic Guid VehicleGenDepartmentId {get; set; } = Guid.Empty;
\n\t\t
\n\t\tpublic bool VehicleGenIsTopDownload{get;set;}
\n\t\tpublic bool VehicleGenIsBackDownload{get;set;}
\n\t\tpublic bool VehicleGenIsSideDownload{get;set;}
\n\t\tpublic bool VehicleGenIsPouring{get;set;}
\n\t\tpublic bool VehicleGenIsEmbankment{get;set;}
\n\t}
\n\tpublic class Request 
\n\t{
\n\t\tpublic Guid Id {get;set;} = Guid.Empty;
\n\t\tpublic DateTime GenDownloadDateFrom {get;set;} = DateTime.MinValue;
\n\t\tpublic DateTime GenDownloadDateTo {get;set;} = DateTime.MinValue;
\n\t\tpublic Guid GenOwnerId {get;set;} = Guid.Empty;
\n\t\tpublic Guid GenAuthorId {get;set;} = Guid.Empty;
\n\t\tpublic Guid GenDownloadTypeId {get;set;} = Guid.Empty;
\n
\n\t\tpublic bool GenIsTemperatureRegime = false;
\n
\n\t\tpublic decimal  GenVolume {get;set;} = 0m;
\n\t\tpublic decimal GenTonnage {get;set;} = 0m;
\n\t\tpublic Guid GenVehicleTypeId {get;set;} = Guid.Empty;
\n
\n\t\tpublic Guid GenManagerTeamId {get;set;} = Guid.Empty;
\n\t\tpublic Guid GenDepartmentId {get;set;} = Guid.Empty;
\n\t\tpublic Guid DownloadGenCountryId {get;set;} = Guid.Empty;
\n\t\tpublic Guid DownloadGenRegionId {get;set;} = Guid.Empty;
\n\t\tpublic Guid DownloadGenCityId {get;set;} = Guid.Empty;
\n\t\tpublic Guid UnloadingGenCountryId {get;set;} = Guid.Empty;
\n\t\tpublic Guid UnloadingGenRegionId {get;set;} = Guid.Empty;
\n\t\tpublic Guid UnloadingGenCityId {get;set;} = Guid.Empty;
\n\t\t
\n\t\tpublic bool GenIsTopDownload{get;set;}
\n\t\tpublic bool GenIsBackDownload{get;set;}
\n\t\tpublic bool GenIsSideDownload{get;set;}
\n\t\tpublic bool GenIsPouring{get;set;}
\n\t\tpublic bool GenIsEmbankment{get;set;}
\n\t\t
\n\t\t
\n\t}
\n}",
  "MetaData": "{\r
\n  \"MetaData\": {\r
\n    \"Schema\": {\r
\n      \"ManagerName\": \"SourceCodeSchemaManager\",\r
\n      \"UId\": \"447399cf-9eed-43de-96ff-f1db31473cfd\",\r
\n      \"A2\": \"GenNeuralNetworkAdapter\",\r
\n      \"A5\": \"0bd87f3b-859d-4a49-9e89-ba853bfeb6e6\",\r
\n      \"B1\": [],\r
\n      \"B2\": [],\r
\n      \"B3\": [],\r
\n      \"B6\": \"92cea9ed-4580-cf30-a59e-c5bde6193519\",\r
\n      \"B8\": \"8.0.8.4807\",\r
\n      \"HD1\": \"50e3acc0-26fc-4237-a095-849a1d534bd3\"\r
\n    }\r
\n  }\r
\n}",
  "LocalizableValues": [
    {
      "Culture": "ru-RU",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "GenNeuralNetworkAdapter",
      "ImageData": ""
    },
    {
      "Culture": "en-US",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "GenNeuralNetworkAdapter",
      "ImageData": ""
    },
    {
      "Culture": "uk-UA",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "GenNeuralNetworkAdapter",
      "ImageData": ""
    }
  ],
  "Properties": [
    {
      "Name": "CreatedInVersion",
      "Value": "8.0.8.4807"
    }
  ]
}