using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectBee.Domain
{
    /// <summary>
    /// a basic game object with its mesh, world position, scaling and rotation and the resulting world matrix as fields
    /// </summary>
    public class GameObject
    {
        public Model ObjectModel { get; set; }
        public string ObjectModelContentName { get; set; }
        public Vector3 PositionInWorld { get; set; }
        public float ScalingInWorld { get; set; }
        public Vector3 RotationAxis { get; set; }
        public float RotationAngleInDegrees;
        public Matrix World { get; set; }
    }
}
