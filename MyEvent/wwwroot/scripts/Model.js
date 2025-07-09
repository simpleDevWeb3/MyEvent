
export const state = {
    coords: {},
    address: {},
    Events:[],
}

const getCoords = async function () {
    return new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
    });
};

export const setAddress = async function () {
    try {
        const position = await getCoords();
        const { latitude: lat, longitude: lng } = position.coords;

        state.coords = { lat, lng };
        const res = await fetch(`https://api.geoapify.com/v1/geocode/reverse?lat=${lat}&lon=${lng}&apiKey=8fc0ccffb1c646c583f403f8e9c167da`)

        if (!res.ok) throw new Error(`Error:{}${res.statusText}`)

        const data = await res.json();
        const address = data.features[0].properties;
        console.log(address);

        state.address = {
            state: address.state,
            city: address.city,
            name: address.name,
            suburb: address.suburb,
            street: address?.street,
            postcode: address.postcode,
        }


    } catch (error) {
        throw (error);
    }
}

export const getEvents = async function () {
    try {
        const res = await fetch(`api/Event/Location/${state.address.city?.trim() || state.address.state}`);

        if (!res.ok) throw new error(`Someting wrong{}: ${res.statusText}`)

        const data = await res.json();

       

        const Events = data.map(e => ({
            EventId: e.eventId,
            Title: e.title,
            ImageUrl: e.imageUrl,
            Street: e.street,
            City: e.city,
            State: e.state,
            Longitude: e.longitude,
            Latitude: e.latitude,
            Date: e.date,          
            Organizer: e.organizer, 
            StartTime: e.startTime,
            EndTime: e.endTime,
            Description:e.description
        }));

        state.Events = Events;

    } catch (error) {
        throw (error);
    }

}