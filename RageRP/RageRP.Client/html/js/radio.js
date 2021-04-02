var radio = function() {
    var version = "1.0.0.0";
    var currentFreq = "";
    return {
        init: function(freq) {
            currentFreq = freq.toString();
            $('.radio .screen .freq').val(freq);
            $('.ptt').unbind().click(function() {
                
            });
            $('.panic').unbind().click(function() {
                mp.trigger("panic");
            });
            $('.action > .btnSet').unbind().click(function() {
                currentFreq = $('.freq').val();
                mp.trigger("setFreq", currentFreq);
            });
            $('.action > .btnReset').unbind().click(function() {
                $('.freq').val(currentFreq);
            });
            $('.action > .btnClear').unbind().click(function() {
                $('.freq').val('');
            });
            $('.numpad .btnRadio').unbind().click(function() {
                var before = currentFreq.split('.')[0];
                var after = currentFreq.split('.')[1];
                
                var value = $(this).html().trim();
                var _currentFreq = $('.freq').val();

                if(_currentFreq != "") {
                    var _value = "";
                    if(typeof after === 'undefined') {
                        if(before.length >= 3) {
                            _value = `${_currentFreq}.${value}`;
                        } else {
                            _value = `${_currentFreq}${value}`;
                        }
                    } else {
                        if(before.length <= 3) {
                            if(after.length < 3) {
                                _value = `${before}.${after}${value}`;
                            }
                        }
                    }
                    
                    if(_value != "") {
                        $('.freq').val(_value);
                        currentFreq = _value.toString();
                    }
                } else {
                    $('.freq').val(value);
                    currentFreq = value.toString();
                }
            });
            $('.volume > .currentVolume > .btn.volume').unbind().click(function () {
                var currentVol = parseInt($('.volume > .currentVolume > span').html().trim());
                if($(this).hasClass("up")) {
                    if(currentVol != 10) {
                        currentVol = currentVol + 1;
                        if(currentVol == 10) {
                            $('.volume > .currentVolume').addClass("alt").removeClass("alt2");
                        } else if(currentVol == 1) {
                            $('.volume > .currentVolume').addClass("alt2").removeClass("alt");
                        } else {
                            $('.volume > .currentVolume').removeClass("alt alt2");
                        }
                    }
                } else if($(this).hasClass("down")) {
                    if(currentVol != 0) {
                        currentVol = currentVol - 1;
                        if(currentVol != 10) {
                            $('.volume > .currentVolume').removeClass("alt alt2");
                            if(currentVol == 1) {
                                $('.volume > .currentVolume').addClass("alt2").removeClass("alt");
                            }
                        }
                    }
                }
                $('.volume > .currentVolume > span').html(currentVol);
                mp.trigger("changeVolume", currentVol);
            });
        },
        show: function() {
            $('.radio').addClass("open");
        },
        hide: function() {
            $('.radio').removeClass("open");
        },
        playSound: function() {
            var audio = new Audio('../sounds/radio.ogg');
            audio.volume = 0.4;
            audio.play();
        },
        volume: function() {

        }
    };
}();