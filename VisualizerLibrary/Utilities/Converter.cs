namespace VisualizerLibrary.Utilities;

public static class Converter
{
    public static T GetEnum<T>(string value) where T : struct, Enum
    {
        var enumArray = Enum.GetNames(typeof(T)).ToArray();
        var enumValues = Enum.GetValues(typeof(T));
        if (enumValues is null) throw new Exception($"Can not cast to enum from {typeof(T).FullName}");
        for (var i = 0; i < enumArray.Length; i++)
        {
            if (enumArray[i] == value)
            {
                var result = (T)(enumValues.GetValue(i) ??
                                 throw new Exception($"Can not get value from {typeof(T).FullName} enum"));
                return result;
            }
        }
        throw new Exception("Incorrect enum value");
    }
}
