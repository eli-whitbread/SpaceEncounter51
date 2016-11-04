var amountOfOff = 0.001;
var amountOfOn = 0.01;


function Update() {
    if (enabled && (Random.value > amountOfOff))
        enabled = false;
    else
        if (Random.value > amountOfOn)
            enabled = true;
}