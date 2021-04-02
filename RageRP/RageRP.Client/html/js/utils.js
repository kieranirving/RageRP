var utils = function () {
    return {
        init: function() {
            
        }
    }
}();

// Create our number formatter.
var moneyF = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});