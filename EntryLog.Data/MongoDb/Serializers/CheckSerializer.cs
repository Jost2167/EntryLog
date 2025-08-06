using EntryLog.Entities.Entities;
using MongoDB.Bson.Serialization;

namespace EntryLog.Data.MongoDb.Serializers;

internal static class CheckSerializer
{
    public static void Init()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Check)))
        {
            BsonClassMap.RegisterClassMap<Check>(cm =>
            {
                cm.MapMember(c=>c.Method).SetElementName("metodo");
                cm.MapMember(c=>c.DeviceName)
                    .SetElementName("nombre_dispositivo")
                    .SetIgnoreIfNull(true);  // Ignora la propiedad si es nula
                cm.MapMember(c=>c.Date).SetElementName("fecha");
                cm.MapMember(c=>c.Location).SetElementName("ubicacion");
                cm.MapMember(c=>c.PhotoUrl).SetElementName("url_foto");
                cm.MapMember(c => c.Note)
                    .SetElementName("nota")
                    .SetIgnoreIfNull(true);
            });
        } 
    }
}