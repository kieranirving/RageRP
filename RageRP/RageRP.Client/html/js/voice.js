var voice = function() {
    var version = "1.0.0.0";
    var timeout;
    return {
        init: function() {

        },
        toggleTalking: function(istalking) {
            if(istalking === "True") {
                $('.voice > .mic').removeClass("active");
            } else {
                $('.voice > .mic').addClass("active");
            }
        },
        changeTalkingDistance: function(type) {
            clearTimeout(timeout);
            $('.voice > .talkingDistance').html(type).addClass("active");
            setTimeout(function() { $('.voice > .talkingDistance').addClass("transition"); }, 1);
            timeout = setTimeout(function() {
                $('.voice > .talkingDistance').addClass("fade");
                setTimeout(function() {
                    $('.voice > .talkingDistance').removeClass("active").removeClass("fade transition");
                }, 1000);
            }, 2000);
        },
        toggleMuted: function(mute, message) {
            if(mute === 'True') {
                $('.muted:not(.voice)').addClass("show").html(message);
                $('.voice').addClass("muted");
                $('.mic > i').removeClass("fa-microphone").addClass("fa-microphone-slash");
            } else {
                $('.muted:not(.voice)').removeClass("show").empty();
                $('.voice').removeClass("muted");
                $('.mic > i').addClass("fa-microphone").removeClass("fa-microphone-slash");
            }
        }
    }
}();