using System;
using System.Collections.Generic;
using System.Reflection;

namespace Omega.IoC
{
    internal sealed class DefaultInjector : IInjectorValidator, IConstructorInfoProvider
    {
        private readonly Dictionary<Type, Entry> map;
        // TODO
        public bool AllowValueTypeImplementation;
        public bool AllowValueTypeImplementationWithoutDefinedConstructor;

        public bool CanBeInjected(Type type, out RejectConstructorInjectionReason rejectReason)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsValueType && !AllowValueTypeImplementation)
            {
                rejectReason = RejectConstructorInjectionReason.CantInjectToValueType;
                return false;
            }

            if (type.IsInterface)
            {
                rejectReason = RejectConstructorInjectionReason.CantInjectToInterface;
                return false;
            }

            if (type.IsAbstract)
            {
                rejectReason = type.IsSealed
                    ? RejectConstructorInjectionReason.CantInjectToStaticClass
                    : RejectConstructorInjectionReason.CantInjectToAbstractClass;

                return false;
            }

            if (type.IsArray)
            {
                rejectReason = RejectConstructorInjectionReason.CantInjectToArray;

                return false;
            }


            if (!map.TryGetValue(type, out var entry))
                map.Add(type, entry = CreateEntry(type));

            rejectReason = entry.RejectInjectionReason;
            return entry.CanBeInject;
        }

        public ParameterInfo[] GetConstructorParameters(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (!map.TryGetValue(type, out var entry))
                map.Add(type, entry = CreateEntry(type));

            if (!entry.CanBeInject)
                throw new InvalidOperationException(entry.RejectInjectionReason.ToString());

            return entry.ConstructorParameters;
        }

        private Entry CreateEntry(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (constructors.Length == 0)
            {
                if (type.IsValueType)
                    if (AllowValueTypeImplementation && AllowValueTypeImplementationWithoutDefinedConstructor)
                        return new Entry(Array.Empty<ParameterInfo>());

                return new Entry(RejectConstructorInjectionReason.ConstructorNotFound);
            }

            if (constructors.Length > 1)
                return new Entry(RejectConstructorInjectionReason.ConstructorsConflict);

            // if(constructors.Length == 1)
            var constructor = constructors[0];
            var constructorParameters = constructor.GetParameters();
            return new Entry(constructorParameters);
        }

        public DefaultInjector(int initialCapacity = 70)
        {
            map = new Dictionary<Type, Entry>(initialCapacity);
        }

        private struct Entry
        {
            public bool CanBeInject;
            public RejectConstructorInjectionReason RejectInjectionReason;
            public ParameterInfo[] ConstructorParameters;

            public Entry(RejectConstructorInjectionReason rejectInjectionReason)
            {
                CanBeInject = false;
                RejectInjectionReason = rejectInjectionReason;
                ConstructorParameters = default;
            }

            public Entry(ParameterInfo[] constructorParameters) : this()
            {
                CanBeInject = true;
                RejectInjectionReason = default;
                ConstructorParameters = constructorParameters;
            }
        }
    }
}