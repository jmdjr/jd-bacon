var startingPoint;
private var direction = "up";


function Awake () {
startingPoint = transform.position.x;
}

function Update () {
if (transform.position.x > startingPoint + 5) {
direction = "down";
}
if (transform.position.x < startingPoint - 2) {
direction = "up";
}
if (direction == "up") {
transform.position.x = transform.position.x + 0.1;
}
else {
transform.position.x = transform.position.x - 0.1;
}


}