namespace Scripts.Map.Config
{
    public class ObjectConfig
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Rotation { get; set; }
        public string Model { get; set; }
        public Action Actions { get; set; }
        public Teleport Teleport { get; set; }
        public Door Door { get; set; }
    }
}
