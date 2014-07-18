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

    }
}
