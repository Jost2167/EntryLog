using EntryLog.Entities.Entities;
using EntryLog.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace EntryLog.Data.MongoDb.Serializers;

internal static class WorkSessionSerializer
{
    public static void Init()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(WorkSession)))
        {
            BsonClassMap.RegisterClassMap<WorkSession>(cm =>
            {
                // Indica cual sera el miembro que actuara como Id
                cm.SetIdMember(cm.GetMemberMap(x=>x.Id));
                
                // MapIdMember marca Id como _id del documento
                cm.MapIdMember(c => c.Id)
                    // Permite que Mongo genere automáticamente el Id
                    .SetIdGenerator(GuidGenerator.Instance)
                    // Serializa el Id como un Guid estándar
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                
                cm.MapMember(c=>c.EmployeeId).SetElementName("id_empleado");
                cm.MapMember(c => c.CheckIn).SetElementName("entrada");
                cm.MapMember(c => c.CheckOut).SetElementName("salida");
                cm.MapMember(c => c.TotalWorked)
                    .SetElementName("tiempo_trabajado")
                    .SetIgnoreIfNull(true);
                cm.MapMember(c => c.Status)
                    .SetElementName("estado")
                    // Serializa el estado como una cadena en lugar de un número entero
                    .SetSerializer(new EnumSerializer<SessionStatus>(BsonType.String));
            });
        }
    }
} 