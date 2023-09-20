using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omega.Kulibin
{
    public sealed class RaycastView : ViewBase
    {
        public EventSystem EventSystem;
        public GraphicRaycaster UIRaycaster;
    }
    
    public sealed class RaycastPresenter : PresenterBase<RaycastView>
    {
        private readonly int _defaultLayer;
        private readonly int _botLayer;
        private readonly int _buttonModuleLayer;
        private readonly int _flagLayer;
        private readonly int _constructorBlocksLayer;

        public RaycastPresenter(RaycastView view) : base(view)
        {
            _defaultLayer = LayerMask.GetMask("Default");
            _botLayer = LayerMask.GetMask("OmegaBot");
            _buttonModuleLayer = LayerMask.GetMask("ButtonModule");
            _flagLayer = LayerMask.GetMask("Flag");
            _constructorBlocksLayer = LayerMask.GetMask("Default", "Container");
        }

        public bool GraphicRaycast(Vector2 position, out List<RaycastResult> results)
        {
            var pointerData = new PointerEventData(_view.EventSystem) { position = position };

            results = new List<RaycastResult>();
            _view.UIRaycaster.Raycast(pointerData, results);

            return results.Count > 0;
        }
        
        public bool PhysicsRaycast(Ray ray, out RaycastHit hit, int layerMask) =>
            Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

        public bool PhysicsRaycast(Ray ray, out RaycastHit hit) => 
            Physics.Raycast(ray, out hit, Mathf.Infinity);

        public bool DefaultRaycast(Ray ray, out RaycastHit hit) => PhysicsRaycast(ray, out hit, _defaultLayer);
        public bool BotRaycast(Ray ray, out RaycastHit hit) => PhysicsRaycast(ray, out hit, _botLayer);
        public bool FlagRaycast(Ray ray, out RaycastHit hit) => PhysicsRaycast(ray, out hit, _flagLayer);
        public bool ConstructorBlocksRaycast(Ray ray, out RaycastHit hit) => PhysicsRaycast(ray, out hit, _constructorBlocksLayer);
        
        public bool ButtonModuleRaycast(Ray ray, out RaycastHit hit) => PhysicsRaycast(ray, out hit, _buttonModuleLayer);

        public bool TopDownSphereCast(Vector3 topDownOrigin, out RaycastHit hit, LayerMask layerMask , float radius = 0.1f)
        {
            topDownOrigin.y += 10000f;
            var topDownRay = new Ray(topDownOrigin, Vector3.down);
            return Physics.SphereCast(topDownRay, radius, out hit, Mathf.Infinity, layerMask);
        }

        public bool DefaultTopDownSphereCast(Vector3 topDownOrigin, out RaycastHit hit, float radius = 0.1f) =>
            TopDownSphereCast(topDownOrigin, out hit, _defaultLayer, radius);
        public bool ConstructorBlocksTopDownSpherecast(Vector3 topDownOrigin, out RaycastHit hit, float radius = 0.1f) =>
            TopDownSphereCast(topDownOrigin, out hit, _constructorBlocksLayer, radius);
    }
}