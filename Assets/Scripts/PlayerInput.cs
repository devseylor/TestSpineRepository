using Spine.Unity;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerInput : MonoBehaviour
    {
        #region Inspector
        public string attackButton = "Fire1";
        public string aimButton = "Fire2";
        public Transform characterTransform;

        public PlayerModel model;
        public float minSpeedForIdle = 0.1f;


        public float moveSpeed = 5f;

        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void OnValidate()
        {
            if (model == null)
                model = GetComponent<PlayerModel>();
        }
        #endregion

        private void Update()
        {
            if (model == null) return;
            Vector3 mouseScreenPos =  new Vector3( Input.mousePosition.x, 0, 10);
            Vector3 distanceToMouse = characterTransform.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 10)); //camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = camera.ScreenToWorldPoint(mouseScreenPos);

            //float localSpeed = Mathf.Lerp(moveSpeed, minSpeedForIdle, Mathf.InverseLerp(0, 2, Vector3.Distance(transform.position, targetPosition)));

            Vector3 positionToMove = new Vector3(mousePos.x,0,0);
            model.TryMove(moveSpeed, positionToMove);


            if (Input.GetButton(attackButton))
                model.TryShoot();
            if (Input.GetButtonDown(aimButton))
                model.StartAim();
            if (Input.GetButtonUp(aimButton))
                model.StopAim();
        }
    }
}
