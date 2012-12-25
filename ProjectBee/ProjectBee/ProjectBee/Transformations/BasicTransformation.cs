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
            float angle = MathHelper.ToRadians(toBeRotated.RotationAngleInDegrees);
            float angleHalf = angle / 2;
            float sinAngleHalf = (float)Math.Sin(angleHalf);
            Quaternion rotationQuaternion = new Quaternion(axis.X * sinAngleHalf, axis.Y * sinAngleHalf, axis.Z * sinAngleHalf, (float)Math.Cos(angleHalf));
            rotationQuaternion.Normalize();

            Matrix rotationMatrix = Matrix.CreateFromQuaternion(rotationQuaternion);

            toBeRotated.World *= rotationMatrix;
        }

        public void TransformGameObject(ref GameObject toTransform)
        {
            toTransform.World = Matrix.Identity;
            ScaleGameObject(ref toTransform);
            TranslateGameObject(ref toTransform);
            RotateGameObject(ref toTransform);
        }
    }
}
