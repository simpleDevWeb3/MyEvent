

export const state = {
    Filter: {
        Query: null,
        Category: null,
        StartDate: null,
        EndDate: null,
        Location: null,
        Price: null,
        Organizer: null,
    },
    currentEvent: {},
    coords: {},
    address: {},
    Events: [], //Events data
    Search: [], //User search result Events
    Paging: {
        Page: 1,
        ResultPerPage: 4,
    },
    Sort: null

    
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

const eventObj = function (data) {
    console.log(data);
    return data.map(e => ({
        EventId: e.eventId,
        Title: e.title,
        ImageUrl: e.imageUrl,
        Street: e.address.street,
        City: e.address.city,
        State: e.address.state,
        Longitude: e.address.longitude,
        Latitude: e.address.latitude,
        Date: e.detail.date,
        Organizer: e.detail.organizer,
        StartTime: e.detail.startTime,
        EndTime: e.detail.endTime,
        Description: e.detail.description,
        Category: {
            id: e.category.id,
            name: e.category.name
        }
    }));

}
export const fetchEvent = async function (id) {
    try {
        const res = await fetch(`/api/Event/Id/${encodeURIComponent(id)}`) 
        if (!res.ok) throw new Error(`Someting wrong{}: ${res.statusText}`)

        const data = await res.json();

        const Events = eventObj(data);

        state.currentEvent = Events;

    } catch (error) {
        throw (error);
    }
}
export const getEvents = async function () {
    try {
        const res = await fetch(`/api/Event/All/`);

        if (!res.ok) throw new Error(`Someting wrong{}: ${res.statusText}`)

        const data = await res.json();



        const Events = eventObj(data);

        state.Events = Events;
        if (state.Sort) sortResult(state.Sort,'Events')
    } catch (error) {
        throw (error);
    }

}

export const getRecommended = async function(tags){
    try {
        const res = await fetch(`/api/Event/Recommended/${tags}`);
        if (!res.ok) throw new Error(`${await res.text()}`)
        const data = await res.json();

        const Events = eventObj(data);
        state.Events = Events;
    } catch (error) {
        console.log(error);
    }
}
export const getByTags = async function (tags) {
    try {
     const res = await fetch(`/api/Event/${encodeURIComponent(tags)}`);

        if (!res.ok) throw new Error(`${await res.text()}`)

        const data = await res.json();



        const Events = eventObj(data);

        state.Events = Events;
        console.log(state.Sort)
        if (state.Sort) sortResult(state.Sort, 'Events')

    } catch (error) {
        throw (error);
    }

}

export const getSearch = async function (query) {
    try {
       

        const res = await fetch(`/api/Event/Search/${query}`);
        if (!res.ok) throw new Error(`Something went wrong{}:${res.statusText}`);
        const data = await res.json();

        const Result = eventObj(data);

        state.Search = Result;
       


    } catch (error) {
        throw (error);
    }
}

export const sortResult = function (sort = 'asc',type = 'Search') {
    if (sort === 'asc')
        state[type].sort((a, b) => a.Title.localeCompare(b.Title));
    else if (sort === 'desc')
        state[type].sort((a, b) => b.Title.localeCompare(a.Title));
    else if(sort === 'most recent')
        state[type].sort((a, b) => new Date(a.Date) - new Date(b.Date));
    else if (sort === 'oldest')
        state[type].sort((a, b) => new Date(b.Date) - new Date(a.Date));
    console.log(state[type])
}

export const getFilterQuery = async function () {
    try {

        const queryParams = new URLSearchParams();
        //Looping append query 
        for (const key in state.Filter) {
            const value = state.Filter[key];
            if (value !== null && value !== '') {
                queryParams.append(key, value);
            }
        }

        const res = await fetch(`/api/Event/Filter?${queryParams.toString()}`);

        if (!res.ok) throw new Error(`Something went wrong{}:${res.statusText}`);
        const data = await res.json();


        const Result  = eventObj(data);


        state.Search = Result;
        console.log(state.Sort);

        if (state.Sort) sortSearchResult(state.Sort)


    } catch (error) {
        console.log(error);
    }
}

