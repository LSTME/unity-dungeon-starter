namespace Scripts.Map.Config
{
    public class ObjectConfig
    {
        private string _rotation;
        
        public string Type { get; set; }
        public string Name { get; set; }
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
        public string Model { get; set; }
        public Action Actions { get; set; }
        public Teleport Teleport { get; set; }
        public Door Door { get; set; }
        public Torch Torch { get; set; }
        public Chest Chest { get; set; }
    }
}
