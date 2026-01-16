window.mapHelper = {
    map: null,
    markers: [],

    initMap: function (elementId, centerLat, centerLng, zoom) {
        if (this.map) {
            this.map.remove();
        }

        this.map = L.map(elementId).setView([centerLat, centerLng], zoom);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(this.map);

        this.markers = [];
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

    clearMarkers: function () {
        if (!this.map) return;

        this.markers.forEach(marker => marker.remove());
        this.markers = [];
    }
};