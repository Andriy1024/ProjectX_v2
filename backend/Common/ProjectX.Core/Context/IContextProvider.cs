namespace ProjectX.Core.Context;

public interface IContextProvider
{
    IContext Current();
}