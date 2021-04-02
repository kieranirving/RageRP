var spawnSelect = function () {
    var version = "1.0.0.0",
        resourceName = "",
        init = false;
    var spawnList = [];
    return {
        init: function(spawnLocations) {
            if(!init) {
                var spawnLoc = JSON.parse(spawnLocations);
                console.log(spawnLoc);
                for(var i = 0; i < spawnLoc.length; i++) {
                    spawnList.push(spawnLoc[i]);
                }
                spawnSelect.changeCamera(spawnList[0]);
                $('.btnBack').on("click", function() {
                    var spawnId = parseInt($('#spawnId').val());
                    var newSpawnIndex = 0;
                    var currentSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnId));
                    var firstSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnList[0].id));
                    var lastSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnList[spawnList.length - 1].id));
                    if(currentSpawnIndex != firstSpawnIndex) {
                        newSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnId));
                    } else {
                        newSpawnIndex = lastSpawnIndex + 1;
                    }
                    newSpawnIndex--;
                    spawnSelect.changeCamera(spawnList[newSpawnIndex]);
                });
                $('.btnForward').on("click", function() {
                    var spawnId = parseInt($('#spawnId').val());
                    var newSpawnIndex = 0;
                    var currentSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnId));
                    var firstSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnList[0].id));
                    var lastSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnList[spawnList.length - 1].id));
    
                    if(currentSpawnIndex != lastSpawnIndex) {
                        newSpawnIndex = spawnList.indexOf(spawnList.find(x => x.id === spawnId));
                    } else {
                        newSpawnIndex = firstSpawnIndex - 1;
                    }
                    newSpawnIndex++;
                    spawnSelect.changeCamera(spawnList[newSpawnIndex]);
                });
                $('.btnSpawn').on("click", function() {
                    var spawnId = parseInt($('#spawnId').val());
                    mp.trigger("SelectSpawn", spawnId);
                });
                init = true;
            }
        },
        changeCamera: function(location) {
            $('.spawnSelector .title').html(location.Name);
            $('.spawnSelector #spawnId').val(location.id);
            mp.trigger("ChangeLocation", location.id);
        }
    }
}();