using System.Collections;
using System.Collections.Generic;

public class Plant {
  public float xPos;            // X Coordinate.
  public float yPos;            // Y Coordinate.
  public bool alive;

  public Plant(float x, float y) {
    xPos = x;
    yPos = y;
    alive = true;
  }

  public Plant(Plant clonable) {
    xPos = clonable.xPos;
    yPos = clonable.yPos;
    alive = clonable.alive;
  }

  public void die() {
    alive = false;
  }
}