using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGUtil //stands for grapple game utility
{
    [Serializable]
    public struct ClampFloat : IEquatable<ClampFloat>
    {
        public float Min;
        public float Max;

        [SerializeField] float _Value;

        public ClampFloat(float _Min = 0, float _Max = 1, float _val = 0) //constructor
        {
            Min = _Min;
            Max = _Max;
            _Value = Mathf.Clamp(_val, Min, Max);
        }

        public float Value //modifier and accessor for the value. stored as private for displaying 
        {
            get { return _Value; }
            set { _Value = Mathf.Clamp(value, Min, Max); }
        }

        public bool Equals(ClampFloat other)
        {
            return (Min == other.Min && Max == other.Max && Value == other.Value);
        }
    }
    
    [Serializable]
    public struct ClampFloat01 : IEquatable<ClampFloat01>
    {
        [SerializeField] float _Value;

        public ClampFloat01(float _val = 0) //constructor
        {
            _Value = Mathf.Clamp01(_val);
        }

        public float Value //modifier and accessor for the value. stored as private for displaying 
        {
            get { return _Value; }
            set { _Value = Mathf.Clamp01(value); }
        }

        public bool Equals(ClampFloat01 other)
        {
            return (Value == other.Value);
        }
    }

    [Serializable]
    public struct RangeF
    {
        public float Min;
        public float Max;

        public RangeF(float _min = 0, float _max = 1)
        {
            Min = _min;
            Max = _max;
        }
    }

    public static class GGMath
    {
        //------------------------------------------- BLERP -------------------------------------------------------//

        /// <summary>
        /// Bouncy Interpolation <br></br>
        /// </summary>
        /// https://www.desmos.com/calculator/jwrtshsuqn Desmos Graph Link
        public static Vector3 Blerp(ref Vector3 Current, ref Vector3 Velocity, Vector3 Target, float MaxDistance = -1, float acceleration = 500f, double drag = 5E-9f, float dt = 1 / 60)
        {
            Velocity += (Target - Current) * acceleration * dt;
            Velocity = new Vector3((float)(Velocity.x * Math.Pow(drag, dt)), 
                (float)(Velocity.y * Math.Pow(drag, dt)), 
                (float)(Velocity.z * Math.Pow(drag, dt))); //Multiply by a double and THEN cast, to minimize loss
            var dv = (Math.Pow(drag, dt * dt) - 1) / (dt * Math.Log(drag));
            Current += Velocity * (float)dv;
            if (MaxDistance >= 0) Current = Vector3.Distance(Current, Target) < MaxDistance ? Target : (Current - Target).normalized * MaxDistance + Target; //clamp to within MaxDistance of the target
            return Current;
        }

        /// <summary>
        /// Bouncy Interpolation
        /// </summary>
        public static double Blerp(ref double Current, ref double Velocity, double Target, float MaxDistance = -1, double acceleration = 500, double drag = 5E-9f, float dt = 1 / 60)
        {
            Velocity += (Target - Current) * acceleration * dt;
            Velocity *= Math.Pow(drag, dt);
            Current += Velocity * (Math.Pow(drag, dt * dt) - 1) / (dt * Math.Log(drag));
            if (MaxDistance >= 0) if (MaxDistance >= 0) Current = Math.Abs(Current - Target) < MaxDistance ? Target : Math.Sign(Current - Target) * MaxDistance + Target; //clamp to within MaxDistance of the target
            return Current;
        }

        // - - - - - - - - BLERP EXTENSIONS - - - - - - - - //

        /// <summary>
        /// Bouncy interpolation
        /// </summary>
        public static Vector3 Blerp(this Vector3 Current, ref Vector3 Velocity, Vector3 Target, float MaxDistance = -1, float acceleration = 500f, float drag = 20f, float dt = 1 / 60)
        {
            Velocity += (Target - Current) * acceleration * dt;
            Velocity = new Vector3((float)(Velocity.x * Math.Pow(drag, dt)),
                (float)(Velocity.y * Math.Pow(drag, dt)),
                (float)(Velocity.z * Math.Pow(drag, dt))); //Multiply by a double and THEN cast, to minimize loss
            var dv = (Math.Pow(drag, dt * dt) - 1) / (dt * Math.Log(drag));
            Current += Velocity * (float)dv;
            if (MaxDistance >= 0) Current = Vector3.Distance(Current, Target) < MaxDistance ? Target : (Current - Target).normalized * MaxDistance + Target; //clamp to within MaxDistance of the target
            return Current;
        }

        //------------------------------------------- REMAP & CLAMP -------------------------------------------------------//

        ///<summary>
        /// Returns a value with it's domain remapped from (fromMin, fromMax) to (toMin, to Max)
        ///</summary>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (toMin - fromMin) * (toMax - fromMax) + fromMax;
        }

        public static float Remap2(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float pos = Mathf.InverseLerp(fromMin, fromMax, value);
            return (toMax - toMin) * pos + toMin;
        }

        public static Vector3 ClampVector(this Vector3 value, Vector3 min, Vector3 max)
        {
            Vector3 result = new Vector3();
            result.x = Mathf.Clamp(value.x, min.x, max.x);
            result.y = Mathf.Clamp(value.y, min.y, max.y);
            result.z = Mathf.Clamp(value.z, min.z, max.z);

            return result;
        }

        //------------------------------------------- SPACE CONVERSION(S) -------------------------------------------------------//
        /// <summary>
        /// Convert from a worldspace point to a canvas space point
        /// </summary>
        public static Vector2 WorldToCanvasPoint(Vector3 WorldPoint, RectTransform CanvasRect, Camera cam)
        {
            Vector2 ViewportPos = cam.WorldToViewportPoint(WorldPoint);
            Vector2 WorldObjectScreenPos = new Vector2(
                ((ViewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x / 2)),
                ((ViewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y / 2)));

            return WorldObjectScreenPos;
        }


        //------------------------------------------- UNITYENGINE.PHYSICS EXTENTIONS -------------------------------------------------------//
        /// <summary>
        /// Physics cast in a cone shape, similar to SphereCastAll()<br></br>
        /// Credit to walterellisfun on github<br></br>
        /// Link: https://github.com/walterellisfun/ConeCast/blob/master/ConeCastExtension.cs
        /// </summary>
        public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance, layerMask, queryTriggerInteraction);
            List<RaycastHit> coneCastHitList = new List<RaycastHit>();

            if (sphereCastHits.Length > 0)
            {
                for (int i = 0; i < sphereCastHits.Length; i++)
                {
                    Vector3 hitPoint = sphereCastHits[i].point;
                    Vector3 directionToHit = hitPoint - origin;
                    float angleToHit = Vector3.Angle(direction, directionToHit);

                    if (angleToHit < coneAngle)
                    {
                        coneCastHitList.Add(sphereCastHits[i]);
                    }
                }
            }

            RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
            coneCastHits = coneCastHitList.ToArray();

            return coneCastHits;
        }

        /// <summary>
        /// Creates a sphere cast that grows in size
        /// </summary>
        public static RaycastHit GrowthSphereCast(Vector3 position, float maxRadius, Vector3 direction, int stepCount = 5, float range = Mathf.Infinity, int layerMask = ~0, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            RaycastHit _hit = new RaycastHit();
            if (!Physics.SphereCast(position, maxRadius, direction, out _hit, range, layerMask, queryTriggerInteraction)) return _hit; //will never grow to hit anything
            
            //the initial raycast is step zero so set step to 1
            int step = 1;

            float radius = maxRadius;

            RaycastHit closestHit = _hit;
            while (step <= stepCount)
            {
                radius += (_hit.collider ? -1 : 1) * maxRadius / Mathf.Pow(2, step); //increase or decrease radius based on whether a hit was found.
                if (Physics.SphereCast(position, radius, direction, out _hit, range, layerMask, queryTriggerInteraction)) closestHit = _hit; //cast and update closest hit
                step++; //increment step#
            }
            

            return closestHit;
        }
    }
}
