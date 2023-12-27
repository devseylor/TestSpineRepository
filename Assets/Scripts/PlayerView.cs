using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerView : MonoBehaviour
    {
        #region Inspector
        [Header("Components")]
        public PlayerModel model;
        public SkeletonAnimation skeletonAnimation;

        public AnimationReferenceAsset run, idle, aim, shoot, walk;

        [Header("Effects")]
        public ParticleSystem gunParticles;
        #endregion

        SpineState previousViewState;

        void Start()
        {
            if (skeletonAnimation == null) return;
            model.ShootEvent += PlayShoot;
            model.StartAimEvent += StartPlayingAim;
            model.StopAimEvent += StopPlayingAim;
        }

        void Update()
        {
            if (skeletonAnimation == null) return;
            if (model == null) return;

            if ((skeletonAnimation.skeleton.ScaleX < 0) != model.facingLeft)
            {  // Detect changes in model.facingLeft
                Turn(model.facingLeft);
            }

            // Detect changes in model.state
            SpineState currentModelState = model.state;

            if (previousViewState != currentModelState)
            {
                PlayNewStableAnimation();
            }

            previousViewState = currentModelState;
        }

        void PlayNewStableAnimation()
        {
            SpineState newModelState = model.state;
            Spine.Animation nextAnimation;

            // Add conditionals to not interrupt transient animations.

            if (newModelState == SpineState.Running)
            {
                nextAnimation = run;
            }
            else if(newModelState == SpineState.Walk)
            {
                nextAnimation = walk;
            }
            else
            {
                nextAnimation = idle;
            }

            skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        }

        [ContextMenu("Check Tracks")]
        void CheckTracks()
        {
            Spine.AnimationState state = skeletonAnimation.AnimationState;
            Debug.Log(state.GetCurrent(0));
            Debug.Log(state.GetCurrent(1));
        }

        #region Transient Actions
        public void PlayShoot()
        {
            TrackEntry shootTrack = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
            shootTrack.AttachmentThreshold = 1f;
            shootTrack.MixDuration = 0f;
            skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);

            TrackEntry aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
            aimTrack.AttachmentThreshold = 1f;
            aimTrack.MixDuration = 0f;
            skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);

            gunParticles.Play();
        }

        public void StartPlayingAim()
        {
            TrackEntry aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, true);
            aimTrack.AttachmentThreshold = 1f;
            aimTrack.MixDuration = 0f;
        }

        public void StopPlayingAim()
        {
            skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
        }

        public void Turn(bool facingLeft)
        {
            skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
        }
        #endregion

        #region Utility
        public float GetRandomPitch(float maxPitchOffset)
        {
            return 1f + Random.Range(-maxPitchOffset, maxPitchOffset);
        }
        #endregion
    }
}
