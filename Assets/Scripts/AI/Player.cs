namespace Scripts.AI
{
    public class Player : PlayerBase
    {
        public void Start()
        {
            StartMap("cvicenie2");
            // TurnRight();
            // for (int i = 0; i < 3; i++) {
            //     MoveForward();
            // }
            // TurnLeft();
            // while (FrontBlock().IsWalkable) {
            //     MoveForward();
            // }
            // TurnRight();
            // MoveForward();
            // TurnRight();
            // TurnLeft();
            // MoveForward();
            // MoveForward();
            // TurnRight();
            // for (int i = 0; i < 2; i++) {
            //     MoveForward();
            // }
            // TurnRight();
            // while (FrontBlock().IsWalkable) {
            //     MoveForward();
            // }
            // UseBlock();
            // TurnAround();
            // while (!LeftBlock().IsPressable) {
            //     MoveForward();
            // }
            // TurnLeft();
            // while (!BackBlock().IsPickable) {
            //     MoveBackward();
            // }
            // TurnAround();
            // UseBlock();
            // while (!FrontBlock().IsPressable) {
            //     MoveBackward();
            // }
            // UseBlock();
            // TurnRight();
            // while (!LeftBlock().IsPickable) {
            //     MoveForward();
            // }
            // MoveBackward();
            // TurnRight();
            // MoveBackward();
            // while (LeftBlock().IsPickable) {
            //     MoveBackward();
            // }
            // TurnAround();
            // MoveBackward();
            // TurnLeft();
            // MoveForward();
            // MoveForward();
            // MoveForward();
        }

        public void Tick() {

        }

        void TurnAround() {
            TurnRight();
            TurnRight();
        }
    }
}
