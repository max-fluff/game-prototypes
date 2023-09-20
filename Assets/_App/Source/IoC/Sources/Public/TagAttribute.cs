using System;

namespace Omega.IoC
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class TagAttribute : Attribute
    {
        public readonly string Tag;

        public TagAttribute(string tag)
        {
            // TODO: mb use `Tag = string.Intern(tag)` for possible improve performance in negative equality checks by different references ?
            Tag = tag;
        }
    }
}