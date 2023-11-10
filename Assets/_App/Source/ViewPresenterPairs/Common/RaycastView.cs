using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaxFluff.Prototypes
{
    public sealed class RaycastView : ViewBase
    {
        public EventSystem EventSystem;
        public GraphicRaycaster UIRaycaster;
    }

    public sealed class RaycastPresenter : PresenterBase<RaycastView>
    {
        private readonly int _defaultLayer;

        public RaycastPresenter(RaycastView view) : base(view)
        {
            _defaultLayer = LayerMask.GetMask("Default");
        }

        public bool GraphicRaycast(Vector2 position, out List<RaycastResult> results)
        {
            var pointerData = new PointerEventData(_view.EventSystem) { position = position };

            results = new List<RaycastResult>();
            _view.UIRaycaster.Raycast(pointerData, results);

            return results.Count > 0;
        }

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, float distance, int layerMask) =>
            Physics.Raycast(ray, out hit, distance, layerMask);

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, float distance) =>
            Physics.Raycast(ray, out hit, distance);

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit) =>
            Physics.Raycast(ray, out hit, Mathf.Infinity);

        public bool DefaultRaycast(Ray ray, out RaycastHit hit, float distance) =>
            PhysicsRaycast(ray, out hit, distance, _defaultLayer);

        public bool TopDownSphereCast(Vector3 topDownOrigin, out RaycastHit hit, LayerMask layerMask,
            float radius = 0.1f)
        {
            topDownOrigin.y += 10000f;
            var topDownRay = new Ray(topDownOrigin, Vector3.down);
            return Physics.SphereCast(topDownRay, radius, out hit, Mathf.Infinity, layerMask);
        }
    }
}