
using System;
namespace weatherapp.main
{
    public interface IService<T>
    {
        event Action<T> OnComplete;
        event Action OnFail;
        void StartService();
    }

    public interface IServiceWithParameters<T, TParam> : IService<T>
    {
        void StartService(TParam param);
    }
}