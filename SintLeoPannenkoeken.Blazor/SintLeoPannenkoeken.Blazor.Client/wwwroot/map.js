window.mapHelper = {
    map: null,
    markers: [],
    routeControl: null,

    initMap: function (elementId, centerLat, centerLng, zoom) {
        if (this.map) {
            this.map.remove();
        }

        document.getElementById(elementId).style.display = '';

        this.map = L.map(elementId).setView([centerLat, centerLng], zoom);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(this.map);

        this.markers = [];
        this.routeControl = null;
    },

    addMarker: function (lat, lng, popupContent) {
        if (!this.map) return;

        var marker = L.marker([lat, lng]).addTo(this.map);
        if (popupContent) {
            marker.bindPopup(popupContent);
        }
        this.markers.push(marker);
    },

    fitBounds: function () {
        if (!this.map || this.markers.length === 0) return;

        var group = L.featureGroup(this.markers);
        this.map.fitBounds(group.getBounds().pad(0.1));
    },

    setRoute: function (waypoints) {
        if (!this.map || !waypoints || waypoints.length < 2 || !L.Routing) return;

        if (this.routeControl) {
            this.map.removeControl(this.routeControl);
        }

        this.routeControl = L.Routing.control({
            waypoints: waypoints.map(point => L.latLng(point.latitude, point.longitude)),
            addWaypoints: false,
            draggableWaypoints: false,
            fitSelectedRoutes: false,
            lineOptions: {
                styles: [{ color: '#1976d2', opacity: 0.85, weight: 6 }]
            },
            createMarker: function () {
                return null;
            }
        }).addTo(this.map);

        var routingContainer = this.routeControl.getContainer();
        if (routingContainer) {
            routingContainer.style.display = 'none';
        }
    },

    clearMarkers: function () {
        if (!this.map) return;

        this.markers.forEach(marker => marker.remove());
        this.markers = [];
    }
};
