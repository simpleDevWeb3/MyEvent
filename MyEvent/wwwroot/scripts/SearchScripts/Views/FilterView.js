class FilterView {
    _ParentEl = $('.filter-sidebar')[0]

    AddHandleChange(handler) {
        $(this._ParentEl).on("change",".filter-group",(e) => {
            console.log("changess");
            const query = $(e.target).val();
            console.log();
            handler(query);
        })
    }

    



}

export default new FilterView();