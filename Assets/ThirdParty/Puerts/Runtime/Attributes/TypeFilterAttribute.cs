using System;

namespace Puerts.Attributes {

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class TypeFilterAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class PuertsBlacklistAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class TypingExportAttribute : Attribute { }

}
