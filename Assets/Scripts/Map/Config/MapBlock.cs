using System.Collections.Generic;

namespace Scripts.Map.Config
{
    public class MapBlock
    {
        private int[] _position;
        private List<ObjectConfig> _objects;

        public int[] Position
        {
            get { return _position; }
            set
            {
                if (value.Length == 2)
                {
                    _position = value;
                }
                else
                {
                    var _default = new int[2];
                    _default[0] = 0;
                    _default[1] = 0;
                    _position = _default;
                }
            }
        }
        public List<ObjectConfig> Objects { 
            get { return _objects; }
            set
            {
                if (value != null)
                {
                    _objects = value;
                }
                else
                {
                    _objects = new List<ObjectConfig>();
                }
            }
        }
    }
}
