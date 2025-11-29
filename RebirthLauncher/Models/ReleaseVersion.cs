namespace RebirthLauncher.Models
{
    /// <summary>
    /// Represents a software release version number with major, minor, build, and revision components.
    /// </summary>
    /// <remarks>
    /// This class provides immutable version number representation with comparison and equality capabilities.
    /// It follows semantic versioning principles and allows for easy version comparisons.
    /// </remarks>
    public sealed class ReleaseVersion : IComparable<ReleaseVersion>, IEquatable<ReleaseVersion>
    {
        /// <summary>
        /// Gets the major version number.
        /// </summary>
        /// <value>A short representing the major version.</value>
        public short Major { get; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        /// <value>A short representing the minor version.</value>
        public short Minor { get; }

        /// <summary>
        /// Gets the build version number.
        /// </summary>
        /// <value>A short representing the build version.</value>
        public short Build { get; }

        /// <summary>
        /// Gets the revision version number.
        /// </summary>
        /// <value>A short representing the revision version.</value>
        public short Revision { get; }

        /// <summary>
        /// Represents a version number of 0.0.0.0.
        /// </summary>
        public static ReleaseVersion Zero { get; } = new ReleaseVersion(0, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseVersion"/> class with specified version components.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="build">The build version number.</param>
        /// <param name="revision">The revision version number.</param>
        public ReleaseVersion(short major, short minor, short build, short revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseVersion"/> class from a version string.
        /// </summary>
        /// <param name="version">A dot-separated version string (e.g., "1.2.3.4").</param>
        /// <exception cref="FormatException">Thrown if the version string cannot be parsed.</exception>
        /// <remarks>
        /// If the version string does not contain exactly 4 components, 
        /// the version will be set to 0.0.0.0.
        /// </remarks>
        public ReleaseVersion(string? version)
        {
            if (version == null)
            {
                Major = Minor = Build = Revision = 0;
                return;
            }
            var parts = version.Split('.', StringSplitOptions.None);
            if (parts.Length != 4)
            {
                Major = Minor = Build = Revision = 0;
                return;
            }

            Major = short.TryParse(parts[0], out short major) ? major : (short)0;
            Minor = short.TryParse(parts[1], out short minor) ? minor : (short)0;
            Build = short.TryParse(parts[2], out short build) ? build : (short)0;
            Revision = short.TryParse(parts[3], out short revision) ? revision : (short)0;
        }

        /// <summary>
        /// Determines if the current version is newer than another version.
        /// </summary>
        /// <param name="other">The version to compare against.</param>
        /// <returns>True if the current version is newer, otherwise false.</returns>
        public bool IsNewerThan(ReleaseVersion other)
        {
            return CompareTo(other) > 0;
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">The version to compare to.</param>
        /// <returns>
        /// A value that indicates the relative order of the versions:
        /// Less than zero: Current version is older
        /// Zero: Versions are equal
        /// Greater than zero: Current version is newer
        /// </returns>
        public int CompareTo(ReleaseVersion? other)
        {
            if (other is null) return 1;

            int majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;

            int minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;

            int buildComparison = Build.CompareTo(other.Build);
            if (buildComparison != 0) return buildComparison;

            return Revision.CompareTo(other.Revision);
        }

        /// <summary>
        /// Determines whether the current version is equal to another version.
        /// </summary>
        /// <param name="other">The version to compare with the current version.</param>
        /// <returns>True if the versions are equal; otherwise, false.</returns>
        public bool Equals(ReleaseVersion? other)
        {
            if (other is null) return false;
            return Major == other.Major &&
                   Minor == other.Minor &&
                   Build == other.Build &&
                   Revision == other.Revision;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current version.
        /// </summary>
        /// <param name="obj">The object to compare with the current version.</param>
        /// <returns>True if the specified object is equal to the current version; otherwise, false.</returns>
        public override bool Equals(object? obj) =>
            obj is ReleaseVersion other && Equals(other);

        /// <summary>
        /// Serves as the default hash function for the ReleaseVersion.
        /// </summary>
        /// <returns>A hash code for the current version.</returns>
        public override int GetHashCode() =>
            HashCode.Combine(Major, Minor, Build, Revision);

        /// <summary>
        /// Returns a string representation of the version.
        /// </summary>
        /// <returns>A string in the format "Major.Minor.Build.Revision".</returns>
        public override string ToString() =>
            $"{Major}.{Minor}.{Build}.{Revision}";

        /// <summary>
        /// Determines if one version is greater than another.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the left version is greater than the right version; otherwise, false.</returns>
        public static bool operator >(ReleaseVersion? left, ReleaseVersion? right) =>
            left is not null && right is not null && left.CompareTo(right) > 0;

        /// <summary>
        /// Determines if one version is less than another.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the left version is less than the right version; otherwise, false.</returns>
        public static bool operator <(ReleaseVersion left, ReleaseVersion right) =>
            left?.CompareTo(right) < 0;

        /// <summary>
        /// Determines if one version is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the left version is greater than or equal to the right version; otherwise, false.</returns>
        public static bool operator >=(ReleaseVersion left, ReleaseVersion right) =>
            left?.CompareTo(right) >= 0;

        /// <summary>
        /// Determines if one version is less than or equal to another.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the left version is less than or equal to the right version; otherwise, false.</returns>
        public static bool operator <=(ReleaseVersion left, ReleaseVersion right) =>
            left?.CompareTo(right) <= 0;

        /// <summary>
        /// Determines if two versions are equal.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the versions are equal; otherwise, false.</returns>
        public static bool operator ==(ReleaseVersion? left, ReleaseVersion? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two versions are not equal.
        /// </summary>
        /// <param name="left">The first version to compare.</param>
        /// <param name="right">The second version to compare.</param>
        /// <returns>True if the versions are not equal; otherwise, false.</returns>
        public static bool operator !=(ReleaseVersion left, ReleaseVersion right)
        {
            return !(left == right);
        }
    }
}

