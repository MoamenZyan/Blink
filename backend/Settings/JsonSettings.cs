using Newtonsoft.Json;

public static class JsonSettings
{
    // Configure Json serialization settings
    // This is for ignoring object referencing loop
    public static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        PreserveReferencesHandling = PreserveReferencesHandling.All
    };
}