using System;

namespace Omega.IoC
{
    internal struct TypeWithTag : IEquatable<TypeWithTag>
    {
        public readonly Type Type;
        public readonly string Tag;

        public readonly bool IsDefault => Type is null;
        public readonly bool IsUntagged => Tag is null; // TODO: mb rework with string.Empty?
        
        public TypeWithTag(Type type, string tag)
        {
            Type = type;
            Tag = tag;
        }
        
        public static bool operator ==(TypeWithTag lhs, TypeWithTag rhs)
            => lhs.Equals(rhs);

        public static bool operator !=(TypeWithTag lhs, TypeWithTag rhs) 
            => !lhs.Equals(rhs);

        public bool Equals(TypeWithTag other) 
            => Type == other.Type && Tag == other.Tag;

        public override bool Equals(object obj) 
            => obj is TypeWithTag other && Equals(other);

        public override string ToString() 
            => IsUntagged 
                ? Type.ToString()
                : $"({Type}, with tag: {Tag})";

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Tag != null ? Tag.GetHashCode() : 0);
            }
        }
    }
}