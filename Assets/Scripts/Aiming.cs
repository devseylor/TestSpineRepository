using Spine;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class Aiming : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;

        [SpineBone(dataField: "skeletonAnimation")]
        public string boneName;
        public Camera cam;

        Bone bone;

        void OnValidate()
        {
            if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimation>();
        }

        void Start()
        {
            bone = skeletonAnimation.Skeleton.FindBone(boneName);
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(mousePosition);
            Vector3 skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
            skeletonSpacePoint.x *= skeletonAnimation.Skeleton.ScaleX;
            skeletonSpacePoint.y *= skeletonAnimation.Skeleton.ScaleY;
            bone.SetLocalPosition(skeletonSpacePoint);
        }
    }
}
