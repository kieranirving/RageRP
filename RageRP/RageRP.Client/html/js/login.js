var login = function () {
    var version = "1.0.0.0",
        resourceName = "",
        playerName = "",
        //VehicleDragDrop
        vehicleDragDrop = false, currentDragDrop = null, currentDragDropParent = null;
    return {
        init: function(playerName) {
            $('#CharacterName').html(playerName);
            $('#loginModal').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
            $('.btnDisconnect').off("click").on("click", function() {
                mp.trigger("disconnected", "Disconnected from server");
            });
            $('.btnLogin').off("click").on("click", function () {
                mp.trigger("login", $('#Password').val());
            });

            $(document).off("keyup").on("keyup", function (e) {
                if (e.keyCode == 13) {
                    $('.btnLogin').trigger("click");
                }
            });
        },
        disconnect: function(reason, expTime) {
            $('#disconnectModal .modal-content.disconnected .reason > .value').html(reason);
            if(typeof expTime !== 'undefined') {
                $('#disconnectModal .modal-content.disconnected .expTime > .value').html(expTime).parent().removeAttr("hidden");
            }
            $('#disconnectModal').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        }
    }
}();