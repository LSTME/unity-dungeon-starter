namespace Scripts.Interfaces
{
    interface IOpenable
    {
        void ActionOpen(string target);
        void ActionClose(string target);

        bool GetOpenState();
    }
}
