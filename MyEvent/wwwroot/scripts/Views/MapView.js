

class MapView {

    _parentEl = $('#map')[0];
    _zoomLvl = 13;
    _zoomTo = 14;
    _map;

    render(data) {
        this._data = data;
        this._LoadMap();
    }

    MoveToCoords(lat,lng) {
        this._map.setView([lat, lng], this._zoomTo);
      
    }

    _LoadMap() {
        console.log(this._data);
        const {lat,lng} = this._data.coords;

        this._map = L.map(this._parentEl).setView([lat,lng], this._zoomLvl);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(this._map);

        this._data.Events.map(event=> {
            this._RenderMarker(event);
        })

    }

    _RenderMarker(event) {
        console.log('🧭 _RenderMarker was called:', event);
        console.trace('Trace for marker source');
        L.marker([event.Latitude,event.Longitude]).addTo(this._map)
            .bindPopup(
          L.popup({
          maxWidth: 250,
          minWidth: 100,
          autoClose: false,
          closeOnClick: false,
       
        }))
         .setPopupContent(`${event.Title}`)
      
          
    }
    _ResizeObserver() {

        const resizeObserver = new ResizeObserver(() => {
            this._map.invalidateSize();
        });

        const mapDiv = $('.map-container')[0];
        resizeObserver.observe(mapDiv);

    }


    addHandlerLocate(handler) {

        $(this._parentEl).on('click', function (e) {
            e.preventDefault();
          
            if (!e.target.closest('.locate__btn')) return;

            handler();
        })
    }
}

export default new MapView();