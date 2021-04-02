mp.events.add({
    'playerStartExitVehicle': (player) => {
        if (player.vehicle.engine) player.vehicle.engine = true;
    }
});﻿