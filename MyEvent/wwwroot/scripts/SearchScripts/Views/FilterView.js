class FilterView {
    _ParentEl = $('.filter-sidebar')[0]

    AddHandleChange(handler) {
        
        $(this._ParentEl).on("change", ".filter-group", (e) => {
            console.log("changess");

            // Loop through direct child input and select elements
            $(this._ParentEl).find('input, select').each(function () {
                const name = $(this).attr('name');
                const value = $(this).val();

                console.log("Setting up params)");

                const currentParams = new URLSearchParams(window.location.search);
                if (value && currentParams.get(name) !== value) {
                    currentParams.set(name, value); 
                    history.pushState({}, '', `${location.pathname}?${currentParams}`);
                }
                  
                
            });
            console.log(this._filters);
            const query = $(e.target).val();           // Value of the changed filter-group input/select
            console.log(query);
           
      


            handler(query);                            // Pass value to the handler
        });
    }
    



}

export default new FilterView();