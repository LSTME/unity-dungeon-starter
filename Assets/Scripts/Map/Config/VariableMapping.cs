using UnityEngine;

namespace Scripts.Map.Config
{
    public class VariableMapping
    {
        public string Name { get; set; }
        public bool Negate { get; set; }

        public bool GetState()
        {
            var GameLogic = getGameLogic();
            if (GameLogic == null) return false;

            var State = GameLogic.GetVariable(Name);

            if (Negate) return !State;
            return State;
        }

        private GameLogic getGameLogic()
        {
            var Maps = GameObject.FindGameObjectsWithTag("Map");

            foreach (var Map in Maps)
            {
                var component = Map.GetComponent<MapGenerator>();
                if (component != null) return component.GameLogic;
            }

            return null;
        }
    }
}
