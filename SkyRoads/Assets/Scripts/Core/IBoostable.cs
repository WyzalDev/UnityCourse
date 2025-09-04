using Core.Data;

namespace Core
{
    public interface IBoostable
    {
        public void OnBoostStart(BoostData startParameters);
        public void OnBoostEnd(BoostData endParameters);
    }
}