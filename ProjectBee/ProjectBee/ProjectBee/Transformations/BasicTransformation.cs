using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBee.Domain;
using Microsoft.Xna.Framework;

namespace ProjectBee.Transformations
{
    class BasicTransformation
    {
        public void ScaleGameObject(ref GameObject toBeScaled) {
            float scale = toBeScaled.ScalingInWorld;

            Matrix scalingMatrix;

            Matrix.CreateScale(scale, out scalingMatrix);

            toBeScaled.World *= scalingMatrix;
        }

        public void TranslateGameObject(ref GameObject toBeTranslated)
        {
            Vector3 posInWorldSpace = toBeTranslated.PositionInWorld;

            Matrix translationMatrix = Matrix.CreateTranslation(posInWorldSpace);

            toBeTranslated.World *= translationMatrix;
        }

        public void RotateGameObject(ref GameObject toBeRotated)
        {
            Vector3 axis = toBeRotated.RotationAxis;
            float angleHalf = MathHelper.ToRadians(toBeRotated.RotationAngleInDegrees) / 2.0f;
            float angleSin = (float) Math.Sin(angleHalf);

            Vector3 quatVector = new Vector3(axis.X * angleSin, axis.Y * angleSin, axis.Z * angleSin);
            float quatScalar = (float)Math.Cos(angleSin);

            Quaternion rotationQuaternion = new Quaternion(quatVector, quatScalar);
        }

        public void TransformGameObject(ref GameObject toTransform)
        {
            toTransform.World = Matrix.Identity;
            RotateGameObject(ref toTransform);
            ScaleGameObject(ref toTransform);
            TranslateGameObject(ref toTransform);
        }
    }
}
