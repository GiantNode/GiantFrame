﻿using System;

namespace Giant.Share
{
    public class RandomHelper
    {
        private static readonly Random random = new Random();

        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
