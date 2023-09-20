using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omega.IoC
{
    internal class DefaultScopedObjectProvider : IScopedObjectProvider
    {
        private readonly Dictionary<TypeWithTag, ObjectEntry> accessToEntryMap;

        public static DefaultScopedObjectProvider ValidateAndBuild(List<ObjectInfo> scopedObjects,
            IConstructorInfoProvider constructorInfoProvider, IoContainer parentContainer)
        {
            var bestCapacity = (int)(scopedObjects.Count * (1 / 0.7));
            var accessTypeToEntryMap = new Dictionary<TypeWithTag, ObjectEntry>(bestCapacity);

            // Prefill the dictionary to further reduce the complexity of the algorithm to `O(n)` instead of `O(n^2)`
            foreach (var objectInfo in scopedObjects)
                accessTypeToEntryMap.Add(objectInfo.AccessType, new ObjectEntry
                {
                    Info = objectInfo,
                    Dependencies = null
                });

            var emptyDependencies = Array.Empty<ObjectEntry>();

            void ComputeDependencies(ObjectEntry entry)
            {
                // skip dependency computing for already created singleton instances 
                if (entry.Info.Instance != null)
                {
                    entry.DependenciesIsComputed = true;
                    return;
                }

                var implType = entry.Info.ImplementationType;

                var parameterInfos = constructorInfoProvider.GetConstructorParameters(implType);
                if (parameterInfos.Length is 0)
                {
                    entry.Dependencies = emptyDependencies;
                    return;
                }

                var dependencies = new ObjectEntry[parameterInfos.Length];
                entry.Dependencies = dependencies;
                entry.DependenciesIsComputing = true;

                for (int i = 0; i < dependencies.Length; i++)
                {
                    var parameterInfo = parameterInfos[i];
                    var parameterRawType = parameterInfo.ParameterType;
                    var parameterTag = parameterInfo.GetTagOrNull();
                    var accessType = new TypeWithTag(parameterRawType, parameterTag);
                    
                    if (!accessTypeToEntryMap.TryGetValue(accessType, out var dependencyEntry))
                    {
                        if (parentContainer?.CanResolve(accessType) ?? false)
                            dependencyEntry = new ObjectEntry
                            {
                                OtherContainer = parentContainer,
                                Info = new ObjectInfo { AccessType = accessType },
                                DependenciesIsComputed = true,
                            };
                        else Fail_DependencyNotFound(entry, accessType);
                    }

                    if (dependencyEntry.DependenciesIsComputing)
                        Fail_CircularDependencyDetected(entry, dependencyEntry);

                    dependencies[i] = dependencyEntry;

                    if (!dependencyEntry.DependenciesIsComputed)
                        ComputeDependencies(dependencyEntry);
                }

                entry.DependenciesIsComputing = false;
                entry.DependenciesIsComputed = true;
            }

            static void Fail_CircularDependencyDetected(ObjectEntry initialEntry, ObjectEntry dependencyEntry)
            {
                var implType = initialEntry.Info.ImplementationType;
                var accessType = initialEntry.Info.AccessType;
                var dependencyAccessType = dependencyEntry.Info.AccessType;

                if (initialEntry == dependencyEntry)
                    throw new SelfDependenceDetectedException(implType, accessType);

                throw new CircularDependencyDetectedException(implType, accessType, dependencyAccessType);
            }

            static void Fail_DependencyNotFound(ObjectEntry initialEntry, TypeWithTag dependencyType)
            {
                var implType = initialEntry.Info.ImplementationType;

                throw new DependencyNotFoundException(implType, dependencyType);
            }

            foreach (var entry in accessTypeToEntryMap.Values)
                if (entry.Dependencies is null)
                    ComputeDependencies(entry);

            return new DefaultScopedObjectProvider(accessTypeToEntryMap);
        }

        public bool TryGetScopedInstance(TypeWithTag accessType, out object result)
        {
            // scope check, scope filter
            if (!accessToEntryMap.TryGetValue(accessType, out var objectEntry))
            {
                result = null;
                return false;
            }

            result = GetUnscopedInstance(objectEntry);
            return true;
        }

        public bool CanGetScopedInstance(TypeWithTag accessType) => accessToEntryMap.ContainsKey(accessType);

        private object GetUnscopedInstance(ObjectEntry objectEntry)
        {
            if (objectEntry.OtherContainer != null)
            {
                return objectEntry.OtherContainer.ResolveInternal(objectEntry.Info.AccessType);
            }

            if (objectEntry.Info.IsSingleton)
            {
                if (objectEntry.Info.Instance == null)
                    objectEntry.Info.Instance = CreateInstance(objectEntry);

                return objectEntry.Info.Instance;
            }

            return CreateInstance(objectEntry);
        }

        private object CreateInstance(ObjectEntry objectEntry)
        {
            var dependencies = objectEntry.Dependencies;
            var args = new object[dependencies.Length];

            for (var i = 0; i < args.Length; i++)
            {
                var dependency = dependencies[i];
                var dependencyInstance = GetUnscopedInstance(dependency);
                args[i] = dependencyInstance;
            }

            return Activator.CreateInstance(objectEntry.Info.ImplementationType, args);
        }

        private DefaultScopedObjectProvider(Dictionary<TypeWithTag, ObjectEntry> objects) => accessToEntryMap = objects;

        private sealed class ObjectEntry
        {
            /// <summary>
            /// If not null then `OtherContainer.Resolve` must be used when getting instance 
            /// </summary>
            public IoContainer OtherContainer;

            public ObjectInfo Info;
            public ObjectEntry[] Dependencies;

            // TODO: mb use enum?
            public bool DependenciesIsComputing;
            public bool DependenciesIsComputed;
        }
    }
}