var can, ctx, step, steps = 0, delay = 20;
// delay = redraw delay
// steps = number of lines
// step = current / start line
// can holds the canvas object which is transfered from the html. 

function init() { // init the canvas
    can = document.getElementById("MyCanvas1");
    ctx = can.getContext("2d");
    ctx.fillStyle = "black";
    ctx.font = "20pt Verdana";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    step = can.height;
    steps = can.height + 100;
    RunTextTopToBottom();
}

function RunTextTopToBottom() { // draw the running text
    step--;
    ctx.clearRect(0, 0, can.width, can.height);
    ctx.save();
    ctx.translate(can.width / 2, step); //moves the (0,0) coordinates to the x/2 & y = step line
    var txt = 'Shaulis Blog, a project by:\nZeev Manilovich\nRotem Adhoh\nMiri Kuskina\nLital Gilboa';
    var x = 0;
    var y = 0;
    var lineheight = 50;
    var lines = txt.split('\n');
    for (var i = 0; i < lines.length; i++)
        ctx.fillText(lines[i], x, y + (i * lineheight));
    ctx.restore();
    if (step == -can.height)
        step = can.height;
    if (step < steps)
        var t = setTimeout('RunTextTopToBottom()', delay);
}