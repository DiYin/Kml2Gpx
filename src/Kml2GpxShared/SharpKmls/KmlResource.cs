namespace Kml2Gpx.SharpKmls;

/// <summary>
/// KML resource
/// </summary>
public class KmlResource
{
    public string FileName { get; set; }
    public byte[] Blob { get; set; }

    /// <summary>
    /// Compare the current instance of the <see cref="KmlResource"/> to an specified instance of the <see cref="KmlResource"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool Equals(KmlResource other)
    {
        return string.Equals(FileName, other.FileName) && Equals(Blob, other.Blob);
    }

    /// <summary>
    /// Compare the current instance of the <see cref="KmlResource"/> to an specified instance of the <see cref="KmlResource"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((KmlResource)obj);
    }

    /// <summary>
    /// Get the hash code of the current instance of the <see cref="KmlResource"/>.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((FileName?.GetHashCode() ?? 0) * 397) ^ (Blob?.GetHashCode() ?? 0);
        }
    }

    /// <summary>
    /// Clone the current instance of the <see cref="KmlResource"/>.
    /// </summary>
    /// <returns></returns>
    public KmlResource Clone()
    {
        return new KmlResource
        {
            FileName = this.FileName,
            Blob = this.Blob
        };
    }
}
