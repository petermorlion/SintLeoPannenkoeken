using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.External.RouteXL;
using SintLeoPannenkoeken.Blazor.Options;

namespace SintLeoPannenkoeken.Blazor.Tests.Routing
{
    public class RouteXLRoutePlannerTests
    {
        [Fact]
        public void SelectStopsForOptimization_UsesFreeTierLimit_WhenNoApiKeyIsConfigured()
        {
            var options = new RouteXLOptions
            {
                FreeTierStopLimit = 2
            };

            var details = new List<ChauffeurRondeDetailDto>
            {
                CreateDetail(1, 51.1, 3.1),
                CreateDetail(2, 51.2, 3.2),
                CreateDetail(3, 51.3, 3.3)
            };

            var selectedStops = RouteXLRoutePlanner.SelectStopsForOptimization(details, options);

            Assert.Equal(new[] { 1, 2 }, selectedStops.Select(detail => detail.BestellingId));
        }

        [Fact]
        public void SelectStopsForOptimization_UsesAllRoutableStops_WhenApiKeyIsConfigured()
        {
            var options = new RouteXLOptions
            {
                ApiKey = "paid-key",
                FreeTierStopLimit = 1
            };

            var details = new List<ChauffeurRondeDetailDto>
            {
                CreateDetail(1, 51.1, 3.1),
                CreateDetail(2, 51.2, 3.2),
                new ChauffeurRondeDetailDto { BestellingId = 3 }
            };

            var selectedStops = RouteXLRoutePlanner.SelectStopsForOptimization(details, options);

            Assert.Equal(new[] { 1, 2 }, selectedStops.Select(detail => detail.BestellingId));
        }

        [Fact]
        public void OrderDetails_ReordersOptimizedStops_AndAppendsRemainingStops()
        {
            var details = new List<ChauffeurRondeDetailDto>
            {
                CreateDetail(1, 51.1, 3.1),
                CreateDetail(2, 51.2, 3.2),
                CreateDetail(3, 51.3, 3.3),
                CreateDetail(4, 51.4, 3.4)
            };

            var orderedDetails = RouteXLRoutePlanner.OrderDetails(details, new[] { 3, 1 });

            Assert.Equal(new[] { 3, 1, 2, 4 }, orderedDetails.Select(detail => detail.BestellingId));
        }

        [Fact]
        public void BuildRouteWaypoints_FollowsOptimizedStopOrder()
        {
            var details = new List<ChauffeurRondeDetailDto>
            {
                CreateDetail(10, 51.1, 3.1),
                CreateDetail(11, 51.2, 3.2)
            };

            var waypoints = RouteXLRoutePlanner.BuildRouteWaypoints(details, new[] { 11, 10 });

            Assert.Collection(
                waypoints,
                waypoint =>
                {
                    Assert.Equal(51.2, waypoint.Latitude);
                    Assert.Equal(3.2, waypoint.Longitude);
                },
                waypoint =>
                {
                    Assert.Equal(51.1, waypoint.Latitude);
                    Assert.Equal(3.1, waypoint.Longitude);
                });
        }

        private static ChauffeurRondeDetailDto CreateDetail(int bestellingId, double latitude, double longitude)
        {
            return new ChauffeurRondeDetailDto
            {
                BestellingId = bestellingId,
                Position = new PositionDto
                {
                    Latitude = latitude,
                    Longitude = longitude
                }
            };
        }
    }
}
