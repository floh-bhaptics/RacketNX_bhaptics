using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MelonLoader;
using HarmonyLib;
using UnityEngine;
using MyBhapticsTactsuit;

namespace RacketNX_bhaptics
{
    public class RacketNX_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static bool rightHanded = true;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        
        [HarmonyPatch(typeof(Announcer), "PowerupAcquired", new Type[] { typeof(PowerupType) })]
        public class bhaptics_PowerUp
        {
            [HarmonyPostfix]
            public static void Postfix(PowerupType pu)
            {
                switch (pu)
                {
                    case PowerupType.SplashDamage:
                        break;
                    case PowerupType.Nuke:
                        break;
                    case PowerupType.GodMode:
                        break;
                    case PowerupType.SuperFlow:
                        break;
                    case PowerupType.BlasterMaze:
                        break;
                    case PowerupType.Jackpot:
                        break;
                    case PowerupType.Hyperspace:
                        break;
                    case PowerupType.IceBreaker:
                        break;
                    case PowerupType.WreckingBall:
                        break;
                    case PowerupType.Supersize:
                        tactsuitVr.PlaybackHaptics("PowerUpFeet");
                        tactsuitVr.PlaybackHaptics("SuperSize");
                        return;
                    default:
                        break;
                }
                tactsuitVr.LOG("Powerup: " + pu.ToString());
                tactsuitVr.PlaybackHaptics("PowerUpFeet");
                tactsuitVr.PlaybackHaptics("PowerUp");
            }
        }

        [HarmonyPatch(typeof(RacketHaptic), "onBallCollision", new Type[] { typeof(Ball.BallCollision) })]
        public class bhaptics_HitBall
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.Recoil("Blade", rightHanded);
                tactsuitVr.StopPulling();
            }
        }

        
        [HarmonyPatch(typeof(Announcer), "gameEndReason", new Type[] { typeof(GameEndReason) })]
        public class bhaptics_AnnounceGameEnd
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopThreads();
            }
        }

        [HarmonyPatch(typeof(NukeSurface), "explode", new Type[] {  })]
        public class bhaptics_NukeExplode
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("RumbleFeet");
                tactsuitVr.PlaybackHaptics("ExplosionBelly");
            }
        }


        [HarmonyPatch(typeof(PowerupWreckingBall), "playBreakSound", new Type[] { typeof(Vector3) })]
        public class bhaptics_WreckingBallBreak
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("RumbleFeet", 0.7f);
                tactsuitVr.PlaybackHaptics("ExplosionBelly", 0.7f);
            }
        }

        [HarmonyPatch(typeof(RacketModel), "SetupHand", new Type[] { typeof(VRPlugin.Side) })]
        public class bhaptics_SetHandedness
        {
            [HarmonyPostfix]
            public static void Postfix(VRPlugin.Side side)
            {
                rightHanded = (side == VRPlugin.Side.Right);
            }
        }

        [HarmonyPatch(typeof(RacketSling), "StartPullingBall", new Type[] { typeof(Ball) })]
        public class bhaptics_StartPullingBall
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.LOG("StartPulling");
                tactsuitVr.StartPulling(rightHanded);
            }
        }

        [HarmonyPatch(typeof(RacketSling), "StopPullingBall", new Type[] {  })]
        public class bhaptics_StopPullingBall
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.LOG("StoppPulling");
                tactsuitVr.StopPulling();
            }
        }
        
        [HarmonyPatch(typeof(Player), "StartSlowMotion", new Type[] { typeof(GameObject), typeof(float), typeof(float) })]
        public class bhaptics_StartSlowMotion
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance, GameObject sender, float fRequestedSlowMotion, float fRate)
            {
                if (!sender.name.Contains("Ball")) return;
                tactsuitVr.StartHeartBeat();
            }
        }

        [HarmonyPatch(typeof(Player), "StopSlowMotion", new Type[] { typeof(GameObject) })]
        public class bhaptics_StopSlowMotion
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(GameEventsManager), "gameEnded", new Type[] { typeof(GameEndReason) })]
        public class bhaptics_GameEnded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopThreads();
            }
        }

        [HarmonyPatch(typeof(GameEventsManager), "roundEnded", new Type[] { typeof(GameEndReason) })]
        public class bhaptics_RoundEnded
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopThreads();
            }
        }

    }
}
