/* 
 * We have to put this here because it keeps calling login instead of startup and its 5:30,
 * I'm too tired to be trying to figure out why this shit doesnt work.
 * Fuck this shit
 * 
 */
var login = function () {
    return {
        init: function (playerName) {
            startup.init(playerName);
        }
    };
}();
var startup = function () {
    var version = "1.0.0.0",
        resourceName = "",
        playerName = "",
        //VehicleDragDrop
        vehicleDragDrop = false, currentDragDrop = null, currentDragDropParent = null;
    var init = false;
    return {
        init: function (playerName) {
            if (!init) {
                $('select').select2({
                    minimumResultsForSearch: -1
                });
                $('#playerName').html(playerName);
                startup.showSplash();
                $('.body').addClass("scroll");
                init = true;
            }
        },
        showSplash: function () {
            $('.loading > .text').addClass("show");
            setTimeout(function () {
                $('.loading > .text').addClass("out");
                setTimeout(function () {
                    $('.loading > .text').removeClass("show out").html("Loading Player Data");
                    mp.trigger("getPlayerData");
                    setTimeout(function () {
                        $('.loading > .text').addClass("show");
                        $('.loading > .animation').addClass("show");
                    }, 10);
                }, 1000);
            }, 2000);
        },
        showCharacterSelection: function (chars) {
            $('.characterSelection').empty();
            var characters = JSON.parse(chars);
            for (var i = 0; i < characters.length; i++) {
                var character = characters[i];
                $('.characterSelection').append(`<div class="col-md-3">
                                                    <div class="characterCard">
                                                        <div class="name">
                                                            {1}
                                                        </div>
                                                        <div class="delete" data-toggle="modal" data-target="#areYouSureModal" data-characterid="{0}" data-charactername="{1}">
                                                            <i class="fa fa-times"></i>
                                                        </div>
                                                        <div class="stats">
                                                            <p>Gender: {2}</p>
                                                            <p>Date of Birth: {3}</p>
                                                            <p>Cash: \${4}</p>
                                                            <p>Bank: \${5}</p>
                                                            <button class="btn btn-success btnLoadCharacter" data-characterid="{0}">
                                                                Select Character
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>`.format(character.Character_ID, character.CharacterName, character.GenderString, character.Cash, character.Bank));
            }
            $('.characterSelection').append(`<div class="col-md-3">
                                                <div class="addNew" data-toggle="modal" data-target="#newCharacterModal">
                                                    <div>
                                                        <div class="icon">
                                                            <i class="fa fa-plus-circle"></i>
                                                        </div>
                                                    <div class="text">
                                                        Add New Character
                                                    </div>
                                                    </div>
                                                </div>
                                            </div>`).addClass("show");
            $('.characterCard > .delete').off("click").on("click", function () {
                var name = $(this).data("charactername");
                var id = $(this).data("characterid");
                $('#areYouSureModal .characterName').html(name);
                mp.trigger("deleteCharacter", id);
            });
            $('#areYouSureModal').on('hidden.bs.modal', function (e) {
                $('#areYouSureModal .characterName').html("");
            });
            $('.btnLoadCharacter').off("click").on("click", function () {
                var characterName = $(this).data("charactername");
                var id = $(this).data("characterid");
                var loadingText = "Loading Character {0}";
                $('.characterSelection').removeClass("show");
                $('.verticalHorizontal').removeClass("hide");
                $('.loading > .text').html(loadingText.format(characterName));
                mp.trigger("loadCharacter", id);
            });
            $('.btnSaveNewCharacter').off("click").on("click", function () {
                mp.trigger("createCharacter", $('#CharacterName').val(), $('#Year').val(), $('#Month').val(), $('#Day').val());
            });

            $('.loading > .text').addClass("out");
            $('.verticalHorizontal').addClass("hide");
        },
        loadingCharacter: function (characterID) {
            var loadingText = "Loading Character {0}";
            $('.characterSelection').removeClass("show");
            $('.verticalHorizontal').removeClass("hide");
            $('.loading > .text').html(loadingText.format(characterName));
            mp.trigger("loadCharacter", id);
        }
    };
}();