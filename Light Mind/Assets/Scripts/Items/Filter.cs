﻿using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    public class Filter : ItemBase
    {
        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            IsColorable = true;
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        public override void Update()
        {   
            base.Update();
        }

        // Update the current filter color
        public override void SetColor(RayColor color)
        {
            base.SetColor(color);
            
            // Changed the color of the object
            _meshRenderer.material.color = color.GetColor();

            UpdateEmittedRays();
        }

        // Calculate the filtered color
        private RayColor FilterColor(RayColor color)
        {
            // Calculate the filtered color
            var filteredColor = new RayColor(Color.R && color.R, Color.G && color.G, Color.B && color.B, RayColor.DEFAULT_ALPHA);

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
            var newColor = new RayColor(ray.Color.R ^ filteredColor.R, ray.Color.G ^ filteredColor.G, ray.Color.B ^ filteredColor.B, RayColor.DEFAULT_ALPHA);
            EmitNewRay(ray.Direction, newColor, ray);
        }
    }
}