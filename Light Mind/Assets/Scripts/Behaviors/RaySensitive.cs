using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;
using Ray = Items.Ray;

namespace Behaviors
{
    public class RaySensitive : MonoBehaviour
    {
        public List<Ray> ReceveidRays;
        public List<Ray> EmittedRays;
        protected MeshCollider _meshCollider;
        
        public AudioSource[] AudioSources;
        public ParticleSystem ParticleSystem;
        
        
        public virtual void Start()
        {
            _meshCollider = GetComponent<MeshCollider>();
            if (_meshCollider) _meshCollider.convex = false;
            AudioSources = GetComponents<AudioSource>();
            ParticleSystem = GetComponent<ParticleSystem>();
            ReceveidRays = new List<Ray>();
            EmittedRays = new List<Ray>();
        }

        public virtual void ResetRays()
        {
        }
        
        public virtual void Update ()
        {
            for (int i = 0; i < EmittedRays.Count; i++)
            {
                if (EmittedRays[i].Enabled
                    && (EmittedRays[i].Color.R || EmittedRays[i].Color.B || EmittedRays[i].Color.G))
                {
                    EmittedRays[i].Emit();
                }
            }
        }

        public void Disable()
        {
            DestroyEmittedRays();
            if (_meshCollider) _meshCollider.convex = false;

        }

        public void Enable()
        {
            if (_meshCollider) _meshCollider.convex = true;
            ResetRays();
        }
        
        // Launched when a ray hits the object
        public virtual void HitEnter(Ray ray)
        {
            for (int i = 0; i < ReceveidRays.Count; i++)
            {
                if (ReceveidRays[i].Id == ray.Id)
                {   
                    return;
                }
            }
            
            this.ReceveidRays.Add(ray);
            
            // Log a new Hit evenement
            Debug.LogWarning(string.Format("{0} {1} hit {2} {3} with color {4} and direction {5}",
                ray.RayEmitter.transform.gameObject,
                ray.RayEmitter.transform.gameObject.GetInstanceID(),
                transform.gameObject,
                transform.gameObject.GetInstanceID(),
                ray.Color,
                ray.Direction
            ));
        }
        
        // Launched when a ray stops hitting the object
        public virtual void HitExit(Ray ray)
        {
            foreach (var emittedRay in EmittedRays)
            {
                if (emittedRay.Parent != null && emittedRay.Parent.Id == ray.Id)
                {
                    Ray.Delete(emittedRay);
                }
            }
              
            EmittedRays.RemoveAll(r => r.Parent != null && r.Parent.Id == ray.Id);
            
            for (int i = 0; i < ReceveidRays.Count; i++)
            {
                if (ReceveidRays[i].Id == ray.Id)
                {
                    ReceveidRays.RemoveAt(i);

                    Debug.LogWarning(string.Format("{0} {1} stopped hitting {2} {3} with color {4} and direction {5}",
                        ray.RayEmitter.transform.gameObject,
                        ray.RayEmitter.transform.gameObject.GetInstanceID(),
                        transform.gameObject,
                        transform.gameObject.GetInstanceID(),
                        ray.Color,
                        ray.Direction
                    ));
                    break;
                }
            }
            
            if (EmittedRays.Count == 0) StopReboundFeedback();
        }

        private void StartReboundFeedback()
        {
            if (AudioSources != null && AudioSources.Length >= 2 && !AudioSources[1].isPlaying)
                AudioSources[1].Play();
            
            if (ParticleSystem != null && !ParticleSystem.isPlaying) 
                ParticleSystem.Play();
        }
        
        private void StopReboundFeedback()
        {
           
            if (ParticleSystem != null && ParticleSystem.isPlaying) 
                ParticleSystem.Stop();
        }
        
        
        public Ray EmitNewRay(Direction direction, RayColor rayColor, Ray parent)
        {
            Debug.LogWarning(string.Format("Emitting new ray from {0} with color {1} and direction {2}",
                transform.gameObject,
                rayColor,
                direction
            ));
            Ray ray = new Ray(this, rayColor, direction, parent);
            EmittedRays.Add(ray);
            StartReboundFeedback();
            return ray;
        }

        public void DestroyEmittedRays()
        {
            foreach (var emittedRay in EmittedRays)
            {
                Ray.Delete(emittedRay);
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

        public MeshCollider MeshCollider
        {
            get { return _meshCollider; }
        }
    }
}