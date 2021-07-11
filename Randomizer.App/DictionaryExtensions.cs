﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.App
{
    public static class DictionaryExtensions
    {
        public static void Increment<TKey>(this Dictionary<TKey, int> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, 1);
            }
            else
            {
                dictionary[key]++;
            }
        }
    }
}
