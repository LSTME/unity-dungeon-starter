namespace Scripts.Interfaces
{
    interface ISwitchable
    {
        void ActionSwitchOn(string target);
        void ActionSwitchOff(string target);
        
        bool GetSwitchState();
    }
}
