using EntryLog.Entities.Entities;
using MongoDB.Bson.Serialization;

namespace EntryLog.Data.MongoDb.Serializers;

// Define como MongoDB debe serializar y deserializar la clase Location sin modificar la clase original.
internal static class LocationSerializer
{
    public static void Init()
    {
        // Verifica si la clase Location ya está registrada en el BsonClassMap.
        if (!BsonClassMap.IsClassMapRegistered(typeof(Location)))
        {
            // Registra la clase Location en el BsonClassMap para que MongoDB sepa cómo serializar y deserializar esta clase.
            BsonClassMap.RegisterClassMap<Location>(cm =>
            {
                // Mapea las propiedadades de la clase Location a los nombres de los campos en el documento BSON.
                cm.MapMember(c=>c.Latitude).SetElementName("latitud");
                cm.MapMember(c=>c.Longitude).SetElementName("longitud");
                cm.MapMember(c=>c.ApproximateAddress)
                    .SetElementName("direccion_aproximada")
                    .SetIgnoreIfNull(true); // Ignora la propiedad si es nula
                cm.MapMember(c=>c.IpAddress).SetElementName("direccion_ip");
            });
        }
    }
}