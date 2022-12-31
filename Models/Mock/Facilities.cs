namespace Architeptable.Models;

public static partial class Mock
{
    public static readonly Facilities Facilities = new(null)
    {
        All = new[]
        {
            new Facilities.FacilityModel { Name = "Nuclear Power Plant" }
        }
    };
}