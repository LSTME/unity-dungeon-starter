namespace Scripts.Map.Config
{
    public class Teleport
    {
        private int[] _target;
        private string _rotation;
        public int[] Target {
            get
            {
                return _target;
            }
            set
            {
                if (value.Length == 2)
                    _target = value;
                else
                {
                    _target = new int[2];
                    _target[0] = 2;
                    _target[1] = 2;
                }
            }
        }

        public string Rotation
        {
            get { return _rotation; }
            set
            {
                if (value == null)
                {
                    _rotation = null;
                    return;
                }
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
