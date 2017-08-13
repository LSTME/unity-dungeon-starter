namespace Scripts.Interfaces.Logic
{
    public interface ILogical
    {
        void SetVariable(string variable, bool value);
        bool GetVariable(string Variable);
    }
}
