using System.Collections.Generic;

namespace Scripts.Map.Config
{
    public class Config
    {
        private Player _player;
        public List<MapBlock> MapBlocks { get; set; }
        public GameLogic GameLogic { get; set; }

        public Player Player
        {
            get
            {
                if (_player != null) return _player;
                
                _player = new Player();
                
                int[] position = new int[2];
                position[0] = 1;
                position[1] = 1;

                _player.Start = position;
                _player.Rotation = "N";
                
                return _player;
            }
            set { _player = value; }
        }
    }
}
