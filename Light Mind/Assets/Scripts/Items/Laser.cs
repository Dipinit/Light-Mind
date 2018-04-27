using System.Collections.Generic;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;

namespace Items
{
    // Ray source
    public class Laser : ItemBase
    {
        public List<RaySource> Sources;
        private List<RaySource> _sources;
        
        public override void Awake()
        {
            base.Awake();
            Sources = new List<RaySource>();
            _sources = new List<RaySource>();
        }

        public void Start()
        {
            ResetRays();
        }

        public override void ResetRays()
        {
            foreach (var ray in EmittedRays)
            {
                Ray.Delete(ray);
            }
            EmittedRays.Clear();
            
            for (int i = 0; i < _sources.Count; i++)
            {
                if (_sources[i].Enabled && (_sources[i].Color.R || _sources[i].Color.G || _sources[i].Color.B))
                {
                    EmitNewRay(_sources[i].Direction, _sources[i].Color, null);
                }
            }
        }

        /// <summary>
        /// Add a new ray source into the board.
        /// </summary>
        /// <param name="source"></param>
        public void AddSource(RaySource source)
        {
            Sources.Add(new RaySource(
                source.Direction,
                source.Enabled,
                new RayColor(source.Color.R, source.Color.G, source.Color.B, source.Color.Alpha)));
            _sources.Add(new RaySource(
                source.Direction, 
                source.Enabled, 
                new RayColor(source.Color.R, source.Color.G, source.Color.B, source.Color.Alpha)));
        }
        
        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            
            for (int i = 0; i < _sources.Count; i++)
            {
                if (!(_sources[i].Equals(Sources[i])))
                {
                    _sources[i].Color.R = Sources[i].Color.R;
                    _sources[i].Color.G = Sources[i].Color.G;
                    _sources[i].Color.B = Sources[i].Color.B;
                    _sources[i].Color.Alpha = Sources[i].Color.Alpha;
                    _sources[i].Enabled = Sources[i].Enabled;
                    _sources[i].Direction = Sources[i].Direction;

                    Ray ray = EmittedRays.Find(r => r.Direction == _sources[i].Direction);
                    if (ray != null)
                    {
                        Ray.Delete(ray);
                        EmittedRays.Remove(ray);
                    }
                    
                    if (_sources[i].Enabled && (_sources[i].Color.R || _sources[i].Color.G || _sources[i].Color.B))
                    {
                        EmitNewRay(_sources[i].Direction, _sources[i].Color, null);
                    }
                }
            }
        }
    }
}