import * as Model from "../../scripts/Model.js";
import CardView from "./Views/CardView.js";
import NavView from "./Views/NavView.js";
import CarouselView from "./Views/carouselView.js"
import LoadingSkleton from './Views/LoadingSkleton.js';
import TransitionLoading from './Views/TransitionLoading.js'
import FilterView from '../SearchScripts/Views/FilterView.js'

const ControllerEvent = async function () {

    LoadingSkleton.render();
 
    try {
        await Model.setAddress();
        await Model.getEvents();
        Model.sortResult('asc','Events');
        CardView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
    } 
    
};

const ControllerCategory = async function (label) {
    const params = new URLSearchParams(window.location.search);
    console.log("get params")
    Model.state.Sort = params.get('sort')
    //TransitionLoading.render();
    try {
        //$('.nav-lable').toggle();
        await Model.getByTags(label);
        CardView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
        CardView._RenderError(error);
    }

};

const FilterController = function (sort) {
   
    Model.sortResult(sort, 'Events');
    CardView.render(Model.state.Events)
}

export const init = function () {
    
    CardView.addHandlerDisplay(ControllerEvent);
    CarouselView.addHandleScroll();
    NavView.addHandlerHover();
    NavView.addHandlerClick(ControllerCategory);
    FilterView.AddHandleSort(FilterController);
};
