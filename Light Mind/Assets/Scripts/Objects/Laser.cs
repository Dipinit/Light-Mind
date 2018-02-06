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
            Sources = new List<RaySource>();
            _sources = new List<RaySource>();
            
            Sources.Add(new RaySource(Direction.North, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.NorthEast, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.East, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.SouthEast, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.South, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.SouthWest, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.West, false, new RayColor(true, true, true, 0.9f)));
            Sources.Add(new RaySource(Direction.NorthWest, false, new RayColor(true, true, true, 0.9f)));
            
            _sources.Add(new RaySource(Direction.North, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.NorthEast, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.East, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.SouthEast, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.South, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.SouthWest, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.West, false, new RayColor(true, true, true, 0.9f)));
            _sources.Add(new RaySource(Direction.NorthWest, false, new RayColor(true, true, true, 0.9f)));
        }
        
        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            for (int i = 0; i < _sources.Count; i++)
            {
                //Debug.Log(i.ToString() + _sources[i]);
                //Debug.Log(i.ToString() + _sources[i]);
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
                        Destroy(ray.LineRendererParent);
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