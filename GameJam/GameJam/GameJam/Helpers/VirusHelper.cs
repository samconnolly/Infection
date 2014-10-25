using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public static class VirusHelper
    {
        private static Vector2 _position;
        private static Vector2 _velocity;
        private static List<Virusling> _viruslings;
        private static Virus _virus;

        private static Vector2 _position2;
        private static Vector2 _velocity2;
        private static List<Virusling> _viruslings2;
        private static Virus _virus2;


        private static int radiusCount = 0;
        private static int speedCount = 0;
        private static int orbitCount = 0;

        private static int radiusCountP2 = 0;
        private static int speedCountP2 = 0;
        private static int orbitCountP2 = 0;
        
        public static float radius1 = 30.0f;
        public static float radius2 = 40.0f;
        public static float radius3 = 150.0f;

        public static float islow = 0.999f;
        public static float oslow = 0.5f;
        public static float ooslow = 1.0f;

        public static float iaccn = 4.0f;
        public static float oaccn = 5.0f;
        public static float ooaccn = 1.0f;
        public static float oooaccn = 10.0f;


        public static float radius1P2 = 30.0f;
        public static float radius2P2 = 40.0f;
        public static float radius3P2 = 150.0f;

        public static float islowP2 = 0.999f;
        public static float oslowP2 = 0.5f;
        public static float ooslowP2 = 1.0f;

        public static float iaccnP2 = 4.0f;
        public static float oaccnP2 = 5.0f;
        public static float ooaccnP2 = 1.0f;
        public static float oooaccnP2 = 10.0f;

        public static float repel = 0.001f;

        public static int rot = 1;
        public static float rotSpeed = 0.05f;


        public static float repelP2 = 0.001f;

        public static int rotP2 = 1;
        public static float rotSpeedP2 = 0.05f;

        public static Vector2 VirusPosition
        {
            get { return _position; }
            set { _position = value; }
        }

        public static Vector2 VirusVelocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public static List<Virusling> Viruslings
        {
            get { return _viruslings; }
            set { _viruslings = value; }
        }

        public static Virus Virus
        {
            get { return _virus; }
            set { _virus = value; }
        }
        
        public static Vector2 VirusPositionP2
        {
            get { return _position2; }
            set { _position2 = value; }
        }

        public static Vector2 VirusVelocityP2
        {
            get { return _velocity2; }
            set { _velocity2 = value; }
        }

        public static List<Virusling> ViruslingsP2
        {
            get { return _viruslings2; }
            set { _viruslings2 = value; }
        }

        public static Virus VirusP2
        {
            get { return _virus2; }
            set { _virus2 = value; }
        }

        public static float Radius1
        {
            get { return radius1; }
            set { radius1 = value; }
        }

        public static float Radius2
        {
            get { return radius2; }
            set { radius2 = value; }
        }

        public static float Radius3
        {
            get { return radius3; }
            set { radius3 = value; }
        }

        public static float InnerSlow
        {
            get { return islow; }
            set { islow = value; }
        }

        public static float OuterSlow
        {
            get { return oslow; }
            set { oslow = value; }
        }

        public static float OuterOuterSlow
        {
            get { return ooslow; }
            set { ooslow = value; }
        }

        public static float InnerAccn
        {
            get { return iaccn; }
            set { iaccn = value; }
        }

        public static float OuterAccn
        {
            get { return oaccn; }
            set { oaccn = value; }
        }

        public static float OuterOuterAccn
        {
            get { return ooaccn; }
            set { ooaccn = value; }
        }

        public static float OuterOuterOuterAccn
        {
            get { return oooaccn; }
            set { oooaccn = value; }
        }

        public static int Rotation
        {
            get { return rot; }
            set { rot = value; }
        }

        public static float RotationSpeed
        {
            get { return rotSpeed; }
            set { rotSpeed = value; }
        }

        public static float Repel
        {
            get { return repel; }
            set { repel = value; }
        }

        public static float Radius1P2
        {
            get { return radius1P2; }
            set { radius1P2 = value; }
        }

        public static float Radius2P2
        {
            get { return radius2P2; }
            set { radius2P2 = value; }
        }

        public static float Radius3P2
        {
            get { return radius3P2; }
            set { radius3P2 = value; }
        }

        public static float InnerSlowP2
        {
            get { return islowP2; }
            set { islowP2 = value; }
        }

        public static float OuterSlowP2
        {
            get { return oslowP2; }
            set { oslowP2 = value; }
        }

        public static float OuterOuterSlowP2
        {
            get { return ooslowP2; }
            set { ooslowP2 = value; }
        }

        public static float InnerAccnP2
        {
            get { return iaccnP2; }
            set { iaccnP2 = value; }
        }

        public static float OuterAccnP2
        {
            get { return oaccnP2; }
            set { oaccnP2 = value; }
        }

        public static float OuterOuterAccnP2
        {
            get { return ooaccnP2; }
            set { ooaccnP2 = value; }
        }

        public static float OuterOuterOuterAccnP2
        {
            get { return oooaccnP2; }
            set { oooaccnP2 = value; }
        }

        public static int RotationP2
        {
            get { return rotP2; }
            set { rotP2 = value; }
        }

        public static float RotationSpeedP2
        {
            get { return rotSpeedP2; }
            set { rotSpeedP2 = value; }
        }

        public static float RepelP2
        {
            get { return repelP2; }
            set { repelP2 = value; }
        }

        public static int RadiusCount
        {
            get { return radiusCount; }
            set { radiusCount = value; }
        }

        public static int SpeedCount
        {
            get { return speedCount; }
            set { speedCount = value; }
        }

        public static int OrbitCount
        {
            get { return orbitCount; }
            set { orbitCount = value; }
        }

        public static int RadiusCountP2
        {
            get { return radiusCountP2; }
            set { radiusCountP2 = value; }
        }

        public static int SpeedCountP2
        {
            get { return speedCountP2; }
            set { speedCountP2 = value; }
        }

        public static int OrbitCountP2
        {
            get { return orbitCountP2; }
            set { orbitCountP2 = value; }
        }

    }
}
