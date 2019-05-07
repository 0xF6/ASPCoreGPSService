namespace gps_service.Core
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Models;
    using Newtonsoft.Json;

    public static class StorageManager
    {
        public static IEnumerable<string> AllSessions =>
            from sid in
                from file in Directory.EnumerateFiles("./storage/", "*.sid", SearchOption.AllDirectories)
                select File.ReadAllText(file)
            select sid;

        public static IEnumerable<StorageEntity> AllStorages =>
            from entity in
                from session in AllSessions
                select new StorageEntity(session)
            select entity;

        public static void CreateStorage(ProcessedGeo[] geo)
        {
            var sid = geo.First().SessionID;
            var time = geo.First().Timestamp;

            Directory.CreateDirectory($"./storage/{sid}");
            File.WriteAllText($"./storage/{sid}/_.sid", sid);
            File.WriteAllText($"./storage/{sid}/_.time", time.ToString(CultureInfo.InvariantCulture));
            var json = JsonConvert.SerializeObject(geo, Formatting.Indented);

            File.WriteAllText($"./storage/{sid}/_.json", json);
            ZipFile.CreateFromDirectory($"./storage/{sid}", $"./storage/cfz/{sid}.zip");
        }
    }

    public readonly struct StorageEntity
    {
        public StorageEntity(string sessionId)
        {
            this.SessionID = sessionId;
            this.ZipBytes = File.ReadAllBytes($"./storage/cfz/{sessionId}.zip");
        }

        public readonly string SessionID;
        public readonly byte[] ZipBytes;
    }
}