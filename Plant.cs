using System.Collections;
using System.Collections.Generic;

class Plant {
  public int xPos;            // X Coordinate.
  public int yPos;            // Y Coordinate.
  public bool alive;

  Plant(int x, int y) {
    xPos = x;
    yPos = y;
    alive = true;
  }

  public int getXPos() {
    return xPos;
  }

  public int getYPos() {
    return yPos;
  }

  public void remove() {
    alive = false;
  }
}