var store = function() {
    var version = "1.0.0.0";
    var _vList;
    return {
        openStore: function(store, vehicleList) {
            _vList = $.parseJSON(vehicleList);
            switch(store) {
                case "PDM":
                    $('.vehicleStore .title > .img').attr("src", "img/stores/vehiclestores/pdm.png");
                    for(var i = 0; i < _vList.length; i++) {
                        var vehicle = _vList[i];
                        $('.vehicleStore > .selection > .vehicleList').append('<div class="vehicle" data-name="{0}">\
                            <div class="name">\
                                {0}\
                            </div>\
                            <div class="bottom">\
                                <span class="price">\
                                    ${1}\
                                </span>\
                                <span class="actions">\
                                    <button class="btn btn-view" data-id="{2}">\
                                        <i class="fa fa-eye"></i>\
                                    </button>\
                                    <button class="btn btn-buy" data-id="{2}">\
                                        <i class="fa fa-shopping-cart"></i> Buy\
                                    </button>\
                                </span>\
                            </div>\
                        </div>'.format(vehicle.VehicleName, vehicle.Price, vehicle.VehicleCode));
                    }
                    $(document).on("keyup", function (e) {
                        if (e.keyCode === 27) mp.trigger("HideStore");
                    });
                    $('.closeWindow').off().on("click", function() {
                        mp.trigger("HideStore");
                    });
                    $('.vehicleStore .vehicleList > .vehicle').off().on("click", function(e) {
                        if(!$(e.currentTarget).hasClass("vehicle") && !$(e.currentTarget).hasClass("btn") && !$(e.target).hasClass("btn"))
                            return;

                        var _this = this;
                        var _id = $(_this).data("id");
                        if(typeof _id === 'undefined'){
                            _id = $(e.target).data("id");
                            _this = e.target;
                        }

                        if($(_this).hasClass("btn-view")) {
                            mp.trigger("ViewVehicle", _id);
                        } else if($(_this).hasClass("btn-buy")) {
                            
                        } else {
                            if($(this).hasClass("open")) {
                                $(this).removeClass("open");
                            } else {
                                $('.vehicleStore .vehicleList > .vehicle').removeClass("open");
                                $(this).addClass("open");
                            }
                        }
                    });
                    $('.vehicleStore .search > input').off().on("keyDown keyUp keyPress", function() {
                        var query = $(this).val();
                        $('.vehicle').each(function() {
                            var name = $(this).data("name");
                            if(name.contains(query)) {
                                $(this).show();
                            } else {
                                $(this).hide();
                            }
                        });
                    });
                    break;
                default:
                    break;
            }
        }
    }
}();