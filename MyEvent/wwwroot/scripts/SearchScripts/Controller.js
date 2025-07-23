import * as Model from '../../scripts/Model.js'
import MapView from './Views/MapView.js';
import ResultView from './Views/ResultView.js';
import searchView from './Views/searchView.js';
import FilterView from './Views/FilterView.js';




const renderMap = async function () {
    try {
        const data = new URLSearchParams(window.location.search).get('q');
        await Model.getSearch(data);
        await Model.setAddress();
       

        console.log("load map");
        console.log(Model.state);

        MapView.render(Model.state);

        ResultView.render(Model.state.Search);
    } catch (error) {
        console.log(error);
    }
}

const SearchController =  async function (data) {
   
   
    try {
        const newParams = new URLSearchParams();
        newParams.set('q', data);

        const newUrl = `${window.location.pathname}?${newParams.toString()}`;
        window.history.pushState({}, '', newUrl);
        console.log("New URL:", newUrl);
       // Get search result from api
        await Model.getSearch(data);
        console.log(Model.state.Search);
        ResultView.render(Model.state.Search)


       // const { Latitude: lat, Longitude: lng } = Model.state.Search[0];
        MapView._UpdateMarkers(Model.state.Search)
       // MapView.MoveToCoords(lat, lng);
        ResultView.expand();
        
    } catch (error) {
        console.log(error);
    }
    
    
}
const HoverHandler = function (eventId) {
    const event = Model.state.Search.find(e => e.EventId === eventId);

    MapView.MoveToCoords(event.Latitude, event.Longitude);
    ResultView.highlightEvent(event.EventId);
}


const fillterHandler = async function (query) {
    try {
        const params = new URLSearchParams(window.location.search);
        console.log("get params")

        Model.state.Filter.Query = params.get('q');
        Model.state.Filter.Category = params.get('category');
        Model.state.Filter.StartDate = params.get('StartDate');
        Model.state.Filter.EndDate = params.get('EndDate');
        Model.state.Filter.City = params.get('City');
        Model.state.Filter.Organizer = params.get('Organizer');

        console.log('fetch data');
        await Model.getFilterQuery();
        console.log(Model.state.Search);

        console.log('display new relevent result');
        ResultView.render(Model.state.Search)

        console.log('display new relevent result on map');
        MapView._UpdateMarkers(Model.state.Search)

    } catch (error) {
        console.log(error);
    }

}







export const init = function () {
    //View
    renderMap(); 
    

    searchView.AddSearchHandler(SearchController);
    ResultView.AddHover(HoverHandler);
    FilterView.AddHandleChange(fillterHandler);
}

