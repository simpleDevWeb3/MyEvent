
import CountdownView from './View/CountdownView.js';
import MapView from './View/MapView.js';
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



export const init = function () {
    //BreadCrumpView.render();
    MapView.Addhandler(CountdownController);
    
}