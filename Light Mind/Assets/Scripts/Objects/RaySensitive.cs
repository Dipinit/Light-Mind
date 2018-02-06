using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class RaySensitive : MonoBehaviour
    {
        public List<Ray> ReceveidRays;
        public List<Ray> EmittedRays;
        
        
        public virtual void Start()
        {
            ReceveidRays = new List<Ray>();
            EmittedRays = new List<Ray>();
        }
        
        public virtual void Update ()
        {
            for (int i = 0; i < EmittedRays.Count; i++)
            {
                if (EmittedRays[i].Enabled && (EmittedRays[i].Color.R || EmittedRays[i].Color.B || EmittedRays[i].Color.G))
                    EmittedRays[i].Emit();
            }
        }
        
        // Launched when a ray hits the object
        public virtual void HitEnter(Ray ray)
        {
            for (int i = 0; i < ReceveidRays.Count; i++)
            {
                if (ReceveidRays[i] == ray)
                {   
                    return;
                }
            }
            
            this.ReceveidRays.Add(ray);
            
            // Log a new Hit evenement
            Debug.Log(string.Format("{0} {1} hit {2} {3} with color {4} and direction {5}",
                ray.RayEmitter.transform.gameObject,
                ray.RayEmitter.transform.gameObject.GetInstanceID(),
                transform.parent.gameObject,
                transform.parent.gameObject.GetInstanceID(),
                ray.Color,
                ray.Direction
            ));
        }
        
        // Launched when a ray stops hitting the object
        public virtual void HitExit(Ray ray)
        {
            foreach (var emittedRay in EmittedRays)
            {
                if (emittedRay.Parent == ray)
                {
                    Destroy(emittedRay.LineRendererParent);
                }
            }
              
            EmittedRays.RemoveAll(r => r.Parent == ray);
            
            for (int i = 0; i < ReceveidRays.Count; i++)
            {
                if (ReceveidRays[i] == ray)
                {
                    ReceveidRays.RemoveAt(i);

                    Debug.Log(string.Format("{0} {1} stopped hitting {2} {3} with color {4} and direction {5}",
                        ray.RayEmitter.transform.gameObject,
                        ray.RayEmitter.transform.gameObject.GetInstanceID(),
                        transform.parent.gameObject,
                        transform.parent.gameObject.GetInstanceID(),
                        ray.Color,
                        ray.Direction
                    ));
                    break;
                }
            }
        }
        
        public Ray EmitNewRay(Direction direction, RayColor rayColor, Ray parent)
        {
            Debug.Log(string.Format("Emitting new ray from {0} with color {1} and direction {2}",
                transform.parent.gameObject,
                rayColor,
                direction
            ));
            Ray ray = new Ray(this, rayColor, direction, parent);
            EmittedRays.Add(ray);
            return ray;
        }

        public void DestroyEmittedRays()
        {
            foreach (var emittedRay in EmittedRays)
            {
                Destroy(emittedRay.LineRendererParent);
            }
            
            EmittedRays.Clear();
        }
        
        public virtual void HandleReceivedRay(Ray ray)
        {
        }

        public virtual void UpdateEmittedRays()
        {
            DestroyEmittedRays();
            foreach (var ray in ReceveidRays)
            {
                HandleReceivedRay(ray);
            }
        }
    }
}