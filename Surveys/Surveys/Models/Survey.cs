﻿using System;

namespace Surveys
{
    public class Survey
    {
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public string FavoriteTeam { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public override string ToString()
        {
            return $"{Name} | {Birthdate} | {FavoriteTeam} | {Lat} | {Lon}";
        }
    }
}
