import * as Model from "../../scripts/Model.js";
import MoreView from "./Views/MoreView.js";

const ControllerMoreView = async function () {
    try {
        await Model.setAddress();
        await Model.getEvents();
        MoreView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
    }
    
};

export const init = function () {

    MoreView.addHandlerDisplay(ControllerMoreView);
};
