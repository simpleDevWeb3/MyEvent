class SearchView {
    _ParentEl = $('.search-form')[0];



   AddToggleSearchBar() {

        $('.search-btn').on('click', function (e) {
            $('.search-bar').toggleClass('active');
            $('.search-bar').css("border", "none")
        })

    }

    AddfocusSearchInput() {
        $('.search-bar').on('click', function (e) {
          $('.search-bar').css("border", "1px solid blue")
        })
    }   

    AddSearchHandler(handler) {

      //Add recommend live search 

        $(this._ParentEl).on('submit', function (e) {
           

            e.preventDefault();
            const query = $('#search').val().trim();
            if (query.length <= 3) return;
            console.log(query);


            handler(query);
           

        })  


    }

    

  

    
}

export default new SearchView();