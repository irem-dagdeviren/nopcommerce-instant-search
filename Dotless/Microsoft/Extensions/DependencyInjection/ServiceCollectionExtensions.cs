// Decompiled with JetBrains decompiler
// Type: Microsoft.Extensions.DependencyInjection.ServiceCollectionExtensions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services) where TDecorator : TService => services != null ? services.DecorateDescriptors(typeof (TService), (Func<ServiceDescriptor, ServiceDescriptor>) (x => x.Decorate(typeof (TDecorator)))) : throw new ArgumentNullException(nameof (services));

    private static IServiceCollection DecorateDescriptors(
      this IServiceCollection services,
      Type serviceType,
      Func<ServiceDescriptor, ServiceDescriptor> decorator)
    {
      if (services.TryDecorateDescriptors(serviceType, decorator))
        return services;
      throw new MissingTypeRegistrationException(serviceType);
    }

    private static bool TryDecorateDescriptors(
      this IServiceCollection services,
      Type serviceType,
      Func<ServiceDescriptor, ServiceDescriptor> decorator)
    {
      ICollection<ServiceDescriptor> descriptors;
      if (!services.TryGetDescriptors(serviceType, out descriptors))
        return false;
      foreach (ServiceDescriptor serviceDescriptor in (IEnumerable<ServiceDescriptor>) descriptors)
      {
        int index = ((IList<ServiceDescriptor>) services).IndexOf(serviceDescriptor);
        ((IList<ServiceDescriptor>) services).Insert(index, decorator(serviceDescriptor));
        ((ICollection<ServiceDescriptor>) services).Remove(serviceDescriptor);
      }
      return true;
    }

    private static bool TryGetDescriptors(
      this IServiceCollection services,
      Type serviceType,
      out ICollection<ServiceDescriptor> descriptors)
    {
      return (descriptors = (ICollection<ServiceDescriptor>) ((IEnumerable<ServiceDescriptor>) services).Where<ServiceDescriptor>((Func<ServiceDescriptor, bool>) (service => service.ServiceType == serviceType)).ToArray<ServiceDescriptor>()).Any<ServiceDescriptor>();
    }

    private static ServiceDescriptor Decorate(this ServiceDescriptor descriptor, Type decoratorType) => descriptor.WithFactory((Func<IServiceProvider, object>) (provider => provider.CreateInstance(decoratorType, provider.GetInstance(descriptor))));

    private static ServiceDescriptor WithFactory(
      this ServiceDescriptor descriptor,
      Func<IServiceProvider, object> factory)
    {
      return ServiceDescriptor.Describe(descriptor.ServiceType, factory, descriptor.Lifetime);
    }

    private static object GetInstance(this IServiceProvider provider, ServiceDescriptor descriptor)
    {
      if (descriptor.ImplementationInstance != null)
        return descriptor.ImplementationInstance;
      return descriptor.ImplementationType != (Type) null ? provider.GetServiceOrCreateInstance(descriptor.ImplementationType) : descriptor.ImplementationFactory(provider);
    }

    private static object GetServiceOrCreateInstance(this IServiceProvider provider, Type type) => ActivatorUtilities.GetServiceOrCreateInstance(provider, type);

    private static object CreateInstance(
      this IServiceProvider provider,
      Type type,
      params object[] arguments)
    {
      return ActivatorUtilities.CreateInstance(provider, type, arguments);
    }
  }
}
