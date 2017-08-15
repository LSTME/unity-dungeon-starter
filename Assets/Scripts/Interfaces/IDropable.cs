namespace Scripts.Interfaces
{
    interface IDropable
    {
        bool DropObject();

        void SignalRemove();
    }
}
