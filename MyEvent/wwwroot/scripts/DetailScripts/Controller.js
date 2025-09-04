
import CountdownView from './View/CountdownView.js';
import MapView from './View/MapView.js';
import RecommendView from './View/RecommendView.js';
import AttandeView from './View/AttandeView.js';
import * as Model from '../Model.js'


const CountdownController = async function () {
    try {
        const ParamId = location.pathname + location.search;
        const Id = ParamId.split('?')[1].split('=')[1]
        console.log();
        await Model.fetchEvent(Id);
  
        CountdownView.render(Model.state.currentEvent[0]);
        MapView.render(Model.state.currentEvent[0])




        console.log(Model.state.currentEvent);



    } catch (error) {
        console.log(error);
    }
}

const RecommededController = async function (tag,eventId) {
    try {
        //set data 
        Model.clearEvents()
        await Model.getRecommended(tag,eventId);
        console.log(Model.state.Events);
        RecommendView.render(Model.state.Events);
    } catch (error) {
        console.log(error)
    }
}

const AttandeController = async function (eventId) {
    try {
        await Model.getAttande(eventId);
        console.log(Model.state.currentEvent.Attande);
        AttandeView.render(Model.state.currentEvent.Attande);


    } catch (error) {

    }

}




export const init = function () {
    //BreadCrumpView.render();
    MapView.Addhandler(CountdownController);
    RecommendView.addHandlerDefault(RecommededController);
    RecommendView.addHandlerDisplay(RecommededController);
    AttandeView.addHandler(AttandeController);
}