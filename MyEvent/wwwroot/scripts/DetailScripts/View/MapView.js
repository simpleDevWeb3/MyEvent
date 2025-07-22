class MapView {
    _parentEl = $('#map')[0];
    _zoomLvl = 20;
    _data;
    _map;

    render(data) {
        this._data = data;

        this._LoadMap();
    }

    _LoadMap() {
        const lat = this._data.Latitude;
        const lng = this._data.Longitude;

        this._map = L.map(this._parentEl).setView([lat, lng], this._zoomLvl);

        L.tileLayer('https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
        }).addTo(this._map);

        this._RenderMarker(this._data); 
    }

    _RenderMarker(event) {
        L.marker([event.Latitude, event.Longitude])
            .addTo(this._map)
            .bindPopup(
                L.popup({
                    maxWidth: 250,
                    minWidth: 100,
                    autoClose: true,
                    closeOnClick: false,
                })
            )
           .setPopupContent(`${event.Title}`);
    }

    Addhandler(handler) {
        $(document).ready(() => {
            handler();
        });
    }
}

export default new MapView();
