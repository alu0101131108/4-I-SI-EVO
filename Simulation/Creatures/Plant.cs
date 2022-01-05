using System.Collections;
using System.Collections.Generic;

class Plant {
  public float xPos;            // X Coordinate.
  public float yPos;            // Y Coordinate.
  public bool alive;

  Plant(float x, float y) {
    xPos = x;
    yPos = y;
    alive = true;
  }

  public void remove() {
    alive = false;
  }
}