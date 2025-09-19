using EntryLog.Entities.Entities;
using EntryLog.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace EntryLog.Data.MongoDb.Serializers;

internal static class AppUserSerializer
{
    public static void Init()
    {
        if(!BsonClassMap.IsClassMapRegistered(typeof(AppUser)))
        {
            BsonClassMap.RegisterClassMap<AppUser>(cm =>
            {
                cm.SetIdMember(cm.GetMemberMap(x=>x.Id));
                
                cm.MapIdMember(c => c.Id)
                    .SetElementName("_id")
                    .SetIdGenerator(GuidGenerator.Instance)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cm.MapMember(c=>c.Name).SetElementName("nombre");
                cm.MapMember(c=>c.Code).SetElementName("codigo");
                cm.MapMember(c => c.Role)
                    .SetElementName("rol")
                    .SetSerializer(new EnumSerializer<RoleType>(BsonType.String));
                cm.MapMember(c => c.Email).SetElementName("email");
                cm.MapMember(c => c.CellPhone).SetElementName("celular");
                cm.MapMember(c => c.Password).SetElementName("clave");
                cm.MapMember(c => c.Attempts).SetElementName("intentos");
                cm.MapMember(c => c.RecoveryToken)
                    .SetElementName("token_recuperacion")
                    .SetIgnoreIfNull(true);
                cm.MapMember(c => c.RecoveryTokenActive).SetElementName("token_activo");
                cm.MapMember(c => c.Active).SetElementName("activo");
            });
        }
    }
}