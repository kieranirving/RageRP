const maxDistance = 25 * 25;
const width = 0.03;
const color = [255, 255, 255, 255];

mp.nametags.enabled = false;

mp.events.add('render', (nametags) => {
    const graphics = mp.game.graphics;
    const screenRes = graphics.getScreenResolution(0, 0);

    nametags.forEach(nametag => {
        let [player, x, y, distance] = nametag;

        if (distance <= maxDistance) {
            let scale = (distance / maxDistance);
            if (scale < 0.6) scale = 0.6;

            y -= scale * (0.005 * (screenRes.y / 1080));

            let red = 255; let green = 255; let blue = 255;

            let name = player.getVariable("handleID");
            //mp.gui.chat.push(`name ${name}`);

            let isTalking = player.getVariable("isTalking");
            if (isTalking) {
                red = 79; green = 79; blue = 114;
            }

            let isAdminOnDuty = player.getVariable("isAdminOnDuty");
            let isMuted = player.getVariable("isMuted");
            let characterName = player.getVariable("CharacterName");
            if (isAdminOnDuty) {
                name = `Admin ${characterName} (${name})`;
                red = 255;
                if (isTalking)
                    green = 63;
                else
                    green = 0;
                blue = 0;
            }

            mp.game.graphics.drawText(`${name}`, [x, y],
            {
                font: 4,
                color: [red, green, blue, 255],
                scale: [0.4, 0.4],
                outline: true
            });

            if (isMuted) {
                mp.game.graphics.drawText(`MUTED`, [x, y + 5],
                {
                    font: 4,
                    color: [255, 0, 0, 255],
                    scale: [0.4, 0.4],
                    outline: true
                });
            }
        }
    });
});