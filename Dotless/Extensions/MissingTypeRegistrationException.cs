// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Extensions.MissingTypeRegistrationException
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nop.Plugin.InstantSearch.Dotless.Extensions
{
  public class MissingTypeRegistrationException : InvalidOperationException
  {
    public MissingTypeRegistrationException(Type serviceType)
      : base(string.Format("Could not find any registered services for type '{0}'.", (object) MissingTypeRegistrationException.GetFriendlyName(serviceType)))
    {
      this.ServiceType = serviceType;
    }

    public Type ServiceType { get; }

    private static string GetFriendlyName(Type type)
    {
      if (type == typeof (int))
        return "int";
      if (type == typeof (short))
        return "short";
      if (type == typeof (byte))
        return "byte";
      if (type == typeof (bool))
        return "bool";
      if (type == typeof (char))
        return "char";
      if (type == typeof (long))
        return "long";
      if (type == typeof (float))
        return "float";
      if (type == typeof (double))
        return "double";
      if (type == typeof (Decimal))
        return "decimal";
      if (type == typeof (string))
        return "string";
      if (type == typeof (object))
        return "object";
      TypeInfo typeInfo = type.GetTypeInfo();
      return typeInfo.IsGenericType ? MissingTypeRegistrationException.GetGenericFriendlyName(typeInfo) : type.Name;
    }

    private static string GetGenericFriendlyName(TypeInfo typeInfo)
    {
      string[] array = ((IEnumerable<Type>) typeInfo.GenericTypeArguments).Select<Type, string>(new Func<Type, string>(MissingTypeRegistrationException.GetFriendlyName)).ToArray<string>();
      return string.Format("{0}<{1}>", (object) ((IEnumerable<string>) typeInfo.Name.Split('`')).First<string>(), (object) string.Join(", ", array));
    }
  }
}
