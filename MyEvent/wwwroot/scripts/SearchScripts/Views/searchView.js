class SearchView {
    _ParentEl = $('.search-form')[0];

 

    AddSearchHandler(handler) {

      //Add recommend live search 

        $(this._ParentEl).on('submit', function (e) {
           

            e.preventDefault();
            const query = $('#search').val().trim();
            if (window.location.pathname !== '/Home/Search') {
                window.location.href = `/Home/Search?q=${encodeURIComponent(query)}`;
            }
            console.log(query);

       
            handler(query);
           

        })  


    }

    

  

    
}

export default new SearchView();