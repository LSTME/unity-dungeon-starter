namespace Scripts.AI
{
    public class Player : PlayerBase
    {
        public void Start()
        {
            StartMap("cvicenie1");

			ShowMessage ("Toto je test!", UnityEngine.Color.red);
        }

        public void Tick()
        {
        }
    }
}