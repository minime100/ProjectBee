using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBee.Domain;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ProjectBee.LevelParsing
{
    public class LevelParser
    {
        public Level LoadLevel(string levelName)
        {
            XmlLevel loaded = ParseXml(levelName);

            return Parse(loaded);
        }

        private XmlLevel ParseXml(string levelName)
        {
            FileStream inputStream = new FileStream("Content/levels/" + levelName + ".xml", FileMode.Open);

            XmlSerializer serializer = new XmlSerializer(typeof(XmlLevel));
            return (XmlLevel)serializer.Deserialize(inputStream);
        }

        private Level Parse(XmlLevel toParse)
        {
            Level parsed = new Level();

            parsed.AmbientLight = new Vector4(toParse.ambientLightColor.red, toParse.ambientLightColor.blue,
                toParse.ambientLightColor.blue, toParse.ambientLightIntensity);

            parsed.Lights = new List<LightSource>();
            foreach (XmlLightSource lightSource in toParse.lightSources)
            {
                LightSource parsedLightSource = new LightSource();

                parsedLightSource.Light = new Vector4(lightSource.color.red, lightSource.color.green,
                    lightSource.color.blue, lightSource.lightIntensity);
                parsedLightSource.PositionInWorld = new Vector4(lightSource.position.x,
                    lightSource.position.y, lightSource.position.z, lightSource.position.w);

                parsed.Lights.Add(parsedLightSource);
            }

            parsed.gameObjects = new List<GameObject>();
            foreach (XmlGameObject gameObject in toParse.gameObjects)
            {
                GameObject parsedObject = new GameObject();

                // TODO error handling
                parsedObject.ObjectModelContentName = gameObject.model;

                parsedObject.PositionInWorld = new Vector3(gameObject.position.x,
                    gameObject.position.y, gameObject.position.z);

                parsedObject.RotationAxis = new Vector3(gameObject.rotationAxis.x,
                    gameObject.rotationAxis.y, gameObject.rotationAxis.z);

                parsedObject.ScalingInWorld = gameObject.scaling;

                parsedObject.World = Matrix.Identity;

                parsed.gameObjects.Add(parsedObject);
            }

            return parsed;
        }
    }
}
