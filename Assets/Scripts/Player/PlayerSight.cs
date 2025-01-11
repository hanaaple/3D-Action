using Character;
using UnityEngine;

namespace Player
{
    public class PlayerSight : Sight
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        protected override void Update()
        {
            transform.position = _camera.transform.position;
            transform.rotation = _camera.transform.rotation;
            depth = _camera.farClipPlane;
            fov = _camera.fieldOfView;
            aspect = _camera.aspect;
            
            base.Update();
        }
    }
}