namespace DeliveryBot.Shared.Dto.Geolocation;

public class RoutesDto
{
    public List<Route> Routes { get; set; }
}

public class Route
{
    public List<Leg> Legs { get; set; }
    public Geometry Geometry { get; set; }
    public double Distance { get; set; }
    public double Duration { get; set; }
}

public class Leg
{
    public List<Step> Steps { get; set; }
}

public class Step
{
    public List<Intersection> Intersections { get; set; }
    public Maneuver Maneuver { get; set; }
    public double Distance { get; set; }
    public Geometry Geometry { get; set; }
}

public class Intersection
{
    public double Duration { get; set; }
    public List<double> Location { get; set; }
}

public class Maneuver
{
    public string Type { get; set; }
    public string Modifier { get; set; }
    public List<double> Location { get; set; }
}

public class Geometry
{
    public List<List<double>> Coordinates { get; set; }
}
