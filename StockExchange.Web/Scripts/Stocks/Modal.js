/*
* Stocks/Index Modal JavaScript File
*/

// Fill the modal with data provided by the button that activated it.
$('#stocksModal').on('show.bs.modal', function (e) {
    var button = $(e.relatedTarget);
    var companyCode = button.data('company'); // Company code
    var actionLabel = button.data('actionlabel'); // Either "buy" or "sell", corresponds with StocksController actions
    var modal = $(this);

    // Reset & set the max value of input field for sell action (Not allowed to sell more than what you jave)
    $('#stocksAmount').prop('max', null);

    // Set the max values for input field. 
    // Number of user owned stocks when selling, 
    // 1000000 when buying.
    if (actionLabel === "sell") {
        var maxStocks = button.data('maxstocks');
        $('#stocksAmount').prop('max', maxStocks);
    } else {
        $('#stocksAmount').prop('max', 1000000);
    }
    $('#stocksCompanyCode').text(companyCode);

    // Set the initial stocks value and update it on input change
    $('#stocksPriceTotal').text(calculateActionStocksValueTotal($('#stocksCompanyCode').text(), 1))
    $('#stocksAmount').on('change', function () {
        $('#stocksPriceTotal').text(calculateActionStocksValueTotal(companyCode, this.value));
    });

    $('#stocksAmount').val("1"); // Set 1 as the default value, seems to be the best way
    modal.find('.stocksActionLabel').text(actionLabel);
})

// Buy/Sell button inside the modal popup.
$('#stocksModalForm').on('submit', function (e) {
    e.preventDefault();
    var action = $('.stocksActionLabel').first().text();
    var company = $('#stocksCompanyCode').text();
    var amount = $('#stocksAmount').val();
    // Confirm the action before sending the request
    if (confirm("Do you really want to " + action + " " + amount + " " + company + " stock/s?")) {
        $.ajax({
            url: "Stocks/" + action,
            type: "POST",
            data: {
                company: company,
                amount: amount
            }
        }).done(function (response) {
            // Replace the partial view with the response, dismiss the modal. 
            // Restart the site refresher so it doesn't immediately dismiss the bootstrap alert, register bootstrap tooltips again.
            $('#stocksContent').html(response);
            $('#stocksModal').modal('toggle');
            siteRefresher.reset();
            $('[rel=tooltip]').tooltip();
        })
    }
})

// Dynamically calculate the total value of stocks in a given transaction
var calculateActionStocksValueTotal = function (companyCode, amount) {
    var price = parseFloat($('#' + companyCode + '-price').text().replace(',', '.'));
    return (price * amount).toFixed(2);
}

