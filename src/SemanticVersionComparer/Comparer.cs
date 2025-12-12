namespace SemanticVersionComparer
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Comparer
    {
        /// <summary>
        /// Verifica cual de las dos versiones semánticas es mayor
        /// </summary>
        /// <returns>El string que fue indentificado como de mayor tamaño</returns>
        public static string Compare(string v1, string v2)
        {
            if (CompareVersions(v1, v2) == 1)
                return v1;
            return v2;
        }

        /// <summary>
        /// Compara dos versiones semánticas
        /// </summary>
        /// <param name="v1">Primera versión (ej: "1.2.3" o "1.0.0-alpha.1")</param>
        /// <param name="v2">Segunda versión</param>
        /// <returns>-1 si v1 < v2, 0 si v1 == v2, 1 si v1 > v2</returns>
        private static int CompareVersions(string v1, string v2)
        {
            var ver1 = ParseVersion(v1);
            var ver2 = ParseVersion(v2);

            // Comparar major, minor, patch
            if (ver1.Major != ver2.Major)
                return ver1.Major > ver2.Major ? 1 : -1;
            if (ver1.Minor != ver2.Minor)
                return ver1.Minor > ver2.Minor ? 1 : -1;
            if (ver1.Patch != ver2.Patch)
                return ver1.Patch > ver2.Patch ? 1 : -1;

            // Comparar pre-release
            if (string.IsNullOrEmpty(ver1.PreRelease) && string.IsNullOrEmpty(ver2.PreRelease))
                return 0;
            if (string.IsNullOrEmpty(ver1.PreRelease) && !string.IsNullOrEmpty(ver2.PreRelease))
                return 1;
            if (!string.IsNullOrEmpty(ver1.PreRelease) && string.IsNullOrEmpty(ver2.PreRelease))
                return -1;

            // Ambas tienen pre-release, compararlos
            return ComparePreRelease(ver1.PreRelease, ver2.PreRelease);
        }

        private static SemanticVersion ParseVersion(string version)
        {
            var parts = version.Split('-');
            var mainVersion = parts[0];
            var preRelease = parts.Length > 1 ? parts[1] : null;

            var numbers = mainVersion.Split('.').Select(int.Parse).ToArray();

            return new SemanticVersion(
                major: numbers[0],
                minor: numbers.Length > 1 ? numbers[1] : 0,
                patch: numbers.Length > 2 ? numbers[2] : 0,
                preRelease: preRelease);
        }

        private static int ComparePreRelease(string pre1, string pre2)
        {
            var p1 = ParsePreRelease(pre1);
            var p2 = ParsePreRelease(pre2);

            var order = new Dictionary<string, int>
            {
                { "alpha", 1 },
                { "beta", 2 },
                { "rc", 3 }
            };

            var type1 = order.ContainsKey(p1.Type) ? order[p1.Type] : 0;
            var type2 = order.ContainsKey(p2.Type) ? order[p2.Type] : 0;

            if (type1 != type2) return type1 > type2 ? 1 : -1;
            if (p1.Number != p2.Number) return p1.Number > p2.Number ? 1 : -1;

            return string.Compare(pre1, pre2, StringComparison.Ordinal);
        }

        private static PreReleaseInfo ParsePreRelease(string preRelease)
        {
            var match = Regex.Match(preRelease, @"^(alpha|beta|rc)\.?(\d+)?$");

            if (!match.Success)
                return new PreReleaseInfo(type: preRelease, number: 0);

            return new PreReleaseInfo(type: match.Groups[1].Value, number: match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0);
        }

        private sealed class SemanticVersion(int major, int minor, int patch, string preRelease)
        {

            public int Major { get; set; } = major;
            public int Minor { get; set; } = minor;
            public int Patch { get; set; } = patch;
            public string? PreRelease { get; set; } = preRelease;
        }

        private sealed class PreReleaseInfo(string type, int number)
        {
            public string Type { get; set; } = type;
            public int Number { get; set; } = number;
        }
    }
}