﻿using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    public class Filter : ItemBase
    {
        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;
        
        // The current filter color
        private RayColor _color;

        // Public filter color settings
        public bool Red;
        public bool Green;
        public bool Blue;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            _meshRenderer = GetComponent<MeshRenderer>();
            UpdateColor();
        }

        // Update is called once per frame
        public override void Update()
        {   
            base.Update();
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                UpdateColor();
            }
        }

        // Update the current filter color
        private void UpdateColor()
        {
            // Calculate the color based on filter color settings
            _color = new RayColor(Red, Green, Blue, 0.9f);
            
            // Changed the color of the object
            _meshRenderer.material.color = _color.GetColor();

            UpdateEmittedRays();
        }

        // Calculate the filtered color
        private RayColor FilterColor(RayColor color)
        {
            // Calculate the filtered color
            var filteredColor = new RayColor(Red && color.R, Green && color.G, Blue && color.B, 0.9f);

            return filteredColor;
        }
        
        // Launched when a ray hits the mirror
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }

        public override void HandleReceivedRay(Ray ray)
        {
            RayColor filteredColor = FilterColor(ray.Color);
            if (filteredColor.R || filteredColor.G || filteredColor.B)
            {
                EmitNewRay(ray.Direction, filteredColor, ray);
            }
        }
    }
}