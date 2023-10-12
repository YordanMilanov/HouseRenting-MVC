function statistics() {
    $('#statistics_btn').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        $.get('https://localhost:7028/api/statistics', function (data) {
            $('#total_houses').text(data.totalHouses + " Houses");
            $('#total_rents').text(data.totalRents + " Rents");
            $('#statistics_box').removeClass('d-none');
            $('#statistics_btn').hide();
        });
    })
}