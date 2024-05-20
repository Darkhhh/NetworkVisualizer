using System.Reflection;
using VisualizerLibrary.Extensions;

namespace VisualizerLibrary.Utilities;

public static class CodeToText
{
    public static string FromInterface<T>() where T : class
    {
        var type = typeof(T);
        var str = string.Empty;
        var flags = BindingFlags.Instance | BindingFlags.Public;
        foreach (var method in type.GetMethods(flags))
        {
            str += $"\tpublic {GetReturnType(method)} {method.Name}({GetParameters(method)})\n\t{{\n\n\t}}";
            str += "\n\n";
        }
        return str;
    }

    public static string FromAbstractClass<T>() where T : class
    {
        var type = typeof(T);
        var str = string.Empty;
        var flags = BindingFlags.Instance | BindingFlags.Public;
        foreach (var method in type.GetMethods(flags))
        {
            if (!method.IsAbstract) continue;
            str += $"\tpublic override {GetReturnType(method)} {method.Name}({GetParameters(method)})\n\t{{\n\n\t}}";
            str += "\n\n";
        }
        return str;
    }

    private static string GetReturnType(MethodInfo method)
    {
        var type = method.ReturnType;
        if (type.Name is "Void") return "void";

        var str = method.ReturnType.IsGenericType ? GenericArgument(method.ReturnType) : NonGenericArgument(method.ReturnType);
        if (method.ReturnParameter.IsNullable()) str += "?";

        return str;
    }

    private static string GenericArgument(Type type)
    {
        if (!type.IsGenericType) return NonGenericArgument(type);

        var str = new string(type.Name.Where(c => char.IsLetter(c)).ToArray()) + "<";

        foreach (var arg in type.GetGenericArguments())
        {
            str += GenericArgument(arg) + ", ";
        }

        str = str.Remove(str.Length - 2);
        str += ">";
        return str;
    }

    private static string NonGenericArgument(Type type)
    {
        if (type.IsGenericType) throw new Exception();

        if (type.IsPrimitive)
        {
            switch (type.Name)
            {
                case "Double":
                    return "double";
                case "Int32":
                    return "int";
                case "Boolean":
                    return "bool";
                case "Single":
                    return "float";
                case "Byte":
                    return "byte";
                case "Char":
                    return "char";
            };
        }
        if (type.Name is "String") return "string";
        return type.Name;
    }

    private static string GetParameters(MethodInfo method)
    {
        var str = string.Empty;
        if (method.GetParameters().Length == 0) return str;
        foreach (var param in method.GetParameters())
        {
            var paramTypeStr = string.Empty;
            var paramType = param.ParameterType;
            if (paramType.IsGenericType) paramTypeStr = GenericArgument(paramType);
            else paramTypeStr = NonGenericArgument(paramType);

            var paramName = param.Name;
            if (param.IsNullable()) str += paramTypeStr + "? " + paramName + ", ";
            else str += paramTypeStr + " " + paramName + ", ";
        }
        return str.Remove(str.Length - 2);
    }

}
