var money = function() {
    var version = "1.0.0.0";
    var changeTimeout, amountTimeout;
    return {
        init: function(amount) {
            var formatted = moneyF.format(amount);
            formatted = formatted.replace("$","");
            formatted = formatted.replace(".00", "");
            $('.amount').html(formatted);
        },
        setAmount: function(amount) {
            clearTimeout(amountTimeout);
            var formatted = moneyF.format(amount);
            formatted = formatted.replace("$","");
            formatted = formatted.replace(".00", "");
            $('.amount').html(formatted);
            $('.amount, .indicator').addClass("active");
            amountTimeout = setTimeout(function() {
                $('.amount, .indicator').removeClass("active");
            }, 3000);
        },
        add: function(amount) {
            clearTimeout(changeTimeout);
            $('.amountChange').html("<span class='icon'><i class='fa fa-plus'></i></span><span class='value'>{0}</span>".format(amount)).removeClass("remove").addClass("add");
            changeTimeout = setTimeout(function() {
                $('.amountChange').removeClass("add");
                setTimeout(function() {
                    $('.amountChange').empty();
                }, 1000);
            }, 3000);
        },
        remove: function(amount) {
            clearTimeout(changeTimeout);
            $('.amountChange').html("<span class='icon'><i class='fa fa-minus'></i></span><span class='value'>{0}</span>".format(amount)).removeClass("add").addClass("remove");
            changeTimeout = setTimeout(function() {
                $('.amountChange').removeClass("remove");
                setTimeout(function() {
                    $('.amountChange').empty();
                }, 1000);
            }, 3000);
        }
        // animateAmount: function(start, end, duration) {
        //     var range = end - start;
        //     var current = start;
        //     var increment = end > start? 1 : -1;
        //     var stepTime = Math.abs(Math.floor(duration / range));
        //     var timer = setInterval(function() {
        //         current += increment;
        //         var formatted = moneyF.format(current);
        //         formatted = formatted.replace("$", "");
        //         $('.amount').html(formatted);
        //         if (current == end) {
        //             clearInterval(timer);
        //         }
        //     }, stepTime);
        // }
    }
}();