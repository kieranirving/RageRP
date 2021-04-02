var inventory = function () {
    var version = "1.0.0.0",
        resourceName = "",
        playerName = "",
        //VehicleDragDrop
        vehicleDragDrop = false, currentDragDrop = null, currentDragDropParent = null;
    return {
        init: function(playerName) {
            $('select').select2({
                minimumResultsForSearch: -1
            });
            $('.body').addClass("scroll");
            inventory.toggleInventory();
            //inventory.toggleVehicleInventory();

            /*for(var i = 0; i < 19; i++) {
                $('.playerInventory > .scrollWrap > .itemWrap').append(`<div class="col-md-6 item empty">
                        <div class="wrap">
                            <div class="icon">
                            
                            </div>
                            <div class="title">
                                EMPTY
                            </div>
                        </div>
                    </div>`);
            }*/
        },
        toggleInventory: function() {
            if($('.playerInventoryWrap').hasClass("open")) {
                $('.playerInventoryWrap, .playerInventoryWrap > .playerInventory').removeClass("open");
                $('.playerInventoryWrap, .playerInventoryWrap > .playerInventoryActiveItem').removeClass("open half posSort");
                setTimeout(function() {
                    $('.playerInventory .item.show').removeClass("show");
                }, 500);
            } else {
                $('.playerInventoryWrap, .playerInventoryWrap > .playerInventory').addClass("open");
                $('.playerInventoryWrap .item:not(.empty)').unbind().click(function() {
                    var currentItem = $('.playerInventory .item.show');
                    if($(this).hasClass("show")) {
                        $(this).removeClass("show");
                        $('.playerInventoryWrap > .playerInventoryActiveItem').removeClass("open half posSort");
                    } else {
                        var types = $(this).data("types").split(',');
                        var title = $(this).find(".title").html().trim();
                        var timeout = 0;
                        if($(currentItem).length !== 0) {
                            $(currentItem).removeClass("show");
                            if($('.playerInventoryWrap > .playerInventoryActiveItem').hasClass("open")) {
                                $('.playerInventoryWrap > .playerInventoryActiveItem').removeClass("open").addClass("half");
                                timeout = 500;
                            } else {
                                timeout = 0;
                            }
                        } else {
                            timeout = 0;
                        }
                        setTimeout(function() {
                            $('.playerInventoryWrap > .playerInventoryActiveItem').addClass("open");
                            $('.playerInventoryWrap > .playerInventoryActiveItem > .title').html(title);
                            app.handleInvActionMenu(types);
                        }, timeout);
                        $(this).addClass("show");
                    }
                });
            }
        },
        toggleVehicleInventory: function() {
            if($('.playerInventoryWrap').hasClass("open")) {
                $('.vehicleInventory').removeClass("open");
                $('.playerInventoryWrap, .playerInventoryWrap > .playerInventory').removeClass("open");
                $('.playerInventoryWrap, .playerInventoryWrap > .playerInventoryActiveItem').removeClass("open half posSort");
                setTimeout(function() {
                    $('.playerInventory .item.show').removeClass("show");
                }, 500);
            } else {
                $('.vehicleInventory').addClass("open");
                app.toggleInventory();
            }
            app.toggleVehicleDragAndDrop();
        },
        toggleVehicleDragAndDrop: function() {
            vehicleDragDrop = !vehicleDragDrop;
            if(vehicleDragDrop) {
                $('.playerInventory .item, .vehicleInventory .item').attr("draggable", true);
                $('.playerInventory .item, .vehicleInventory .item').on("dragstart", function(ev) {
                    currentDragDrop = $(this).children(".wrap");
                    currentDragDropParent = this;
                });
                $('.playerInventory .item, .vehicleInventory .item').on("drop", function(ev) {
                    if(!$(this).hasClass("empty")) return;
                    if(this === currentDragDropParent) return;
                    ev.preventDefault();
                    var html = $(currentDragDrop).html();
                    $(this).html("<div class='wrap'>{0}'</div>".format(html));
                    $(this).removeClass("empty");
                    app.defaultItem(currentDragDropParent);
                });
                $('.playerInventory .item, .vehicleInventory .item').on("dragover", function(ev) {
                    ev.preventDefault();
                });
            } else {
                $('.playerInventory .item, .vehicleInventory .item').off("drop dragover");
            }
        },
        defaultItem: function(item) {
            $(item).removeAttr("data-types").removeClass("open").addClass("empty");
            $(item).find(".icon").empty();
            $(item).find(".title").empty();
            $(item).find(".title").html("EMPTY");
        },
        handleInvActionMenu: function(types) {
            $('.playerInventoryWrap > .playerInventoryActiveItem > .menu > .action').attr("disabled","");
            for(var i = 0; i < types.length; i++) {
                $('.playerInventoryWrap > .playerInventoryActiveItem > .menu > .action.{0}'.format(types[i].trim())).removeAttr("disabled");
            }
        }
    }
}();