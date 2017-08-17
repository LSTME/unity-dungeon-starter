namespace Scripts.Map.Config
{
    public class Player
    {
        private int[] _startPosition;
        private string _rotation;
        public int[] Start {
            get
            {
                return _startPosition;
            }
            set
            {
                if (value.Length == 2)
                    _startPosition = value;
                else
                {
                    _startPosition = new int[2];
                    _startPosition[0] = 1;
                    _startPosition[1] = 1;
                }
            }
        }

        public string Rotation
        {
            get { return _rotation; }
            set
            {
                if (value.Length == 0)
                {
                    _rotation = "N"; 
                    return;
                }
                switch (value.ToUpper()[0])
                {
                    case 'N':
                    case 'S':
                    case 'W':
                    case 'E':
                        _rotation = value.ToUpper()[0].ToString();
                        break;
                    default:
                        _rotation = "N";
                        break;
                }
            }
        }
    }
}