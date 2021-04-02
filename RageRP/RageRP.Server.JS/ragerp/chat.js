mp.events.add("playerChat", (player,message) =>{
    player.call('Send_ToChat',[player,message]);
});