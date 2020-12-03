using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerGameTests.Infra
{
    public static class Extensions
    {
        public static void ShouldEqual<T>(this T thiz, T other)
        {
            bool equals = thiz.Equals(other);

            if (!equals)
                throw new Exception($"{thiz} not equals to {other}");
        }
    }
}
