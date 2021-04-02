var phone = function() {
    var version = "RageDroid 0.0.0.1";
    var callSeconds = 0,
        activeCall = false,
        isCounting = false;
    var numbers = [
        {
          "id": 1,
          "Name": "Testing",
          "Number": "(555) 456 7890"
        },
        {
          "id": 2,
          "Name": "Testing Tester",
          "Number": "(555) 789 0123"
        },
        {
          "id": 3,
          "Name": "Testing Testerson",
          "Number": "(555) 123 4567"
        }
      ];
    var messages = [
        {
            "contactid": 1,
            "Name": "Testing",
            "NewMessages": 5,
            "messages": [{
                    "type": 1,
                    "message": "testing 123testing 123testing 123testing 123testing 123testing 123testing 123testing 123testing 123testing 123testing 123testing 123"
                },{
                    "type": 2,
                    "message": "tester 321"
                }
            ]
        },
        {
          "contactid": 2,
          "Name": "Testing Tester",
          "NewMessages": 0,
          "messages": [{
                    "type": 1,
                    "message": "testing 1234"
                },{
                    "type": 2,
                    "message": "tester 3215"
                }
            ]
        },
        {
          "contactid": 3,
          "Name": "Testing Testerson",
          "NewMessages": 0,
          "messages": [{
                    "type": 1,
                    "message": "testing 1237"
                },{
                    "type": 2,
                    "message": "tester 3218"
                }
            ]
        }
      ];
    return {
        init: function () {
            $('.homeButton').click(function() {
                if(activeCall) {
                    $('.callStatus').removeClass("open");
                    $('.notificationBar').addClass("returnToCall");
                    $('.return').click(function() {
                        $('.callStatus').addClass("open");
                        $('.notificationBar').removeClass("returnToCall");
                    });
                }
                
                phone.closeCurrentApp();
            });
            $('.app').click(function () {
                var type = $(this).data("type");
                phone.openApp(type);
            });
            $('.muteBtn').click(function () {
                phone.muteCall(this);
            });
        },
        openCaller: function(state, callerName, callerNumber) {
            $('.callStatus > .actions .action').unbind();
            switch (state) {
                case "OUTCALL":
                    activeCall = true;
                    $('.navigation').removeClass("hide");
                    $('.callTimer, .callAction').removeAttr("hidden");
                    callerNumber = phone.formatPhoneNumber(callerNumber);
                    $('.callerNumber').html(callerNumber);
                    if(callerName == "")
                        $('.caller').html("Unknown");
                    else 
                        $('.caller').html(callerName);
                    $('.callTimer').html("Connecting...")
                    $('.callStatus > .actions').prop("hidden", true);
                    setTimeout(function() {
                        phone.closeCurrentApp();
                    }, 250);
                    //mp.trigger("startCall", )
                    break;
                case "INCALL":
                    if(callerName == "")
                        callerName = "Unknown";
                    callerNumber = phone.formatPhoneNumber(callerNumber);
                    $('.caller').html(callerName);
                    $('.callerNumber').html(callerNumber);
                    $('.navigation').addClass("hide");
                    $('.callStatus > .actions .action').click(function() {
                        if($(this).hasClass("answer")) {
                            phone.openCaller("CONNECTED")
                            //mp.trigger("answerCall");
                        }
                        if($(this).hasClass("reject")) {
                            phone.closeCaller();
                            //mp.trigger("rejectCall");
                        }
                    });
                    break;
                case "CONNECTED":
                    $('.callTimer').html('<span class="callMinutes">00</span>:<span class="callSeconds">00</span>');
                    activeCall = true;
                    $('.navigation').removeClass("hide");
                    $('.callTimer, .callAction').removeAttr("hidden");
                    $('.callStatus > .actions').prop("hidden", true);
                    $('.callStatus > .callAction > .actions .action.reject').click(function() {
                        phone.closeCaller();
                    });
                    phone.countTime();
                    break;
            }
            $('.callStatus').addClass("open");
        },
        countTime: function() {
            if(!isCounting && activeCall) {
                isCounting = true;
                callSeconds++;
                var seconds = phone.padTimer(callSeconds % 60);
                var minutes = phone.padTimer(parseInt(callSeconds / 60));
                $('.callMinutes').html(minutes);
                $('.callSeconds').html(seconds);
                setTimeout(function() {
                    phone.countTime();
                }, 1000);
                activeCall = true;
                isCounting = false;
            }
        },
        padTimer: function (val) {
            var valString = val + "";
            if (valString.length < 2) {
                return "0" + valString;
            } else {
                return valString;
            }
        },
        closeCaller: function() {
            activeCall = false;
            callSeconds = 0;
            $('.callStatus').removeClass("open");
            $('.navigation').removeClass("hide");
            setTimeout(function() {
                $('.callTimer, .callAction').prop("hidden", true);
                $('.callStatus > .actions').removeAttr("hidden");
                $('.callStatus > .actions .action').unbind();
                $('.caller .callerNumber').html("");
                $('.callTimer').empty().html('<span class="callMinutes">00</span>:<span class="callSeconds">00</span>');
            }, 500);
        },
        muteCall: function(_this) {
            if($(_this).hasClass("active")) {
                $(_this).removeClass("active");
            } else {
                $(_this).addClass("active");
            }
            //mp.trigger("mutecall");
        },
        openApp: function(appType) {
            switch(appType) {
                case "Dialler":
                    $.get('phoneTemplates/dialler.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                        $('.dialPad .background:not(.backspace, .call)').unbind().click(function() {
                            var value = $(this).html().trim();
                            if($('.diallingNumber').val().length <= 10) {
                                value = phone.formatPhoneNumber(`${$('.diallingNumber').val()}${value}`);
                                $('.diallingNumber').val(value);
                            }
                        });
                        $('.dialPad .background.backspace').unbind().click(function() {
                            var value = $('.diallingNumber').val();
                            value = phone.formatPhoneNumber(value.substring(0, value.length-1));
                            $('.diallingNumber').val(value);
                        });
                        $('.dialPad .background.call').unbind().click(function() {
                            var value = $('.diallingNumber').val();
                            phone.openCaller("OUTCALL", "", value);
                        })
                    });
                    break;
                case "Phonebook":
                    $.get('phoneTemplates/phonebook.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                        for(var i = 0; i < numbers.length; i++) {
                            $('.contactList').append(`<div class="contact" data-id="${numbers[i].id}" data-name="${numbers[i].Name}"><div class="name">${numbers[i].Name}<div class="icon"><i class="fa fa-angle-right"></i></div></div></div>`);
                        }
                        $('.contactList').append("<br>");
                        $('.contact').unbind().click(function() {
                            var id = $(this).data("id");
                            var contact = numbers.find(x => x.id == id);
                            $('.name > p').html(contact.Name);
                            $('.number > p').html(contact.Number);
                            $('.number').data("number", contact.Number);
                            $('.viewer').removeAttr("hidden");
                            $('.options').addClass("record");
                            $('.viewer .call').unbind().click(function() {
                                var number = phone.defaultNumber($('.number').html().trim());
                                phone.openCaller("OUTCALL", $('.name > p').html().trim(), "5551234567")
                            });
                            $('.viewer .back').unbind().click(function() {
                                $('.viewer').prop("hidden", true);
                                $('.options').removeClass("record");
                                $('.number').data("number", "");
                                $('.number > p, .name > p').empty();
                            });
                        });
                    });
                    break;
                case "Messaging":
                    $.get('phoneTemplates/messaging.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                        for(var i = 0; i < messages.length; i++) {
                            var message = messages[i];
                            var newMessages = "";
                            if(message.NewMessages != 0) {
                                newMessages = `<div class="newMessage">${message.NewMessages}</div>`;
                            }
                            $('.contactList').append(`<div class="contact" data-id="${message.contactid}" data-name="${message.Name}"><div class="name">${message.Name}<div class="icon"><i class="fa fa-angle-right"></i></div>${newMessages}</div></div>`);
                        }
                        $('.contact').unbind().click(function() {
                            var id = $(this).data("id");
                            var _messages = messages.find(x => x.contactid == id);
                            $('.options').prop("hidden", true);
                            $('.viewer.messages > .name > p').html(_messages.Name);
                            for(var i = 0; i < _messages.messages.length; i++) {
                                var message = _messages.messages[i];
                                if(message.type == 1) { //Inbound
                                    $('.viewer.messages > .messages').append(`<div class="message"><div class="inbound">${message.message}</div></div>`);
                                } else if(message.type == 2) { //Outbound
                                    $('.viewer.messages > .messages').append(`<div class="message"><div class="outbound">${message.message}</div></div>`);
                                }
                                $('.viewer.messages').removeAttr("hidden");
                                $('.viewer.messages > .name > .back').unbind().click(function() {
                                    $('.viewer.messages').prop("hidden", true);
                                    $('.viewer.messages > .name > p, .viewer.messages > .messages').empty();
                                    $('.options').removeAttr("hidden");
                                });
                            }
                        });
                        $('.contactList').append("<br>");
                    });
                    break;
                case "Settings":
                    $.get('phoneTemplates/settings.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                    });
                    break;
                case "MailAds":
                    $.get('phoneTemplates/mail.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                    });
                    break;
                case "Banking":
                    $.get('phoneTemplates/banking.html', function(data) {
                        $('.currentApp').html(data).addClass("open");
                    });
                    break;
            }
            if(!activeCall) {
                $('.notificationBar').addClass("appOpen");
            }
        },
        closeCurrentApp: function() {
            $('.currentApp').removeClass("open");
            $('.notificationBar').removeClass("appOpen");
            setTimeout(function() {
                $('.currentApp').empty();
            }, 500);
        },
        checkPhoneNumber: function(phoneNumber) {
            var cleaned = ('' + phoneNumber).replace(/\D/g, '')
            var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/)
            return match;
        },
        formatPhoneNumber: function(phoneNumberString) {
            var cleaned = ('' + phoneNumberString).replace(/\D/g, '')
            var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/)
            if (match) {
                return '(' + match[1] + ') ' + match[2] + ' ' + match[3]
            }
            phoneNumberString = phoneNumberString.replace("(", "");
            phoneNumberString = phoneNumberString.replace(")", "");
            phoneNumberString = phoneNumberString.trim();
            return phoneNumberString;
        },
        defaultNumber: function(phoneNumberString) {
            var cleaned = phoneNumberString.replace(/\(/g, '');
                cleaned = cleaned.replace(/\)/g, '');
                cleaned = cleaned.replace(/ /g, "");
            return cleaned;
        },
        showPhone: function() {
            $('.phone').addClass("open");
            $(document).keyup(function (e) {
                if (e.keyCode === 27) phone.hidePhone();
            });
        },
        hidePhone: function () {
            phone.closeCurrentApp();
            $('.phone').removeClass("open");
            $(document).unbind();
            mp.trigger("HidePhone");
        }
    }
}();