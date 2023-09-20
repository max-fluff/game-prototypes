using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Omega.IoC
{
    public delegate void ConfigureDelegate(IoContainerBuilder builder);

    public sealed class IoContainer
    {
        private readonly IoContainer parent;
        private readonly IScopedObjectProvider scopedObjectProvider;

        public static IoContainer Configure(ConfigureDelegate action)
            => Configure(null, action);

        public static IoContainer Configure(IoContainer parent, ConfigureDelegate action)
        {
            var injector = new DefaultInjector();
            var builder = new IoContainerBuilder(parent, injector);
            action(builder);

            var configuration = builder.GetConfiguration();
            var objectProvider = DefaultScopedObjectProvider.ValidateAndBuild(configuration, injector, parent);
            return new IoContainer(parent, objectProvider);
        }

        internal bool CanResolve(TypeWithTag accessType)
        {
            if (scopedObjectProvider.CanGetScopedInstance(accessType))
                return true;

            if (parent is null)
                return false;

            return parent.CanResolve(accessType);
        }

        public TAccess Resolve<TAccess>(string tag = null)
            where TAccess : class // non value type constraint 
        {
            var accessType = typeof(TAccess);
            return (TAccess)ResolveInternal(new TypeWithTag(accessType, tag));
        }

        public bool TryResolve<TAccess>(out TAccess result, string tag = null)
            where TAccess : class // non value type constraint
        {
            var accessType = typeof(TAccess);

            if (scopedObjectProvider.TryGetScopedInstance(new TypeWithTag(accessType, tag), out var resultObject))
            {
                result = (TAccess)resultObject;
                return true;
            }

            if (parent is null)
            {
                result = default;
                return false;
            }

            result = parent.Resolve<TAccess>();
            return true;
        }

        private IoContainer(IoContainer parent, IScopedObjectProvider scopedObjectProvider)
        {
            this.parent = parent;
            this.scopedObjectProvider = scopedObjectProvider;
        }

        internal object ResolveInternal(TypeWithTag accessType)
        {
            if (scopedObjectProvider.TryGetScopedInstance(accessType, out var result))
                return result;

            if (parent is null)
                throw new InvalidOperationException($"Object type {accessType} is not configured in the container");

            return parent.ResolveInternal(accessType);
        }
    }

    public readonly ref struct IoContainerBuilder
    {
        private readonly IoContainer parent;
        private readonly DefaultInjector _injector;
        private readonly List<ObjectInfo> items;

        // TODO
        internal bool AllowValueTypeImplementation
        {
            get => _injector.AllowValueTypeImplementation;
            set => _injector.AllowValueTypeImplementation = value;
        }

        public void Add<TAccess, TImpl>(string tag = null)
            where TImpl : class, TAccess
        {
            var accessType = typeof(TAccess);
            var implementationType = typeof(TImpl);

            Add(accessType, tag, implementationType, isSingleton: false);
        }

        public void AddSingleton<TAccess, TImpl>(string tag = null)
            where TImpl : class, TAccess
        {
            var accessType = typeof(TAccess);
            var implementationType = typeof(TImpl);

            Add(accessType, tag, implementationType, isSingleton: true);
        }

        public void AddSingleton<TAccess, TImpl>(TImpl instance, string tag = null)
            where TImpl : class, TAccess
        {
            var accessType = typeof(TAccess);
            var implementationType = typeof(TImpl);

            Add(accessType, tag, implementationType, isSingleton: true, instance);
        }

        public void Add<TImpl>(string tag = null)
            where TImpl : class
        {
            var accessType = typeof(TImpl);
            var implementationType = typeof(TImpl);

            Add(accessType, tag, implementationType, isSingleton: false);
        }

        public void AddSingleton<TImpl>(string tag = null)
            where TImpl : class
        {
            var accessType = typeof(TImpl);
            var implementationType = typeof(TImpl);

            Add(accessType, tag, implementationType, isSingleton: true);
        }

        public void AddSingleton<TAccess>(TAccess instance, string tag = null)
        {
            var accessType = typeof(TAccess);
            var implementationType = instance.GetType();

            Add(accessType, tag, implementationType, isSingleton: true, instance);
        }


        /// <summary>
        /// Calling code must make sure that 'implementationType' implements the 'accessType'
        /// </summary>
        /// <param name="rawAccessType"></param>
        /// <param name="accessTag"></param>
        /// <param name="implementationType"></param>
        /// <param name="isSingleton"></param>
        /// <exception cref="Exception"></exception>
        private void Add(Type rawAccessType, string accessTag,
            Type implementationType,
            bool isSingleton, object instance = null)
        {
            // We should not check if we are able to create new instances in case the dependency is a singleton-instance
            if (!isSingleton) 
                if (!_injector.CanBeInjected(implementationType, out var rejectReason))
                    Fail_InvalidImpl(implementationType, rejectReason);

            var accessType = new TypeWithTag(rawAccessType, accessTag);

            // It is not possible to define multiple implementations for the same accessType
            if (items.TryFind(e => e.AccessType == accessType, out var definedInfo))
                Fail_UnableMultipleImpls(accessType, definedInfo.ImplementationType, implementationType);

            // The implementation is already defined in the parent container 
            if (parent != null && parent.CanResolve(accessType))
                Fail_UnableOverlapImpls(accessType);

            if (instance != null && !isSingleton)
                throw new InvalidOperationException("Internal error. Cant configure instance as not singleton");

            var entry = new ObjectInfo()
            {
                AccessType = accessType,
                ImplementationType = implementationType,
                IsSingleton = isSingleton,
                Instance = instance
            };

            items.Add(entry);

            static void Fail_UnableMultipleImpls(TypeWithTag accessType, Type firstImpl, Type secondType)
                => throw new UnableMultipleImplementationsException(accessType, firstImpl, secondType);

            static void Fail_UnableOverlapImpls(TypeWithTag accessType)
                => throw new UnableOverlapImplementationException(accessType);

            static void Fail_InvalidImpl(Type implType, RejectConstructorInjectionReason rejectReason)
                => throw InvalidImplementationTypeException.New(implType, rejectReason);
        }

        internal IoContainerBuilder(IoContainer parent, DefaultInjector injector)
        {
            this.parent = parent;
            this._injector = injector;
            this.items = new List<ObjectInfo>(7);
        }

        internal List<ObjectInfo> GetConfiguration()
        {
            return items;
        }
    }
}