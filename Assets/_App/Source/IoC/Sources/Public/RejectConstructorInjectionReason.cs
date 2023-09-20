namespace Omega.IoC
{
    public enum RejectConstructorInjectionReason
    {
        Unknown,
        
        CantInjectToAbstractClass,
        CantInjectToInterface,
        CantInjectToStaticClass,
        CantInjectToValueType,
        
        CantInjectToArray,
        
        ConstructorNotFound,
        ConstructorsConflict,
    }
}