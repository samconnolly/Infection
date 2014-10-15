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

        public static float repel = 0.001f;

        public static int rot = 1;
        public static float rotSpeed = 0.05f;

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

    }
}
