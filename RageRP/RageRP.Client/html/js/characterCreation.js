var charCreate = function () {
    var version = "1.0.0.0",
        resourceName = "";
    var currentGender = 0,
        init = false;
    const father = ["Benjamin", "Daniel", "Joshua", "Noah", "Andrew", "Juan", "Alex", "Isaac", "Evan", "Ethan", "Vincent", "Angel", "Diego", "Adrian", "Gabriel", "Michael", "Santiago", "Kevin", "Louis", "Samuel", "Anthony", "Claude", "Niko", "John"],
          mother = ["Hannah", "Aubrey", "Jasmine", "Gisele", "Amelia", "Isabella", "Zoe", "Ava", "Camila", "Violet", "Sophia", "Evelyn", "Nicole", "Ashley", "Gracie", "Brianna", "Natalie", "Olivia", "Elizabeth", "Charlotte", "Emma", "Misty"],
          features = ["Nose Width", "Nose Bottom Height", "Nose Tip Length", "Nose Bridge Depth", "Nose Tip Height", "Nose Broken", "Brow Height", "Brow Depth", "Cheekbone Height", "Cheekbone Width", "Cheek Depth", "Eye Size", "Lip Thickness", "Jaw Width", "Jaw Shape", "Chin Height", "Chin Depth", "Chin Width", "Chin Indent", "Neck Width"],
          appearance = ["Blemishes", "Facial Hair", "Eyebrows", "Ageing", "Makeup", "Blush", "Complexion", "Sun Damage", "Lipstick", "Moles & Freckles", "Chest Hair"],
          appearanceItems = [
              0,// blemish
              0,// facial hair
              0,// eyebrows
              0,// ageing
              0,// makeup
              0,// blush
              0,// complexion
              0,// sun damage
              0,// lipstick
              0,// freckles
              0// chest hair
          ];
    const minFeature = -1.0,
          maxFeature = 1.00;
    return {
        init: function (blemish, facialhair, eyebrows, ageing, makeup, blush, complextion, sundamage, lipstick, freckles, chesthair) {
            if (!init) {
                mp.trigger("setCharacterGender", 0);
                $('.genderSel').on("click", function() {
                    $('.genderSel').removeClass("active");
                    $(this).addClass("active");
                    currentGender = $(this).data("type");
                    mp.trigger("setCharacterGender", currentGender);
                });

                $('.btnConfirmGender').on("click", function() {
                    //currentGender = $('.genderSel.active').data("type");
                    //mp.trigger("setCharacterGender", currentGender);
                    $('.charSelection .loading').removeClass("hide");
                    $('.genderSelectWrap').removeClass("active");
                    charCreate.showCustomizer();
                });
                
                $('.genderSelectWrap').addClass("active");
                $('.charSelection .loading').addClass("hide");

                appearanceItems[0] = blemish;
                appearanceItems[1] = facialhair;
                appearanceItems[2] = eyebrows;
                appearanceItems[3] = ageing;
                appearanceItems[4] = makeup;
                appearanceItems[5] = blush;
                appearanceItems[6] = complextion;
                appearanceItems[7] = sundamage;
                appearanceItems[8] = lipstick;
                appearanceItems[9] = freckles;
                appearanceItems[10] = chesthair;

                //Remove this when in production
                //$('.charSelection .loading').removeClass("hide");
                //$('.genderSelectWrap').removeClass("active");
                //charCreate.showCustomizer();

                init = true;
            }
        },
        showCustomizer: function () {
            $('.customiserWrap').addClass("active");
            $('.charSelection .loading').addClass("hide");

            $('.customiserWrap .nav > .nav-item:not(.confirmModal)').on("click", function() {
                $('.customiserWrap .nav-item:not(.confirmModal), .customiserWrap .nav-item a').removeClass("active");
                $(this).addClass("active");
                $(this).find('a').addClass("active");
                var container = "";
                if($(this).hasClass("nav-item")) {
                    container = $(this).find('.nav-link').data("container");
                } else {
                    container = $(this).data("container");
                }
                if(container == "customFace" || container == "parentSelect") {
                    mp.trigger("editFace");
                } else {
                    mp.trigger("editBody");
                }
                $('.customiserWrap > .row').removeClass("active");
                $('.customiserWrap > .row.{0}'.format(container)).addClass("active");
            });

            var j = 0;
            var fhtml = "";
            for(var i = 0; i < features.length; i++) {
                if(j == 0) { 
                    fhtml += "<div class='row'>"; 
                }
                fhtml += '\
                    <div class="faceItem col-md-6">\
                        <div class="menuLabel middle">\
                            {0}\
                        </div>\
                        <div class="input-group">\
                            <input type="text" class="form-control featureValue {1}" value="0">\
                            <div class="input-group-append buttonFeature">\
                                <button class="btn btn-success plus" data-feature="{1}" type="button"><i class="fa fa-plus"></i></button>\
                                <button class="btn btn-danger minus" data-feature="{1}" type="button"><i class="fa fa-minus"></i></button>\
                            </div>\
                        </div>\
                    </div>'.format(features[i], features[i].replace(/\s/g,''));
                if(j == 1) { 
                    fhtml += "</div>"; 
                    j = 0; 
                    $('.customFace > .content').append(fhtml); 
                    fhtml = "";
                } else { 
                    j++; 
                }
            }
            $('.buttonFeature .btn').on("click", function() {
                charCreate.featureValueHandler(this);
            });
            $('.featureValue').change(function() {
                var _val = $(this).val();
                if(_val >= minFeature && _val <= maxFeature) {
                    //Do Nothing
                } else {
                    $(this).val("0");
                }
                charCreate.UpdateCharacter();
            });

            for(var i = 0; i < appearance.length; i++) {
                $('.row.customOther').append('\
                <div class="col-md-12">\
                    <div class="faceItem">\
                        <div class="menuLabel middle">\
                            {0}\
                        </div>\
                        <input type="hidden" class="slideInputValue {1}" value="0">\
                        <div class="slideInput other" data-id="{2}">\
                            <div class="back"></div>\
                            <div class="choice">\
                                \
                            </div>\
                            <div class="forward"></div>\
                        </div>\
                    </div>\
                </div>'.format(appearance[i], appearance[i].replace(/\s/g,'').replace(/&/g,''), i)); 
            }


            $('.confirmModal').on("click", function() {
                $('#confirmModal').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });
                $('.btnIamHappy').off("click").on("click", function() {
                    mp.trigger("SaveCharacter");
                });
            });
            
            var currentMother = $('.slideInputValue.mother').val(),
                currentFather = $('.slideInputValue.father').val();
            $('.slideInput.mother > .choice').html(mother[currentMother]);
            $('.slideInput.father > .choice').html(father[currentFather]);
            $('.slideInput.other > .choice').html(appearanceItems[0][0]);
            $('.slideInput > .back, .slideInput > .forward').on("click", function() {
                charCreate.handleSlideChoice(this);
            });
        },
        featureValueHandler: function(elm) {
            var _this = elm;
            var feature = $(_this).data("feature");
            var _val = parseFloat($(".featureValue.{0}".format(feature)).val());
            if($(_this).hasClass("plus")) {
                _val = _val + 0.1;
            } else if($(_this).hasClass("minus")) {
                _val = _val - 0.1;
            }
            _val = Math.round(_val * 100) / 100
            if(_val >= minFeature && _val <= maxFeature) {
                $(".featureValue.{0}".format(feature)).val(_val).trigger("change");
            }
        },
        handleSlideChoice: function(elm) {
            var _this = elm;
            var type = "", value = 0, text = "", dataid = "";
            var currentMother = $('.slideInputValue.mother').val(),
                currentFather = $('.slideInputValue.father').val();
            if($(elm).parents().hasClass("mother")) {
                type = "mother"; currentMother++; value = currentMother;
                text = mother[currentMother];
                dataid = type;
            } else if($(elm).parents().hasClass("father")) {
                type = "father"; currentFather++; value = currentFather;
                text = father[currentFather];
                dataid = type;
            } else if($(elm).parents().hasClass("other")) {
                debugger;
                var type = $(elm).parents(".faceItem").find('.menuLabel').text().replace(/\s/g,'');
                var currentOther = parseInt($('.slideInputValue.{0}'.format(type)).val());
                currentOther++;
                var id = $(elm).parents('.slideInput').data("id");
                text = appearanceItems[id][currentOther];
                value = currentOther;
                dataid = id;
            }
            $(".slideInputValue.{0}".format(type)).val(value);
            $('.slideInput[data-id="{0}"] > .choice'.format(dataid)).html(text);
            charCreate.UpdateCharacter();
        },
        UpdateCharacter: function() {
            var firstHeadShape = $('.slideInputValue.mother').val(),
                secondHeadShape = $('.slideInputValue.father').val(),
                firstSkinTone = 0,
                secondSkinTone = 0,
                hairModel = 0,
                firstHairColor = 0,
                secondHairColor = 0,
                beardModel = $('.slideInputValue.FacialHair').val(),
                beardColor = 0,
                chestModel = $('.slideInputValue.ChestHair').val(),
                chestColor = 0,
                blemishesModel = $('.slideInputValue.Blemishes').val(),
                ageingModel = $('.slideInputValue.Ageing').val(),
                complexionModel = $('.slideInputValue.Complexion').val(),
                sundamageModel = $('.slideInputValue.SunDamage').val(),
                frecklesModel = $('.slideInputValue.MolesFreckles').val(),
                eyesColor = 0,
                eyebrowsModel = $('.slideInputValue.Eyebrows').val(),
                eyebrowsColor = 0,
                makeupModel = $('.slideInputValue.Makeup').val(),
                blushModel = $('.slideInputValue.Blush').val(),
                blushColor = 0,
                lipstickModel = $('.slideInputValue.Lipstick').val(),
                lipstickColor = 0,

                headMix = 0,
                skinMix = 0,
                
                noseWidth = $('.featureValue.NoseWidth').val(),
                noseHeight = $('.featureValue.HoseBottomHeight').val(),
                noseLength = $('.featureValue.NoseTipLength').val(),
                noseBridge = $('.featureValue.NoseBridgeDepth').val(),
                noseTip = $('.featureValue.NoseTipHeight').val(),
                noseShift = $('.featureValue.NoseBroken').val(),
                browHeight = $('.featureValue.BrowHeight').val(),
                browWidth = $('.featureValue.BrowDepth').val(),
                cheekboneHeight = $('.featureValue.CheekboneHeight').val(),
                cheekboneWidth = $('.featureValue.CheekboneWidth').val(),
                cheeksWidth = $('.featureValue.CheekDepth').val(),
                eyes = $('.featureValue.EyeSize').val(),
                lips = $('.featureValue.LipThickness').val(),
                jawWidth = $('.featureValue.JawWidth').val(),
                jawHeight = $('.featureValue.JawShape').val(),
                chinLength = $('.featureValue.ChinHeight').val(),
                chinPosition = $('.featureValue.ChinDepth').val(),
                chinWidth = $('.featureValue.ChinWidth').val(),
                chinShape = $('.featureValue.ChinIndent').val(),
                neckWidth = $('.featureValue.NeckWidth').val();
            mp.trigger("updateCharacter", 
                        firstHeadShape,
                        secondHeadShape,
                        firstSkinTone,
                        secondSkinTone,
                        hairModel,
                        firstHairColor,
                        secondHairColor,
                        beardModel,
                        beardColor,
                        chestModel,
                        chestColor,
                        blemishesModel,
                        ageingModel,
                        complexionModel,
                        sundamageModel,
                        frecklesModel,
                        eyesColor,
                        eyebrowsModel,
                        eyebrowsColor,
                        makeupModel,
                        blushModel,
                        blushColor,
                        lipstickModel,
                        lipstickColor,
                        headMix,
                        skinMix,
                        noseWidth,
                        noseHeight,
                        noseLength,
                        noseBridge,
                        noseTip,
                        noseShift,
                        browHeight,
                        browWidth,
                        cheekboneHeight,
                        cheekboneWidth,
                        cheeksWidth,
                        eyes,
                        lips,
                        jawWidth,
                        jawHeight,
                        chinLength,
                        chinPosition,
                        chinWidth,
                        chinShape,
                        neckWidth)
        }
    }
}();