/*
* Stocks/Index Auto-Refresher JavaScript File
*/

// A builder for site refresher, used for easier access its functions.
var refresher = function () {
    var refresher = this; // Bind 'this'
    refresher.start = function () {
        refreshIntervalId = setInterval(function () {
            var tempScrollTop = $(window).scrollTop(); // Save scroll position
            $('#stocksContent').load('Stocks/Stocks', function () {
                $(window).scrollTop(tempScrollTop); // Restore scroll position
                $('[rel=tooltip]').tooltip(); // Add tooltips again
                $('#stocksPriceTotal').text(calculateActionStocksValueTotal($('#stocksCompanyCode').text(), $('#stocksAmount').val())); // Update the current total value of stocks in modal
                $('#graph').prop("src", "Stocks/DrawGraph?" + new Date().valueOf()); // Reload graph image
            });
        },
        25000); // Refresh every 25 seconds
    };
    refresher.stop = function () {
        clearInterval(refreshIntervalId);
    };
    // Restart the refresher
    refresher.reset = function () {
        refresher.stop();
        refresher.start();
    };
}
// Initalize and start the refresher
var siteRefresher = new refresher();
siteRefresher.start();