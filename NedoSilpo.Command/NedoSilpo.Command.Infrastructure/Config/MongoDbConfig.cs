namespace NedoSilpo.Command.Infrastructure.Config;

public record MongoDbConfig( string ConnectionString, string Database, string Collection)
{
    public MongoDbConfig() : this(string.Empty, string.Empty, string.Empty)
    {

    }
}
