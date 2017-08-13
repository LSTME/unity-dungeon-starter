namespace Scripts.Map.Config
{
    abstract public class AbstractActionPerformer
    {
        public Action Actions { get; set; }

        public void PerformActions(int actionType)
        {
            if (Actions == null) return;

            var actions = Actions.getActions(actionType);

            if (actions == null) return;

            foreach (var action in actions)
            {
                action.PerformAction();
            }
        }
    }
}
