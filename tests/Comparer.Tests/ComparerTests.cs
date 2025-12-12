namespace Comparer.Tests
{
    public class ComparerTests
    {
        [Fact]
        public void Compare_MajorHigher_ReturnsV1()
        {
            var v1 = "2.0.0";
            var v2 = "1.9.9";


            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v1, result);
        }

        [Fact]
        public void Compare_MinorHigher_ReturnsV2()
        {
            var v1 = "1.5.0";
            var v2 = "1.6.0";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v2, result);
        }

        [Fact]
        public void Compare_PatchHigher_ReturnsV1()
        {
            var v1 = "1.2.4";
            var v2 = "1.2.3";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v1, result);
        }

        [Fact]
        public void Compare_StableVsPreRelease_StableIsGreater()
        {
            var v1 = "1.0.0-alpha";
            var v2 = "1.0.0";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v2, result);
        }

        [Fact]
        public void Compare_PreReleaseTypeOrder_BetaGreaterThanAlpha()
        {
            var v1 = "1.0.0-alpha.2";
            var v2 = "1.0.0-beta.1";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v2, result);
        }

        [Fact]
        public void Compare_PreReleaseSameType_NumberComparison()
        {
            var v1 = "1.0.0-alpha.10";
            var v2 = "1.0.0-alpha.2";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v1, result);
        }

        [Fact]
        public void Compare_UnknownPreReleaseType_ResolvedByOrderingAndLexicographic()
        {
            var v1 = "1.0.0-alpha";
            var v2 = "1.0.0-custom";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v1, result);
        }

        [Fact]
        public void Compare_EqualVersions_ReturnsSecondParameter()
        {
            var v1 = "1.0.0";
            var v2 = "1.0.0";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v2, result);
            Assert.Equal(v1, result);
        }

        [Fact]
        public void Compare_ShortVersionFormats_TreatedCorrectly()
        {
            var v1 = "1";
            var v2 = "1.0.1";

            var result = SemanticVersionComparer.Comparer.Compare(v1, v2);

            Assert.Equal(v2, result);
        }
    }
}