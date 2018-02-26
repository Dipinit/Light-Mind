using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Mirror : ItemBase
    {
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            IsOrientable = true;
        }
        
        protected override void OnOrientationChange()
        {
            UpdateEmittedRays();
        }
        
        public override void HandleReceivedRay(Ray ray)
        {
            Direction reflectionDirection = GetReflectionDirection(ray);
            if (reflectionDirection != Direction.None)
            {
                EmitNewRay(reflectionDirection, ray.Color, ray);
            }
        }
        
        // Launched when a ray hits the mirror
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }
        
        private Direction GetReflectionDirection(Ray ray)
        {
            switch (Orientation)
            {
                case Direction.East:
                    // If the hitting ray is on a valid face...
                    if (ray.Direction == Direction.SouthWest || ray.Direction == Direction.NorthWest)
                    {
                        // ... setting the reflected ray direction based on the hitting ray direction
                        if (ray.Direction == Direction.SouthWest)
                            return Direction.SouthEast;
                        if (ray.Direction == Direction.NorthWest)
                            return Direction.NorthEast;
                    }
                    // If the hitting ray is not on a valid face...
                    else
                    {
                        // ... the mirror is not reflecting
                        return Direction.None;
                    }

                    break;

                // etc for each direction (somewhat long but working and fast)
                case Direction.NorthEast:
                    if (ray.Direction == Direction.West || ray.Direction == Direction.South)
                    {
                        if (ray.Direction == Direction.West)
                            return Direction.North;
                        if (ray.Direction == Direction.South)
                            return Direction.East;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.North:
                    if (ray.Direction == Direction.SouthEast || ray.Direction == Direction.SouthWest)
                    {
                        if (ray.Direction == Direction.SouthEast)
                            return Direction.NorthEast;
                        if (ray.Direction == Direction.SouthWest)
                            return Direction.NorthWest;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.NorthWest:
                    if (ray.Direction == Direction.East || ray.Direction == Direction.South)
                    {
                        if (ray.Direction == Direction.East)
                            return Direction.North;
                        if (ray.Direction == Direction.South)
                            return Direction.West;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.West:
                    if (ray.Direction == Direction.NorthEast || ray.Direction == Direction.SouthEast)
                    {
                        if (ray.Direction == Direction.NorthEast)
                            return Direction.NorthWest;
                        if (ray.Direction == Direction.SouthEast)
                            return Direction.SouthWest;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.SouthWest:
                    if (ray.Direction == Direction.East || ray.Direction == Direction.North)
                    {
                        if (ray.Direction == Direction.East)
                            return Direction.South;
                        if (ray.Direction == Direction.North)
                            return Direction.West;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.South:
                    if (ray.Direction == Direction.NorthWest || ray.Direction == Direction.NorthEast)
                    {
                        if (ray.Direction == Direction.NorthWest)
                            return Direction.SouthWest;
                        if (ray.Direction == Direction.NorthEast)
                            return Direction.SouthEast;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
                case Direction.SouthEast:
                    if (ray.Direction == Direction.West || ray.Direction == Direction.North)
                    {
                        if (ray.Direction == Direction.West)
                            return Direction.South;
                        if (ray.Direction == Direction.North)
                            return Direction.East;
                    }
                    else
                    {
                        return Direction.None;
                    }

                    break;
            }
            return Direction.None;
        }

    }
}