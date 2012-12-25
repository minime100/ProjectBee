using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectBee.Domain
{
    public class Level
    {
        /// <summary>
        /// uses the RGBA model, each value should be between 0 and 1,
        /// alpha value is used to determine intensity of the light
        /// </summary>
        public Vector4 AmbientLight { get; set; }
        public ICollection<LightSource> Lights { get; set; }
        public ICollection<GameObject> gameObjects { get; set; }
    }
}
