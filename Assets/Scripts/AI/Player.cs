namespace Scripts.AI
{
    public class Player : PlayerBase
    {
        private int maxChest = -1;

        public void Start()
        {
            StartMap("cvicenie3");            
        }

        public void Tick()
        {
            if (IsKeyDown("w"))
            {
                MoveForward();
            }
            else if (IsKeyDown("s"))
            {
                MoveBackward();
            }
            else if (IsKeyDown("a"))
            {
                TurnLeft();
            }
            else if (IsKeyDown("d"))
            {
                TurnRight();
            }
            else if (IsKeyDown("space"))
            {
                var coinsBefore = GetNumberOfCollectedCoins();
                UseBlock();
                var coinsAfter = GetNumberOfCollectedCoins();
                var deltaCoins = coinsAfter - coinsBefore;
                if (deltaCoins > maxChest)
                {
                    maxChest = deltaCoins;
                }
                ShowMessage(string.Format("Vybral si práve {0}\nmax truhlica bola zatiaľ {1}", deltaCoins, maxChest));                
            }                    
        }
    }
}