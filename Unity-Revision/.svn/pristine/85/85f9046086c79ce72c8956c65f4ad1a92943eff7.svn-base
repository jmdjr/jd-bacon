var startingPoint;
private var direction = "up";


function Awake () {
startingPoint = transform.position.y;
}

function Update () {
if (transform.position.y > startingPoint + 5) {
direction = "down";
}
if (transform.position.y < startingPoint - 2) {
direction = "up";
}
if (direction == "up") {
transform.position.y = transform.position.y + 0.1;
}
else {
transform.position.y = transform.position.y - 0.1;
}

}