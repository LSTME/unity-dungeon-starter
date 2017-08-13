namespace Scripts.Map.Config
{
    public class Counter : AbstractActionPerformer
    {
        private bool initialized = false;

        private const int FIRED_NEVER = 0;
        private const int FIRED_WHEN_MAX = 1;
        private const int FIRED_WHEN_MIN = 2;
        private const int FIRED_IN_BETWEEN = 3;
        
        private int Value = 0;

        private int lastFiredWhen = FIRED_NEVER;

        public string Name { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Start { get; set; }
        public bool FireAlways { get; set; }

        public void Initialize()
        {
            if (initialized) return;

            Value = Start;

            initialized = true;
        }

        public void Increment()
        {
            Initialize();
            
            Value = System.Math.Min(Max, ++Value);

            FireActions();
        }

        public void Decrement()
        {
            Initialize();
            
            Value = System.Math.Max(Min, --Value);

            FireActions();
        }

        private void FireActions()
        {
            if (!FireAlways && IsLastFireMatchCurrent()) return;

            switch (GetCurrentFire())
            {
                case FIRED_WHEN_MAX:
                    PerformActions(Action.ACTION_ON_MAX);
                    break;
                case FIRED_WHEN_MIN:
                    PerformActions(Action.ACTION_ON_MIN);
                    break;
                case FIRED_IN_BETWEEN:
                    PerformActions(Action.ACTION_ON_ELSE);
                    break;
            }

            SetLastFired();
        }

        private bool IsValueMax()
        {
            return Value == Max;
        }

        private bool IsValueMin()
        {
            return Value == Min;
        }

        private bool IsValueInBetween()
        {
            return Min < Value && Value < Max;
        }

        private void SetLastFired()
        {
            lastFiredWhen = GetCurrentFire();
        }

        private int GetCurrentFire()
        {
            if (IsValueMax()) { return FIRED_WHEN_MAX; }
            if (IsValueMin()) { return FIRED_WHEN_MIN; }
            if (IsValueInBetween()) { return FIRED_IN_BETWEEN; }
            return FIRED_NEVER;
        }

        private bool IsLastFireMatchCurrent()
        {
            return lastFiredWhen == GetCurrentFire();
        }
    }
}
