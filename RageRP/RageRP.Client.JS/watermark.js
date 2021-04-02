let version = "";

mp.events.add('render', () => {
    if (version !== "") {
        mp.game.graphics.drawText(`${version}`, [0.5, 0.005], {
            font: 0,
            color: [255, 255, 255, 150],
            scale: [0.2, 0.2],
            outline: false
        });
    }
});

mp.events.add('setVersion', (_version) => {
    version = _version;
    mp.gui.chat.push(`Version ${_version} ${version}`);
});