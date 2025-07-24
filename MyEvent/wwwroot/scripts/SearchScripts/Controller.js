import * as Model from '../../scripts/Model.js'
import MapView from './Views/MapView.js';
import ResultView from './Views/ResultView.js';
import searchView from './Views/searchView.js';
import FilterView from './Views/FilterView.js';
import PagerView from './Views/PagerView.js';



const renderMap = async function () {
    try {
        //Get query from search
        const data = new URLSearchParams(window.location.search).get('q');
        //fetch result
        await Model.getSearch(data);
        await Model.setAddress();
       

        console.log("load map");
        console.log(Model.state);

        MapView.render(Model.state);
        //Render result
        ResultView.render(Model.state.Search, Model.state.Paging);
        PagerView.render(Model.state);
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
        //Render result
        Model.state.Paging.Page = 1;
        ResultView.render(Model.state.Search, Model.state.Paging);
        PagerView.render(Model.state);


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
        Model.state.Paging.Page = 1;
        console.log(Model.state.Paging.Page);
        ResultView.render(Model.state.Search, Model.state.Paging);
        PagerView.render(Model.state);

        console.log('display new relevent result on map');
        MapView._UpdateMarkers(Model.state.Search)

    } catch (error) {
        console.log(error);
    }

}

const PagingController = function (page) {
    console.log('hi');
    console.log(page);
    Model.state.Paging.Page = page;
    ResultView.render(Model.state.Search, Model.state.Paging);
    PagerView.render(Model.state);
}






export const init = function () {
    //View
    renderMap(); 
    

    searchView.AddSearchHandler(SearchController);
    ResultView.AddHover(HoverHandler);
    FilterView.AddHandleChange(fillterHandler);
    FilterView.AddHandleToggle();
    PagerView.addHandlerPager(PagingController);
}

