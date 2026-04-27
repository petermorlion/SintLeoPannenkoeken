using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Options;

namespace SintLeoPannenkoeken.Blazor.External.RouteXL
{
    public static class RouteXLRoutePlanner
    {
        public static IList<ChauffeurRondeDetailDto> SelectStopsForOptimization(IList<ChauffeurRondeDetailDto> details, RouteXLOptions options)
        {
            var stopLimit = options.HasApiKey
                ? int.MaxValue
                : Math.Max(2, options.FreeTierStopLimit);

            return details
                .Where(detail => detail.Position != null)
                .Take(stopLimit)
                .ToList();
        }

        public static IList<int> ExtractStopIds(RouteXLTourResponse? response)
        {
            if (response?.Route == null || response.Route.Count == 0)
            {
                return new List<int>();
            }

            return response.Route
                .Select(routeEntry => new
                {
                    SortOrder = int.TryParse(routeEntry.Key, out var sortOrder) ? sortOrder : int.MaxValue,
                    routeEntry.Value.Name
                })
                .OrderBy(routeEntry => routeEntry.SortOrder)
                .Select(routeEntry => routeEntry.Name)
                .Where(name => int.TryParse(name, out _))
                .Select(int.Parse)
                .ToList();
        }

        public static IList<ChauffeurRondeDetailDto> OrderDetails(IList<ChauffeurRondeDetailDto> details, IEnumerable<int> orderedStopIds)
        {
            var orderedIdList = orderedStopIds.ToList();
            var detailsById = details.ToDictionary(detail => detail.BestellingId);
            var orderedDetails = new List<ChauffeurRondeDetailDto>();

            foreach (var stopId in orderedIdList)
            {
                if (detailsById.TryGetValue(stopId, out var detail))
                {
                    orderedDetails.Add(detail);
                }
            }

            var orderedIdSet = orderedIdList.ToHashSet();
            orderedDetails.AddRange(details.Where(detail => !orderedIdSet.Contains(detail.BestellingId)));

            return orderedDetails;
        }

        public static IList<PositionDto> BuildRouteWaypoints(IList<ChauffeurRondeDetailDto> details, IEnumerable<int> orderedStopIds)
        {
            var detailsById = details.ToDictionary(detail => detail.BestellingId);
            var waypoints = new List<PositionDto>();

            foreach (var stopId in orderedStopIds)
            {
                if (detailsById.TryGetValue(stopId, out var detail) && detail.Position != null)
                {
                    waypoints.Add(detail.Position);
                }
            }

            return waypoints;
        }
    }
}
