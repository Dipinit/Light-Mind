using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    // Ray source
    public class Laser : RaySensitive
    {
        public List<RaySource> Sources;
        private List<RaySource> _sources;
        
        public override void Start()
        {
            base.Start();
            InitSources();
            ResetRays();
        }

        private void InitSources()
        {
            if (Sources == null)
                Sources = new List<RaySource>();
            if (_sources == null)
                _sources = new List<RaySource>();
        }

        private void ResetRays()
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

        public void AddSource(RaySource source)
        {
            InitSources();

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