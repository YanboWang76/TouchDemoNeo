using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.HandController
{
    public class HandGestureRecognizerSample : HandGestureRecognizerBase
    {
        /// <summary>
        /// Override this method to associate each hand pose with your game logic.
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="args">Contains the updated hand pose.</param>
        protected override void HandPoseChangedHandler(object sender, HandPoseChangedEventArgs args)
        {
            base.HandPoseChangedHandler(sender, args);
            switch (HandPose)
            {
                case HandPoseType.Normal:
                    Debug.Log("Hand pose back to normal.");
                    break;

                case HandPoseType.SpidermanReady:
                    Debug.Log("Spider ready pose.");
                    break;

                case HandPoseType.SpidermanShoot:
                    Debug.Log("Spiderman shoot pose.");
                    TeleportationStart();
                    break;

                case HandPoseType.SpidermanReset:
                    Debug.Log("Spiderman pose reset.");
                    break;
            }
        }
        
        /// <summary>
        /// This is a sample teleportation method. Put the teleportation logic here if teleportation
        /// is used together with hand gesture recognition.
        /// </summary>
        private void TeleportationStart()
        {
            // Switch off collision detection during teleportation. We don't want hands to have
            // force feedback if hand models bump into anything when moved to the teleported position
            OnSwitchHandCollisionDetection(false);
            // Switch off motion transition during teleportation. If this is not switched off, we
            // will see players teleport to the new position first, followed by the hand models slowly
            // moving towards the new position.
            OnSwitchHandMotionTransition(false);

            // Initiate teleportation here

        }

        /// <summary>
        /// This method should be called somewhere to finish up the teleportation process
        /// </summary>
        public void TeleportationFinished()
        {
            // Remember to restore collision detection and motion transition.
            OnSwitchHandCollisionDetection(true);
            OnSwitchHandMotionTransition(true);
        }
    }
}
