﻿using System;
using UnityEngine;

namespace Util.SerializableProperty
{
    [Serializable]
    public class SerializableQuaternion
    {
        /// <summary>
        /// x component
        /// </summary>
        public float x;

        /// <summary>
        /// y component
        /// </summary>
        public float y;

        /// <summary>
        /// z component
        /// </summary>
        public float z;

        /// <summary>
        /// w component
        /// </summary>
        public float w;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        /// <param name="rZ"></param>
        /// <param name="rW"></param>
        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }

        /// <summary>
        /// Automatic conversion from SerializableVector3 to Vector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SerializableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }
}