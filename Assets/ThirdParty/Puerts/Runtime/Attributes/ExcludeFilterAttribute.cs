using System;

namespace Puerts.Attributes {

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class ExcludeFilterAttribute : Attribute { }

}
