namespace ProjectX.Core.Context;

public interface IContextAccessor
{
    IContext? Context { get; set; }
}